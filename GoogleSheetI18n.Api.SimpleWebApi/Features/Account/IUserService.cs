using System.Threading.Tasks;

namespace GoogleSheetI18n.Api.SimpleWebApi.Features.Account
{
    public interface IUserService
    {
        Task<bool> ValidateCredentials(string userName, string passWord, out User user);
    }
}
