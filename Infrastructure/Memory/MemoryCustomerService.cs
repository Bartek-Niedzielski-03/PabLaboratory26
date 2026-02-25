using AppCore.Interfaces;
using AppCore.Models;

namespace Infrastructure.Memory;

public class MemoryCustomerService : ICustomerService
{
    public IEnumerable<Customer> GetCustomers()
    {
        return
        [
            new Customer()
            {
                Id = 1,
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "a@mpa.pl",
                Phone = "111-111-111",
                AddressId = 11
            },
            new Customer()
            {
                Id = 1,
                FirstName = "Janusz",
                LastName = "Nowak",
                Email = "dn@opa.pl",
                Phone = "222-111-111",
                AddressId = 22
            }
        ];
    }

    public Task<IEnumerable<Customer>> GetCustomersAsync()
    {
        throw new NotImplementedException();
    }
}