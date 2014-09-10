using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NameIt.Domain.Services
{
    public class BlockService
    {
        private readonly TaxonomyService _taxonomies;
        private readonly List<Block> _bucket = new List<Block>();
        public BlockService(TaxonomyService taxonomies)
        {
            _taxonomies = taxonomies;

            Init();
        }

        public IList<Block> GetAll()
        {
            return _bucket;
        }

        public IList<Block> Find(params int[] taxonomies)
        {
            var result = _bucket
                .Where(block => block.Taxonomies.Select(x => x.Id)
                    .Intersect(taxonomies).Any())
                    .ToList();
            return result;
        } 

        private void Init()
        {
            var tempTaxonomies = _taxonomies.GetAll();
            _bucket.AddRange(new[]
            {
                new Block
                {
                    Id = 1,
                    Name = "Elvis Presley",
                    Taxonomies = tempTaxonomies.Where(x => new[] {3, 6}.Contains(x.Id)).ToList(),
                    Visual =
                        new Visual
                        {
                            BlockId = 1,
                            Url = "http://cdn.visualnews.com/wp-content/uploads/2013/12/1939_elvis-aaron-presley.jpg"
                        }
                },
                new Block
                {
                    Id = 2,
                    Name = "Barack Obama",
                    Taxonomies = tempTaxonomies.Where(x => new[] {7, 6}.Contains(x.Id)).ToList(),
                    Visual =
                        new Visual {BlockId = 2, Url = "http://cdn.visualnews.com/wp-content/uploads/2013/12/Barack-Obama.jpg"}
                },
                new Block
                {
                    Id = 3,
                    Name = "George W. Bush",
                    Taxonomies = tempTaxonomies.Where(x => new[] {7, 6}.Contains(x.Id)).ToList(),
                    Visual =
                        new Visual {BlockId = 1, Url = "http://cdn.visualnews.com/wp-content/uploads/2013/12/George-W-Bush.jpg"}
                },
                new Block
                {
                    Id = 3,
                    Name = "Cassius Clay",
                    Taxonomies = tempTaxonomies.Where(x => new[] {2, 6}.Contains(x.Id)).ToList(),
                    Visual =
                        new Visual {BlockId = 3, Url = "http://cdn.visualnews.com/wp-content/uploads/2013/12/Muhammed-Ali.jpg"}
                },
                new Block
                {
                    Id = 4,
                    Name = "Richard Nixon",
                    Taxonomies = tempTaxonomies.Where(x => new[] {7, 6}.Contains(x.Id)).ToList(),
                    Visual =
                        new Visual
                        {
                            BlockId = 4,
                            Url = "http://cdn.visualnews.com/wp-content/uploads/2013/12/Young-Richard-Nixon.jpg"
                        }
                },
                new Block
                {
                    Id = 5,
                    Name = "Robin Williams",
                    Taxonomies = tempTaxonomies.Where(x => new[] {3, 6}.Contains(x.Id)).ToList(),
                    Visual =
                        new Visual
                        {
                            BlockId = 5,
                            Url = "http://media-cache-ak0.pinimg.com/736x/09/44/15/094415de362229d9eca96177e3a63abc.jpg"
                        }
                },
                new Block
                {
                    Id = 6,
                    Name = "Keith Richards",
                    Taxonomies = tempTaxonomies.Where(x => new[] {3, 6}.Contains(x.Id)).ToList(),
                    Visual =
                        new Visual
                        {
                            BlockId = 6,
                            Url = "http://media-cache-ak0.pinimg.com/736x/37/ef/e5/37efe54184e370722f2e344d2003a199.jpg"
                        }
                },
            });
        }
    }
}
