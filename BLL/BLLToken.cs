using Microsoft.AspNetCore.SignalR;
using SQMS.DAL;
using SQMS.Models;
using SQMS.Models.ViewModels;
using SQMS.SignalRHub;
using SQMS.Utility;
using System.Data;

namespace SQMS.BLL
{
    public class BLLToken
    {
        private readonly IHubContext<notifyDisplay> _hubContext;
        public BLLToken(IHubContext<notifyDisplay> hubContext)
        {
             _hubContext = hubContext;
        }
        public BLLToken()
        {
                
        }
        public List<VMTokenQueue> GetAll()
        {
            DALToken dal = new DALToken();
            DataTable dt = dal.GetAll();
            return ObjectMappingList(dt);
        }

        public List<tblTokenQueue> GetBy(DateTime from_date, DateTime to_date, int token_id = 0, int branch_id = 0, int service_type_id = 0, int service_status_id = 0,
            int counter_id = 0, string user_id = null, string token_prefix = null, string contact_no = null)
        {
            DALToken dal = new DALToken();
            DataTable dt = dal.GetBy(from_date, to_date, token_id, branch_id, service_type_id, service_status_id, counter_id, user_id, token_prefix, contact_no);
            return ObjectMappingListToken(dt);
        }

        public List<VMTokenSkipped> GetSkipped(int? branch_id, string user_id)
        {
            DALToken dal = new DALToken();
            DataTable dt = dal.GetSkipped(branch_id, user_id);
            return ObjectMappingList_SkippedList(dt);
        }

        public List<VMTokenQueue> GetByBranchId(int branch_id)
        {
            DALToken dal = new DALToken();
            DataTable dt = dal.GetByBranchId(branch_id);
            return ObjectMappingList(dt);
        }

