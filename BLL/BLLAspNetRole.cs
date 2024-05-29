using SQMS.DAL;
using SQMS.Models;
using System.Data;

namespace SQMS.BLL
{
    public class BLLAspNetRole
    {
        public List<AspNetRole> GetAllRoles()
        {
            DALAspNetRole dal = new DALAspNetRole();
            DataTable dt = dal.GetAllRoles();
            return ObjectMappingList(dt);
        }

        internal List<AspNetRole> ObjectMappingList(DataTable dt)
        {
            List<AspNetRole> list = new List<AspNetRole>();
            foreach (DataRow row in dt.Rows)
            {
                AspNetRole user = new AspNetRole();
                user.Id = (row["Id"] == DBNull.Value ? null : row["Id"].ToString());
                user.Name = (row["Name"] == DBNull.Value ? null : row["Name"].ToString());

                list.Add(user);
            }
            return list;
        }
    }
}
