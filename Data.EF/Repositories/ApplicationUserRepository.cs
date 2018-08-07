using Domain;
using Domain.Aggregates.References;
using Domain.Interface.Repositories;
using System;
using System.Threading.Tasks;

namespace Data.EF.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly DatabaseContext context;

        public ApplicationUserRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public Task<Result> AddLoginAsync(string userId, string login)
        {
            throw new NotImplementedException();
        }

        public Task<Result> AddPasswordAsync(string userId, string password)
        {
            throw new NotImplementedException();
        }

        public Task<Result> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<Result> CreateAsync(ApplicationUserAggregate user)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUserAggregate> FindAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUserAggregate> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}