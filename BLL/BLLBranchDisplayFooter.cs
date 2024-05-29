using SQMS.DAL;
using SQMS.Models.ViewModels;
using System.Data;

namespace SQMS.BLL
{
    public class BLLBranchDisplayFooter
    {
        private readonly BLLDisplayFooter _displayFooter;
        public BLLBranchDisplayFooter(IWebHostEnvironment webHostEnvironment)
        {
            _displayFooter = new BLLDisplayFooter(webHostEnvironment);
        }
        public List<VMBranchDisplayFooter> GetAll()
        {
            DALBranchDisplayFooter dal = new DALBranchDisplayFooter();
            DataTable dt = dal.GetAll();
            return ObjectMappingList(dt);
        }

        internal VMBranchDisplayFooter ObjectMapping(DataRow row)
        {

            VMBranchDisplayFooter displayFooter = new VMBranchDisplayFooter();
            displayFooter.branch_display_footer_id = Convert.ToInt32(row["BRANCH_DISPLAY_FOOTER_ID"] == DBNull.Value ? 0 : row["BRANCH_DISPLAY_FOOTER_ID"]);
            displayFooter.display_footer_id = Convert.ToInt32(row["DISPLAY_FOOTER_ID"] == DBNull.Value ? 0 : row["DISPLAY_FOOTER_ID"]);
            displayFooter.content_en = (row["CONTENT_EN"] == DBNull.Value ? "" : row["CONTENT_EN"].ToString());
            displayFooter.content_bn = _displayFooter.GetDisplayFooterBn(displayFooter.display_footer_id);
            displayFooter.branch_id = Convert.ToInt32(row["BRANCH_ID"] == DBNull.Value ? 0 : row["BRANCH_ID"]);
            displayFooter.branch_name = (row["BRANCH_NAME"] == DBNull.Value ? "" : row["BRANCH_NAME"].ToString());
            return displayFooter;
        }

        internal List<VMBranchDisplayFooter> ObjectMappingList(DataTable dt)
        {
            List<VMBranchDisplayFooter> list = new List<VMBranchDisplayFooter>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ObjectMapping(row));

            }
            return list;
        }
        public VMBranchDisplayFooter GetById(int id)
        {
            DALBranchDisplayFooter dal = new DALBranchDisplayFooter();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }

        public void Create(VMBranchDisplayFooter displayFooter)
        {
            DALBranchDisplayFooter dal = new DALBranchDisplayFooter();
            int display_footer_id = dal.Insert(displayFooter);
            displayFooter.display_footer_id = display_footer_id;
        }
        public void Edit(VMBranchDisplayFooter displayFooter)
        {
            DALBranchDisplayFooter dal = new DALBranchDisplayFooter();
            dal.Update(displayFooter);

        }
        public void Remove(int id)
        {
            DALBranchDisplayFooter dal = new DALBranchDisplayFooter();
            dal.Delete(id);

        }
    }
}
