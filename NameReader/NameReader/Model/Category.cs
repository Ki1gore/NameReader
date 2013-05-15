using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameReader.Model
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string ArticleCategory { get; set; }
        public virtual Article Article { get; set; }
    }
}
