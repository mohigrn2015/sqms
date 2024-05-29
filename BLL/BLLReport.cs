using SQMS.DAL;
using SQMS.Models.ReportModels;
using SQMS.Models;
using System.Data;

namespace SQMS.BLL
{
    public class BLLReport
    {
        public DataTable LocalCustomerReportDT(int branch_id, string user_id, string login_user_id, int counter_id, int customer_type_id, int service_sub_type_id, DateTime start_date, DateTime end_date)
        {
            DALReport dal = new DALReport();
            return dal.LocalCustomerReport(branch_id, user_id, login_user_id, counter_id, customer_type_id, service_sub_type_id, start_date, end_date);


        }
        public List<RM_LocalCustomer> LocalCustomerReport(int branch_id, string user_id, string login_user_id, int counter_id, int customer_type_id, int service_sub_type_id, DateTime start_date, DateTime end_date)
        {
            DALReport dal = new DALReport();
            DataTable dt = dal.LocalCustomerReport(branch_id, user_id, login_user_id, counter_id, customer_type_id, service_sub_type_id, start_date, end_date);
            List<RM_LocalCustomer> list = new List<RM_LocalCustomer>();
            foreach (DataRow row in dt.Rows)
            {
                RM_LocalCustomer localCustomer = new RM_LocalCustomer();

                localCustomer.branch_name = (row["BRANCH_NAME"] == DBNull.Value ? "" : row["BRANCH_NAME"].ToString());
                localCustomer.service_datetime = Convert.ToDateTime(row["SERVICE_DATETIME"] == DBNull.Value ? "" : row["SERVICE_DATETIME"].ToString()).ToString("dd/MMM/yyyy");
                localCustomer.service_sub_type_name = (row["service_sub_type_name"] == DBNull.Value ? "" : row["service_sub_type_name"].ToString());
                localCustomer.end_time = Convert.ToDateTime(row["end_time"] == DBNull.Value ? "" : row["end_time"].ToString()).ToString("hh:mm:ss");
                localCustomer.contact_no = (row["contact_no"] == DBNull.Value ? "" : row["contact_no"].ToString());
                localCustomer.customer_name = (row["customer_name"] == DBNull.Value ? "" : row["customer_name"].ToString());
                localCustomer.im_msisdn = (row["IM_MSISDN"] == DBNull.Value ? "" : row["IM_MSISDN"].ToString());
                localCustomer.im_name = (row["IM_Name"] == DBNull.Value ? "" : row["IM_Name"].ToString());
                localCustomer.remarks = (row["Remarks"] == DBNull.Value ? "" : row["Remarks"].ToString());
                localCustomer.further_followUp_needed = (row["Further_followUp_needed"] == DBNull.Value ? "" : row["Further_followUp_needed"].ToString());
                localCustomer.FollowUp_date = (row["FollowUp_date"] == DBNull.Value ? "" : Convert.ToDateTime(row["FollowUp_date"].ToString()).ToString("dd/MMM/yyyy"));
                list.Add(localCustomer);
            }
            return list;
        }

        public List<RM_Break> BreakReport(int branch_id, string user_id, string login_user_id, int counter_id, int break_type_id, DateTime start_date, DateTime end_date)
        {
            DALReport dal = new DALReport();
            DataTable dt = dal.BreakReport(branch_id, user_id, login_user_id, counter_id, break_type_id, start_date, end_date);
            List<RM_Break> list = new List<RM_Break>();
            foreach (DataRow row in dt.Rows)
            {
                RM_Break a_break = new RM_Break();

                a_break.branch_name = (row["branch_name"] == DBNull.Value ? "" : row["branch_name"].ToString());
                a_break.create_time = Convert.ToDateTime(row["create_time"] == DBNull.Value ? "" : row["create_time"].ToString()).ToString("dd/MMM/yyyy");
                a_break.username = (row["username"] == DBNull.Value ? "" : row["username"].ToString());
                a_break.userid = (row["userid"] == DBNull.Value ? "" : row["userid"].ToString());
                a_break.counter_no = (row["counter_no"] == DBNull.Value ? "" : row["counter_no"].ToString());
                a_break.break_type_name = (row["break_type_name"] == DBNull.Value ? "" : row["break_type_name"].ToString());
                a_break.end_time = (row["end_time"] == DBNull.Value ? "" : Convert.ToDateTime(row["end_time"].ToString()).ToString("hh:mm:ss"));
                a_break.start_time = (row["start_time"] == DBNull.Value ? "" : Convert.ToDateTime(row["start_time"].ToString()).ToString("hh:mm:ss"));
                a_break.duration = (row["duration"] == DBNull.Value ? "" : row["duration"].ToString());
                list.Add(a_break);
            }
            return list;
        }

