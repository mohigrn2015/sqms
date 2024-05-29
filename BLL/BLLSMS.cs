using Oracle.ManagedDataAccess.Client;
using SQMS.DAL;
using SQMS.Models.RequestModel;

namespace SQMS.BLL
{
    public class BLLSMS
    {
        public async Task SendTokenSMSToSRDADB(SMSDataResponseModel model)
        { 
            OracleDataManager manager = new OracleDataManager();
            long? pkValue = 0;
            try
            {
                manager.AddParameter(new OracleParameter("P_MSISDN", model.msisdn));
                manager.AddParameter(new OracleParameter("P_IS_BN", model.is_bn));
                manager.AddParameter(new OracleParameter("P_TOKEN_NO", model.token_no));
                manager.AddParameter(new OracleParameter("P_TOKEN_MESSAGE", model.message));

                pkValue = await manager.CallStoredProcedure_InsertSRDA("USP_SENDSMS");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
