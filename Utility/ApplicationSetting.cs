using SQMS.BLL;

namespace SQMS.Utility
{
    public class ApplicationSetting
    {
        public static string DisplayPath
        {
            get
            {
                return StaticConfigValue.GetDisplayPath();
            }
        }
        public static int PaddingLeft
        {
            get
            {
                return PaddingLeftDB();
            }
        }

        public static int PaddingLeftDB()
        {
            BLLGlobalSettings globalSettings = new BLLGlobalSettings();
            return globalSettings.Get().padding_left;
        }

        public static string dispalyFooterAdd
        {
            get
            {
                return dispalyFooterAddDB();
            }
        }
        public static string dispalyFooterAddDB()
        {
            BLLGlobalSettings globalSettings = new BLLGlobalSettings();
            return globalSettings.Get().display_footer_ad;
        }

        public static string dispalyWelcome
        {
            get
            {
                return StaticConfigValue.GetDisplayWelcome();
            }
        }

        public static string dispalyVideo
        {
            get
            {
                return StaticConfigValue.GetDisplayVideo();
            }
        }
        public static string defaultTokenPrefix
        {
            get
            {
                return defaultTokenPrefixDB();
            }
        }
        public static string defaultTokenPrefixDB()
        {
            BLLGlobalSettings globalSettings = new BLLGlobalSettings();
            return globalSettings.Get().default_token_prefix;
        }

        public static string voiceText
        {
            get
            {
                return StaticConfigValue.GetVoiceText();
            }
        }

        public static string speakLanguage
        {
            get
            {
                return StaticConfigValue.GetSpeakLanguage();
            }
        }

        public static string speakGender
        {
            get
            {
                return StaticConfigValue.GetSpeakGender();
            }
        }

        public static string speakRate
        {
            get
            {
                return StaticConfigValue.GetSpeakRate();
            }
        }

        public static string DisplayWhenEmptyToken
        {
            get
            {
                return StaticConfigValue.GetDisplayWhenEmptyToken();
            }
        }

        public static string galleryDefaultPath
        {
            get
            {
                return StaticConfigValue.GetGalleryDefaultPath();
            }
        }

        public static string galleryDBPath
        { 
            get
            {
                return StaticConfigValue.GetGalleryDBPath();
            }
        }

        public static int passwordRequiredLength
        {
            get
            {
                return StaticConfigValue.GetRequiredPasswordLength();
            }
        }
        public static int MaxFailedAccessAttemptsBeforeLockout
        {
            get
            {
                return StaticConfigValue.GetMaxFailedAccessAttemptsBeforeLockout();
            }
        }

        public static int PasswordExpiredAfter
        {
            get
            {
                return StaticConfigValue.GetPasswordExpiredAfter();
            }
        }

        public static int PasswordExpiryNotifyBefore
        {
            get
            {
                return StaticConfigValue.GetPasswordExpiryNotifyBefore();
            }
        }
        public static int PasswordLastCheckingCount
        {
            get
            {
                return StaticConfigValue.GetPasswordLastCheckingCount();
            }
        }

        public static bool AllowActiveDirectoryUser
        {
            get
            {
                return StaticConfigValue.GetAllowActiveDirectoryUser();
            }
        }

        public static bool UserEmailRequired
        {
            get
            {
                return StaticConfigValue.GetUserEmailRequired();
            }
        }

        public static string ActiveDirectoryInfo
        {
            get
            {
                return StaticConfigValue.GetActiveDirectoryInfo();
            }
        }

        public static string ReportCSVSeparator
        {
            get
            {
                return ReportCSVSeparatorDB();
            }
        }

        public static string ReportCSVSeparatorDB()
        {
            BLLGlobalSettings globalSettings = new BLLGlobalSettings();
            return globalSettings.Get().report_csv_separator;
        }

        public static bool AllowSignalRLoggin
        {
            get
            {
                return StaticConfigValue.GetAllowSignalRLoggin();
            }
        }


        public static string msgText
        {
            get
            {
                return msgTextDB();
            }
        }
        public static string msgText_Bn
        {
            get
            {
                return msgText_BN_DB();
            }
        }
        public static string msgTextDB()
        {
            BLLGlobalSettings globalSettings = new BLLGlobalSettings();
            return globalSettings.Get().msg_text;
        }
        public static string msgText_BN_DB()
        {
            BLLGlobalSettings globalSettings = new BLLGlobalSettings();
            return globalSettings.Get().msg_text_Bn;
        }

        public static bool isMsgBn
        {
            get
            {
                return isMsgBnDB();
            }
        }

        public static bool isMsgBnDB()
        {
            BLLGlobalSettings globalSettings = new BLLGlobalSettings();
            return globalSettings.Get().is_msg_bn;
        }

        public static int AppRequestTimeOut
        {
            get
            {
                return StaticConfigValue.GetAppRequestTimeOut();
            }
        }

        public static string ADMAPI
        {
            get
            {
                return StaticConfigValue.GetADMAPI();
            }
        }
        public static string ADAPI
        {
            get
            {
                return StaticConfigValue.GetBlAuthenticationAPI();
            }
        }
        public static string ADUser
        {
            get
            {
                return StaticConfigValue.GetApplicationName();
            }
        }
        public static string ADPassword
        {
            get
            {
                return StaticConfigValue.GetApplicationKey();
            }
        }
        public static string GetErrorLogFilePath
        {
            get
            {
                return StaticConfigValue.GetErrorLogFilePath();
            }
        }
    }

}
