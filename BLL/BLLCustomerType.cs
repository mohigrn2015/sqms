﻿using SQMS.DAL;
using SQMS.Models;
using SQMS.Utility;
using System.Data;

namespace SQMS.BLL
{
    public class BLLCustomerType
    {
        public List<tblCustomerType> GetAll()
        {
            DALCustomerType dal = new DALCustomerType();
            DataTable dt = dal.GetAll();
            return ObjectMapping(dt);
        }
        public tblCustomerType GetById(int id)
        {
            DALCustomerType dal = new DALCustomerType();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }
        public void Create(tblCustomerType CustomerType)
        {
            DALCustomerType dal = new DALCustomerType();
            int Customer_type_id = dal.Insert(CustomerType);
            CustomerType.customer_type_id = Customer_type_id;
        }
        public void Edit(tblCustomerType CustomerType)
        {
            DALCustomerType dal = new DALCustomerType();
            dal.Update(CustomerType);

        }
        public void Remove(int id)
        {
            DALCustomerType dal = new DALCustomerType();
            dal.Delete(id);

        }
        internal List<tblCustomerType> ObjectMapping(DataTable dt)
        {
            List<tblCustomerType> list = new List<tblCustomerType>();
            var defaultTokenPrefix = ApplicationSetting.defaultTokenPrefix;
            foreach (DataRow row in dt.Rows)
            {
                tblCustomerType CustomerType = new tblCustomerType();
                CustomerType.customer_type_id = Convert.ToInt32(row["Customer_type_id"] == DBNull.Value ? 0 : row["Customer_type_id"]);
                CustomerType.customer_type_name = (row["Customer_type_name"] == DBNull.Value ? null : row["Customer_type_name"].ToString());
                CustomerType.priority = Convert.ToInt32(row["priority"] == DBNull.Value ? 0 : row["priority"]);
                CustomerType.token_prefix = (row["token_prefix"] == DBNull.Value ? defaultTokenPrefix : row["token_prefix"].ToString());
                CustomerType.is_default = Convert.ToInt32(row["is_default"] == DBNull.Value ? 0 : row["is_default"]);
                list.Add(CustomerType);

            }
            return list;
        }

        internal tblCustomerType ObjectMapping(DataRow row)
        {

            tblCustomerType CustomerType = new tblCustomerType();
            CustomerType.customer_type_id = Convert.ToInt32(row["Customer_type_id"] == DBNull.Value ? 0 : row["Customer_type_id"]);
            CustomerType.customer_type_name = (row["Customer_type_name"] == DBNull.Value ? null : row["Customer_type_name"].ToString());
            CustomerType.priority = Convert.ToInt32(row["priority"] == DBNull.Value ? 0 : row["priority"]);
            CustomerType.token_prefix = (row["token_prefix"] == DBNull.Value ? ApplicationSetting.defaultTokenPrefix : row["token_prefix"].ToString());
            CustomerType.is_default = Convert.ToInt32(row["is_default"] == DBNull.Value ? 0 : row["is_default"]);

            return CustomerType;
        }
    }
}
