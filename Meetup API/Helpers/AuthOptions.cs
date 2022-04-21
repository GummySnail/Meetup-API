using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Meetup_API.Helpers
{
    public class AuthOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; } 
        public string Secret { get; set; } 
        public int TokenLifeTime { get; set; }
        public SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
    }
}
