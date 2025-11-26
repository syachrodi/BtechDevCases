using Microsoft.Extensions.Options;
using System.Dynamic;
using TakeHomeAssignment.General.AccountAPI.Model;
using TakeHomeAssignment.General.AccountAPI.Model.General;
using TakeHomeAssignment.General.AccountAPI.Repository.Interfaces;
using TakeHomeAssignment.General.AccountAPI.Utils;
using static TakeHomeAssignment.General.AccountAPI.Model.General.Responses;

namespace TakeHomeAssignment.General.AccountAPI.Repository
{
    public class AuthRepository : IAuthRepository
    {

        public readonly ApplicationSetting _applicationSetting;

        public AuthRepository(IOptions<ApplicationSetting> applicationSetting)
        {
            _applicationSetting = applicationSetting.Value;
        }

        public BasicResponse Login(LoginUserParam param)
        {
            BasicResponse result = new BasicResponse();

            if (!InMemoryUserStore.Users.TryGetValue(param.Email, out var user))
            {
                result.Message = Messages.PasswordConfirmationNotMatch;
                return result;
            }

            if (!PasswordHasher.VerifyPassword(param.Password, user.PasswordHash))
            {
                result.Message = Messages.PasswordConfirmationNotMatch;
                return result;
            }

            string token = TokenUtil.GenerateJwtToken(user, _applicationSetting);
            
            if(!string.IsNullOrEmpty(token))
            {
                SessionStore.UserActivity[param.Email] = DateTime.UtcNow;

                dynamic data = new ExpandoObject();
                data.Token = token;

                result.Status = true;
                result.Data = data;
                result.Message = Messages.LoginSuccess;
            }

            return result;
        }

        public RegisterResponse Register(RegisterUserParam param)
        {
            RegisterResponse result = new RegisterResponse();

            if(param.Password != param.ConfirmPassword)
            {
                result.Message = Messages.PasswordConfirmationNotMatch;
                return result;
            }

            if (InMemoryUserStore.Users.ContainsKey(param.Email))
            {
                result.Message = Messages.EmailAlreadyRegistered;
                return result;
            }

            User userData = new User()
            {
                Id = new Guid(),
                Email = param.Email,
                PasswordHash = PasswordHasher.HashPassword(param.Password)
            };

            InMemoryUserStore.Users.Add(userData.Email, userData);

            result.Status = true;
            result.Message = Messages.UserRegisterSuccess;

            return result;
        }
    }
}
