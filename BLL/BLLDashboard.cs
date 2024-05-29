using SQMS.DAL;
using SQMS.Models.ViewModels;
using System.Data;

namespace SQMS.BLL
{
    public class BLLDashboard
    {
        public List<VMDashboardAdmin> GetAdminDashboard()
        {
            DALDashboard dal = new DALDashboard();
            DataTable dt = dal.GetAdminDashboard();
            return ObjectMappingListAdmin(dt);
        }

        public List<VMDashboardBranchAdminCounters> GetBranchAdminDashboard(int branch_id, List<VMDashboardBranchAdminServicesTokens> servicesTokens, List<VMDashboardBranchAdminServicesWaitings> servicesWaitings)
        {
            DALDashboard dal = new DALDashboard();
            DataSet ds = dal.GetBranchAdminDashboard(branch_id);
            ObjectMappingListServicesTokens(ds.Tables["ServicesTokens"], servicesTokens);
            ObjectMappingListServicesWaitings(ds.Tables["ServicesWaitings"], servicesWaitings);
            return ObjectMappingListCounters(ds.Tables["COUNTERS"]);
        }

        public List<VMDashboardBranchService> GetBranchServiceList(int branch_id)
        {
            DALDashboard dal = new DALDashboard();
            DataTable dt = dal.GetBranchServiceList(branch_id);

            List<VMDashboardBranchService> list = new List<VMDashboardBranchService>();
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardBranchService dashboard = new VMDashboardBranchService()
                {
                    service_name = (row["service_name"] == DBNull.Value ? null : row["service_name"].ToString()),
                    served = (row["served"] == DBNull.Value ? 0 : Convert.ToInt32(row["served"]))
                };

                list.Add(dashboard);

            }

            return list;
        }

        public List<VMDashboardBranchToken> GetBranchTokenList(int branch_id)
        {
            DALDashboard dal = new DALDashboard();
            DataTable dt = dal.GetBranchTokenList(branch_id);

            List<VMDashboardBranchToken> list = new List<VMDashboardBranchToken>();
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardBranchToken dashboard = new VMDashboardBranchToken()
                {
                    service_name = (row["service_name"] == DBNull.Value ? null : row["service_name"].ToString()),
                    served = (row["served"] == DBNull.Value ? 0 : Convert.ToInt32(row["served"])),
                    serving = (row["serving"] == DBNull.Value ? 0 : Convert.ToInt32(row["serving"])),
                    missing = (row["missing"] == DBNull.Value ? 0 : Convert.ToInt32(row["missing"])),
                    waiting = (row["waiting"] == DBNull.Value ? 0 : Convert.ToInt32(row["waiting"]))
                };

                list.Add(dashboard);

            }

