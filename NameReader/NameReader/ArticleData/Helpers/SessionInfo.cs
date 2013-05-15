using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameReader.ArticleData.Helpers
{
    /// <summary>
    /// A container for various search info: any unavailable urls, total results count from bing, opencalais service errors, duplicate articles in the bing search results
    /// </summary>
    public sealed class SessionInfo
    {
        //making this a singleton so I don't have to new up anything anywhere else in the app - it can just be used straight off
        private static SessionInfo instance;

        private List<string> unavailableUrls;
        private int bingTotalResults;
        private int serviceErrors;
        private int duplicateArticleCount;

        private SessionInfo() 
        {
            unavailableUrls = new List<string>();
            bingTotalResults = 0;
            serviceErrors = 0;
            duplicateArticleCount = 0;
        }

        public static SessionInfo Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SessionInfo();
                }
                return instance;
            }
        }

        public void AddUnavailableURL(string url)
        {
            unavailableUrls.Add(url);
        }

        public void SetBingTotalResults(int count)
        {
            bingTotalResults = count;
        }

        public void AddServiceError()
        {
            serviceErrors++;
        }

        public List<string> GetUnavailableUrls()
        {
            return unavailableUrls;
        }

        public int GetServiceErrorCount()
        {
            return serviceErrors;
        }

        public int GetBingTotalResults()
        {
            return bingTotalResults;
        }

        public void AddDuplicateArticle()
        {
            duplicateArticleCount++;
        }

        public int GetDuplicateArticleCount()
        {
            return duplicateArticleCount;
        }

    }
}
