using DataAccessLibrary;
using EmployeeAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static GlobalValues.GlobalVariables;

namespace EmployeeAPI.V1.TokenManager
{
    public class JwtTokenManager
    {

        public string CreateToken(int branchID, int employeeID)
        {
            var tokenString = "";
            try
            {
                        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("myEncryptionKey@143#"));

                        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                        var claims = new List<Claim>
                            {
                            new Claim(ClaimTypes.Name, branchID + "," + employeeID)
                        };
                           var tokeOptions = new JwtSecurityToken(
                           issuer: "http://localhost:5000",
                           audience: "http://localhost:5000",
                           claims: claims,
                           notBefore: DateTime.Now,
                           signingCredentials: signinCredentials
                           );
                         tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            }
            catch (Exception e) { }

            return tokenString;
        }
    }
}
