using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Meetup_API.Helpers
{
    public class AuthOptions
    {
        public string Issuer { get; set; } //тот кто сгенерировал токен
        public string Audience { get; set; } //для кого предназначался токен
        public string Secret { get; set; } //секретная строка симметричного шифрования
        public int TokenLifeTime { get; set; }
        public SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)); // генерация симметричного ключа из секрета
    }
}
