using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Platform.Model.Auth;
using SendEmailsWithDotNet5.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Platform.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private UserManager<UserIdentity> _userManager;
        private IConfiguration _config;
        private readonly IMailingService _mailingService;

        public AuthController(UserManager<UserIdentity> userManager,IConfiguration config, IMailingService mailingService)
        {
            _userManager = userManager;
            _config = config;
            _mailingService = mailingService;
        }
        [HttpPost("Signup")]
        public async Task<IActionResult> SignUp(SignupViewModel SignupObject)
        {
            if (ModelState.IsValid)
            {
                var identityUser = new UserIdentity
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
                        var claims = new List<Claim>()
                                {
                                    new Claim ("UserName",user.UserName),
                                    new Claim(ClaimTypes.NameIdentifier,user.Id),
                                };
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
                        var token = new JwtSecurityToken(
                            issuer: _config["JWT:ValidIssuer"],
                            audience: _config["JWT:ValidAudience"],
                            claims: claims,
                            expires: DateTime.Now.AddDays(2),
                            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
                        string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

                        return Ok(new { isSuccess = true, token = tokenAsString, user = user });
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

        #region reserPassword

        [HttpPost("resetPassword")]
        public async Task<IActionResult> resetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            // Generate a random reset code (you can customize the length and format)
            string resetCode = GenerateRandomCode(8);

            // Update the user record with the reset code
            user.resetCode = resetCode;
            user.PasswordHash = null;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
              var isSend =  await SendEmail(user);
                if (isSend)
                {
                    return Ok("Email Send Seccessfuly");
                }
                else
                {
                    return BadRequest("Can't Send To This Email");
                }
             
            }
            else
            {
                return StatusCode(500, "Failed to generate reset code");
            }
        }
        private bool CheckResetCode(string resetCode)
        {
            // Check if any user has the given reset code
            var usersWithCode = _userManager.Users.Where(u => u.resetCode == resetCode).ToList();
            return usersWithCode.Any();
        }

        private string GenerateRandomCode(int length)
        {
            // Generate a random code using characters and numbers
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var code = new char[length];
            for (int i = 0; i < length; i++)
            {
                code[i] = chars[random.Next(chars.Length)];
            }

            // Check if the generated code is unique
            if (CheckResetCode(new string(code)))
            {
                // If the code is not unique, generate a new code recursively
                return GenerateRandomCode(length);
            }

            // If the code is unique, return it
            return new string(code);
        }


        private async Task<bool> SendEmail(UserIdentity user)
        {
            try
            {
                var filePath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\EmailTemplate\\resetCode.html";
                var str = new StreamReader(filePath);

                var mailText = str.ReadToEnd();
                str.Close();

                mailText = mailText.Replace("[resetCode]", user.resetCode);

                await _mailingService.SendEmailAsync(user.Email, "Welcome From Platform This Is Reset Code", mailText);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword(string code, string newPassword)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.resetCode == code);

                if (user != null)
                {
                    if (newPassword.Length >= 8)
                    {
                        var result = await _userManager.AddPasswordAsync(user, newPassword);

                        if (result.Succeeded)
                        {
                            return Ok("Password reset successfully.");
                        }
                        else
                        {
                            // Concatenate error messages
                            var errors = string.Join("\n", result.Errors.Select(e => e.Description));
                            return BadRequest($"Failed to reset password: {errors}");
                        }
                    }
                    else
                    {
                        return BadRequest("Password does not meet complexity requirements.");
                    }
                }
                else
                {
                    return BadRequest("Invalid Reset Code.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
