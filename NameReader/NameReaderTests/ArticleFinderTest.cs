using HtmlAgilityPack;
using NameReader.ArticleDownload;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NameReaderTests
{
    [TestFixture]
    public class ArticleFinderTest
    {
        int pageCount = 5;
        /// <summary>
        /// test to make sure there are no duplicate articles in the search results
        /// </summary>
        [Test]
        public void NoDuplicates()
        {
            ArticleFinder af = new ArticleFinder();
            //10 pages = 150 results

            var records = af.GetArticles("arrested", pageCount);
            
            var dupliactes = from a in records
                             group a by a.BingArticleID into grouped
                             where grouped.Count() > 1
                             select grouped.Key;
            Assert.Less(dupliactes.Count(), 1);
        }

        /// <summary>
        /// using XPATH to only get content from <p> tags within <article> tags, if that fails get <p> tags from div tags, if that fails get all HTML.
        /// "nodes" should never be empty and no exceptions except WebException and ArgumentNullException should be thrown
        /// </summary>
        [Test]
        public void EmptyContentTest()
        {
            StringBuilder htmlString = new StringBuilder();
            ArticleFinder af = new ArticleFinder();
            //10 pages = 150 results

            var records = af.GetArticles("arrested", pageCount);
            List<string> htmlList = new List<string>();
            using (WebClient client = new WebClient())
            {
                foreach (var item in records)
                {
                    try
                    {
                        htmlString.Append(client.DownloadString(item.URL));
                        htmlList.Add(htmlString.ToString());
                    }
                    catch (WebException)
                    {
                        string url = item.URL;
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
                htmlDoc.Load(new StringReader(htmlString.ToString()));
                IEnumerable<HtmlNode> nodes;
                try
                {
                    nodes = from HtmlNode node in
                                htmlDoc.DocumentNode.SelectNodes("//div/article/p") //get the everything in the article
                            select node;
                }
                catch (ArgumentNullException)
                {
                    try
                    {
                        nodes = from HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//div/p") //  get every p tag
                                select node;
                    }
                    catch (ArgumentNullException)
                    {
                        nodes = from HtmlNode node in htmlDoc.DocumentNode.DescendantsAndSelf()// get everything
                                select node;
                    }
                    
                }
                

                Assert.IsNotEmpty(nodes);
            }
            
        }

        /// <summary>
        /// some of the articles stored in the db have empty content attribute for the following URLS:
        /// http://www.news.com.au/breaking-news/world/indon-says-2-arrested-for-myanmar-plot/story-e6frfkui-1226634619523
        ///http://www.elpasotimes.com/newupdated/ci_23175234/el-paso-man-arrested-charged-cocaine-possession
        ///http://www.twincities.com/crime/ci_23160767/st-paul-man-arrested-after-mother-shot-is
        ///http://www.news-record.com/news/1174493-91/three-arrested-in-home-invasion
        ///http://www.mercurynews.com/news/ci_23159399/teacher-arrested-allegedly-molesting-girls
        ///http://www.menafn.com/menafn/9c990483-5c0b-4bb9-87ff-8ade7ac1a9c2/BRIEF-Tulsa-man-arrested-on-drug-gun-complaints
        /// </summary>
        [Test]
        public void EmptyArticleContentTest()
        {
            string[] urls = {"http://www.news.com.au/breaking-news/world/indon-says-2-arrested-for-myanmar-plot/story-e6frfkui-1226634619523",
                            "http://www.elpasotimes.com/newupdated/ci_23175234/el-paso-man-arrested-charged-cocaine-possession",
                            "http://www.twincities.com/crime/ci_23160767/st-paul-man-arrested-after-mother-shot-is",
                            "http://www.news-record.com/news/1174493-91/three-arrested-in-home-invasion",
                            "http://www.mercurynews.com/news/ci_23159399/teacher-arrested-allegedly-molesting-girls",
                            "http://www.menafn.com/menafn/9c990483-5c0b-4bb9-87ff-8ade7ac1a9c2/BRIEF-Tulsa-man-arrested-on-drug-gun-complaints"};

            List<Bing.NewsResult> bnrList = new List<Bing.NewsResult>();
            foreach (var item in urls)
            {
                bnrList.Add(new Bing.NewsResult { Url = item });
            }

            ArticleFinder af = new ArticleFinder();
            var temp = af.GetResults(bnrList);
            int emptyCounter = 0;
            foreach (var item in temp)
            {
                if (string.IsNullOrEmpty(item.Content) || string.IsNullOrWhiteSpace(item.Content))
                {
                    emptyCounter++;
                }
            }
            Assert.Less(emptyCounter, 1);
        }
    }
}
