using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using MUNityAngular.Models.Core;


namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class AuthKey
    { 

        public string AuthKeyValue { get; set; }

        public User User { get; set; }

        public DateTime GenerationDate { get; set; }

        public string GenerationIp { get; set; }

        public string GenerationDevice { get; set; }

        public AuthKey()
        {
            RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
            byte[] randoms = new byte[64];
            rngCsp.GetBytes(randoms);
            AuthKeyValue = Convert.ToBase64String(randoms);
        }
    }
}
