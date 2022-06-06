using Assignment3_API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment3_API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    private readonly IProductsRepository _productRepository;

    public ProductController(IProductsRepository productRepository)
    {
      _productRepository = productRepository;
    }

    [HttpGet]
    [Route("dashboarddata")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> getDashBoardData()// get the products from the db
    {
      try
      {

        var products = await _productRepository.GetDashboardData();
        return Ok(products);
      }
      catch (Exception)
      {
        throw;
      }
    }

    
  }
}
