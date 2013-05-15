using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameReader.Model
{
    public class NameReaderContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
    }
}
