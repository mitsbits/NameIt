using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameIt.Dal.Repositories
{
    public struct CrudBaseRepositoryConfiguration
    {

        public bool AutoSave
        {
            get;
            set;
        }
        public bool DisposeContext
        {
            get;
            set;
        }
    }
}
