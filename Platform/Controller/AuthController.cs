using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Platform.Model.Auth;

namespace Platform.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private UserManager<IdentityUser> _userManager;
        public AuthController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpPost("Signup")]
        public async Task<IActionResult> SignUp(SignupViewModel SignupObject)
        {
            if (ModelState.IsValid)
            {
                var identityUser = new IdentityUser
                {
                    UserName = SignupObject.Name,
                    Email = SignupObject.Email,
                    PhoneNumber = SignupObject.phone
                };
                var result = await _userManager.CreateAsync(identityUser, SignupObject.Password);

                if (result.Succeeded)
                {
                    return Ok("User Created Successfuly");
                }
                else
                {
                    return BadRequest($"Can't Create User Check {result.Errors.Select(c => c.Description)}");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
