using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Meetup_API.Helpers
{
    public class AuthOptions
    {
      
        public string Secret { get; set; } 
        public int TokenLifeTime { get; set; }
        public SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
    }
}
