using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment3_API.Models
{
  public interface IProductsRepository
  {
    void Add<T>(T entity) where T : class; // adding an entity
    void Delete<T>(T entity) where T : class;
    Task<bool> SaveChanges(); // asynchronous function
    Task<Product[]> GetDashboardData(); /// get all the products




  }
}
