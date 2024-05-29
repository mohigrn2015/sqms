using SQMS.DAL;
using SQMS.Models.ViewModels;
using System.Data;

namespace SQMS.BLL
{
    public class BLLBranchPlayList
    {
        public List<VMBranchPlayList> GetAll()
        {
            DALBranchPlayList dal = new DALBranchPlayList();
            DataTable dt = dal.GetAll();
            return ObjectMappingList(dt);
        }

        internal VMBranchPlayList ObjectMapping(DataRow row)
        {

            VMBranchPlayList playlist = new VMBranchPlayList();
            playlist.branch_playlist_id = Convert.ToInt32(row["BRANCH_playlist_ID"] == DBNull.Value ? 0 : row["BRANCH_playlist_ID"]);
            playlist.playlist_id = Convert.ToInt32(row["playlist_ID"] == DBNull.Value ? 0 : row["playlist_ID"]);
            playlist.playlist_name = (row["playlist_name"] == DBNull.Value ? "" : row["playlist_name"].ToString());
            playlist.branch_id = Convert.ToInt32(row["BRANCH_ID"] == DBNull.Value ? 0 : row["BRANCH_ID"]);
            playlist.branch_name = (row["BRANCH_NAME"] == DBNull.Value ? "" : row["BRANCH_NAME"].ToString());
            return playlist;
        }

        internal List<VMBranchPlayList> ObjectMappingList(DataTable dt)
        {
            List<VMBranchPlayList> list = new List<VMBranchPlayList>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ObjectMapping(row));

            }
            return list;
        }
        public VMBranchPlayList GetById(int id)
        {
            DALBranchPlayList dal = new DALBranchPlayList();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }

        public void Create(VMBranchPlayList playlist)
        {
            DALBranchPlayList dal = new DALBranchPlayList();
            int playlist_id = dal.Insert(playlist);
            playlist.playlist_id = playlist_id;
        }
        public void Edit(VMBranchPlayList playlist)
        {
            DALBranchPlayList dal = new DALBranchPlayList();
            dal.Update(playlist);

        }
        public void Remove(int id)
        {
            DALBranchPlayList dal = new DALBranchPlayList();
            dal.Delete(id);

        }
        //public void Dispose()
        //{
        //    Dispose(true);

        //}

    }
}
