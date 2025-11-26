namespace TakeHomeAssignment.General.AccountAPI.Model.General
{
    public class ApplicationSetting
    {
        public string JWTValidHours { get; set; }
        public string JWTValidScale { get; set; }
        public string JWTValidMinutes { get; set; }
        public string JWTValidDays { get; set; }
        public string JWTIdleMinutes { get; set; }
    }
}
