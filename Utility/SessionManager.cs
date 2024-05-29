using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQMS.Models;
using System.Web;
namespace SQMS.Utility
{
    public class SessionManager
    {
        public SessionManager()
        {

        }
        private readonly IHttpContextAccessor _session;
        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor;
        }

        public string user_id
        {
            get
            {
                return _session.HttpContext.Session.GetString("user_id");
            }
            set
            {
                _session.HttpContext.Session.SetString("user_id", value);
            }
        }

        public string user_name
        {
            get
            {
                return String.IsNullOrEmpty(_session.HttpContext.Session.GetString("user_name")) ? "" : _session.HttpContext.Session.GetString("user_name");
            }
            set
            {
                _session.HttpContext.Session.SetString("user_name", value);
            }
        }

        public string LoginProvider
        {
            get
            {
                return String.IsNullOrEmpty(_session.HttpContext.Session.GetString("LoginProvider")) ? "" : _session.HttpContext.Session.GetString("LoginProvider");
            }
            set
            {
                _session.HttpContext.Session.SetString("LoginProvider", value);
            }
        }

        public int branch_id
        {
            get
            {
                return _session.HttpContext.Session.GetInt32("branch_id") == 0 ? 0:Convert.ToInt32(_session.HttpContext.Session.GetInt32("branch_id"));
            }
            set
            {
                _session.HttpContext.Session.SetInt32("branch_id", value);
            }
        }

        public string branch_name
        {
            get
            {
                return String.IsNullOrEmpty(_session.HttpContext.Session.GetString("branch_name")) ? "" : _session.HttpContext.Session.GetString("branch_name");
            }
            set
            {
                _session.HttpContext.Session.SetString("branch_name", value);
            }
        }

        public string branch_static_ip
        {
            get
            {
                return String.IsNullOrEmpty(_session.HttpContext.Session.GetString("branch_static_ip")) ? "" : _session.HttpContext.Session.GetString("branch_static_ip");
            }
            set
            {
                _session.HttpContext.Session.SetString("branch_static_ip", value);
            }
        }

        public int counter_id
        {
            get
            {
                return Convert.ToInt32(_session.HttpContext.Session.GetString("counter_id"));
            }
            set
            {
                _session.HttpContext.Session.SetInt32("counter_id", value);
            }
        }

        public string counter_idv2
        {
            get
            {
                return _session.HttpContext.Session.GetString("counter_id");
            }
            set
            {
                _session.HttpContext.Session.SetString("counter_id", value);
            }
        }

        public string counter_no
        {
            get
            {
                return String.IsNullOrEmpty(_session.HttpContext.Session.GetString("counter_no")) ? "" : _session.HttpContext.Session.GetString("counter_no");
            }
            set
            {
                _session.HttpContext.Session.SetString("counter_no", value);
            }
        }

        public Exception error
        {
            get
            {
                Exception exception = null;
                // Check if _session is null
                if (_session.HttpContext.Session == null)
                    return null;

                string errorMessage = _session.HttpContext.Session.GetString("error");

                if (string.IsNullOrEmpty(errorMessage))
                    return null;

                try
                {
                    exception = new Exception(errorMessage);
                    return exception;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            set
            {
                if (value != null)
                {
                    if (_session.HttpContext.Session == null)
                        return;

                    _session.HttpContext.Session.SetString("error", value.Message);
                }                
            }
        }

        public int PasswordExpiryNotifyBeforeDays
        {
            get
            {
                return _session.HttpContext.Session.GetString("PasswordExpiryNotifyBeforeDays") == null ? 0 : Convert.ToInt32(_session.HttpContext.Session.GetString("PasswordExpiryNotifyBeforeDays"));
            }
            set
            {
                _session.HttpContext.Session.SetInt32("PasswordExpiryNotifyBeforeDays", value);
            }
        }

        public bool IsPasswordExpired
        {
            get
            {
                string stringValue = _session.HttpContext.Session.GetString("IsPasswordExpired");

                if (string.IsNullOrEmpty(stringValue))
                {
                    return false;
                }
                else
                {
                    return bool.TryParse(stringValue, out bool result) ? result : false;
                }
            }
            set
            {
                _session.HttpContext.Session.SetString("IsPasswordExpired", value.ToString());
            }
        }


        public bool ForceChangeConfirmed
        {
            get
            {
                string stringValue = _session.HttpContext.Session.GetString("ForceChangeConfirmed");

                if (string.IsNullOrEmpty(stringValue))
                {
                    return false;
                }
                else
                {
                    return bool.TryParse(stringValue, out bool result) ? result : false;
                }
            }
            set
            {
                _session.HttpContext.Session.SetString("ForceChangeConfirmed", value.ToString());
            }
        }

        public bool IsActiveDirectoryUser
        {
            get
            {
                string stringValue = _session.HttpContext.Session.GetString("IsActiveDirectoryUser");

                if (string.IsNullOrEmpty(stringValue))
                {
                    return false;
                }
                else
                {
                    return bool.TryParse(stringValue, out bool result) ? result : false;
                }
            }
            set
            {
                _session.HttpContext.Session.SetString("IsActiveDirectoryUser", value.ToString());
            }

        }

        public void Close()
        {
            _session.HttpContext.Session.Clear();
            _session.HttpContext.Session.Remove("user_id");
        }
    }
}
