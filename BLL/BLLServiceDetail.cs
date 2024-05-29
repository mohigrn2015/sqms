﻿using SQMS.Models.ViewModels;
using SQMS.Models;
using System.Data;
using SQMS.DAL;
//using SQMS.Models.MetaModels;

namespace SQMS.BLL
{
    public class BLLServiceDetail
    {
        public List<VMServiceDetails> GetAllCurrentDate(int? branch_id, string user_id)
        {
            DALServiceDetail dal = new DALServiceDetail();
            DataTable dt = dal.GetAllCurrentDate(branch_id, user_id);
            return ObjectMappingListVM(dt);
        }
        public List<tblServiceDetail> GetAllService(int? branch_id, string user_id)
        {
            DALServiceDetail dal = new DALServiceDetail();
            DataTable dt = dal.GetAllServices(branch_id, user_id);
            return ObjectMappingList(dt);
        }

        public List<tblServiceDetail> GetByCustomerID(long customer_id)
        {
            DALServiceDetail dal = new DALServiceDetail();
            DataTable dt = dal.GetByCustomerID(customer_id);
            return ObjectMappingList(dt);
        }

        public List<tblServiceSubType> GetNewToken(int branch_id, int counter_id, string userid, out long token_id, out string token_prefix, out int token_no, out string contact_no, out string service_type, out DateTime start_time, out string customer_name, out string address, out DateTime generate_time, out int is_break)
        {
            List<tblServiceSubType> serviceSubTypeList = new List<tblServiceSubType>();
            DALServiceDetail dal = new DALServiceDetail();
            serviceSubTypeList = dal.GetNewToken(branch_id, counter_id, userid, out token_id, out token_prefix, out token_no, out contact_no, out service_type, out start_time, out customer_name, out address, out generate_time, out is_break);

            return serviceSubTypeList;
        }
        public List<tblServiceSubType> CallManualToken(int branch_id, int counter_id, string userid, string token_no, out long token_id, out string contact_no, out string service_type, out DateTime start_time, out string customer_name, out string address)
        {

            DALServiceDetail dal = new DALServiceDetail();
            List<tblServiceSubType> serviceSubTypeList = dal.CallManualToken(branch_id, counter_id, userid, token_no, out token_id, out contact_no, out service_type, out start_time, out customer_name, out address);

            if (serviceSubTypeList.Count == 0) throw new Exception("Token number is not found or not free to call");

            //BLLServiceSubType serviceSubTypeManager = new BLLServiceSubType();
            //List<tblServiceSubType> serviceSubTypeList = serviceSubTypeManager.ObjectMappingListTBL(dt);

            return serviceSubTypeList;
        }
        public int Transfer(int branch_id, string counter_no, long token_id) //Added return value
        {
            DALServiceDetail dal = new DALServiceDetail();
            return dal.Transfer(branch_id, counter_no, token_id);  //Added return
        }
        public void GetNextTokenList(int token_id)
        {
            DALServiceDetail dal = new DALServiceDetail();
            dal.CancelToken(token_id);
            //ObjectMappingList_GetNextTokenList(dt);
        }
        public void Create(VMServiceDetails servicedetail)
        {
            DALServiceDetail dal = new DALServiceDetail();
            int service_id = dal.Insert(servicedetail);
            servicedetail.service_id = service_id;
        }

        public void AddService(VMServiceDetails servicedetail)
        {
            DALServiceDetail dal = new DALServiceDetail();
            int service_id = dal.AddService(servicedetail);
            servicedetail.service_id = service_id;
        }

        public int CancelToken(long token_id)
        {
            DALServiceDetail dal = new DALServiceDetail();
            return dal.CancelToken(token_id);

        }
        internal List<VMServiceDetails> ObjectMappingListVM(DataTable dt)
        {
            List<VMServiceDetails> list = new List<VMServiceDetails>();
            foreach (DataRow row in dt.Rows)
            {
                VMServiceDetails servicedetail = new VMServiceDetails();
                //servicedetail.counter_id = Convert.ToInt32(row["counter_id"] == DBNull.Value ? 0 : row["counter_id"]);
                servicedetail.counter_no = (row["counter_no"] == DBNull.Value ? null : row["counter_no"].ToString());
                servicedetail.branch_name = (row["branch_name"] == DBNull.Value ? null : row["branch_name"].ToString());
                servicedetail.UserName = (row["UserName"] == DBNull.Value ? null : row["UserName"].ToString());
                servicedetail.service_datetime = Convert.ToDateTime(row["service_datetime"] == DBNull.Value ? null : row["service_datetime"].ToString());
                servicedetail.start_time = Convert.ToDateTime(row["start_time"] == DBNull.Value ? null : row["start_time"].ToString());
                servicedetail.end_time = Convert.ToDateTime(row["end_time"] == DBNull.Value ? null : row["end_time"].ToString());
                servicedetail.customer_name = (row["customer_name"] == DBNull.Value ? null : row["customer_name"].ToString());
                servicedetail.contact_no = (row["contact_no"] == DBNull.Value ? null : row["contact_no"].ToString());
                servicedetail.issues = (row["issues"] == DBNull.Value ? null : row["issues"].ToString());
                servicedetail.solutions = (row["solutions"] == DBNull.Value ? null : row["solutions"].ToString());
                servicedetail.service_sub_type_name = (row["service_sub_type_name"] == DBNull.Value ? null : row["service_sub_type_name"].ToString());




                list.Add(servicedetail);

            }
            return list;
        }
        internal List<tblServiceDetail> ObjectMappingList(DataTable dt)
        {
            List<tblServiceDetail> list = new List<tblServiceDetail>();
            foreach (DataRow row in dt.Rows)
            {
                tblServiceDetail servicedetail = new tblServiceDetail();

                servicedetail.start_time = Convert.ToDateTime(row["start_time"] == DBNull.Value ? null : row["start_time"].ToString());
                servicedetail.end_time = Convert.ToDateTime(row["end_time"] == DBNull.Value ? null : row["end_time"].ToString());
                servicedetail.service_id = Convert.ToInt32(row["service_id"] == DBNull.Value ? 0 : row["service_id"]);
                servicedetail.service_datetime = Convert.ToDateTime(row["service_datetime"] == DBNull.Value ? null : row["service_datetime"].ToString());
                servicedetail.token_id = Convert.ToInt32(row["token_id"] == DBNull.Value ? null : row["token_id"].ToString());

                servicedetail.issues = (row["issues"] == DBNull.Value ? null : row["issues"].ToString());
                servicedetail.solutions = (row["solutions"] == DBNull.Value ? null : row["solutions"].ToString());
                servicedetail.customer_id = Convert.ToInt32(row["customer_id"] == DBNull.Value ? 0 : row["customer_id"]);




                list.Add(servicedetail);

            }
            return list;
        }
    }
}