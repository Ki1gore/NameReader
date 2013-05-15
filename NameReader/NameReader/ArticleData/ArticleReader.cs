using NameReader.ArticleData.Helpers;
using NameReader.ArticleDownload;
using NameReader.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NameReader.ArticleData
{
    /// <summary>
    /// Gets news articels form ArticleFinder, sends each one to opencalais service, parses the result and returns a list of Model.Article
    /// </summary>
    public class ArticleReader
    {
        private CalaisService.calaisSoapClient csSOAP; //calais client
        private StringBuilder sb; //used to get the response string from the calais API call
        public ArticleReader()
        {
            csSOAP = new CalaisService.calaisSoapClient();
            sb = new StringBuilder();
        }

        public List<Article> GetArticleData(string searchTerm, int pages)
        {
            //get article finder results
            ArticleFinder af = new ArticleFinder();
            var finderResults = af.GetArticles(searchTerm, pages); //get the articles from the article finder
            List<Article> articleList = new List<Article>();
            foreach (var item in finderResults)
            {
                articleList.Add(GenerateArticle(item));
            }
            
            return articleList;
        }

        //Contacts the calais service and gets the response RDF string then calls GetCategoreisAndPeople to get names and categories from the document,
        //finally calls GetNewArticle to new up a Model.Article and return it
        private Article GenerateArticle(ArticleFinderResult item)
        {
            sb.Clear();
            if (string.IsNullOrEmpty(item.RawHtml)) //got plain text content from this item so use params for text
            {
                try
                {
                    sb.Append(csSOAP.Enlighten(App_Resources.openCalaisKey.ToString(), item.Title + " " + item.Content, Params.GetTextParamsXML()));
                }
                catch (Exception ex) 
                {
                    if (ex.GetType() == typeof(TimeoutException)) //the service sometimes doesn't respond so need to catch this
                    {
                        Trace.TraceInformation(DateTime.Now.ToString() + " Timeout in content: " + item.URL); // error logged in log file (see program.cs)
                        SessionInfo.Instance.AddServiceError(); // keep track of the number of service errors 
                    }
                    if (ex.GetType() == typeof(MessageSecurityException))
                    {
                        Trace.TraceInformation(DateTime.Now.ToString() + " Message security exception: " + item.URL + " " + ex.Message); // error logged in log file (see program.cs)
                        SessionInfo.Instance.AddServiceError();
                    }
                    else
                    {
                        Trace.TraceInformation(DateTime.Now.ToString() + " UNEXPECTED ERROR: " + item.URL + " " + ex.Message); // error logged in log file (see program.cs)
                        SessionInfo.Instance.AddServiceError();
                    }
                    
                }
            }
            else //just raw HTML so use params for html
            {
                try
                {
                    sb.Append(csSOAP.Enlighten(App_Resources.openCalaisKey.ToString(), item.RawHtml, Params.GetHTMLParamsXML()));
                }
                catch (Exception ex) 
                {
                    if (ex.GetType() == typeof(TimeoutException))
                    {
                        Trace.TraceInformation(DateTime.Now.ToString() + " Timeout in HTML: " + item.URL);
                        SessionInfo.Instance.AddServiceError();
                    }
                    if (ex.GetType() == typeof(MessageSecurityException))
                    {
                        Trace.TraceInformation(DateTime.Now.ToString() + " Message security exception: " + item.URL);
                        SessionInfo.Instance.AddServiceError();
                    }
                    else
                    {
                        Trace.TraceInformation(DateTime.Now.ToString() + " UNEXPECTED ERROR: " + item.URL + " " + ex.Message);
                        SessionInfo.Instance.AddServiceError();
                    } 
                }
            }
            List<Name> names; //all the names in the documnet content
            List<Category> categories; //any document categories identified 
            string rawDocumentText;  //document content as returned by the service
            GetCategoreisAndPeople(sb, out names, out categories, out rawDocumentText);
            return GetNewArticle(item, names, categories, rawDocumentText);
        }

        private Article GetNewArticle(ArticleFinderResult item, List<Name> names, List<Category> categories, string rawDocumentText)
        {
            return new Article
            {
                ArticleId = GetUrlHash(item.URL),
                Categories = categories,
                Names = names,
                ArticleTitle = item.Title,
                ArticleSource = item.Source,
                ArticleDescription = item.Description,
                //add the document content (which is document text extracted by this app) or add the rawDocumentText (returned by the service)
                //document content extracted by this app is typically more readable (no \n or \t chars everywhere), text returned by the service is often full of these so
                //adding the cleanest text when possible
                ArticleText = (string.IsNullOrEmpty(item.Content) ? rawDocumentText : item.Content),  
                ArticleUrl = item.URL                                                                 
            };
        }

        #region URL to hash methods - had trouble making entity framework accept Articles with a key that was the URL of the document (something to do with the ':', '/', chars probably), so instead i'm hashing the url
        //and returning the hash in string format - that becomes the Model.Article PK
        
        private string GetUrlHash(string p)
        {
            byte[] hashVal;
            byte[] byteArray = Encoding.ASCII.GetBytes(p);
            MD5 md5Hash = MD5.Create();
            hashVal = md5Hash.ComputeHash(byteArray);
            return GetString(hashVal);
        }

        private string GetString(byte[] hashVal)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in hashVal)
            {
                sb.Append(item.ToString("x2"));
            }
            return sb.ToString();
        }
        #endregion

        //attempts to deserialize the response RDF into an ArticleData.Helpers.RDF object, then loops through the object's item collection to discover names and categories in the document
        //if the service response is empty, articles are still returned - just without any names or categories
        public void GetCategoreisAndPeople(StringBuilder sb, out List<Name> names, out List<Category> categories, out string rawDocumentText)
        {
            names = new List<Name>();
            categories = new List<Category>();
            rawDocumentText = null;
            XmlSerializer ser = new XmlSerializer(typeof(RDF));
            RDF rdf;
            if (!(string.IsNullOrEmpty(sb.ToString()))) //if the service timed out, or errored in some other way, sb will be empty
            {
                
                using (XmlReader reader = XmlReader.Create(new StringReader(sb.ToString())))
                {
                    try
                    {
                        rdf = (RDF)ser.Deserialize(reader);
                    }
                    catch (InvalidOperationException) //the service will sometimes return an exception in RDF format which cannot be deserialzed into an ArticleData.Helpers.RDF object
                    {                                 //so just log the error
                        Trace.TraceInformation(DateTime.Now.ToString() + " Unable to parse RDF response " + "response was: " + sb.ToString());
                        rdf = null;
                    }

                }
                if (rdf != null)
                {
                    foreach (var item in rdf.Description)
                    {
                        if (item.docFullDocument != null) //document content
                        {
                            rawDocumentText = item.docFullDocument;
                        }
                        if (item.docPersonCommonName != null) //person common names
                        {
                            names.Add(new Name { PersonName = item.docPersonCommonName });
                        }
                        if (item.docCategoryName != null) //categories into which this document content falls - e.g. "Law_Crime"
                        {
                            categories.Add(new Category { ArticleCategory = item.docCategoryName });
                        }
                    }
                }
            }
        }
    }
}
