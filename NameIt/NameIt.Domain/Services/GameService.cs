﻿using System;
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
                result.SetBucket.Add(r.Next(), new Part
                {
                    Block = block,
                    AlternateNames = ExtractNamesRandomOrder(bucket, block.Name)
                });
            }
            return result;
        }

        private static string[] ExtractNamesRandomOrder(IEnumerable<Block> bucket, string currentName,
            int total = 3)
        {
            var r = new Random();

            var list = bucket.Where(x => x.Name != currentName)
                .Select(x => new { x.Name, Order = r.Next() }).Take(total - 1)
                .Select(o => o.Name)
                .ToList();

            list.Add(currentName);

            list.Sort();
            return list.ToArray();
        }
    }
}
