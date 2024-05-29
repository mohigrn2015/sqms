using SQMS.DAL;
using SQMS.Models;
using System.Data;

namespace SQMS.BLL
{
    public class BLLStatus
    {
        public List<tblServiceStatu> GetAll()
        {
            DALStatus dal = new DALStatus();
            DataTable dt = dal.GetAll();
            return ObjectMappingList(dt);
        }
        internal List<tblServiceStatu> ObjectMappingList(DataTable dt)
        {
            List<tblServiceStatu> list = new List<tblServiceStatu>();
            foreach (DataRow row in dt.Rows)
            {
                tblServiceStatu status = new tblServiceStatu();
                status.service_status_id = Convert.ToInt16(row["service_status_id"] == DBNull.Value ? 0 : row["service_status_id"]);
                status.service_status = (row["service_status"] == DBNull.Value ? null : row["service_status"].ToString());

                list.Add(status);

            }
            return list;
        }
    }
}
