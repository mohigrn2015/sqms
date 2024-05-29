using MySql.Data.MySqlClient;
using SQMS.Models.ViewModels;
using SQMS.Utility;
using System.Data;
using System.Diagnostics;

namespace SQMS.DAL
{
    public class DALDashboard
    {
        MySQLManager manager;
        public DataSet GetBranchAdminDashboard(int branch_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("P_BRANCH_ID", branch_id));

                DataTable dtCounter = manager.CallStoredProcedure_Select("DSP_CHART_COUNTERS_TOKENS");
                DataTable dtServicesTokens = GetBranchAdminDashboardServicesTokens(branch_id);
                DataTable dtServicesWaitings = GetBranchAdminDashboardServicesWaitings(branch_id);

                dtCounter.TableName = "COUNTERS";
                dtServicesTokens.TableName = "ServicesTokens";
                dtServicesWaitings.TableName = "ServicesWaitings";
                DataSet ds = new DataSet();
                ds.Tables.Add(dtCounter);
                ds.Tables.Add(dtServicesTokens);
                ds.Tables.Add(dtServicesWaitings);
                return ds;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALDashboard",
                    procedure_name = "DSP_CHART_COUNTERS_TOKENS",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable GetBranchAdminDashboardServicesTokens(int branch_id)
        {
            manager = new MySQLManager();
            try
            {                
                manager.AddParameter(new MySqlParameter("P_BRANCH_ID", branch_id));

                return manager.CallStoredProcedure_Select("DSP_CHART_SERVICES_TOKENS");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALDashboard",
                    procedure_name = "DSP_CHART_SERVICES_TOKENS",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable GetBranchAdminDashboardServicesWaitings(int branch_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("P_BRANCH_ID", branch_id));

                return manager.CallStoredProcedure_Select("DSP_CHART_SERVICES_WAITINGS");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALDashboard",
                    procedure_name = "DSP_CHART_SERVICES_WAITINGS",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable GetAdminDashboard()
        {
            manager = new MySQLManager();
            try
            {
                return manager.CallStoredProcedure_Select("USP_DASHBOARD_ADMIN");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALDashboard",
                    procedure_name = "USP_DASHBOARD_ADMIN",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable GetBranchServiceList(int branch_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", branch_id));

                return manager.CallStoredProcedure_Select("DSP_Branch_ServiceList");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALDashboard",
                    procedure_name = "DSP_Branch_ServiceList",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable GetBranchTokenList(int branch_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", branch_id));

                return manager.CallStoredProcedure_Select("DSP_Branch_TokenList");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALDashboard",
                    procedure_name = "DSP_Branch_TokenList",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable GetBranchServiceDetailList(int branch_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", branch_id));

                return manager.CallStoredProcedure_Select("DSP_Branch_ServiceDetailList");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALDashboard",
                    procedure_name = "DSP_Branch_ServiceDetailList",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public List<VMDashboardCounterService> GetCounterServiceList(int counter_id, out string serving_time)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_counter_id", counter_id));

                //DataTable dt = manager.CallStoredProcedure_Select("DSP_Counter_ServiceList");
                DataSet dtSet = manager.CallStoredProcedure_SelectDataSet("DSP_Counter_ServiceList");
                DataTable firstResultSet = dtSet.Tables[0];
                DataTable secondResultSet = dtSet.Tables[1];
                serving_time = "00:00:00";

                if (firstResultSet.Rows.Count > 0)
                {
                    foreach(DataRow dr in firstResultSet.Rows)
                    {
                        serving_time = dr["po_ServingTime"] == DBNull.Value ? "00:00:00" : dr["po_ServingTime"].ToString();
                    }
                }

                List<VMDashboardCounterService> list = new List<VMDashboardCounterService>();
                foreach (DataRow row in secondResultSet.Rows)
                {
                    VMDashboardCounterService dashboard = new VMDashboardCounterService()
                    {
                        service_name = (row["service_name"] == DBNull.Value ? null : row["service_name"].ToString()),
                        served = (row["served"] == DBNull.Value ? 0 : Convert.ToInt32(row["served"]))
                    };
                    list.Add(dashboard);

                }
                return list;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALDashboard",
                    procedure_name = "DSP_Counter_ServiceList",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable GetCounterTokenList(int counter_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_counter_id", counter_id));

                DataTable dt = manager.CallStoredProcedure_Select("DSP_Counter_TokenList");

                return dt;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALDashboard",
                    procedure_name = "DSP_Counter_TokenList",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public List<VMDashboardUserService> GetUserServiceList(string user_id, out string serving_time)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_user_id", user_id));

                DataSet dtSet = manager.CallStoredProcedure_SelectDataSet("DSP_User_ServiceList");
                DataTable firstResultSet = dtSet.Tables[0];
                DataTable secondResultSet = dtSet.Tables[1];
                serving_time = "00:00:00";

                if (firstResultSet.Rows.Count > 0)
                {
                    foreach (DataRow dr in firstResultSet.Rows)
                    {
                        serving_time = dr["po_ServingTime"] == DBNull.Value ? "00:00:00" : dr["po_ServingTime"].ToString();
                    }
                }

                List<VMDashboardUserService> list = new List<VMDashboardUserService>();
                foreach (DataRow row in secondResultSet.Rows)
                {
                    VMDashboardUserService dashboard = new VMDashboardUserService()
                    {
                        service_name = (row["service_name"] == DBNull.Value ? null : row["service_name"].ToString()),
                        served = (row["served"] == DBNull.Value ? 0 : Convert.ToInt32(row["served"]))
                    };
                    list.Add(dashboard);

                }
                return list;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALDashboard",
                    procedure_name = "DSP_User_ServiceList",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }


        public DataTable GetUserTokenList(string user_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_user_id", user_id));

                DataTable dt = manager.CallStoredProcedure_Select("DSP_User_TokenList");

                return dt;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALDashboard",
                    procedure_name = "DSP_User_TokenList",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable GetUserServiceDetailList(string user_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_user_id", user_id));

                return manager.CallStoredProcedure_Select("DSP_User_ServiceDetailList");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALDashboard",
                    procedure_name = "DSP_User_ServiceDetailList",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
    }
}
