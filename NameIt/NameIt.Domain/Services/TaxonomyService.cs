using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameIt.Domain.Services
{


    public class TaxonomyService
    {
        private readonly List<Taxonomy> _bucket = new List<Taxonomy>();
        public TaxonomyService()
        {
            _bucket.AddRange(new[]
            {
                new Taxonomy { Display = "Persons", Id = 1 }, 
                new Taxonomy { Display = "Athletes", ParentId = 1, Id = 2 }, 
                new Taxonomy { Display = "Artists", ParentId = 1, Id = 3 },
                new Taxonomy { Display = "Politicians", ParentId = 1, Id = 7 },
                new Taxonomy { Display = "Nationality", Id = 4 }, 
                new Taxonomy { Display = "Greeks", ParentId = 4, Id = 5 }, 
                new Taxonomy { Display = "Americans", ParentId = 4, Id = 6 }
            });
        }

        public IList<Taxonomy> GetAll()
        {
            return _bucket;
        } 
    }
}
