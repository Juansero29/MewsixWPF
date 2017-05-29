using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mewsix.Helpers
{
    public static class IdGenerator
    {
        public static string GetID()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
