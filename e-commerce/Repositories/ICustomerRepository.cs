using System.Collections.Generic;
using ecom.Entities;

namespace ecom.Repositories
{
    public interface ICustomerRepository
    {
        Customers GetCustomer(string uname);
        void AddCustomer(Customers customer);
    }
}