        public List<RM_SingleVSMultipleVisitedCustomer> SingleVSMultipleVisitedReport(int branch_id, string user_id, string login_user_id, int counter_id, int service_sub_type_id, DateTime start_date, DateTime end_date)
        {
            DALReport dal = new DALReport();
            DataTable dt = dal.SingleVSMultipleVisitedReport(branch_id, user_id, login_user_id, counter_id, service_sub_type_id, start_date, end_date);
            List<RM_SingleVSMultipleVisitedCustomer> list = new List<RM_SingleVSMultipleVisitedCustomer>();
            foreach (DataRow row in dt.Rows)
            {
                RM_SingleVSMultipleVisitedCustomer singleVSMultipleVisit = new RM_SingleVSMultipleVisitedCustomer();

                singleVSMultipleVisit.branch_name = (row["branch_name"] == DBNull.Value ? "" : row["branch_name"].ToString());
                singleVSMultipleVisit.service_sub_type_name = (row["service_sub_type_name"] == DBNull.Value ? "" : row["service_sub_type_name"].ToString());
                singleVSMultipleVisit.total_served_token = (row["total_served_token"] == DBNull.Value ? "" : row["total_served_token"].ToString());
                singleVSMultipleVisit.single_visit_customer = (row["single_visit_customer"] == DBNull.Value ? "" : row["single_visit_customer"].ToString());
                singleVSMultipleVisit.multiple_visit_customer = (row["multiple_visit_customer"] == DBNull.Value ? "" : row["multiple_visit_customer"].ToString());

                list.Add(singleVSMultipleVisit);
            }
            return list;
        }

        public List<RM_AgentWiseSummary> AgentWiseSummaryReport(int branch_id, string user_id, string login_user_id, int counter_id, int service_sub_type_id, DateTime start_date, DateTime end_date)
        {
            DALReport dal = new DALReport();
            DataTable dt = dal.AgentWiseSummaryReport(branch_id, user_id, login_user_id, counter_id, service_sub_type_id, start_date, end_date);
            List<RM_AgentWiseSummary> list = new List<RM_AgentWiseSummary>();
            foreach (DataRow row in dt.Rows)
            {
                RM_AgentWiseSummary agent = new RM_AgentWiseSummary();

                agent.branch_name = (row["branch_name"] == DBNull.Value ? "" : row["branch_name"].ToString());
                agent.user_name = (row["user_name"] == DBNull.Value ? "" : row["user_name"].ToString());
                agent.handled_customer = (row["handled_customer"] == DBNull.Value ? "" : row["handled_customer"].ToString());
                agent.average_waiting_time = (row["average_waiting_time"] == DBNull.Value ? "" : row["average_waiting_time"].ToString());
                agent.average_service_time = (row["average_service_time"] == DBNull.Value ? "" : row["average_service_time"].ToString());
                agent.average_TAT = (row["average_TAT"] == DBNull.Value ? "" : row["average_TAT"].ToString());
                agent.user_id = (row["user_id"] == DBNull.Value ? "" : row["user_id"].ToString());
                list.Add(agent);
            }
            return list;
        }

