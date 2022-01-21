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
        
        #region File Manager
        public static bool UseFileServer { get; set; }
        
        public static string MinioEndPoint { get; set; }
        
        public static string MinioRootBucketName { get; set; }
        
        public static string MinioAccessKey { get; set; }
        
        public static string MinioSecretKey { get; set; }
        
        public static string MinioRegion { get; set; }
        #endregion
        
        #region Email Validation
        public static bool UseEmailValidation { get; set; }
        public static string EmailValidationEndPoint { get; set; }
        public static string EmailValidationApiKey { get; set; }
        #endregion
    }
}