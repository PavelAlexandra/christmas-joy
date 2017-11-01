using System;
using System.Security.Cryptography;
using System.Text;

namespace ChristmasJoy.App.Services
{
  public class SignInService
  {
    public bool CheckLoginInPassword(string password, string hashedPassword)
    {
       var hashedInput = GetHashedPassword(password);

       return string.Compare(hashedPassword, hashedInput) == 0;
    }

    public string GetHashedPassword(string password)
    {
      using (var sha256 = SHA256.Create())
      {
        // Send a sample text to hash.  
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        // Get the hashed string.  
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
      }
    }
  }
}
