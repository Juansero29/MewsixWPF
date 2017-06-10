using System;

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
