using System.Security.Cryptography;
using System.Text;

namespace UiPath.Shared.Activities.Utilities;

public static class HashFunctions
{
   public static string GetHash(HashAlgorithm hashAlgorithm, string input)
   {

      // Convert the input string to a byte array and compute the hash.
      var data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

      // Create a new StringBuilder to collect the bytes
      // and create a string.
      var sBuilder = new StringBuilder();

      // Loop through each byte of the hashed data 
      // and format each one as a hexadecimal string.
      foreach ( var t in data )
      {
         sBuilder.Append(t.ToString("x2"));
      }

      // Return the hexadecimal string.
      return sBuilder.ToString();
   }
}