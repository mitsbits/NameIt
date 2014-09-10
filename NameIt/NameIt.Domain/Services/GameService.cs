using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameIt.Domain.Services
{

    public class GameService
    {
        private readonly BlockService _blocks;

        public GameService(BlockService blocks)
        {
            _blocks = blocks;
        }

        public Game GetGame(params int[] taxonomies)
        {
            var bucket = _blocks.Find(taxonomies);
            var result = new Game();
            var r = new Random();
            foreach (var block in bucket)
            {
                result.Set.Add(r.Next(), new Part
                {
                    Block = block,
                    AlternateNames = ExtractNamesRandomOrder(bucket).Where(o => o != block.Name).Take(2).ToArray()
                });
            }
            return result;
        }

        private static IEnumerable<string> ExtractNamesRandomOrder(IEnumerable<Block> bucket)
        {
            var r = new Random();

            return bucket.Select(x => new {x.Name, Order = r.Next() })
                    .OrderBy(o => o.Order)
                    .Select(o => o.Name)
                    .ToArray();
        }
    }
}
