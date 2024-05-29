using SQMS.DAL;
using SQMS.Models.ViewModels;
using SQMS.Models;
using System.Data;

namespace SQMS.BLL
{
    public class BLLBranchUsers
    {
        public List<VMBranchLogin> GetAll()
        {
            DALBranchUsers dal = new DALBranchUsers();
            DataTable dt = dal.GetAll();
            return ObjectMappingList(dt);
        }

        internal List<VMBranchLogin> ObjectMappingList(DataTable dt)
        {
            List<VMBranchLogin> list = new List<VMBranchLogin>();
            foreach (DataRow row in dt.Rows)
            {
                VMBranchLogin branchuser = new VMBranchLogin();
                branchuser.user_branch_id = Convert.ToInt32(row["user_branch_id"] == DBNull.Value ? 0 : row["user_branch_id"]);
                branchuser.branch_id = Convert.ToInt32(row["branch_id"] == DBNull.Value ? 0 : row["branch_id"]);
                branchuser.branch_name = (row["branch_name"] == DBNull.Value ? null : row["branch_name"].ToString());
                branchuser.user_id = (row["user_id"] == DBNull.Value ? null : row["user_id"].ToString());
                branchuser.Hometown = (row["Hometown"] == DBNull.Value ? null : row["Hometown"].ToString());
                branchuser.UserName = (row["UserName"] == DBNull.Value ? null : row["UserName"].ToString());
                if (dt.Columns.Contains("PhoneNumber")) branchuser.PhoneNumber = (row["PhoneNumber"] == DBNull.Value ? null : row["PhoneNumber"].ToString());
                if (dt.Columns.Contains("Email")) branchuser.Email = (row["Email"] == DBNull.Value ? null : row["Email"].ToString());
                branchuser.Name = (row["Name"] == DBNull.Value ? null : row["Name"].ToString());
                if (dt.Columns.Contains("is_active")) branchuser.is_active = Convert.ToInt32(row["is_active"] == DBNull.Value ? 0 : row["is_active"]);
                if (dt.Columns.Contains("ISACTIVEDIRECTORYUSER")) branchuser.is_active_directory_user = Convert.ToInt32(row["ISACTIVEDIRECTORYUSER"] == DBNull.Value ? 0 : row["ISACTIVEDIRECTORYUSER"]);

                list.Add(branchuser);
            }
            return list;
        }
        public tblBranchUser GetById(int id)
        {
            DALBranchUsers dal = new DALBranchUsers();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }
        public void Create(tblBranchUser branchuser)
        {
            DALBranchUsers dal = new DALBranchUsers();
            int user_branch_id = dal.Insert(branchuser);
            branchuser.user_branch_id = user_branch_id;
        }
        public void Edit(tblBranchUser branchuser)
        {
            DALBranchUsers dal = new DALBranchUsers();
            dal.Update(branchuser);
        }
        public void Remove(int id)
        {
            DALBranchUsers dal = new DALBranchUsers();
            dal.Delete(id);

        }
        internal tblBranchUser ObjectMapping(DataRow row)
        {

            tblBranchUser branchuser = new tblBranchUser();
            branchuser.branch_id = Convert.ToInt32(row["branch_id"] == DBNull.Value ? 0 : row["branch_id"]);
            branchuser.user_id = (row["user_id"] == DBNull.Value ? null : row["user_id"].ToString());


            return branchuser;
        }
        public void SyncUsers()
        {
            DALBranchUsers branchUsers = new DALBranchUsers();
            branchUsers.SyncUser();
        }
    }
}
