using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace NameIt.Domain
{
    public class Game
    {
        public Game()
        {
            Set = new Dictionary<int, Part>();
        }

        public IDictionary<int, Part> Set { get; private set; }
    }
}