using System.Threading.Tasks;
using GoogleSheetI18n.Core.Models.User;

namespace GoogleSheetI18n.Infrastructure.Services.UserService
{
    public interface IUserService
    {
        Task<bool> ValidateCredentials(string userName, string passWord, out User user);
    }
}
