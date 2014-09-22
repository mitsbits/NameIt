using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameIt.Dal.Contracts
{
    public interface IRepositoryWithContext
    {
        bool AutoSave { get; set; }
        bool DisposeContext { get; set; }
    }
}
