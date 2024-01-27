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
        public ICompanyRepository Company { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
        }

        public ICategoryRepository CategoryRepository => throw new NotImplementedException();
        public IProductRepository ProductRepository => throw new NotImplementedException();
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
