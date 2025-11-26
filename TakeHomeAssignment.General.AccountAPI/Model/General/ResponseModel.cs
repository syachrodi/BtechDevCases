using TakeHomeAssignment.General.AccountAPI.Utils;

namespace TakeHomeAssignment.General.AccountAPI.Model.General
{
    public class BaseResponseModel
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public Error Error { get; set; }
    }

    public class Responses
    {
        public class RegisterResponse : BaseResponseModel { }

        public class BasicResponse : BaseResponseModel
        {
            public dynamic Data { get; set; }
        }
    }
}
