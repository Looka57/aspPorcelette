// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;

// namespace ASPPorcelette.API.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class TestUserController : ControllerBase
//     {
//         [HttpGet("test")]
//         [AllowAnonymous]
//         public IActionResult Test()
//         {
//             return Ok(new { Message = "Test fonctionne !" });
//         }

//         [HttpGet("user-profile-test")]
//         [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
//         public IActionResult GetProfile()
//         {
//             var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//             return Ok(new
//             {
//                 Message = "Profile endpoint works!",
//                 UserId = userId
//             });
//         }
//     }
// }