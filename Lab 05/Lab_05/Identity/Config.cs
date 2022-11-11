using IdentityModel;
using IdentityServer4.Test;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Identity
{
    public class Config
    {
        public static List<TestUser> Users
        {
            get
            {
                var address = new
                {
                    street_address = "Applegarden, 89",
                    locality = "Main",
                    postal_code = 12345,
                    country = "Soulland"
                };
                return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "11111",
                        Username = "callie",
                        Password = "whatareyouacop",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Callie Lirson"),
                            new Claim(JwtClaimTypes.Email, "yourbestnightmare@email.com")
                        }
                    }
                };
            }
        }
    }
}
