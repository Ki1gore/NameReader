using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameReader.Model
{
    public class Name
    {
        public int NameID { get; set; }
        public string PersonName { get; set; }
        public virtual Article Article { get; set; }
    }
}
