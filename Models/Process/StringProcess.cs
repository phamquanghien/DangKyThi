using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using QuanLyCaThi.Data;

namespace QuanLyCaThi.Models.Process
{
    public class StringProcess
    {
        public string RemoveAccents(string input)
        {
            string normalizedString = input.Normalize(NormalizationForm.FormKD);
            Regex regex = new Regex("[\\p{M}]");
            string result = regex.Replace(normalizedString, "").ToLower().Trim();
            result = result.Replace("đ","d");
            result = result.Replace("  "," ");
            return result;
        }
        public static string CreateMD5Hash(string input)
        {
            // Step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            
            // Step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}