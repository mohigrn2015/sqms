﻿using SQMS.DAL;
using SQMS.Models;
using SQMS.Models.ViewModels;
using System.Data;

namespace SQMS.BLL
{
    public class BLLServiceSubType
    {
        public List<VMServiceType> GetAll()
        {
            DALServiceSubType dal = new DALServiceSubType();
            DataTable dt = dal.GetAll();
            return ObjectMappingListVM(dt);
        }

        public List<VMServiceType> GetByTypeId(int service_type_id, Nullable<int> is_active = null)
        {
            DALServiceSubType dal = new DALServiceSubType();
            DataTable dt = dal.GetByTypeId(service_type_id);
            if (is_active.HasValue)
            {
                DataRow[] dataRows = dt.Select("is_active=" + is_active.Value.ToString());
                return ObjectMappingListTATVM(dataRows.CopyToDataTable());
            }
            else
                return ObjectMappingListTATVM(dt);
        }

        public tblServiceSubType GetById(int id)
        {
            DALServiceSubType dal = new DALServiceSubType();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }
        public void Create(tblServiceSubType servicesubType)
        {
            DALServiceSubType dal = new DALServiceSubType();
            int service_sub_type_id = dal.Insert(servicesubType);
            servicesubType.service_sub_type_id = service_sub_type_id;
        }
        public void Edit(tblServiceSubType servicesubType)
        {
            DALServiceSubType dal = new DALServiceSubType();
            dal.Update(servicesubType);

        }

        public void SetStatus(int service_sub_type_id, int is_active)
        {
            DALServiceSubType dal = new DALServiceSubType();
            dal.SetStatus(service_sub_type_id, is_active);

        }

        public void Remove(int id)
        {
            DALServiceSubType dal = new DALServiceSubType();
            dal.Delete(id);

        }

        public void UpdateTatBulk(string sub_type_id, int time)
        {
            DALServiceSubType dal = new DALServiceSubType();
            dal.UpdateTatBulk(sub_type_id, time);
        }

        internal tblServiceSubType ObjectMapping(DataRow row)
        {

            tblServiceSubType servicesubType = new tblServiceSubType();
            servicesubType.service_sub_type_id = Convert.ToInt32(row["service_sub_type_id"] == DBNull.Value ? 0 : row["service_sub_type_id"]);
            servicesubType.service_sub_type_name = (row["service_sub_type_name"] == DBNull.Value ? null : row["service_sub_type_name"].ToString());
            servicesubType.service_type_id = Convert.ToInt32(row["service_type_id"] == DBNull.Value ? null : row["service_type_id"].ToString());
            servicesubType.max_duration = Convert.ToInt32(row["max_duration"] == DBNull.Value ? null : row["max_duration"].ToString());
            servicesubType.is_active = Convert.ToInt32(row["is_active"] == DBNull.Value ? null : row["is_active"].ToString());
            servicesubType.tat_warning_time = Convert.ToInt32(row["tat_warning_time"] == DBNull.Value ? null : row["tat_warning_time"].ToString());
            return servicesubType;
        }

        internal List<tblServiceSubType> ObjectMappingListTBL(DataTable dt)
        {
            List<tblServiceSubType> list = new List<tblServiceSubType>();
            foreach (DataRow row in dt.Rows)
            {
                tblServiceSubType services = new tblServiceSubType();
                services.service_sub_type_id = Convert.ToInt32(row["service_sub_type_id"] == DBNull.Value ? 0 : row["service_sub_type_id"]);
                services.service_sub_type_name = (row["service_sub_type_name"] == DBNull.Value ? null : row["service_sub_type_name"].ToString());
                services.max_duration = Convert.ToInt32(row["max_duration"] == DBNull.Value ? null : row["max_duration"].ToString());
                services.service_type_id = Convert.ToInt32(row["service_type_id"] == DBNull.Value ? null : row["service_type_id"].ToString());
                services.is_active = Convert.ToInt32(row["is_active"] == DBNull.Value ? null : row["is_active"].ToString());
                services.tat_warning_time = Convert.ToInt32(row["tat_warning_time"] == DBNull.Value ? null : row["tat_warning_time"].ToString());
                list.Add(services);

            }
            return list;
        }

        internal List<VMServiceType> ObjectMappingListVM(DataTable dt)
        {
            List<VMServiceType> list = new List<VMServiceType>();
            foreach (DataRow row in dt.Rows)
            {
                VMServiceType services = new VMServiceType();
                services.service_sub_type_id = Convert.ToInt32(row["service_sub_type_id"] == DBNull.Value ? 0 : row["service_sub_type_id"]);
                services.service_sub_type_name = (row["service_sub_type_name"] == DBNull.Value ? null : row["service_sub_type_name"].ToString());
                services.max_duration = Convert.ToInt32(row["max_duration"] == DBNull.Value ? null : row["max_duration"].ToString());
                services.service_type_name = (row["service_type_name"] == DBNull.Value ? null : row["service_type_name"].ToString());
                services.is_active = Convert.ToInt32(row["is_active"] == DBNull.Value ? null : row["is_active"].ToString());
                list.Add(services);

            }
            return list;
        }

        internal List<VMServiceType> ObjectMappingListTATVM(DataTable dt)
        {
            List<VMServiceType> list = new List<VMServiceType>();
            foreach (DataRow row in dt.Rows)
            {
                VMServiceType services = new VMServiceType();
                services.service_sub_type_id = Convert.ToInt32(row["service_sub_type_id"] == DBNull.Value ? 0 : row["service_sub_type_id"]);
                services.service_sub_type_name = (row["service_sub_type_name"] == DBNull.Value ? null : row["service_sub_type_name"].ToString());
                services.max_duration = Convert.ToInt32(row["max_duration"] == DBNull.Value ? null : row["max_duration"].ToString());
                services.service_type_name = (row["service_type_name"] == DBNull.Value ? null : row["service_type_name"].ToString());
                services.is_active = Convert.ToInt32(row["is_active"] == DBNull.Value ? null : row["is_active"].ToString());
                services.tat_warning_time = Convert.ToInt32(row["tat_warning_time"] == DBNull.Value ? null : row["tat_warning_time"].ToString());
                list.Add(services);
            }
            return list;
        }
    }
}
