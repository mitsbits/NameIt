using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameIt.Domain
{
   public class Taxonomy
    {
       public Taxonomy()
       {
           Children = new List<Taxonomy>();
       }
       public int Id { get; set; }
       public int? ParentId { get; set; }
       public string Display { get; set; }
       public IList<Taxonomy> Children { get; set; }
    }
}
