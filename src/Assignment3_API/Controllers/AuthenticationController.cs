using Assignment3_API.Models;
using Assignment3_API.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3_API.Controllers
{ 
  [Route("api/[controller]")]
  [ApiController]
  public class AuthenticationController : ControllerBase
  {
    private readonly IProductsRepository _productRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserClaimsPrincipalFactory<AppUser> _claimsPrincipalFactory;
    private readonly IConfiguration _configuration;

    public AuthenticationController(IProductsRepository productRepository, UserManager<AppUser> userManager, IUserClaimsPrincipalFactory<AppUser> claimsPrincipalFactory, IConfiguration configuration)
    {
      _productRepository = productRepository;
      _userManager = userManager;
      _claimsPrincipalFactory = claimsPrincipalFactory;
      _configuration = configuration;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> registerUser(UserViewModel userView)
    {
      var user = await _userManager.FindByNameAsync(userView.Email);

      if(user == null)
      {
        user = new AppUser
        {
          Id = Guid.NewGuid().ToString(),
          UserName = userView.Email,
          Email = userView.Email,

        };

        var result = await _userManager.CreateAsync(user, userView.Password);

        if(result.Errors.Count() > 0)
        {
          StatusCode(StatusCodes.Status500InternalServerError, "Internal error occured. Please contact support");
        }
      }
      else
      {
        return StatusCode(StatusCodes.Status403Forbidden, "Account already exists");
      }

      return Ok("Account created successfully");
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> loginUser(UserViewModel userView)
    {
      var user = await _userManager.FindByNameAsync(userView.Email);

      if (user != null && await _userManager.CheckPasswordAsync(user, userView.Password))
      {
        try
        {
          var principal = await _claimsPrincipalFactory.CreateAsync(user);
          await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);

          return GenerateToken(user);


        }
        catch (Exception)
        {
          return StatusCode(StatusCodes.Status500InternalServerError, "Internal error occured. Please contact support");
        }
      }
      else
      {
        return NotFound("Does not exist");
      }
    }

    [HttpGet]
    private ActionResult GenerateToken(AppUser appUser)
    {
      var claims = new[]
      {
        new Claim(JwtRegisteredClaimNames.Sub, appUser.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.UniqueName, appUser.UserName)
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
      var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
          _configuration["Tokens:Issuer"],
          _configuration["Tokens:Audience"],
          claims,
          signingCredentials: credentials,
          expires: DateTime.UtcNow.AddHours(3)
     );

      return Created("", new
      {
        token = new JwtSecurityTokenHandler().WriteToken(token),
        expiration = token.ValidTo

      });
    }

  }
}
