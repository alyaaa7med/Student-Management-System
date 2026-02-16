using api_lab2.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api_lab2.Controllers
{
    [Route("api/accounts")]

    public class AuthenticationController : Controller
    {
        
        [HttpPost]
        public ActionResult login(LoginDTO ldto)
        {
            Console.WriteLine(ldto.Email + " " + ldto.Password);
            if (ldto.Email == "alyaa.com" && ldto.Password == "123")
            {
                var userdata = new List<Claim>();
                userdata.Add(new Claim("email", ldto.Email));
                userdata.Add(new Claim("phone", "01090575194"));

                var seckey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("it is my key , it is used for assigning and checking "));
                var signkey = new SigningCredentials(seckey, SecurityAlgorithms.HmacSha256);
                //generate token 
                //header => type :Bearer ,hash algorithm : JWT 
                //payload => claims , expire date 
                //signature => hashing (secert key) 
                var tokenObj = new JwtSecurityToken(
                    claims:userdata,
                    expires:DateTime.Now.AddDays(1),
                    signingCredentials: signkey //hashed secret key 

                    );
                //make the token object is a string 
                var string_token = new JwtSecurityTokenHandler().WriteToken(tokenObj);

                //send it to the user 
                return Ok(string_token);

            }

            else
            {
                return Unauthorized();
            }

        }

        [HttpGet]
        [Authorize] 
        public ActionResult get()
        {
            return Ok("valid");
        }
    }
}
