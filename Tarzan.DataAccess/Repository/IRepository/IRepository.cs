using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tarzan.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        // T - Category
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T, bool>> filter); // i will write it like this if iam using Linq in my function 
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
