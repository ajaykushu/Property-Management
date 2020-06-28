namespace API.Controllers
{
    internal static class StringConstants
    {
        public static string PwdChanged = "Successfully Updated.";
        public static string EmailNotFound = "Email Not Found.";
        public static string Error = "Some Error Occured";
        public static string EmailSent = "Email sent.";
        public static string WrongCredentials = "Email not found or wrong credentials.";
        public static string ExceptionMsg = "An Exception Has Occured.";
        public static string TokenForgotPwd = "An Exception has occured, Token not verified.";
        public static string SystemError = "An Exception has occured, Internal server error.";
        public static string TokenForgotCmpt = "An Exception has occured, while processing token forgot method.";
        public static string VerifyEmailToken = "An Exception has occured, Verify.";
        public static string ClaimsVerified = "An Exception has occured.";
        public static string PwdChangeStrt = "An Exception has occured.";
        public static string PwdChangeCmpt = "An Exception has occured.";

        public static string LoggerStarts { get; internal set; }
        public static object LoggerStopped { get; internal set; }
    }
}