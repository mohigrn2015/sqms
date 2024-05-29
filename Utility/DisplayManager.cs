using SQMS.Models.ViewModels;
using System.Text;

namespace SQMS.Utility
{
    public class DisplayManager
    {

        private BLL.BLLToken dbManager = new BLL.BLLToken();
        public void CreateTextFile(int branch_id, string static_ip)
        {
            string textFileValue = GetInProgressTokens(branch_id);
            string nextTokens = GetNextTokens(branch_id);

            if (nextTokens.Length > 0)
                textFileValue = textFileValue + "\n" + nextTokens;

            string filePath = Path.Combine(ApplicationSetting.DisplayPath, static_ip + ".txt");
            StreamWriter sw = File.CreateText(filePath);
            sw.Write(textFileValue);
            sw.Close();
        }

        public string GetInProgressTokens(int branch_id)
        {
            List<VMTokenProgress> progressingTokenList = GetInProgressTokenList(branch_id);

            StringBuilder sb = new StringBuilder();
            if (progressingTokenList.Any())
            {
                foreach (var token in progressingTokenList)
                {
                    sb.Append(token.token_no + "\t");
                }
            }


            return sb.ToString().TrimEnd('\t');
        }

        public List<VMTokenProgress> GetInProgressTokenList(int branch_id)
        {
            try
            {

                List<VMTokenProgress> progressingTokens = dbManager.GetProgressTokenList(branch_id).ToList();

                return progressingTokens;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public string GetNextTokens(int branch_id)
        {
            return dbManager.GetNextTokenList(branch_id);
        }





    }
}
