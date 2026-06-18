namespace EventPortal.Services
{
    public class RegistrationResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public static RegistrationResult Success() => new RegistrationResult { IsSuccess = true };
        public static RegistrationResult Fail(string message) => new RegistrationResult { IsSuccess = false, ErrorMessage = message };
    }
}
