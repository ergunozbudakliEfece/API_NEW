using System.Text;

namespace SQL_API.Utils.Encryptions
{
    public static class Hashing
    {
        public static string MD5(this string PlainText)
        {
            using var provider = System.Security.Cryptography.MD5.Create();
            StringBuilder builder = new StringBuilder();

            foreach (byte b in provider.ComputeHash(Encoding.UTF8.GetBytes(PlainText)))
                builder.Append(b.ToString("x2").ToLower());

            return builder.ToString();
        }
    }
}
