using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace NameIt.Domain
{
    public class Game
    {
        public Game()
        {
            SetBucket = new Dictionary<int, Part>();
        }

        public IDictionary<int, Part> SetBucket { get; private set; }

        public Part[] Parts
        {
            get
            {
                return SetBucket.Keys.OrderBy(x => x).ToList().Select(key => SetBucket[key]).ToArray();
            }
        }
    }
}