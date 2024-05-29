using Microsoft.AspNetCore.Identity;
using SQMS.DAL;
using SQMS.Models;
using System.Data;
using System.Reflection.PortableExecutable;

namespace SQMS.BLL
{
    public class WebLoginBLL
    {
        public UserInfo IsUserExist(string loginName)
        {
            try
            {
                DALActiveDirectoryLogin dal = new DALActiveDirectoryLogin();
                DataTable dt = dal.GetUserInfoByLoginName(loginName);
                if (dt.Rows.Count > 0)
                    return ObjectMapping(dt.Rows[0]);
                else return null;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string Update_Pas(IdentityUser user)
        {
            string userId = string.Empty;
            try
            {
                DALActiveDirectoryLogin dal = new DALActiveDirectoryLogin();
                DataTable dt = dal.Update_userPas(user);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows) 
                    {
                        userId = (row["ID"] == DBNull.Value ? null : row["ID"].ToString());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return userId;
        }

        public string Change_Pass(ChangePassReqModel user)
        {
            string userId = string.Empty;
            try
            {
                DALActiveDirectoryLogin dal = new DALActiveDirectoryLogin();
                DataTable dt = dal.Change_userPas(user);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        userId = (row["ID"] == DBNull.Value ? null : row["ID"].ToString());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return userId;
        }

        internal UserInfo ObjectMapping(DataRow row)
        {

            UserInfo userinfo = new UserInfo();
            userinfo.IsInternal = (row["ISACTIVEDIRECTORYUSER"].ToString() == "1" ? true : false);
            userinfo.user_id = (row["ID"] == DBNull.Value ? null : row["ID"].ToString());
            userinfo.user_name = (row["HOMETOWN"] == DBNull.Value ? null : row["HOMETOWN"].ToString());
            userinfo.user_login_name = (row["USERNAME"] == DBNull.Value ? null : row["USERNAME"].ToString());
            userinfo.role_name = (row["role_name"] == DBNull.Value ? null : row["role_name"].ToString());
            userinfo.branch_id = (row["branch_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["branch_id"]));
            userinfo.branch_name = (row["branch_name"] == DBNull.Value ? null : row["branch_name"].ToString());
            userinfo.branch_static_ip = (row["branch_static_ip"] == DBNull.Value ? null : row["branch_static_ip"].ToString());
            userinfo.force_change_confirmed = (row["FORCECHANGECONFIRMED"] == DBNull.Value ? false : (Convert.ToInt32(row["FORCECHANGECONFIRMED"]) == 1 ? true : false));
            return userinfo;
        }

        public UserInfoV2 GetUserInformation(string loginName)
        {
            try
            { 
                DALActiveDirectoryLogin dal = new DALActiveDirectoryLogin();
                DataTable dt = dal.GetUserInformation(loginName);
                if (dt.Rows.Count > 0)
                    return ObjectMappingUserInfo(dt.Rows[0]);
                else return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public UserInfoV2 GetUserInformationById(string userId)
        {
            try
            {
                DALActiveDirectoryLogin dal = new DALActiveDirectoryLogin();
                DataTable dt = dal.GetUserInformationById(userId);
                if (dt.Rows.Count > 0)
                    return ObjectMappingUserInfo(dt.Rows[0]);
                else return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetRoleId(string roleName)
        {
            string roleId = string.Empty;
            try
            {
                DALActiveDirectoryLogin dal = new DALActiveDirectoryLogin();
                DataTable dt = dal.GetUserRoleId(roleName);
                if(dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        roleId = (row["ID"] == DBNull.Value ? null : row["ID"].ToString());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return roleId;
        }

        public string Insert_Role(string user_id, string role_id)
        {
            try
            {
                DALActiveDirectoryLogin dal = new DALActiveDirectoryLogin();
                DataTable dt = dal.INSERT_Role(user_id,role_id);
                if (dt.Rows.Count > 0)
                    return "";
                else return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string Insert_user(IdentityUser userInfo, string HomeTown, int ForceChangeConfirmed, int isActiveDerectoryUser)
        {
            try
            {
                DALActiveDirectoryLogin dal = new DALActiveDirectoryLogin();
                DataTable dt = dal.INSERT_USERS(userInfo, HomeTown, ForceChangeConfirmed, isActiveDerectoryUser);
                if (dt.Rows.Count > 0)
                    return ObjectMappingUserCreate(dt.Rows[0]);
                else return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal string ObjectMappingUserCreate(DataRow row)
        {
            string user_id = row["USERID"] == DBNull.Value ? "" : row["USERID"].ToString();
            return user_id;
        }

        internal UserInfoV2 ObjectMappingUserInfo(DataRow row)
        {

            UserInfoV2 userinfo = new UserInfoV2();
            userinfo.IsInternal = (row["ISACTIVEDIRECTORYUSER"].ToString() == "1" ? true : false);
            userinfo.user_id = (row["ID"] == DBNull.Value ? null : row["ID"].ToString());
            userinfo.user_name = (row["HOMETOWN"] == DBNull.Value ? null : row["HOMETOWN"].ToString());
            userinfo.user_login_name = (row["USERNAME"] == DBNull.Value ? null : row["USERNAME"].ToString());
            userinfo.role_name = (row["role_name"] == DBNull.Value ? null : row["role_name"].ToString());
            userinfo.password_hash = (row["PASSWORDHASH"] == DBNull.Value ? null : row["PASSWORDHASH"].ToString());
            userinfo.email = (row["EMAIL"] == DBNull.Value ? null : row["EMAIL"].ToString());
            userinfo.branch_id = (row["branch_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["branch_id"]));
            userinfo.branch_name = (row["branch_name"] == DBNull.Value ? null : row["branch_name"].ToString());
            userinfo.branch_static_ip = (row["branch_static_ip"] == DBNull.Value ? null : row["branch_static_ip"].ToString());
            userinfo.force_change_confirmed = (row["FORCECHANGECONFIRMED"] == DBNull.Value ? false : (Convert.ToInt32(row["FORCECHANGECONFIRMED"]) == 1 ? true : false));
            return userinfo;
        }

        public List<RightInformation> GetRightInformationList(string loginName)
        {
            DALActiveDirectoryLogin dal = new DALActiveDirectoryLogin();
            DataTable dt = dal.GetRightInformationList(loginName);
            return ObjectMappingList(dt);
        }

        internal List<RightInformation> ObjectMappingList(DataTable dt)
        {
            List<RightInformation> list = new List<RightInformation>();
            foreach (DataRow row in dt.Rows)
            {
                RightInformation rightInformation = new RightInformation();
                rightInformation.PAGE_ID = Convert.ToInt32(row["PAGE_ID"] == DBNull.Value ? 0 : row["PAGE_ID"]);
                rightInformation.PAGE_NAME = (row["PAGE_NAME"] == DBNull.Value ? null : row["PAGE_NAME"].ToString());
                list.Add(rightInformation);
            }
            return list;
        }

        //internal bool IsAdLoginSuccess(LogReqModel model)
        //{
        //    bool isAdLoginSuccess = false;
        //    DALActiveDirectoryLogin dal = new DALActiveDirectoryLogin();
        //    UserInfoModel userinfo = dal.GetUserInfo(model.UserName);

        //    if (userinfo.is_locked.ToUpper() == "N" && userinfo.user_status.ToUpper() == "Y")

        //    {
        //        if (userinfo.is_internal.ToUpper() == "Y")
        //        {

        //            isAdLoginSuccess = ADLogin(model);
        //        }

        //        else

        //        {
        //            isAdLoginSuccess = HashLogin(model, userinfo);
        //        }
        //    }

        //    return isAdLoginSuccess;

        //}

        //public string GetDomainName()
        //{
        //    return ConfigurationManager.AppSettings["DomainName"].ToString();
        //}

        //internal bool CheckUserPasswordInActiveDerectory(LogReqModel model)
        //{
        //    string userName = model.UserName.Replace("'", "#").Trim();
        //    string password = model.UserPassword.Trim();
        //    string domainName = GetDomainName();

        //    using (DirectoryEntry dEntry = new DirectoryEntry(string.Format("LDAP://{0}", domainName), userName, password))
        //    {
        //        using (DirectorySearcher dSearch = new DirectorySearcher(dEntry))
        //        {
        //            try
        //            {
        //                SearchResult result = dSearch.FindOne();
        //                result.GetDirectoryEntry();
        //                return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                return false;
        //            }
        //        }
        //    }

        //}

        //private bool ADLogin(LogReqModel model)
        //{
        //    return CheckUserPasswordInActiveDerectory(model);
        //}

        //private bool HashLogin(LogReqModel model, UserInfoModel userinfo)
        //{
        //    bool isPasswordMatch = false;
        //    if (userinfo.hash_password == FormsAuthentication.HashPasswordForStoringInConfigFile(model.user_password, "MD5"))
        //    {
        //        isPasswordMatch = true;
        //    }

        //    else
        //    {
        //        isPasswordMatch = false;
        //    }

        //    return isPasswordMatch;
        //}

        //private bool ADLogin(LogReqModel model)
        //{
        //    return CheckUserPasswordInActiveDerectory(model);
        //}

        //private bool HashLogin(LogReqModel model, UserInfoModel userinfo)
        //{
        //    bool isPasswordMatch = false;
        //    if (userinfo.hash_password == FormsAuthentication.HashPasswordForStoringInConfigFile(model.user_password, "MD5"))
        //    {
        //        isPasswordMatch = true;
        //    }

        //    else
        //    {
        //        isPasswordMatch = false;
        //    }

        //    return isPasswordMatch;
        //}
    }
}
