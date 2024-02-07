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
                    return Ok("User Created Successfully");
                }
                else
                {
                    // Convert IEnumerable<string> to a single string
                    var errorMessage = string.Join(", ", result.Errors.Select(c => c.Description));
                    return BadRequest($"Can't Create User. Check: {errorMessage}");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
   
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel LoginObject)
        {
            if (ModelState.IsValid)
            {
               var user = await _userManager.FindByEmailAsync(LoginObject.Email);
                if (user != null)
                {
                    var checkPassword = await _userManager.CheckPasswordAsync(user, LoginObject.Password);
                    if (checkPassword)
                    {
                        return Ok("Login Success");
                    }
                    else
                    {
                        return BadRequest("Invalid Password");
                    }
                }
                else
                {
                    return BadRequest($"Invalid Enterd Email : {LoginObject.Email}");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
