using SQMS.DAL;
using SQMS.Models;
using SQMS.Models.ViewModels;
using System.Data;

namespace SQMS.BLL
{
    public class BLLBranch
    {
        public List<tblBranch> GetAllBranch()
        {
            DALBranch dal = new DALBranch();
            DataTable dt = dal.GetAllBranch();
            return ObjectMappingList(dt);
        }

        public List<tblBranch> GetBranchesByUserId(string userId)
        {
            DALBranch dal = new DALBranch();
            DataTable dt = dal.GetBranchesByUserId(userId);
            return ObjectMappingList(dt);
        }

        public List<VMBranchCounterStatus> GetCounterCurrentStatus(int branch_id, int counter_id)
        {
            DALBranch dal = new DALBranch();
            DataTable dt = dal.GetCounterCurrentStatus(branch_id, counter_id);
            return ObjectMappingListCounterStatus(dt);
        }

        internal List<VMBranchCounterStatus> ObjectMappingListCounterStatus(DataTable dt)
        {
            List<VMBranchCounterStatus> list = new List<VMBranchCounterStatus>();
            foreach (DataRow row in dt.Rows)
            {
                VMBranchCounterStatus branch = new VMBranchCounterStatus();
                branch.branch_id = Convert.ToInt32(row["branch_id"] == DBNull.Value ? 0 : row["branch_id"]);
                branch.branch_name = (row["branch_name"] == DBNull.Value ? null : row["branch_name"].ToString());
                branch.counter_id = Convert.ToInt32(row["counter_id"] == DBNull.Value ? 0 : row["counter_id"]);
                branch.counter_no = (row["counter_no"] == DBNull.Value ? null : row["counter_no"].ToString());
                branch.token_id = Convert.ToInt64(row["token_id"] == DBNull.Value ? 0 : row["token_id"]);
                branch.token_prefix = (row["token_prefix"] == DBNull.Value ? null : row["token_prefix"].ToString());
                branch.token_no = Convert.ToInt32(row["token_no"] == DBNull.Value ? 0 : row["token_no"]);
                if (row["call_time"] != DBNull.Value) branch.call_time = Convert.ToDateTime(row["call_time"].ToString());
                branch.user_id = (row["user_id"] == DBNull.Value ? null : row["user_id"].ToString());
                branch.user_name = (row["user_name"] == DBNull.Value ? null : row["user_name"].ToString());
                branch.user_full_name = (row["user_full_name"] == DBNull.Value ? null : row["user_full_name"].ToString());
                if (row["login_time"] != DBNull.Value) branch.login_time = Convert.ToDateTime(row["login_time"].ToString());
                if (row["logout_time"] != DBNull.Value) branch.logout_time = Convert.ToDateTime(row["logout_time"].ToString());
                branch.is_idle = Convert.ToInt32(row["is_idle"] == DBNull.Value ? 0 : row["is_idle"]);
                if (branch.is_idle == 1) branch.idle_from = Convert.ToDateTime(row["idle_from"].ToString());
                list.Add(branch);

            }
            return list;
        }
        internal List<tblBranch> ObjectMappingList(DataTable dt)
        {
            List<tblBranch> list = new List<tblBranch>();
            foreach (DataRow row in dt.Rows)
            {
                tblBranch branch = new tblBranch();
                branch.branch_id = Convert.ToInt32(row["branch_id"] == DBNull.Value ? 0 : row["branch_id"]);
                branch.branch_name = (row["branch_name"] == DBNull.Value ? null : row["branch_name"].ToString());
                branch.contact_no = (row["contact_no"] == DBNull.Value ? null : row["contact_no"].ToString());
                branch.contact_person = (row["contact_person"] == DBNull.Value ? null : row["contact_person"].ToString());
                branch.address = (row["address"] == DBNull.Value ? null : row["address"].ToString());
                branch.static_ip = (row["static_ip"] == DBNull.Value ? null : row["static_ip"].ToString());

                branch.display_next = Convert.ToInt32(row["display_next"] == DBNull.Value ? null : row["display_next"].ToString());

                list.Add(branch);

            }
            return list;
        }
        public tblBranch GetById(int id)
        {
            DALBranch dal = new DALBranch();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }
        public void Create(tblBranch branch)
        {
            DALBranch dal = new DALBranch();
            int branch_id = dal.Insert(branch);
            branch.branch_id = branch_id;
        }
        public void Edit(tblBranch branch)
        {
            DALBranch dal = new DALBranch();
            dal.Update(branch);

        }
        public void Remove(int id)
        {
            DALBranch dal = new DALBranch();
            dal.Delete(id);

        }
        //public void Dispose()
        //{
        //    Dispose(true);

        //}
        internal tblBranch ObjectMapping(DataRow row)
        {

            tblBranch branch = new tblBranch();
            branch.branch_id = Convert.ToInt32(row["branch_id"] == DBNull.Value ? 0 : row["branch_id"]);
            branch.branch_name = (row["branch_name"] == DBNull.Value ? null : row["branch_name"].ToString());
            branch.contact_no = (row["contact_no"] == DBNull.Value ? null : row["contact_no"].ToString());
            branch.contact_person = (row["contact_person"] == DBNull.Value ? null : row["contact_person"].ToString());
            branch.address = (row["address"] == DBNull.Value ? null : row["address"].ToString());
            branch.static_ip = (row["static_ip"] == DBNull.Value ? null : row["static_ip"].ToString());

            branch.display_next = Convert.ToInt32(row["display_next"] == DBNull.Value ? null : row["display_next"].ToString());


            return branch;
        }
    }
}
