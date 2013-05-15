using NameReader.ArticleData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameReaderTests
{
    [TestFixture]
    public class ArticleReaderTest
    {
        int pageCount = 1;
        [Test]
        public void TestArticleReader()
        {
            ArticleReader ar = new ArticleReader();
            var articles = ar.GetArticleData("arrested", pageCount);
            Assert.IsNotEmpty(articles);
        }
    }
}
