using Assignment3_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment3_API.Factory
{
  public class AppUserClaimsPrincipleFactory: UserClaimsPrincipalFactory<AppUser,IdentityRole>
  {
    public AppUserClaimsPrincipleFactory(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> optionsAccesor): base(userManager, roleManager, optionsAccesor)
    {

    }
  }
}
