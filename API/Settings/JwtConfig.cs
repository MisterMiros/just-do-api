using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settings
{
    public class JwtConfig
    {
        public string Issuer { get; private set; }
        public string Key { get; private set; }

        public JwtConfig(string issuer, string key)
        {
            Issuer = issuer;
            Key = key;
        }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
