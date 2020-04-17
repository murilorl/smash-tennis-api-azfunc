namespace App.Service.Auth
{
    public enum PasswordVerificationResult
    {
        Failed,
        Success,
        SuccessRehashNeeded,
    }
}