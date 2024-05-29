namespace SQMS.Utility
{
    public static class ConfigurationValues
    {
        static IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
        public static string GetConnectionStringRead()
        {
            string connectionString = String.Empty;
            try
            {
                connectionString = configuration.GetSection("ConnectionStrings:sqmsConnectionStringRead").Value;
                connectionString = AESCryptography.DecryptAES(connectionString);
            }
            catch (Exception)
            {
                throw new Exception("Key 'sqmsConnectionStringRead' not found in appsettings.json file.");
            }
            return connectionString; 
        }
        public static string GetConnectionStringReadWrite()
        {
            string connectionStringReadWrite = String.Empty;
            try 
            {
                connectionStringReadWrite = configuration.GetSection("ConnectionStrings:sqmsConnectionStringReadAndWrite").Value;
                connectionStringReadWrite = AESCryptography.DecryptAES(connectionStringReadWrite);
            }
            catch (Exception)
            {
                throw new Exception("Key 'sqmsConnectionString' not found in appsettings.json file.");
            }
            return connectionStringReadWrite;
        } 

        public static string GetConnectionStringSRDA()
        {
            string connectionStringSRDA = String.Empty;
            try
            {
                connectionStringSRDA = configuration.GetSection("ConnectionStrings:SRDADBConnectionString").Value;
                connectionStringSRDA = AESCryptography.DecryptAES(connectionStringSRDA);
            }
            catch (Exception)
            {
                throw new Exception("Key 'SRDADBConnectionString' not found in appsettings.json file.");
            }
            return connectionStringSRDA;
        }
        public static string GetDisplayPath()
        {
            string displayPath = String.Empty;
            try
            {
                displayPath = configuration.GetSection("AppSettings:DisplayPath").Value;
            }
            catch (Exception)
            {
                throw new Exception("Key 'displayPath' not found in appsettings.json file.");
            }
            return displayPath;
        }
        public static string GetDisplayWelcome()
        {
            string displayWelcome = String.Empty;
            try
            {
                displayWelcome = configuration.GetSection("AppSettings:dispalyWelcome").Value;
            }
            catch (Exception)
            {
                throw new Exception("Key 'dispalyWelcome' not found in appsettings.json file.");
            }
            return displayWelcome;
        }

        public static string GetDisplayVideo()
        {
            string displayvideo = String.Empty;
            try
            {
                displayvideo = configuration.GetSection("AppSettings:dispalyVideo").Value;
            }
            catch (Exception)
            {
                throw new Exception("Key 'dispalyVideo' not found in appsettings.json file.");
            }
            return displayvideo;
        }

        public static string GetVoiceText()
        {
            string getVoiceText = String.Empty;
            try
            {
                getVoiceText = configuration.GetSection("AppSettings:voiceText").Value;
            }
            catch (Exception)
            {
                throw new Exception("Key 'voiceText' not found in appsettings.json file.");
            }
            return getVoiceText;
        }
        public static string GetSpeakLanguage()
        {
            string getSpeakLanguage = String.Empty;
            try
            {
                getSpeakLanguage = configuration.GetSection("AppSettings:speakLanguage").Value;
            }
            catch (Exception)
            {
                throw new Exception("Key 'speakLanguage' not found in appsettings.json file.");
            }
            return getSpeakLanguage;
        }
        public static string GetSpeakGender()
        {
            string getSpeakGender = String.Empty;
            try
            {
                getSpeakGender = configuration.GetSection("AppSettings:speakGender").Value;
            }
            catch (Exception)
            {
                throw new Exception("Key 'speakGender' not found in appsettings.json file.");
            }
            return getSpeakGender;
        }
        public static string GetSpeakRate()
        {
            string getSpeakRate = String.Empty;
            try
            {
                getSpeakRate = configuration.GetSection("AppSettings:speakRate").Value;
            }
            catch (Exception)
            {
                throw new Exception("Key 'speakRate' not found in appsettings.json file.");
            }
            return getSpeakRate;
        }
        public static string GetDisplayWhenEmptyToken()
        {
            string displayWhenEmptyToken = String.Empty;
            try
            {
                displayWhenEmptyToken = configuration.GetSection("AppSettings:DisplayWhenEmptyToken").Value;
            }
            catch (Exception)
            {
                throw new Exception("Key 'DisplayWhenEmptyToken' not found in appsettings.json file.");
            }
            return displayWhenEmptyToken;
        }
        public static string GetGalleryDefaultPath()
        {
            string galleryDefaultPath = String.Empty;
            try
            {
                galleryDefaultPath = configuration.GetSection("AppSettings:galleryDefaultPath").Value;
            }
            catch (Exception)
            {
                throw new Exception("Key 'galleryDefaultPath' not found in appsettings.json file.");
            }
            return galleryDefaultPath;
        } 

        public static string GetGalleryDBPath()  
        {
            string galleryDbPath = String.Empty;
            try
            {
                galleryDbPath = configuration.GetSection("AppSettings:galleryDBPath").Value;
            }
            catch (Exception)
            {
                throw new Exception("Key 'galleryDBPath' not found in appsettings.json file.");
            }
            return galleryDbPath;
        }
        public static int GetPasswordRequiredLength()
        {
            int requiredPassLength = 0;
            try
            {
                requiredPassLength = Convert.ToInt32(configuration.GetSection("AppSettings:passwordRequiredLength").Value);
            }
            catch (Exception)
            {
                throw new Exception("Key 'passwordRequiredLength' not found in appsettings.json file.");
            }
            return requiredPassLength;
        }
        public static int GetMaxFailedAccessAttemptsBeforeLockout()
        {
            int maxFieldAccessBeforeLogout = 0;
            try
            {
                maxFieldAccessBeforeLogout = Convert.ToInt32(configuration.GetSection("AppSettings:MaxFailedAccessAttemptsBeforeLockout").Value);
            }
            catch (Exception)
            {
                throw new Exception("Key 'MaxFailedAccessAttemptsBeforeLockout' not found in appsettings.json file.");
            }
            return maxFieldAccessBeforeLogout;
        }
        public static int GetPasswordExpiredAfter()
        {
            int PasswordExpiredAfter = 0;
            try
            {
                PasswordExpiredAfter = Convert.ToInt32(configuration.GetSection("AppSettings:PasswordExpiredAfter").Value);
            }
            catch (Exception)
            {
                throw new Exception("Key 'PasswordExpiredAfter' not found in appsettings.json file.");
            }
            return PasswordExpiredAfter;
        }
        public static int GetPasswordExpiryNotifyBefore()
        {
            int PasswordExpiryNotifyBefore = 0;
            try
            {
                PasswordExpiryNotifyBefore = Convert.ToInt32(configuration.GetSection("AppSettings:PasswordExpiryNotifyBefore").Value);
            }
            catch (Exception)
            {
                throw new Exception("Key 'PasswordExpiryNotifyBefore' not found in appsettings.json file.");
            }
            return PasswordExpiryNotifyBefore;
        }
        public static int GetPasswordLastCheckingCount()
        {
            int PasswordLastCheckingCount = 0;
            try
            {
                PasswordLastCheckingCount = Convert.ToInt32(configuration.GetSection("AppSettings:PasswordLastCheckingCount").Value);
            }
            catch (Exception)
            {
                throw new Exception("Key 'PasswordLastCheckingCount' not found in appsettings.json file.");
            }
            return PasswordLastCheckingCount;
        }
        public static bool GetAllowActiveDirectoryUser()
        {
            bool AllowActiveDirectoryUser = false;
            try
            {
                AllowActiveDirectoryUser = Convert.ToBoolean(configuration.GetSection("AppSettings:AllowActiveDirectoryUser").Value);
            }
            catch (Exception)
            {
                throw new Exception("Key 'AllowActiveDirectoryUser' not found in appsettings.json file.");
            }
            return AllowActiveDirectoryUser;
        }
        public static bool GetUserEmailRequired()
        {
            bool UserEmailRequired = false;
            try
            {
                UserEmailRequired = Convert.ToBoolean(configuration.GetSection("AppSettings:UserEmailRequired").Value);
            }
            catch (Exception)
            {
                throw new Exception("Key 'UserEmailRequired' not found in appsettings.json file.");
            }
            return UserEmailRequired;
        }
        public static string GetActiveDirectoryInfo()
        {
            string ActiveDirectoryInfo = String.Empty;
            try
            {
                ActiveDirectoryInfo = configuration.GetSection("AppSettings:ActiveDirectoryInfo").Value;
            }
            catch (Exception)
            {
                throw new Exception("Key 'ActiveDirectoryInfo' not found in appsettings.json file.");
            }
            return ActiveDirectoryInfo;
        }
        public static bool GetAllowSignalRLoggin()
        {
            bool AllowSignalRLoggin = false;
            try
            {
                AllowSignalRLoggin = Convert.ToBoolean(configuration.GetSection("AppSettings:AllowSignalRLoggin").Value);
            }
            catch (Exception)
            {
                throw new Exception("Key 'AllowSignalRLoggin' not found in appsettings.json file.");
            }
            return AllowSignalRLoggin;
        }
        public static int GetAppRequestTimeOut()
        {
            int AppRequestTimeOut = 0;
            try
            {
                AppRequestTimeOut = Convert.ToInt32(configuration.GetSection("AppSettings:AppRequestTimeOut").Value);
            }
            catch (Exception)
            {
                throw new Exception("Key 'AppRequestTimeOut' not found in appsettings.json file.");
            }
            return AppRequestTimeOut;
        }
        public static string GetADMAPI()
        {
            string ADMAPI = String.Empty;
            try
            {
                ADMAPI = configuration.GetSection("AppSettings:ADMAPI").Value;
                ADMAPI = AESCryptography.DecryptAES(ADMAPI);
            }
            catch (Exception)
            {
                throw new Exception("Key 'ADMAPI' not found in appsettings.json file.");
            }
            return ADMAPI;
        }
        public static string GetBlAuthenticationAPI()
        {
            string BlAuthenticationAPI = String.Empty;
            try
            {
                BlAuthenticationAPI = configuration.GetSection("AppSettings:BlAuthenticationAPI").Value;
                BlAuthenticationAPI = AESCryptography.DecryptAES(BlAuthenticationAPI);
            }
            catch (Exception)
            {
                throw new Exception("Key 'BlAuthenticationAPI' not found in appsettings.json file.");
            }
            return BlAuthenticationAPI;
        }
        public static string GetApplicationName()
        {
            string ApplicationName = String.Empty;
            try
            {
                ApplicationName = configuration.GetSection("AppSettings:ApplicationName").Value;
            }
            catch (Exception)
            {
                throw new Exception("Key 'ApplicationName' not found in appsettings.json file.");
            }
            return ApplicationName;
        }
        public static string GetApplicationKey()
        {
            string ApplicationKey = String.Empty; 
            try
            {
                ApplicationKey = configuration.GetSection("AppSettings:ApplicationKey").Value;
                ApplicationKey = AESCryptography.DecryptAES(ApplicationKey);
            }
            catch (Exception)
            {
                throw new Exception("Key 'ApplicationKey' not found in appsettings.json file.");
            }
            return ApplicationKey;
        }
        public static string GetErrorLogFilePath()
        {
            string ErrorLogFilePath = String.Empty;
            try
            {
                ErrorLogFilePath = configuration.GetSection("AppSettings:ErrorLogFilePath").Value;
            }
            catch (Exception)
            {
                throw new Exception("Key 'ErrorLogFilePath' not found in appsettings.json file.");
            }
            return ErrorLogFilePath;
        }
        public static string GetLMSAPIUrl()
        { 
            string LMS_API_URL = String.Empty;
            try
            {
                LMS_API_URL = configuration.GetSection("AppSettings:LMS_API_URL").Value;
                LMS_API_URL = AESCryptography.DecryptAES(LMS_API_URL);
            }
            catch (Exception)
            {
                throw new Exception("Key 'LMS_API_URL' not found in appsettings.json file.");
            }
            return LMS_API_URL;
        }
        public static string GetLmsChannel()
        {
            string LMS_channel = String.Empty;
            try
            {
                LMS_channel = configuration.GetSection("AppSettings:LMS_channel").Value;
            }
            catch (Exception)
            {
                throw new Exception("Key 'LMS_channel' not found in appsettings.json file.");
            }
            return LMS_channel;
        }
        public static string GetLmsTransactionId()
        {
            string LMS_transaction_id = String.Empty;
            try
            {
                LMS_transaction_id = configuration.GetSection("AppSettings:LMS_transaction_id").Value;
            }
            catch (Exception)
            {
                throw new Exception("Key 'LMS_transaction_id' not found in appsettings.json file.");
            }
            return LMS_transaction_id;
        }
        public static string GetDefaultTokenPrfx()
        {
            string defaultTokenPrefix = String.Empty;
            try
            {
                defaultTokenPrefix = configuration.GetSection("AppSettings:defaultTokenPrefix").Value;
            }
            catch (Exception)
            {
                throw new Exception("Key 'defaultTokenPrefix' not found in appsettings.json file.");
            }
            return defaultTokenPrefix;
        }

        public static string GetSMSAPIBaseUrl()
        {
            string smsapibaseurl = String.Empty;
            try
            {
                smsapibaseurl = configuration.GetSection("AppSettings:smsapibaseurl").Value;
                smsapibaseurl = AESCryptography.DecryptAES(smsapibaseurl);
            }
            catch (Exception)
            {
                throw new Exception("Key 'smsapibaseurl' not found in appsettings.json file.");
            }
            return smsapibaseurl;
        }
    }
}
