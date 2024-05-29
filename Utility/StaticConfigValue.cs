namespace SQMS.Utility
{
    public static class StaticConfigValue
    {
        public static string conn { get; private set; }
        public static string connReadWrite { get; private set; }
        public static string connSRDA { get; private set; }
        public static string displayPath { get; private set; }
        public static string displayWelcome { get; private set; } 
        public static string displayVideo { get; private set; }
        public static string voiceText { get; private set; }
        public static string speakLanguage { get; private set; }
        public static string speakGender { get; private set; }
        public static string speakRate { get; private set; }
        public static string displayWhenEmptyToken { get; private set; }
        public static string galleryDefaultPath { get; private set; }
        public static int requiredPasswordLength { get; private set; }
        public static int maxFieldRequiredBeforeLogout { get; private set; }
        public static int PasswordExpiredAfter { get; private set; }
        public static int PasswordExpiryNotifyBefore { get; private set; }
        public static int PasswordLastCheckingCount { get; private set; }
        public static bool AllowActiveDirectoryUser { get; private set; }
        public static bool UserEmailRequired { get; private set; }
        public static string ActiveDirectoryInfo { get; private set; }
        public static bool AllowSignalRLoggin { get; private set; }
        public static int AppRequestTimeOut { get; private set; }
        public static string ADMAPI { get; private set; }
        public static string BlAuthenticationAPI { get; private set; }
        public static string ApplicationName { get; private set; }
        public static string ApplicationKey { get; private set; }
        public static string ErrorLogFilePath { get; private set; }
        public static string lms_api_url { get; private set; }
        public static string lms_channel { get; private set; }
        public static string lms_transactionId { get; private set; }
        public static string default_token_prfx { get; private set; }
        public static string default_Db_path { get; private set; }

          
        public static string GetConnectionStringRead()
        {
            if (String.IsNullOrEmpty(conn))
            {
                conn = ConfigurationValues.GetConnectionStringRead();
            }
            return conn;
        }
        public static string GetConnectionStringReadWrite()
        {
            if (String.IsNullOrEmpty(connReadWrite))
            {
                connReadWrite = ConfigurationValues.GetConnectionStringReadWrite();
            }
            return connReadWrite;
        }
        public static string GetConnectionStringSRDA()
        {
            if (String.IsNullOrEmpty(connSRDA))
            {
                connSRDA = ConfigurationValues.GetConnectionStringSRDA();
            }
            return connSRDA;
        }
        public static string GetDisplayPath()
        {
            if (String.IsNullOrEmpty(displayPath))
            {
                displayPath = ConfigurationValues.GetDisplayPath();
            }
            return displayPath;
        }
        public static string GetDisplayWelcome()
        {
            if (String.IsNullOrEmpty(displayWelcome))
            {
                displayWelcome = ConfigurationValues.GetDisplayWelcome();
            }
            return displayWelcome;
        }
        public static string GetDisplayVideo()
        {
            if (String.IsNullOrEmpty(displayVideo))
            {
                displayVideo = ConfigurationValues.GetDisplayVideo();
            }
            return displayVideo;
        }
        public static string GetVoiceText()
        {
            if (String.IsNullOrEmpty(voiceText))
            {
                voiceText = ConfigurationValues.GetVoiceText();
            }
            return voiceText;
        }
        public static string GetSpeakLanguage()
        {
            if (String.IsNullOrEmpty(speakLanguage))
            {
                speakLanguage = ConfigurationValues.GetSpeakLanguage();
            }
            return speakLanguage;
        }
        public static string GetSpeakGender()
        {
            if (String.IsNullOrEmpty(speakGender))
            {
                speakGender = ConfigurationValues.GetSpeakGender();
            }
            return speakGender;
        }
        public static string GetSpeakRate()
        {
            if (String.IsNullOrEmpty(speakRate))
            {
                speakRate = ConfigurationValues.GetSpeakRate();
            }
            return speakRate;
        }
        public static string GetDisplayWhenEmptyToken()
        {
            if (String.IsNullOrEmpty(displayWhenEmptyToken))
            {
                displayWhenEmptyToken = ConfigurationValues.GetDisplayWhenEmptyToken();
            }
            return displayWhenEmptyToken;
        }
        public static string GetGalleryDefaultPath()
        {
            if (String.IsNullOrEmpty(galleryDefaultPath))
            {
                galleryDefaultPath = ConfigurationValues.GetGalleryDefaultPath();
            }
            return galleryDefaultPath;
        }

        public static string GetGalleryDBPath()
        {
            if (String.IsNullOrEmpty(default_Db_path))
            {
                default_Db_path = ConfigurationValues.GetGalleryDBPath();
            }
            return default_Db_path;
        }

        public static int GetRequiredPasswordLength()
        {
            if (requiredPasswordLength <= 0)
            {
                requiredPasswordLength = ConfigurationValues.GetPasswordRequiredLength();
            }
            return requiredPasswordLength;
        }
        public static int GetMaxFailedAccessAttemptsBeforeLockout()
        {
            if (maxFieldRequiredBeforeLogout <= 0)
            {
                maxFieldRequiredBeforeLogout = ConfigurationValues.GetMaxFailedAccessAttemptsBeforeLockout();
            }
            return maxFieldRequiredBeforeLogout;
        }
        public static int GetPasswordExpiredAfter()
        {
            if (PasswordExpiredAfter <= 0)
            {
                PasswordExpiredAfter = ConfigurationValues.GetPasswordExpiredAfter();
            }
            return PasswordExpiredAfter;
        }
        public static int GetPasswordExpiryNotifyBefore()
        {
            if (PasswordExpiryNotifyBefore <= 0)
            {
                PasswordExpiryNotifyBefore = ConfigurationValues.GetPasswordExpiryNotifyBefore();
            }
            return PasswordExpiryNotifyBefore;
        }
        public static int GetPasswordLastCheckingCount()
        {
            if (PasswordLastCheckingCount <= 0)
            {
                PasswordLastCheckingCount = ConfigurationValues.GetPasswordLastCheckingCount();
            }
            return PasswordLastCheckingCount;
        }
        public static bool GetAllowActiveDirectoryUser()
        {
            AllowActiveDirectoryUser = ConfigurationValues.GetAllowActiveDirectoryUser();

            return AllowActiveDirectoryUser;
        }
        public static bool GetUserEmailRequired()
        {
            UserEmailRequired = ConfigurationValues.GetUserEmailRequired();

            return UserEmailRequired;
        }
        public static string GetActiveDirectoryInfo()
        {
            if (String.IsNullOrEmpty(ActiveDirectoryInfo))
            {
                ActiveDirectoryInfo = ConfigurationValues.GetActiveDirectoryInfo();
            }
            return ActiveDirectoryInfo;
        }
        public static bool GetAllowSignalRLoggin()
        {
            AllowSignalRLoggin = ConfigurationValues.GetAllowSignalRLoggin();

            return AllowSignalRLoggin;
        }
        public static int GetAppRequestTimeOut()
        {
            if (AppRequestTimeOut <= 0)
            {
                AppRequestTimeOut = ConfigurationValues.GetAppRequestTimeOut();
            }
            return AppRequestTimeOut;
        }
        public static string GetADMAPI()
        {
            if (String.IsNullOrEmpty(ADMAPI))
            {
                ADMAPI = ConfigurationValues.GetADMAPI();
            }
            return ADMAPI;
        }   
        public static string GetBlAuthenticationAPI()
        {
            if (String.IsNullOrEmpty(BlAuthenticationAPI))
            {
                BlAuthenticationAPI = ConfigurationValues.GetBlAuthenticationAPI();
            }
            return BlAuthenticationAPI;
        }
        public static string GetApplicationName()
        {
            if (String.IsNullOrEmpty(ApplicationName))
            {
                ApplicationName = ConfigurationValues.GetApplicationName();
            }
            return ApplicationName;
        }
        public static string GetApplicationKey()
        {
            if (String.IsNullOrEmpty(ApplicationKey))
            {
                ApplicationKey = ConfigurationValues.GetApplicationKey();
            }
            return ApplicationKey;
        }
        public static string GetErrorLogFilePath()
        {
            if (String.IsNullOrEmpty(ErrorLogFilePath))
            {
                ErrorLogFilePath = ConfigurationValues.GetErrorLogFilePath();
            }
            return ErrorLogFilePath;
        }
        public static string GetLMSApiUrl()
        {
            if (String.IsNullOrEmpty(lms_api_url))
            {
                lms_api_url = ConfigurationValues.GetLMSAPIUrl();
            }
            return lms_api_url;
        }

        public static string GetLMSChannel()
        {
            if (String.IsNullOrEmpty(lms_channel))
            {
                lms_channel = ConfigurationValues.GetLmsChannel();
            }
            return lms_channel;
        }
        public static string GetLMSTransactionId()
        {
            if (String.IsNullOrEmpty(lms_transactionId))
            {
                lms_transactionId = ConfigurationValues.GetLmsTransactionId();
            }
            return lms_transactionId;
        }
        public static string GetDefault_token_prfx()
        {
            if (String.IsNullOrEmpty(default_token_prfx))
            {
                default_token_prfx = ConfigurationValues.GetDefaultTokenPrfx();
            }
            return default_token_prfx;
        }
    }
}