        public List<RM_ServiceSummary> ServiceSummaryReport(int branch_id, string user_id, string login_user_id, int counter_id, int service_sub_type_id, DateTime start_date, DateTime end_date)
        {
            DALReport dal = new DALReport();
            DataTable dt = dal.ServiceSummaryReport(branch_id, user_id, login_user_id, counter_id, service_sub_type_id, start_date, end_date);
            List<RM_ServiceSummary> list = new List<RM_ServiceSummary>();
            foreach (DataRow row in dt.Rows)
            {
                RM_ServiceSummary serviceSummary = new RM_ServiceSummary();

                serviceSummary.branch_name = (row["branch_name"] == DBNull.Value ? null : row["branch_name"].ToString());
                serviceSummary.service_sub_type_name = (row["SERVICE_SUB_TYPE_NAME"] == DBNull.Value ? null : row["SERVICE_SUB_TYPE_NAME"].ToString());
                serviceSummary.token_served = Convert.ToInt32(row["token_served"] == DBNull.Value ? 0 : row["token_served"]);
                serviceSummary.total_percentage = Convert.ToDecimal(row["total_percentage"] == DBNull.Value ? 0 : row["total_percentage"]);
                serviceSummary.standard_time = (row["standard_time"] == DBNull.Value ? null : row["standard_time"].ToString());
                serviceSummary.actual_time = (row["actual_time"] == DBNull.Value ? null : row["actual_time"].ToString());
                serviceSummary.variance = (row["variance"] == DBNull.Value ? null : row["variance"].ToString());
                list.Add(serviceSummary);
            }
            return list;
        }

