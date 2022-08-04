using System.Security.Cryptography;
using System.Text;

namespace XshapeAPI.Functions
{
    public class functions {
        public static Boolean CompPassword(byte[] dbPassword, string inputPassword)
        {
            byte[] hashInputPassword = SHA1.HashData(ASCIIEncoding.ASCII.GetBytes(inputPassword));

            if (dbPassword.Length == hashInputPassword.Length)
            {
                int i = 0;

                while ((i < hashInputPassword.Length) && (hashInputPassword[i] == dbPassword[i]))
                {
                    i++;
                }
                if (i == hashInputPassword.Length)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            } else 
            {
                return false;
            }
        }

        public static Boolean validDevKey(string devKey) {
            if (devKey != Environment.GetEnvironmentVariable("devKey")) { 
                return false;
            }

            return true;
        }
    }
}