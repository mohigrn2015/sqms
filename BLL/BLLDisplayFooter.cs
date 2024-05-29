using SQMS.DAL;
using SQMS.Models.ViewModels;
using SQMS.Models;
using System.Data;
using Microsoft.AspNetCore.Hosting;

namespace SQMS.BLL
{
    public class BLLDisplayFooter
    { 

        private readonly IWebHostEnvironment _webHostEnvironment;

        public BLLDisplayFooter(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public List<tblDisplayFooter> GetAll()
        {
            DALDisplayFooter dal = new DALDisplayFooter();
            DataTable dt = dal.GetAll();
            return ObjectMappingList(dt);
        }

        internal tblDisplayFooter ObjectMapping(DataRow row)
        {

            tblDisplayFooter displayFooter = new tblDisplayFooter();
            displayFooter.display_footer_id = Convert.ToInt32(row["DISPLAY_FOOTER_ID"] == DBNull.Value ? 0 : row["DISPLAY_FOOTER_ID"]);
            displayFooter.content_en = (row["CONTENT_EN"] == DBNull.Value ? "" : row["CONTENT_EN"].ToString());
            //displayFooter.content_bn = GetDisplayFooterBn(displayFooter.display_footer_id); 
            displayFooter.content_bn = (row["CONTENT_BN"] == DBNull.Value ? "" : row["CONTENT_BN"].ToString());
            displayFooter.is_global = Convert.ToInt32(row["IS_GLOBAL"] == DBNull.Value ? 0 : row["IS_GLOBAL"]);
            return displayFooter;
        }

        public string GetDisplayFooterBn(int display_footer_id)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "DisplayFootersBN", $"{display_footer_id}.txt");

            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                return "";
            }
        }       

        internal VMDisplayFooter ObjectMappingVM(DataRow row)
        {

            VMDisplayFooter displayFooter = new VMDisplayFooter();
            int displayFooterId = Convert.ToInt32(row["DISPLAY_FOOTER_ID"] == DBNull.Value ? 0 : row["DISPLAY_FOOTER_ID"]);
            displayFooter.content_en = (row["CONTENT_EN"] == DBNull.Value ? "" : row["CONTENT_EN"].ToString());
            //displayFooter.content_bn = GetDisplayFooterBn(displayFooterId); 
            displayFooter.content_bn = (row["CONTENT_BN"] == DBNull.Value ? "" : row["CONTENT_BN"].ToString());
            return displayFooter;
        }

        internal List<tblDisplayFooter> ObjectMappingList(DataTable dt)
        {
            List<tblDisplayFooter> list = new List<tblDisplayFooter>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ObjectMapping(row));

            }
            return list;
        }
        public tblDisplayFooter GetById(int id)
        {
            DALDisplayFooter dal = new DALDisplayFooter();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }

        public VMDisplayFooter GetByBranchId(int branch_id)
        {
            DALDisplayFooter dal = new DALDisplayFooter();
            DataTable dt = dal.GetByBranchId(branch_id);
            if (dt.Rows.Count > 0)
                return ObjectMappingVM(dt.Rows[0]);
            else return null;
        }
        public void Create(tblDisplayFooter displayFooter)
        {
            DALDisplayFooter dal = new DALDisplayFooter();
            int display_footer_id = dal.Insert(displayFooter);
            displayFooter.display_footer_id = display_footer_id;
            //AddEditDisplayFooterBn(displayFooter);
        }
        public void Edit(tblDisplayFooter displayFooter)
        {
            DALDisplayFooter dal = new DALDisplayFooter();
            dal.Update(displayFooter);
            //AddEditDisplayFooterBn(displayFooter);
        }

        internal void AddEditDisplayFooterBn(tblDisplayFooter displayFooter)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "DisplayFootersBN", $"{displayFooter.display_footer_id}.txt");

            if (File.Exists(filePath))
            {
                File.WriteAllText(filePath, displayFooter.content_bn);
            }
            else
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.Write(displayFooter.content_bn);
                }
            }
        }
        
        public void Remove(int id)
        {
            DALDisplayFooter dal = new DALDisplayFooter();
            dal.Delete(id);
        }

        internal void deleteDisplayFooterBn(int display_footer_id)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "DisplayFootersBN", $"{display_footer_id}.txt");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
