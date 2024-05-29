using SQMS.DAL;
using SQMS.Models;
using System.Data;

namespace SQMS.BLL
{
    public class BLLMSISDNAndDeviceInfo
    {

        public void InsertHandsetLog(VMHandsetLog handsetLog)
        {
            DALMSISDNAndDeviceInfo dal = new DALMSISDNAndDeviceInfo();
            int log_id = dal.InsertHandsetLog(handsetLog);
            handsetLog.id = log_id;
        }


        public VMDeviceLoanEligiblility GetDeviceLoanInfoByMSISDN(string msisdn)
        {
            DALMSISDNAndDeviceInfo dal = new DALMSISDNAndDeviceInfo();
            DataTable dt = dal.GetDeviceLoanInfoByMSISDN(msisdn);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }


        internal VMDeviceLoanEligiblility ObjectMapping(DataRow row)
        {

            VMDeviceLoanEligiblility loanEligiblility = new VMDeviceLoanEligiblility();
            loanEligiblility.id = Convert.ToInt32(row["ID"] == DBNull.Value ? 0 : row["ID"]);
            loanEligiblility.msisdn = (row["msisdn"] == DBNull.Value ? null : row["msisdn"].ToString());
            loanEligiblility.loan_date = Convert.ToDateTime(row["LOAN_DATE"].ToString());
            loanEligiblility.is_eligible = (row["IS_ELIGIBLE"].ToString() == "1" ? true : false);
            return loanEligiblility;
        }
    }
}
