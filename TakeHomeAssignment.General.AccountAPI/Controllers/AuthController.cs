using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TakeHomeAssignment.General.AccountAPI.Model.General;
using TakeHomeAssignment.General.AccountAPI.Repository.Interfaces;
using TakeHomeAssignment.General.AccountAPI.Utils;
using static TakeHomeAssignment.General.AccountAPI.Model.General.Responses;

namespace TakeHomeAssignment.General.AccountAPI.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthRepository _repo;

        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }

        [AllowAnonymous]
        [HttpPost, Route("Register")]
        public JsonResult Register([FromBody] RegisterUserParam form)
        {
            RegisterResponse result = new RegisterResponse();

            if (!ModelState.IsValid)
            {
                return GenerateJsonResultInvalidParameter(ModelState);
            }

            try
            {
                result = _repo.Register(form);
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = Messages.UnhandledException;
                result.Error = Errors.GetUnhandledExceptionError(e.ToString());
            }

            return new JsonResult(result);
        }

        [AllowAnonymous]
        [HttpPost, Route("Login")]
        public JsonResult Login([FromBody] LoginUserParam form)
        {
            BasicResponse result = new BasicResponse();

            if (!ModelState.IsValid)
            {
                return GenerateJsonResultInvalidParameter(ModelState);
            }

            try
            {
                result = _repo.Login(form);
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Data = null;
                result.Message = Messages.UnhandledException;
                result.Error = Errors.GetUnhandledExceptionError(e.ToString());
            }

            return new JsonResult(result);
        }

        [HttpPost, Route("Me")]
        public JsonResult Me()
        {
            BasicResponse result = new BasicResponse();

            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            result.Message = string.Format("Hello {0}, welcome back", email);
            result.Status = true;

            return new JsonResult(result);
        }
    }
}
