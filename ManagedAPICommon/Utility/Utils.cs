using System.Text;
using System.Threading.Tasks;

namespace ManagedAPI.Common
{
    public class Utils
    {
        public static Map<uint, string> CachedHashes = new Map<uint, string>();

        public static uint GetOrCreateCachedHash(string name)
        {
            if (CachedHashes.Reverse.Contains(name))
            {
                return CachedHashes.Reverse[name];
            }
            else
            {
                uint hash = GetHash(name);
                CachedHashes.Add(hash, name);

                return hash;
            }
        }

        public static uint GetHash(string name)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(name.ToLowerInvariant());

            uint num = 0u;
            for (int i = 0; i < bytes.Length; i++)
            {
                num += (uint)bytes[i];
                num += num << 10;
                num ^= num >> 6;
            }
            num += num << 3;
            num ^= num >> 11;

            return num + (num << 15);
        }
    }
}
