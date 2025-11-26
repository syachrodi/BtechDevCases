using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace TakeHomeAssignment.General.AccountAPI.Utils
{
    public static class ErrorHandling
    {
        internal static string GetFunctionBeginMessage(params string[] AdditionalMessage)
        {
            return new StringBuilder().AppendFormat(
                "{0} Function Start: {1} {2}.",
                new StackTrace().GetFrame(1).GetMethod().Name,
                DateTime.Now,
                AdditionalMessage.IsNullOrEmpty() ? "" : string.Concat("Additional Info: ", JsonConvert.SerializeObject(AdditionalMessage))
            ).ToString();
        }

        internal static string GetFunctionErrorMessage(Error e, params string[] AdditionalMessage)
        {
            return new StringBuilder().AppendFormat(
                "{0} Function Error: {1} {2} {3}.",
                new StackTrace().GetFrame(1).GetMethod().Name,
                DateTime.Now,
                JsonConvert.SerializeObject(e),
                AdditionalMessage.IsNullOrEmpty() ? "" : string.Concat("Additional Info: ", JsonConvert.SerializeObject(AdditionalMessage))
            ).ToString();
        }

        internal static string GetFunctionDebugMessage(params string[] AdditionalMessage)
        {
            return new StringBuilder().AppendFormat(
                "{0} Function Debug: {1} {2}.",
                new StackTrace().GetFrame(1).GetMethod().Name,
                DateTime.Now,
                AdditionalMessage.IsNullOrEmpty() ? "" : string.Concat("Additional Info: ", JsonConvert.SerializeObject(AdditionalMessage))
            ).ToString();
        }

        internal static string GetFunctionSuccessMessage(params string[] AdditionalMessage)
        {
            return new StringBuilder().AppendFormat(
                "{0} Function OK: {1} {2}.",
                new StackTrace().GetFrame(1).GetMethod().Name,
                DateTime.Now,
                AdditionalMessage.IsNullOrEmpty() ? "" : string.Concat("Additional Info: ", JsonConvert.SerializeObject(AdditionalMessage))
            ).ToString();
        }
    }

    public class Error
    {
        public Error(string ErrorDescription, int errorCode)
        {

            this.ErrorDescription = ErrorDescription;

            ErrorCode = string.Format("{0}{1}", "E", string.Concat("0000", errorCode.ToString()).PadRight(5));

        }

        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
    }



    public static class Errors
    {
        #region API Action Errors
        public static readonly Error SuccessNonError = new Error("Success", 0); //0
        public static readonly Error UnhandledException = new Error("Unhandled Exception Occured.", 1); //1
        public static readonly Error EmptyParameter = new Error("Empty Parameter.", 2);//2
        public static readonly Error InvalidParameter = new Error("Invalid Parameter.", 3);//3
        public static readonly Error DataNotFound = new Error("Data Not Found.", 4);//4
        #endregion

        internal static Error GetUnhandledExceptionError(string message) { return new Error(message, 1); }
        internal static Error GetUnhandledExceptionError() { return new Error("Please contact IT Service Desk to see detail error.", 1); }

    }

    public static class Messages
    {
        public static readonly string Success = "Success";
        public static readonly string Failed = "Failed";
        public static readonly string UnhandledException = "Unhandled Exception Occured";
        public static readonly string EmptyParameter = "Parameter Is Null";
        public static readonly string InvalidParameter = "Invalid Parameter";
        public static readonly string PasswordConfirmationNotMatch = "Password not match";
        public static readonly string EmailAlreadyRegistered = "Email already registered";
        public static readonly string UserRegisterSuccess = "Success register user";
        public static readonly string LoginSuccess = "Login success";
    }
}
