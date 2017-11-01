using System;
using System.Collections.Generic;
using System.Text;

namespace ChristmasJoy.Models
{
   public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
    }
}
