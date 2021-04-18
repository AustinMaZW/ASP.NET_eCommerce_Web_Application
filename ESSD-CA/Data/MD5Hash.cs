using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ESSD_CA.Data
{
    public class MD5Hash
    {
        public static string Md5hash(MD5 md5hash, string input)
        {
            byte[] data = md5hash.ComputeHash(Encoding.UTF32.GetBytes(input));

            StringBuilder hashOutput = new StringBuilder();

            for(int i=0; i < data.Length; i++)
            {
                hashOutput.Append(data[i].ToString());
            }

            return hashOutput.ToString();
        }

    }
}