        public List<RM_GeneralSearch> GeneralSearchReport(int branch_id, string user_id, string login_user_id, int counter_id, string msisdn_no, int service_sub_type_id, DateTime start_date, DateTime end_date, string token_no)
        {
            DALReport dal = new DALReport();
            DataTable dt = dal.GeneralSearchReport(branch_id, user_id, login_user_id, counter_id, msisdn_no, service_sub_type_id, start_date, end_date, token_no);
            List<RM_GeneralSearch> list = new List<RM_GeneralSearch>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    RM_GeneralSearch generalSearch = new RM_GeneralSearch();

                    generalSearch.branch_name = (row["BRANCH_NAME"] == DBNull.Value ? "" : row["BRANCH_NAME"].ToString());
                    generalSearch.Service_date = Convert.ToDateTime(row["SERVICE_DATETIME"] == DBNull.Value ? "" : row["SERVICE_DATETIME"].ToString()).ToString("dd/MMM/yyyy");
                    generalSearch.user_name = (row["user_name"] == DBNull.Value ? "" : row["user_name"].ToString());
                    //generalSearch.user_id = (row["user_id"] == DBNull.Value ? "" : row["user_id"].ToString());
                    generalSearch.token_prefix = (row["token_prefix"] == DBNull.Value ? "" : row["token_prefix"].ToString());
                    generalSearch.token_no = (row["token_no"] == DBNull.Value ? "" : generalSearch.token_prefix+row["token_no"].ToString());
                    generalSearch.mobile_no = (row["mobile_no"] == DBNull.Value ? "" : row["mobile_no"].ToString());
                    generalSearch.service_sub_type_name = (row["service_sub_type_name"] == DBNull.Value ? "" : row["service_sub_type_name"].ToString());
                    generalSearch.issue_time = Convert.ToDateTime(row["issue_time"] == DBNull.Value ? "" : row["issue_time"].ToString()).ToString("hh:mm:ss");
                    generalSearch.start_time = Convert.ToDateTime(row["start_time"] == DBNull.Value ? "" : row["start_time"].ToString()).ToString("hh:mm:ss");
                    generalSearch.end_time = Convert.ToDateTime(row["end_time"] == DBNull.Value ? "" : row["end_time"].ToString()).ToString("hh:mm:ss");
                    generalSearch.wating_time = (row["wating_time"] == DBNull.Value ? "" : row["wating_time"].ToString());
                    generalSearch.std_time = (row["std_time"] == DBNull.Value ? "" : row["std_time"].ToString());
                    generalSearch.actual_time = (row["actual_time"] == DBNull.Value ? "" : row["actual_time"].ToString());
                    generalSearch.variance = (row["variance"] == DBNull.Value ? "" : row["variance"].ToString());
                    //generalSearch.remarks = (row["remarks"] == DBNull.Value ? "" : row["remarks"].ToString());
                    generalSearch.issues = (row["ISSUES"] == DBNull.Value ? "" : row["ISSUES"].ToString());
                    generalSearch.solutions = (row["solutions"] == DBNull.Value ? "" : row["solutions"].ToString());
                    generalSearch.refresh_reason = (row["refresh_reason"] == DBNull.Value ? "" : row["refresh_reason"].ToString());

                    list.Add(generalSearch);
                }
                catch (Exception)
                {
                    throw;
                }

            }
            return list;
        }

        public DataTable GeneralSearchReportDT(int branch_id, string user_id, string login_user_id, int counter_id, string msisdn_no, int service_sub_type_id, DateTime start_date, DateTime end_date, string token_no)
        {
            DALReport dal = new DALReport();
            DataTable dt = dal.GeneralSearchReport(branch_id, user_id, login_user_id, counter_id, msisdn_no, service_sub_type_id, start_date, end_date, token_no);

            dt.Columns["BRANCH_NAME"].SetOrdinal(0);
            dt.Columns["SERVICE_DATETIME"].SetOrdinal(1);
            dt.Columns["user_name"].SetOrdinal(2);
            dt.Columns["user_id"].SetOrdinal(3);
            dt.Columns["token_prefix"].SetOrdinal(4);
            dt.Columns["token_no"].SetOrdinal(5);
            dt.Columns["mobile_no"].SetOrdinal(6);
            dt.Columns["service_sub_type_name"].SetOrdinal(7);
            dt.Columns["issue_time"].SetOrdinal(8);
            dt.Columns["start_time"].SetOrdinal(9);
            dt.Columns["end_time"].SetOrdinal(10);
            dt.Columns["wating_time"].SetOrdinal(11);
            dt.Columns["std_time"].SetOrdinal(12);
            dt.Columns["actual_time"].SetOrdinal(13);
            dt.Columns["variance"].SetOrdinal(14);
            dt.Columns["ISSUES"].SetOrdinal(15);
            dt.Columns["solutions"].SetOrdinal(16);
            dt.Columns["refresh_reason"].SetOrdinal(17);

            dt.Columns[6].DataType = typeof(string);
            return dt;
        }


        public List<RM_TopNService> TopNServiceReport(int branch_id, string user_id, string login_user_id, int counter_id, DateTime start_date, DateTime end_date, int topN)
        {
            DALReport dal = new DALReport();
            DataTable dt = dal.TopNServiceReport(branch_id, user_id, login_user_id, counter_id, start_date, end_date, topN);
            List<RM_TopNService> list = new List<RM_TopNService>();
            foreach (DataRow row in dt.Rows)
            {
                RM_TopNService fiveService = new RM_TopNService();

                fiveService.branch_name = (row["branch_name"] == DBNull.Value ? null : row["branch_name"].ToString());
                fiveService.service_name = (row["service_name"] == DBNull.Value ? null : row["service_name"].ToString());
                fiveService.total_service = Convert.ToInt32(row["total_service"] == DBNull.Value ? 0 : row["total_service"]);
                list.Add(fiveService);
            }
            return list;
        }

        public List<RM_TokenExceeding> TokenExceedingReport(int branch_id, string user_id, string login_user_id, int counter_id, int service_sub_type_id, DateTime start_date, DateTime end_date, int flag)
        {
            DALReport dal = new DALReport();
            DataTable dt = dal.TokenExceedingReport(branch_id, user_id, login_user_id, counter_id, service_sub_type_id, start_date, end_date, flag);
            List<RM_TokenExceeding> list = new List<RM_TokenExceeding>();
            foreach (DataRow row in dt.Rows)
            {
                RM_TokenExceeding tenMinExceeding = new RM_TokenExceeding();

                tenMinExceeding.branch_name = (row["branch_name"] == DBNull.Value ? null : row["branch_name"].ToString());
                tenMinExceeding.user_name = (row["user_name"] == DBNull.Value ? null : row["user_name"].ToString());
                tenMinExceeding.user_id = (row["user_id"] == DBNull.Value ? null : row["user_id"].ToString());
                tenMinExceeding.total_served_token = Convert.ToInt32(row["total_served_token"] == DBNull.Value ? 0 : row["total_served_token"]);
                tenMinExceeding.total_exceedig_token = Convert.ToInt32(row["total_exceeding_token"] == DBNull.Value ? 0 : row["total_exceeding_token"]);
                list.Add(tenMinExceeding);
            }
            return list;
        }


        public List<RM_LogoutDetail> LogoutDetailReport(int branch_id, string user_id, string login_user_id, int counter_id, DateTime start_date, DateTime end_date)
        {
            DALReport dal = new DALReport();
            DataTable dt = dal.LogoutDetailReport(branch_id, user_id, login_user_id, counter_id, start_date, end_date);
            List<RM_LogoutDetail> list = new List<RM_LogoutDetail>();
            foreach (DataRow row in dt.Rows)
            {
                RM_LogoutDetail logoutDetail = new RM_LogoutDetail();

                logoutDetail.branch_name = (row["branch_name"] == DBNull.Value ? null : row["branch_name"].ToString());
                logoutDetail.user_name = (row["user_name"] == DBNull.Value ? null : row["user_name"].ToString());
                logoutDetail.user_id = (row["user_id"] == DBNull.Value ? null : row["user_id"].ToString());
                logoutDetail.counter_no = (row["counter_no"] == DBNull.Value ? null : row["counter_no"].ToString());
                logoutDetail.Date = Convert.ToDateTime(row["login_time"] == DBNull.Value ? null : row["login_time"]).ToString("dd-MMM-yyyy");
                logoutDetail.login_timeV2 = Convert.ToDateTime(row["login_time"] == DBNull.Value ? null : row["login_time"]);
                logoutDetail.service_date_formated =Convert.ToDateTime( row["login_time"] == DBNull.Value ? "" : row["login_time"]).ToString("dd/MMM/yyyy");
                logoutDetail.login_time = Convert.ToDateTime(row["login_time"] == DBNull.Value ? null : row["login_time"]).ToString("hh:mm:ss tt");
                logoutDetail.login_time_formated = Convert.ToDateTime(row["login_time"] == DBNull.Value ? null : row["login_time"]).ToString("hh:mm:ss tt");
                if (row["logout_time"] != DBNull.Value) logoutDetail.logout_time_formated = Convert.ToDateTime(row["logout_time"]).ToString("hh:mm:ss tt");
                logoutDetail.duration = (row["logout_time"] == DBNull.Value ? "" : Convert.ToDateTime(row["logout_time"].ToString()).Subtract(logoutDetail.login_timeV2).ToString());
                logoutDetail.logout_reason = (row["logout_reason"] == DBNull.Value ? null : row["logout_reason"].ToString());

                list.Add(logoutDetail);

            }
            return list;
        }

        public List<AspNetUserLoginAttempts> LoginAttemptDetailsReport(int branch_id, string user_id, string login_user_id, int counter_id, int is_success, DateTime start_date, DateTime end_date)
        {
            DALReport dal = new DALReport();
            DataTable dt = dal.LoginAttemptDetailsReport(branch_id, user_id, login_user_id, counter_id, is_success, start_date, end_date);
            List<AspNetUserLoginAttempts> list = new List<AspNetUserLoginAttempts>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    AspNetUserLoginAttempts aspNetUser = new AspNetUserLoginAttempts();
                    aspNetUser.branch_name = (row["branch_name"] == DBNull.Value ? "" : row["branch_name"].ToString());
                    aspNetUser.UserName = (row["UserName"] == DBNull.Value ? "" : row["UserName"].ToString());
                    aspNetUser.FullName = (row["FullName"] == DBNull.Value ? "" : row["FullName"].ToString());
                    aspNetUser.RoleName = (row["RoleName"] == DBNull.Value ? "" : row["RoleName"].ToString());
                    aspNetUser.attempt_time = (row["attempt_time"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["attempt_time"]));
                    aspNetUser.counter_no = (row["counter_no"] == DBNull.Value ? "" : row["counter_no"].ToString());
                    aspNetUser.ip_address = (row["ip_address"] == DBNull.Value ? "" : row["ip_address"].ToString());
                    aspNetUser.machine_name = (row["machine_name"] == DBNull.Value ? "" : row["machine_name"].ToString());
                    aspNetUser.is_success = (row["is_success"] == DBNull.Value ? 0 : Convert.ToInt32(row["is_success"]));
                    aspNetUser.status = aspNetUser.is_success != 1 ? "Fail" : "Success";

                    list.Add(aspNetUser);

                    //list.Add(new AspNetUserLoginAttempts()
                    //{
                    //    branch_name = (row["branch_name"] == DBNull.Value ? "" : row["branch_name"].ToString()),
                    //    UserName = (row["UserName"] == DBNull.Value ? "" : row["UserName"].ToString()),
                    //    FullName = (row["FullName"] == DBNull.Value ? "" : row["FullName"].ToString()),
                    //    RoleName = (row["RoleName"] == DBNull.Value ? "" : row["RoleName"].ToString()),
                    //    attempt_time = (row["attempt_time"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["attempt_time"])),
                    //    counter_no = (row["counter_no"] == DBNull.Value ? "" : row["counter_no"].ToString()),
                    //    ip_address = (row["ip_address"] == DBNull.Value ? "" : row["ip_address"].ToString()),
                    //    machine_name = (row["machine_name"] == DBNull.Value ? "" : row["machine_name"].ToString()),
                    //    is_success = (row["is_success"] == DBNull.Value ? 0 : Convert.ToInt32(row["is_success"])),
                    //    status = is_success != 1 ? "Fail": "Success" 

                    //    //attempt_id = (row["attempt_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["attempt_id"])),
                    //    //LoginProvider = (row["LoginProvider"] == DBNull.Value ? "" : row["LoginProvider"].ToString()),
                    //    //branch_id = (row["branch_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["branch_id"])),
                    //    //user_id = (row["user_id"] == DBNull.Value ? "" : row["user_id"].ToString()),
                    //    // Email = (row["Email"] == DBNull.Value ? "" : row["Email"].ToString()),
                    //    // counter_id = (row["counter_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["counter_id"])),
                    //});
                }

            }
            return list;
        }

        public List<RM_CentreWiseSummary> CentreWiseSummaryReport(int branch_id, string login_user_id, int service_sub_type_id, DateTime start_date, DateTime end_date)
        {
            DALReport dal = new DALReport();
            DataTable dt = dal.CentreWiseSummaryReport(branch_id, login_user_id, service_sub_type_id, start_date, end_date);
            List<RM_CentreWiseSummary> list = new List<RM_CentreWiseSummary>();
            foreach (DataRow row in dt.Rows)
            {
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                RM_CentreWiseSummary centre = new RM_CentreWiseSummary();

                centre.branch_name = (row["branch_name"] == DBNull.Value ? "" : row["branch_name"].ToString());
                centre.handled_customer = (row["handled_customer"] == DBNull.Value ? "" : row["handled_customer"].ToString());
                centre.average_waiting_time = (row["average_waiting_time"] == DBNull.Value ? "" : row["average_waiting_time"].ToString());
                centre.total_std_time = (row["total_std_time"] == DBNull.Value ? "" : row["total_std_time"].ToString());
                centre.actual_serving_time = (row["actual_serving_time"] == DBNull.Value ? "" : row["actual_serving_time"].ToString());
                //centre.actual_serving_time = (row["actual_serving_time"] == DBNull.Value ? "" : row["actual_serving_time"].ToString());
                //centre.variance = (row["variance"] == DBNull.Value ? "" : row["variance"].ToString());
                //centre.average_service_time =(row["average_service_time"] == DBNull.Value ? "" : row["average_service_time"].ToString());

                centre.variance = Convert.ToString(DateTime.ParseExact((centre.total_std_time), "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)
                                    - DateTime.ParseExact(centre.actual_serving_time, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));

                centre.average_service_time = Convert.ToString(TimeSpan.FromMinutes(Math.Round(TimeSpan.Parse(centre.actual_serving_time).TotalMinutes
                                                / Convert.ToInt32(centre.handled_customer))));

                list.Add(centre);
            }
            return list;
        }
    }
}
