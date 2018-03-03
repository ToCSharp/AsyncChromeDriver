using System;
using System.Text;

namespace Zu.Chrome.DriverCore
{
    public class Util
    {
        static Random _rnd = new Random();
        public static string GenerateId()
        {
            var bytes = new byte[16];
            _rnd.NextBytes(bytes);
            return ByteArrayToString(bytes);
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
