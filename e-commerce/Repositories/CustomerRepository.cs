using System.Linq;
using System.Collections.Generic;
using ecom.Entities;

namespace ecom.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private StoreDbContext _context;

        public CustomerRepository(StoreDbContext context)
        {
            _context = context;
        }

        public Customers GetCustomer(string uname)
        {
            return _context.Customers.Where(w => w.Uname == uname).FirstOrDefault();
        }

        public void AddCustomer(Customers customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }
    }
}