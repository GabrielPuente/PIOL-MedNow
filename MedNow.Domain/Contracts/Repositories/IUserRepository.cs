using MedNow.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace MedNow.Domain.Contracts.Repositories
{
    public interface IUserRepository
    {
        Task CreateUser(User user);

        Task<User> GetByEmail(string email);
        
        Task<User> GetById(Guid Id);
    }
}