            return list;
        }

        public List<VMDashboardUserServiceDetail> GetBranchServiceDetailList(int branch_id)
        {
            DALDashboard dal = new DALDashboard();
            DataTable dt = dal.GetBranchServiceDetailList(branch_id);

            List<VMDashboardUserServiceDetail> list = new List<VMDashboardUserServiceDetail>();
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardUserServiceDetail dashboard = new VMDashboardUserServiceDetail()
                {
                    token_prefix = (row["TOKEN_PREFIX"] == DBNull.Value ? null : row["TOKEN_PREFIX"].ToString()),
                    token_no = (row["TOKEN_NO"] == DBNull.Value ? 0 : Convert.ToInt32(row["TOKEN_NO"])),
                    customer_type = (row["CUSTOMER_TYPE"] == DBNull.Value ? null : row["CUSTOMER_TYPE"].ToString()),
                    counter = (row["COUNTER"] == DBNull.Value ? null : row["COUNTER"].ToString()),
                    branch_name = (row["BRANCH_NAME"] == DBNull.Value ? null : row["BRANCH_NAME"].ToString()),
                    service = (row["SERVICE"] == DBNull.Value ? null : row["SERVICE"].ToString()),
                    issue_time = Convert.ToDateTime(row["ISSUE_TIME"] == DBNull.Value ? null : row["ISSUE_TIME"].ToString()),
                    start_time = Convert.ToDateTime(row["START_TIME"] == DBNull.Value ? null : row["START_TIME"].ToString()),
                    end_time = Convert.ToDateTime(row["END_TIME"] == DBNull.Value ? null : row["END_TIME"].ToString()),
                    service_status = (row["SERVICE_STATUS"] == DBNull.Value ? null : row["SERVICE_STATUS"].ToString()),
                };

                list.Add(dashboard);

            }

            return list;
        }

        public List<VMDashboardCounterService> GetCounterServiceList(int counter_id, out string serving_time)
        {
            DALDashboard dal = new DALDashboard();

            List<VMDashboardCounterService> list = dal.GetCounterServiceList(counter_id, out serving_time);

            return list;
        }

        public List<VMDashboardCounterToken> GetCounterTokenList(int counter_id)
        {
            DALDashboard dal = new DALDashboard();
            DataTable dt = dal.GetCounterTokenList(counter_id);

            List<VMDashboardCounterToken> list = new List<VMDashboardCounterToken>();
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardCounterToken dashboard = new VMDashboardCounterToken()
                {
                    service_name = (row["service_name"] == DBNull.Value ? null : row["service_name"].ToString()),
                    served = (row["served"] == DBNull.Value ? 0 : Convert.ToInt32(row["served"])),
                    serving = (row["serving"] == DBNull.Value ? 0 : Convert.ToInt32(row["serving"])),
                    missing = (row["missing"] == DBNull.Value ? 0 : Convert.ToInt32(row["missing"])),
                    waiting = (row["waiting"] == DBNull.Value ? 0 : Convert.ToInt32(row["waiting"]))
                };
                list.Add(dashboard);

            }

            return list;
        }

        public List<VMDashboardUserService> GetUserServiceList(string user_id, out string serving_time)
        {
            DALDashboard dal = new DALDashboard();
            List<VMDashboardUserService> list = dal.GetUserServiceList(user_id, out serving_time);

            return list;
        }

        public List<VMDashboardUserToken> GetUserTokenList(string user_id)
        {
            DALDashboard dal = new DALDashboard();
            DataTable dt = dal.GetUserTokenList(user_id);

            List<VMDashboardUserToken> list = new List<VMDashboardUserToken>();
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardUserToken dashboard = new VMDashboardUserToken()
                {
                    service_name = (row["service_name"] == DBNull.Value ? null : row["service_name"].ToString()),
                    served = (row["served"] == DBNull.Value ? 0 : Convert.ToInt32(row["served"])),
                    serving = (row["serving"] == DBNull.Value ? 0 : Convert.ToInt32(row["serving"])),
                    missing = (row["missing"] == DBNull.Value ? 0 : Convert.ToInt32(row["missing"])),
                    waiting = (row["waiting"] == DBNull.Value ? 0 : Convert.ToInt32(row["waiting"]))
                };
                list.Add(dashboard);

            }

            return list;
        }

        public List<VMDashboardUserServiceDetail> GetUserServiceDetailList(string user_id)
        {
            DALDashboard dal = new DALDashboard();
            DataTable dt = dal.GetUserServiceDetailList(user_id);

            List<VMDashboardUserServiceDetail> list = new List<VMDashboardUserServiceDetail>();
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardUserServiceDetail dashboard = new VMDashboardUserServiceDetail()
                {
                    token_prefix = (row["TOKEN_PREFIX"] == DBNull.Value ? null : row["TOKEN_PREFIX"].ToString()),
                    token_no = (row["TOKEN_NO"] == DBNull.Value ? 0 : Convert.ToInt32(row["TOKEN_NO"])),
                    customer_type = (row["CUSTOMER_TYPE"] == DBNull.Value ? null : row["CUSTOMER_TYPE"].ToString()),
                    counter = (row["COUNTER"] == DBNull.Value ? null : row["COUNTER"].ToString()),
                    branch_name = (row["BRANCH_NAME"] == DBNull.Value ? null : row["BRANCH_NAME"].ToString()),
                    service = (row["SERVICE"] == DBNull.Value ? null : row["SERVICE"].ToString()),
                    issue_time = Convert.ToDateTime(row["ISSUE_TIME"] == DBNull.Value ? null : row["ISSUE_TIME"].ToString()),
                    start_time = Convert.ToDateTime(row["START_TIME"] == DBNull.Value ? null : row["START_TIME"].ToString()),
                    end_time = Convert.ToDateTime(row["END_TIME"] == DBNull.Value ? null : row["END_TIME"].ToString()),
                    service_status = (row["SERVICE_STATUS"] == DBNull.Value ? null : row["SERVICE_STATUS"].ToString())
                };
                list.Add(dashboard);

            }

            return list;
        }


        internal List<VMDashboardAdmin> ObjectMappingListAdmin(DataTable dt)
        {
            List<VMDashboardAdmin> list = new List<VMDashboardAdmin>();
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardAdmin dashboard = new VMDashboardAdmin()
                {
                    branch_name = (row["branch_name"] == DBNull.Value ? null : row["branch_name"].ToString()),
                    tokens = (row["tokens"] == DBNull.Value ? 0 : Convert.ToInt32(row["tokens"])),
                    services = (row["services"] == DBNull.Value ? 0 : Convert.ToInt32(row["services"]))
                };

                list.Add(dashboard);

            }
            return list;
        }

        internal List<VMDashboardBranchAdminCounters> ObjectMappingListCounters(DataTable dt)
        {
            List<VMDashboardBranchAdminCounters> list = new List<VMDashboardBranchAdminCounters>();
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardBranchAdminCounters dashboard = new VMDashboardBranchAdminCounters()
                {
                    counter_no = (row["counter_no"] == DBNull.Value ? null : row["counter_no"].ToString()),
                    tokens = (row["tokens"] == DBNull.Value ? 0 : Convert.ToInt32(row["tokens"]))
                };

                list.Add(dashboard);

            }
            return list;
        }

        internal void ObjectMappingListServicesTokens(DataTable dt, List<VMDashboardBranchAdminServicesTokens> servicesTokens)
        {
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardBranchAdminServicesTokens dashboard = new VMDashboardBranchAdminServicesTokens()
                {
                    service_id = (row["service_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["service_id"])),
                    service_name = (row["service_name"] == DBNull.Value ? null : row["service_name"].ToString()),
                    tokens = (row["tokens"] == DBNull.Value ? 0 : Convert.ToInt32(row["tokens"]))
                };

                servicesTokens.Add(dashboard);

            }
        }

        internal void ObjectMappingListServicesWaitings(DataTable dt, List<VMDashboardBranchAdminServicesWaitings> servicesWaitings)
        {
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardBranchAdminServicesWaitings dashboard = new VMDashboardBranchAdminServicesWaitings()
                {
                    service_id = (row["service_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["service_id"])),
                    service_name = (row["service_name"] == DBNull.Value ? null : row["service_name"].ToString()),
                    tokens = (row["tokens"] == DBNull.Value ? 0 : Convert.ToInt32(row["tokens"]))
                };

                servicesWaitings.Add(dashboard);

            }
        }
    }
}
