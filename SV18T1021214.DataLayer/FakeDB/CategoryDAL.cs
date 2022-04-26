using SV18T1021214.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SV18T1021214.DataLayer.FakeDB
{
    public class CategoryDAL : ICommonDAL<Category>
    {
        public int Add(Category data)
        {
            throw new NotImplementedException();
        }

        public int Count(string searchValue)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int CategoryID)
        {
            throw new NotImplementedException();
        }

        public Category Get(int categoryID)
        {
            throw new NotImplementedException();
        }

        public bool InUsed(int CategoryID)
        {
            throw new NotImplementedException();
        }

        public IList<Category> List(int page, int pageSize, string searchValue)
        {
            throw new NotImplementedException();
        }

        public bool Update(Category data)
        {
            throw new NotImplementedException();
        }
    }
}
