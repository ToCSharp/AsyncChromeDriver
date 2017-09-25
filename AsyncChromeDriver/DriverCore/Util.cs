using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zu.Chrome.DriverCore
{
    public class Util
    {
        public static string GenerateId()
        {
            var rnd = new Random();
            var bytes = new byte[16];
            rnd.NextBytes(bytes);
            return ByteArrayToString(bytes);
            //var sb = new StringBuilder();
            //for (int i = 0; i < 16; i++)
            //{

            //    sb.Append(rnd.Ne()
            //}
            //var msb = base::RandUint64();
            //uint64_t lsb = base::RandUint64();
            //return base::StringPrintf("%016" PRIx64 "%016" PRIx64, msb, lsb);
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
