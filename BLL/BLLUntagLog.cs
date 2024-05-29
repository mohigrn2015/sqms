using SQMS.DAL;
using SQMS.Models;

namespace SQMS.BLL
{
    public class BLLUntagLog
    {
        public void UntagLogCreate(tblUntagLog untagLog)
        {
            DALUntagLog dal = new DALUntagLog();
            int pk = dal.Insert(untagLog);
        }
    }
}
