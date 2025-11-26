using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using static TakeHomeAssignment.General.AccountAPI.Model.General.Responses;
using System.Text;
using TakeHomeAssignment.General.AccountAPI.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TakeHomeAssignment.General.AccountAPI.Controllers
{
    [Authorize, EnableCors("AllowOrigin")]
    public class BaseController : ControllerBase
    {
        internal static JsonResult GenerateJsonResultInvalidParameter(ModelStateDictionary modelObj)
        {
            //_logger.Error(ErrorHandling.GetFunctionErrorMessage(Errors.InvalidParameter));
            string validationMessage = string.Empty;

            StringBuilder _stringBuilder = new StringBuilder();
            foreach (var ms in modelObj.Values)
            {
                foreach (var err in ms.Errors)
                {
                    _stringBuilder.Append(err.ErrorMessage + ";");
                }
            }
            validationMessage = _stringBuilder.ToString();

            if (string.IsNullOrEmpty(validationMessage))
            {
                validationMessage = Messages.InvalidParameter;
            }

            return new JsonResult(
                new BasicResponse
                {
                    Status = false,
                    Message = validationMessage
                }
            );
        }
    }
}
