

using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment3_API.Models
{
  public class ProductsRepository : IProductsRepository
  {
    private readonly AppDbContext _appDbContext;

    public ProductsRepository(AppDbContext appDbContext)
    {
      _appDbContext = appDbContext; // dependency injection
    }

    public void Add<T>(T entity) where T : class
    {
      _appDbContext.Add(entity);
    }



    public void Delete<T>(T entity) where T : class
    {
      _appDbContext.Remove(entity);
    }

    public async Task<Product[]> GetDashboardData()
    {
      IQueryable<Product> query = _appDbContext.Products;//.Where(p => p.Brand != null && p.ProductType != null);
      return await query.ToArrayAsync();
    }


    public async Task<bool> SaveChanges()
    {
      return await _appDbContext.SaveChangesAsync() > 0;
    }
  }
}
