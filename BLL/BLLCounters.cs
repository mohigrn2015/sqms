using SQMS.DAL;
using SQMS.Models;
using SQMS.Models.ViewModels;
using System.Data;

namespace SQMS.BLL
{
    public class BLLCounters
    {
        public List<VMCounter> GetAll()
        {
            DALCounters dal = new DALCounters();
            DataTable dt = dal.GetAll();
            return ObjectMappingListVM(dt);
        }
        public List<tblCounter> GetFree(int branch_id, string user_id)
        {
            DALCounters dal = new DALCounters();
            DataTable dt = dal.GetFree(branch_id, user_id);
            return ObjectMappingList(dt);
        }
        public List<tblCounter> GetAllCounter()
        {
            DALCounters dal = new DALCounters();
            DataTable dt = dal.GetAll();
            return ObjectMappingList(dt);
        }
        public tblCounter GetById(int id)
        {
            DALCounters dal = new DALCounters();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }
        public void Create(tblCounter counter)
        {
            DALCounters dal = new DALCounters();
            int counter_id = dal.Insert(counter);
            counter.counter_id = counter_id;
        }
        public void Edit(tblCounter counter)
        {
            DALCounters dal = new DALCounters();
            dal.Update(counter);

        }
        public void Remove(int id)
        {
            DALCounters dal = new DALCounters();
            dal.Delete(id);

        }

        public List<tblCounter> GetCounterByBrunchId(int id)
        {
            DALCounters dal = new DALCounters();
            DataTable dt = dal.GetCountersByBrunchId(id);
            if (dt.Rows.Count > 0)
                return ObjectMappingList(dt);
            else return null;
        }

        internal tblCounter ObjectMapping(DataRow row)
        {

            tblCounter counter = new tblCounter();
            counter.counter_id = Convert.ToInt32(row["counter_id"] == DBNull.Value ? 0 : row["counter_id"]);
            counter.counter_no = (row["counter_no"] == DBNull.Value ? null : row["counter_no"].ToString());
            counter.location = (row["location"] == DBNull.Value ? null : row["location"].ToString());
            counter.branch_id = Convert.ToInt32(row["branch_id"] == DBNull.Value ? null : row["branch_id"].ToString());
            //if (row.Table.Columns.Contains("is_active")) counter.is_active = Convert.ToInt32(row["is_active"] == DBNull.Value ? 0 : row["is_active"]);

            return counter;
        }
        internal List<VMCounter> ObjectMappingListVM(DataTable dt)
        {
            List<VMCounter> list = new List<VMCounter>();
            foreach (DataRow row in dt.Rows)
            {
                VMCounter counter = new VMCounter();
                counter.counter_id = Convert.ToInt32(row["counter_id"] == DBNull.Value ? 0 : row["counter_id"]);
                counter.counter_no = (row["counter_no"] == DBNull.Value ? null : row["counter_no"].ToString());
                counter.branch_name = (row["branch_name"] == DBNull.Value ? null : row["branch_name"].ToString());
                counter.location = (row["location"] == DBNull.Value ? null : row["location"].ToString());
                if (dt.Columns.Contains("is_active")) counter.is_active = Convert.ToInt32(row["is_active"] == DBNull.Value ? 0 : row["is_active"]);

                list.Add(counter);

            }
            return list;
        }
        internal List<tblCounter> ObjectMappingList(DataTable dt)
        {
            List<tblCounter> list = new List<tblCounter>();
            foreach (DataRow row in dt.Rows)
            {
                var counter = ObjectMapping(row);

                list.Add(counter);

            }
            return list;
        }
    }
}
