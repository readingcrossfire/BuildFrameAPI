using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACHE
{
    public interface ICache
    {
        public T GetCache<T>(string strKey);
    }
}
