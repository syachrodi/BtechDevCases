namespace TakeHomeAssignment.General.AccountAPI.Utils
{
    public static class SessionStore
    {
        // email → last activity timestamp
        public static Dictionary<string, DateTime> UserActivity = new();
    }
}
