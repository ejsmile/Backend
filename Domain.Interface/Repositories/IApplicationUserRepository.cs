using Domain.Aggregates.References;
using System.Threading.Tasks;

namespace Domain.Interface.Repositories
{
    public interface IApplicationUserRepository
    {
        Task<Result> CreateAsync(ApplicationUserAggregate user);

        Task<ApplicationUserAggregate> FindAsync(string userName, string password);

        Task<ApplicationUserAggregate> FindByIdAsync(string userId);

        Task<Result> ChangePasswordAsync(string userId, string oldPassword, string newPassword);

        Task<Result> AddPasswordAsync(string userId, string password);

        Task<Result> AddLoginAsync(string userId, string login);
    }
}