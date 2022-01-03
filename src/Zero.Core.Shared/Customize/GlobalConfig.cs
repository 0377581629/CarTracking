namespace Zero
{
    public static class GlobalConfig
    {
        #region Areas Layout
        public static string AppName { get; set; }
        public static string AppFooter { get; set; }
        public static string AppDescription {get; set; }
        public static string AppKeyword {get; set; }
        public static string AppAuthor {get; set; }
        public static string AppFaviconName {get; set; }
        
        public static string AppDefaultLogo {get; set; }
        #endregion
        
        #region Account Layout
        public static string AppLoginTitle {get; set; }
        public static string AppLoginSubtitle {get; set; }
        public static string AppDefaultLogoLogin {get; set; }
        public static string AppDefaultBackgroundLogin {get; set; }
        public static string AppDefaultBackgroundLoginColor { get; set; }
        public static string AppDefaultBackgroundSize { get; set; }
        #endregion
        
        #region Menu Logo
        public static bool UseMenuLogo { get; set; }
        public static string AppDefaultMenuLogo {get; set; }
        #endregion
        
        #region Upload, Import
        public static string AppPhysPath { get; set; } = "";
        public static string UploadPath { get; set; } = "uploads";
        public static uint MaxUploadFileSize = 3000145728; // 3000MBS
        public static string ImportTemplatePath = "";
        public static string ImportSampleFolders = $"wwwroot/SampleFiles/";
        #endregion
        
        #region Others

        public static string DefaultImageUrl { get; set; } = "";

        #endregion
    }
}