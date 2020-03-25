﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class AuthKey
    {
        public string AuthId { get; set; }

        public User User { get; set; }

        public DateTime GenerationDate { get; set; }

        public string GenerationIp { get; set; }

        public string GenerationDevice { get; set; }

        public AuthKey()
        {
            RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
            byte[] randoms = new byte[64];
            rngCsp.GetBytes(randoms);
            AuthId = Convert.ToBase64String(randoms);
        }
    }
}
