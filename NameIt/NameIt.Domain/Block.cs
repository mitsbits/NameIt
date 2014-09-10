using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameIt.Domain
{
    public class Block
    {
        public Block()
        {
            Taxonomies = new List<Taxonomy>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Taxonomy> Taxonomies { get; set; }
        public Visual Visual { get; set; }
    }
}