        public List<tblTokenQueue> GetAllToken()
        {
            DALToken dal = new DALToken();
            DataTable dt = dal.GetAll();
            return ObjectMappingListToken(dt);
        }
        public string GetNextTokenList(int branch_id)
        {
            DALToken dal = new DALToken();
            return dal.GetNextTokenList(branch_id);
        }
        public List<VMTokenProgress> GetProgressTokenList(int branch_id)
        {
            DALToken dal = new DALToken();
            DataTable dt = dal.GetProgressTokenList(branch_id);
            return ObjectMappingList_ProgressTokenList(dt);
        }
        internal List<VMTokenQueue> ObjectMappingList(DataTable dt)
        {
            List<VMTokenQueue> list = new List<VMTokenQueue>();
            foreach (DataRow row in dt.Rows)
            {
                VMTokenQueue token = new VMTokenQueue();
                token.token_id = Convert.ToInt64(row["token_id"] == DBNull.Value ? 0 : row["token_id"]);
                token.token_prefix = (row["token_prefix"] == DBNull.Value ? ApplicationSetting.defaultTokenPrefix : row["token_prefix"].ToString());
                token.token_no = Convert.ToInt32(row["token_no"] == DBNull.Value ? null : row["token_no"].ToString());
                if (dt.Columns.Contains("counter_no")) token.counter_no = (row["counter_no"] == DBNull.Value ? null : row["counter_no"].ToString());
                token.branch_name = (row["branch_name"] == DBNull.Value ? null : row["branch_name"].ToString());
                token.contact_no = (row["contact_no"] == DBNull.Value ? null : row["contact_no"].ToString());
                token.service_date = Convert.ToDateTime(row["service_date"] == DBNull.Value ? null : row["service_date"].ToString());

                token.service_status = (row["service_status"] == DBNull.Value ? null : row["service_status"].ToString());
                if (dt.Columns.Contains("service_status_id")) token.service_status_id = Convert.ToInt16(row["service_status_id"] == DBNull.Value ? 0 : row["service_status_id"]);

                list.Add(token);

            }
            return list;
        }
        internal List<tblTokenQueue> ObjectMappingListToken(DataTable dt)
        {
            List<tblTokenQueue> list = new List<tblTokenQueue>();
            foreach (DataRow row in dt.Rows)
            {

                tblTokenQueue token = new tblTokenQueue();
                token.token_id = Convert.ToInt64(row["token_id"] == DBNull.Value ? 0 : row["token_id"]);
                token.token_prefix = (row["token_prefix"] == DBNull.Value ? ApplicationSetting.defaultTokenPrefix : row["token_prefix"].ToString());
                token.token_no = Convert.ToInt32(row["token_no"] == DBNull.Value ? null : row["token_no"].ToString());

                token.contact_no = (row["contact_no"] == DBNull.Value ? null : row["contact_no"].ToString());
                token.service_date = Convert.ToDateTime(row["service_date"] == DBNull.Value ? null : row["service_date"].ToString());

                if (dt.Columns.Contains("branch_id")) token.branch_id = Convert.ToInt32(row["branch_id"] == DBNull.Value ? 0 : row["branch_id"]);
                if (dt.Columns.Contains("branch_name")) token.branch_name = (row["branch_name"] == DBNull.Value ? null : row["branch_name"].ToString());
                if (dt.Columns.Contains("service_type_id")) token.service_type_id = Convert.ToInt32(row["service_type_id"] == DBNull.Value ? 0 : row["service_type_id"]);
                if (dt.Columns.Contains("service_type_name")) token.service_type_name = (row["service_type_name"] == DBNull.Value ? null : row["service_type_name"].ToString());
                if (dt.Columns.Contains("cancel_time")) token.cancel_time = Convert.ToDateTime(row["cancel_time"] == DBNull.Value ? null : row["cancel_time"].ToString());
                if (dt.Columns.Contains("service_status_id")) token.service_status_id = Convert.ToInt16(row["service_status_id"] == DBNull.Value ? 0 : row["service_status_id"]);
                if (dt.Columns.Contains("service_status")) token.service_status = (row["service_status"] == DBNull.Value ? null : row["service_status"].ToString());
                if (dt.Columns.Contains("counter_id")) token.counter_id = Convert.ToInt32(row["counter_id"] == DBNull.Value ? 0 : row["counter_id"]);
                if (dt.Columns.Contains("counter_no")) token.counter_no = (row["counter_no"] == DBNull.Value ? null : row["counter_no"].ToString());
                if (dt.Columns.Contains("user_id")) token.user_id = (row["user_id"] == DBNull.Value ? null : row["user_id"].ToString());
                if (dt.Columns.Contains("username")) token.counter_no = (row["username"] == DBNull.Value ? null : row["username"].ToString());
                if (dt.Columns.Contains("hometown")) token.fullname = (row["hometown"] == DBNull.Value ? null : row["hometown"].ToString());
                if (dt.Columns.Contains("calltime")) token.CallTime = Convert.ToDateTime(row["calltime"] == DBNull.Value ? null : row["calltime"].ToString());

                list.Add(token);

            }
            return list;
        }
        internal List<VMNextToken> ObjectMappingList_GetNextTokenList(DataTable dt)
        {
            List<VMNextToken> list = new List<VMNextToken>();
            foreach (DataRow row in dt.Rows)
            {
                VMNextToken token = new VMNextToken();
                token.token_prefix = (row["token_prefix"] == DBNull.Value ? ApplicationSetting.defaultTokenPrefix : row["token_prefix"].ToString());
                token.token_no = Convert.ToInt64(row["token_no"] == DBNull.Value ? null : row["token_no"]);
                token.display_next = Convert.ToInt32(row["display_next"] == DBNull.Value ? null : row["display_next"]);
                token.static_ip = (row["static_ip"] == DBNull.Value ? null : row["static_ip"].ToString());

                list.Add(token);

            }
            return list;
        }
        internal List<VMTokenProgress> ObjectMappingList_ProgressTokenList(DataTable dt)
        {
            List<VMTokenProgress> list = new List<VMTokenProgress>();

            foreach (DataRow row in dt.Rows)
            {
                VMTokenProgress token = new VMTokenProgress();

                token.token_prefix = (row["token_prefix"] == DBNull.Value ? ApplicationSetting.defaultTokenPrefix : row["token_prefix"].ToString());

                token.token_no = (row["token_no"] == DBNull.Value ? ApplicationSetting.DisplayWhenEmptyToken : row["token_no"].ToString().PadLeft(ApplicationSetting.PaddingLeft, '0'));

                token.counter_no = (row["counter_no"] == DBNull.Value ? null : row["counter_no"].ToString());

                token.static_ip = (row["static_ip"] == DBNull.Value ? null : row["static_ip"].ToString());

                list.Add(token);

            }
            return list;
        }

