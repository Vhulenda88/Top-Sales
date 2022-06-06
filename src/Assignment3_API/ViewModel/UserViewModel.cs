using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment3_API.ViewModel
{
  public class UserViewModel
  {
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    public string Password { get; set; }

  }
}
