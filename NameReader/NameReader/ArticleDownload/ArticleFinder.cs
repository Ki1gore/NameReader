using Bing;
using HtmlAgilityPack;
using NameReader.ArticleData.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NameReader.ArticleDownload
{
    /// <summary>
    /// Connects to the bing service and returns html documents based on a search term
    /// </summary>
    public class ArticleFinder
    {
        private BingSearchContainer bingSearchContainer;

        public ArticleFinder()
        {
            bingSearchContainer = new BingSearchContainer(new Uri(App_Resources.bingNewsServiceUrl));
            bingSearchContainer.IgnoreMissingProperties = true;
            bingSearchContainer.Credentials = new NetworkCredential(App_Resources.bingUserName, App_Resources.bingKey);
        }

        public List<ArticleFinderResult> GetArticles(string searchTerm, int pages)
        {
            Console.WriteLine("Finding news articles for the search term " + "\"" + searchTerm + "\"");
            DataServiceQuery<NewsResult> records = null;
            List<NewsResult> nrList = new List<NewsResult>();
            for (int i = 1; i <= pages; i++) //bing service return 15 results per page so to get more results we need to specify which page we want
            {
                //calls the bing news service with the following params:
                //Query: searchTerm
                //Options: none
                //Market: en-GB
                //Adult filter: off
                //Latitude: none
                //Longitude: none
                //NewsLocationOverride: none
                //NewsCategory: all (no category filtering applied)
                //NewsSortBy: sorted by Relevance
                //Results Count (custom property): 15, which is the maximum
                //Page number: the page from which we want 15 results (incremented in this loop so we can get > 15 results each time the app runs)
                records = bingSearchContainer.News(searchTerm, "", "en-GB", "off", null, null, null, null, "Relevance", 15, i);
                foreach (var item in records)
                {
                    nrList.Add(item);
                }
            }
            Console.WriteLine("News results from bing: " + nrList.Count());
            SessionInfo.Instance.SetBingTotalResults(nrList.Count()); //record the number of results found in Session info
            return GetResults(nrList);
        }

        //downloads full html for each article found by bing, and attempts to get only article content, first from p tags in articles, then from p tags in div tags,
        //if both those fail, just get the html and let the calais service deal with it.  Reason for doing this is that the service returns better quality results from
        //plain text than from html - returned names are more relavent to the article content for example.
        public List<ArticleFinderResult> GetResults(List<NewsResult> nrList)
        {
            StringBuilder htmlString = new StringBuilder(); //reusing a stringbuilder rather than newing a string for every download
            Dictionary<string, NewsResult> htmlList = new Dictionary<string, NewsResult>(); // key = full html, value = news result returned by bing
            List<ArticleFinderResult> finderResults = new List<ArticleFinderResult>();
            List<string> urlCheck = new List<string>(); //every url that gets downloaded is added to this list so any dupliacte urls in the bing result list are found before we download the whole page
            using (WebClient client = new WebClient())
            {
                foreach (var item in nrList)
                {
                    try
                    {
                        if (!(urlCheck.Contains(item.Url)))
                        {
                            urlCheck.Add(item.Url);
                            htmlString.Append(client.DownloadString(item.Url));
                            if (!(htmlList.ContainsKey(htmlString.ToString())))
                            {
                                htmlList.Add(htmlString.ToString(), item);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Duplicate article: " + item.Source);
                            SessionInfo.Instance.AddDuplicateArticle();
                        }
                    }
                    catch (WebException wx)
                    {
                        Console.WriteLine("Unable to get page: " + wx.Message + "Page: " + item.Url); //often 403 or sometimes 404 erros
                        SessionInfo.Instance.AddUnavailableURL(item.Url);
                    }
                    finally
                    {
                        htmlString.Clear();
                    }
                }
            }

            foreach (var item in htmlList)
            {
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.Load(new StringReader(item.Key));
                IEnumerable<HtmlNode> nodes;
                try
                {
                    nodes = from HtmlNode node in
                                htmlDoc.DocumentNode.SelectNodes("//div/article/p") //get every p tag in the article tag
                            select node;
                    finderResults.Add(GetResultsFromNodes(nodes, item.Value, item.Key));
                }
                catch (ArgumentNullException)
                {
                    try
                    {
                        nodes = from HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//div/p") //  get every p tag in a div tag
                                select node;
                        finderResults.Add(GetResultsFromNodes(nodes, item.Value, item.Key));
                    }
                    catch (ArgumentNullException)
                    {
                        finderResults.Add(GetResultsFromNodes(null, item.Value, item.Key)); //just get the raw html
                    }
                }
            }
            return finderResults;
        }

        public ArticleFinderResult GetResultsFromNodes(IEnumerable<HtmlNode> nodes, NewsResult newsResult, string p)
        {
            if (nodes == null)
            {
                return new ArticleFinderResult
                {
                    BingArticleID = newsResult.ID,
                    Title = newsResult.Title,
                    Source = newsResult.Source,
                    Description = newsResult.Description,
                    Date = newsResult.Date,
                    URL = newsResult.Url,
                    RawHtml = p
                };
            }
            else
            {
                return new ArticleFinderResult
                {
                    BingArticleID = newsResult.ID,
                    Title = newsResult.Title,
                    Source = newsResult.Source,
                    Description = newsResult.Description,
                    Date = newsResult.Date,
                    URL = newsResult.Url,
                    Content = (string.IsNullOrEmpty(GetContent(nodes)) || string.IsNullOrWhiteSpace(GetContent(nodes)) ? p : GetContent(nodes)) //making sure the article content is never empty
                };
            }
        }

        private string GetContent(IEnumerable<HtmlNode> nodes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in nodes)
            {
                sb.Append(item.InnerText);
            }
            return sb.ToString();
        }

    }

    //container for Article finder results
    public class ArticleFinderResult
    {
        public Guid BingArticleID { get; set; }
        public string Title { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public string URL { get; set; }
        public string Content { get; set; } //used if we get anything from the XPATH search
        public string RawHtml { get; set; } //used if XPATH yeilds nothing
    }
}
