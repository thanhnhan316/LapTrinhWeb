using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV18T1021214.DomainModel;

namespace SV18T1021214.DataLayer
{
    public interface IProductDAL
    {
        List<Product> List(string searchValue = "", int pageSize = 0, int page = 1, int category = 0, int supplier = 0);
        int Count(string searchValue, int category = 0, int supplier = 0);
        Product Get(int id);
        int Add(Product data);
        bool Update(Product data);
        bool Delete(int id);
        bool InUsed(int id);
    }
}
