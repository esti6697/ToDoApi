// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using ToDoApi;

// [ApiController]
// [Route("api/auth")]
// public class AuthController : ControllerBase
// {
//     private readonly JwtService _jwtService;

//     public AuthController(JwtService jwtService)
//     {
//         _jwtService = jwtService;
//     }

//     [HttpPost("login")]
//     public IActionResult Login([FromBody] LoginRequest request)
//     {
//         // אימות משתמש (בפועל זה יהיה מול בסיס נתונים)
//         if (request.Username == "admin" && request.Password == "password")
//         {
//             var token = _jwtService.GenerateToken(request.Username);
//             return Ok(new { Token = token });
//         }

//         return Unauthorized("Invalid username or password");
//     }
// }

// public class LoginRequest
// {
//     public string Username { get; set; }
//     public string Password { get; set; }
// }
