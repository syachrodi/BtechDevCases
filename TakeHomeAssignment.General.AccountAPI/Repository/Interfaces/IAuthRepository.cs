using Microsoft.AspNetCore.Identity;
using TakeHomeAssignment.General.AccountAPI.Model.General;
using static TakeHomeAssignment.General.AccountAPI.Model.General.Responses;

namespace TakeHomeAssignment.General.AccountAPI.Repository.Interfaces
{
    public interface IAuthRepository
    {
        RegisterResponse Register(RegisterUserParam param);
        BasicResponse Login(LoginUserParam param);
    }
}
