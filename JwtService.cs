// using System;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using Microsoft.Extensions.Configuration;
// using Microsoft.IdentityModel.Tokens;

// public class JwtService
// {
//     private readonly string _secretKey;
//     private readonly string _issuer;
//     private readonly string _audience;
//     private readonly int _expiryMinutes;

//     public JwtService(IConfiguration configuration)
//     {
//         _secretKey = configuration["JwtSettings:SecretKey"];
//         _issuer = configuration["JwtSettings:Issuer"];
//         _audience = configuration["JwtSettings:Audience"];
//         _expiryMinutes = int.Parse(configuration["JwtSettings:ExpiryMinutes"]);
//     }

//     public string GenerateToken(string username)
//     {
//         var claims = new[]
//         {
//             new Claim(ClaimTypes.Name, username),
//             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
//         };

//         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
//         var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//         var token = new JwtSecurityToken(
//             issuer: _issuer,
//             audience: _audience,
//             claims: claims,
//             expires: DateTime.Now.AddMinutes(_expiryMinutes),
//             signingCredentials: credentials);

//         return new JwtSecurityTokenHandler().WriteToken(token);
//     }
// }
