using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameReader.Model
{
    /// <summary>
    /// article text -> full text content of the article
    /// article url
    /// article ID -> a hash the article url
    /// Names -> (virtual) instance of Names class
    /// Categories -> (virtual) instance of the Categories class
    /// </summary>
    public class Article
    {
        public string ArticleId { get; set; }
        public string ArticleText { get; set; }
        public string ArticleUrl { get; set; }
        public string ArticleTitle { get; set; }
        public string ArticleSource { get; set; }
        public string ArticleDescription { get; set; }
        public virtual ICollection<Name> Names { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
