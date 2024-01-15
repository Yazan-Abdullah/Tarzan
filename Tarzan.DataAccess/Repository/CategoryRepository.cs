using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tarzan.DataAccess.Data;
using Tarzan.DataAccess.Repository.IRepository;
using Tarzan.Models;

namespace Tarzan.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category> , ICategoryRepository
    {
        private ApplicationDbContext _db;
        public CategoryRepository (ApplicationDbContext db) : base (db)
        {
            _db = db;
        }

        public void Update(Category obj)
        {
            _db.Categories.Update(obj); 
        }
    }
}
