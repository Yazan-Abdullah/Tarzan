using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tarzan.DataAccess.Data;
using Tarzan.DataAccess.Repository.IRepository;

namespace Tarzan.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
        }

        public ICategoryRepository CategoryRepository => throw new NotImplementedException();

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
