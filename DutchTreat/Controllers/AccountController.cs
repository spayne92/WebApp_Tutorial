using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;

namespace DutchTreat.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<StoreUser> _signInManager;
        private readonly UserManager<StoreUser> _userManager;
        private readonly IConfiguration _config;

        public AccountController(ILogger<AccountController> logger, SignInManager<StoreUser> signInManager,
            UserManager<StoreUser> userManager, IConfiguration config)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }

        public IActionResult Login()
        {
            // Checks if user is already logged in.
            if (this.User.Identity.IsAuthenticated)
            {
                // Redirects to home page if already logged in.
                return RedirectToAction("Index", "App");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Allows logging in without retreiving User entity.  
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, 
                    model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        Redirect(Request.Query["ReturnUrl"].First());
                    }
                    return RedirectToAction("Shop", "App");
                }
            }

            // First parameter blank, because issue with entire model, not specific prop.
            ModelState.AddModelError("", "Failed to login.");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "App");
        }

        // API specific token credentials. Uses base REST calls.
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);

                if (user != null)
                {
                    // Checks password without doing anything else.
                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                    if (result.Succeeded)
                    {
                        // Ties info of current API user to stored identity user for tokenization.
                        var claims = new[]
                        {
                            // Sets claim's subject to the user's email.
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            // Creates new unique string to represent each token.
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            // Maps a unique name for the identity available in all controllers (and views).
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                        };

                        // Can append standard user claims from the system to add them into the token.

                        // Creates credentials from key string in the config, so it can be set differently later.
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        // Building token from claims and credentials.
                        var token = new JwtSecurityToken(
                            _config["Tokens:Issuer"],   // Who issued token?
                            _config["Tokens:Audience"], // Who can use the token?
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(30),
                            signingCredentials: credentials
                        );

                        // Anonymous model object used.
                        var results = new
                        {
                            // Creates serialized token string.
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        // No URI returned, because record is not stored.
                        return Created("", results);
                    }
                }
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenNew([FromBody]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);

                if (user != null)
                {
                    // Checks password without doing anything else.
                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                    if (result.Succeeded)
                    {
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.UTF8.GetBytes(_config["Tokens:Key"]);

                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                                // Sets claim's subject to the user's email.
                                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                                // Creates new unique string to represent each token.
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                // Maps a unique name for the identity available in all controllers (and views).
                                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                            }),

                            Expires = DateTime.UtcNow.AddMinutes(60),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                            Audience = _config["Tokens:Audience"],
                            Issuer = _config["Tokens:Issuer"]
                        };

                        var token = tokenHandler.CreateToken(tokenDescriptor);

                        // Anonymous model object used.
                        var returnModel = new
                        {
                            // Creates serialized token string.
                            token = tokenHandler.WriteToken(token),
                            expiration = token.ValidTo
                        };

                        // No URI returned, because record is not stored.
                        return Created("", returnModel);
                    }
                }
            }

            return BadRequest();
        }
    }
}
