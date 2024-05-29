using SQMS.DAL;
using SQMS.Models.ViewModels;
using SQMS.Models;
using System.Data;

namespace SQMS.BLL
{
    public class BLLPlayList
    {
        public List<tblPlayList> GetAll()
        {
            DALPlayList dal = new DALPlayList();
            DataTable dt = dal.GetAll();
            return ObjectMappingList(dt);
        }

        internal tblPlayList ObjectMapping(DataRow row)
        {
            tblPlayList playList = new tblPlayList()
            {
                playlist_id = Convert.ToInt32(row["PLAYLIST_ID"] == DBNull.Value ? 0 : row["PLAYLIST_ID"]),
                playlist_name = (row["PLAYLIST_NAME"] == DBNull.Value ? "" : row["PLAYLIST_NAME"].ToString()),
                is_global = Convert.ToInt32(row["IS_GLOBAL"] == DBNull.Value ? 0 : row["IS_GLOBAL"])
            };
            return playList;
        }

        internal List<tblPlayList> ObjectMappingList(DataTable dt)
        {
            List<tblPlayList> list = new List<tblPlayList>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ObjectMapping(row));

            }
            return list;
        }

        internal List<VMPlayList> ObjectMappingListVM(DataTable dt)
        {
            List<VMPlayList> list = new List<VMPlayList>();
            foreach (DataRow row in dt.Rows)
            {
                VMPlayList playList = new VMPlayList()
                {
                    playlist_id = Convert.ToInt32(row["PLAYLIST_ID"] == DBNull.Value ? 0 : row["PLAYLIST_ID"]),
                    playlist_name = (row["PLAYLIST_NAME"] == DBNull.Value ? "" : row["PLAYLIST_NAME"].ToString()),
                    item_url = (row["ITEM_URL"] == DBNull.Value ? "" : "~" + row["ITEM_URL"].ToString()),
                    file_type = (row["FILE_TYPE"] == DBNull.Value ? "" : row["FILE_TYPE"].ToString()),
                    file_name = (row["FILE_NAME"] == DBNull.Value ? "" : row["FILE_NAME"].ToString()),
                    duration_in_second = Convert.ToInt32(row["DURATION_IN_SECOND"] == DBNull.Value ? 0 : row["DURATION_IN_SECOND"]),
                    sort_order = Convert.ToInt32(row["SORT_ORDER"] == DBNull.Value ? 0 : row["SORT_ORDER"])
                };
                list.Add(playList);
            }
            return list;
        }

        public tblPlayList GetById(int id)
        {
            DALPlayList dal = new DALPlayList();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }

        public List<VMPlayList> GetByBranchId(int branch_id)
        {
            DALPlayList dal = new DALPlayList();
            DataTable dt = dal.GetByBranchId(branch_id);
            return ObjectMappingListVM(dt);

        }
        public void Create(tblPlayList playList)
        {
            DALPlayList dal = new DALPlayList();
            int playlist_id = dal.Insert(playList);
            playList.playlist_id = playlist_id;
        }
        public void Edit(tblPlayList playList)
        {
            DALPlayList dal = new DALPlayList();
            dal.Update(playList);

        }
        public void Remove(int id)
        {
            DALPlayList dal = new DALPlayList();
            dal.Delete(id);

        }

    }
}