        internal List<VMTokenSkipped> ObjectMappingList_SkippedList(DataTable dt)
        {
            List<VMTokenSkipped> list = new List<VMTokenSkipped>();

            foreach (DataRow row in dt.Rows)
            {
                VMTokenSkipped token = new VMTokenSkipped();

                token.token_id = Convert.ToInt64(row["token_id"] == DBNull.Value ? 0 : row["token_id"]);
                token.token_prefix = (row["token_prefix"] == DBNull.Value ? ApplicationSetting.defaultTokenPrefix : row["token_prefix"].ToString());
                token.token_no = Convert.ToInt32(row["token_no"] == DBNull.Value ? null : row["token_no"].ToString());
                token.branch_name = (row["branch_name"] == DBNull.Value ? null : row["branch_name"].ToString());
                token.counter_no = (row["counter_no"] == DBNull.Value ? null : row["counter_no"].ToString());
                token.service_date = Convert.ToDateTime(row["service_date"] == DBNull.Value ? null : row["service_date"].ToString());
                token.service_status_id = Convert.ToInt16(row["service_status_id"] == DBNull.Value ? 0 : row["service_status_id"]);
                token.service_status = (row["service_status"] == DBNull.Value ? null : row["service_status"].ToString());
                token.contact_no = (row["contact_no"] == DBNull.Value ? null : row["contact_no"].ToString());
                token.customer_name = (row["customer_name"] == DBNull.Value ? null : row["customer_name"].ToString());
                token.cancel_time = Convert.ToDateTime(row["cancel_time"] == DBNull.Value ? null : row["cancel_time"].ToString());
                token.user_full_name = (row["HOMETOWN"] == DBNull.Value ? null : row["HOMETOWN"].ToString());

                list.Add(token);
            }
            return list;
        }

        public async Task Create(tblTokenQueue token)
        {
            
            notifyDisplay notify = new notifyDisplay(_hubContext);
            DALToken dal = new DALToken();
            DataTable dt = dal.Insert(token);
            if(dt != null)
            {
                if(dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        token.counter_id = Convert.ToInt32(row["po_counter_id"] == DBNull.Value ? null : row["po_counter_id"].ToString());
                        token.token_no = Convert.ToInt32(row["po_token_no"] == DBNull.Value ? null : row["po_token_no"].ToString());
                        token.token_prefix = row["po_token_prefix"] == DBNull.Value ? null : row["po_token_prefix"].ToString();
                    }
                    if (token.counter_id > 0) await notify.CallToken(Convert.ToInt32(token.counter_id));
                }
            }
            
        }

        public void ReInitiate(long token_id)
        {
            DALToken dal = new DALToken();
            dal.ReInitiate(token_id);
        }
        public void AssignToMe(long token_id, int counter_id)
        {
            DALToken dal = new DALToken();
            dal.AssignToMe(token_id, counter_id);
        }
        public void SendSMS(string msisdn, string tokenNo)
        {
            DALSMSManager dal = new DALSMSManager();
            if (ApplicationSetting.isMsgBn)
            {
                dal.SendSMSBn(msisdn, tokenNo, tokenNo);
            }
            else
            {
                dal.SendSMS(msisdn, string.Format(ApplicationSetting.msgText, tokenNo));
            }
        }
    }
}
