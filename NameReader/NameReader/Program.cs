using NameReader.ArticleData;
using NameReader.ArticleData.Helpers;
using NameReader.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NameReader
{
    class Program
    {
        static void Main(string[] args)
        {
            int pagesCount;
            Trace.Listeners.Add(new TextWriterTraceListener("NameReaderOutput.log", "nameReaderListener")); //located in NameReader\bin\Debug\NameReaderOutput.log
            ArticleReader ar = new ArticleReader();
            while (true)
            {
                Console.WriteLine("Enter the number of pages of results to get (between 1 and 100))");
                Console.WriteLine("To exit, press q");
                if (int.TryParse(Console.ReadLine(), out pagesCount))
                {
                    if (pagesCount > 0 && pagesCount <= 100)
                    {
                        var articles = ar.GetArticleData("arrested", pagesCount);
                        Console.WriteLine(articles.Count() + " results in set");
                        Console.WriteLine("Duplicate articles found: " + SessionInfo.Instance.GetDuplicateArticleCount());
                        Console.WriteLine("Number of service errors: " + SessionInfo.Instance.GetServiceErrorCount());
                        Console.WriteLine("Unavailable URLS:" + SessionInfo.Instance.GetUnavailableUrls().Count());
                        if (SessionInfo.Instance.GetUnavailableUrls().Count() > 0)
                        {
                            foreach (var item in SessionInfo.Instance.GetUnavailableUrls())
                            {
                                Console.WriteLine(item);
                            }
                        }

                        StoreResults(articles);
                        Trace.Flush();
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private static void StoreResults(List<Article> articles)
        {
            int count = articles.Count();
            NameReaderContext ctx = new NameReaderContext();
            foreach (var item in articles)
            {
               if (ctx.Articles.Find(item.ArticleId) == null)
               {
                   ctx.Articles.Add(item);
               }
               else
               {
                   count--;
               }
            }
            ctx.SaveChanges();
            Console.WriteLine(count + " unique articles saved to database");
        }
    }
}
