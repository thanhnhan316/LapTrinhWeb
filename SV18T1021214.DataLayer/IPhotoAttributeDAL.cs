using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV18T1021214.DataLayer
{
    public interface IPhotoAttributeDAL<T> where T : class
    {
        IList<T> List(int productID);
        T Get(int id);
        int Add(T data);
        bool Update(T data);
        bool Delete(int id);
    }
}
