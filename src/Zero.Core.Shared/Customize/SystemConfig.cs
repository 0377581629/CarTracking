namespace Zero
{
    public static class SystemConfig
    {
        /// <summary>
        /// Default user password. Used for reset password function
        /// </summary>
        public static string DefaultPassword { get; set; } = "123qwe";

        public static bool DisableMailService { get; set; } = false;
        
        public static string LogIndex = "";
    }
}