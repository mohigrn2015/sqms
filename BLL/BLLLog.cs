using SQMS.DAL;
using SQMS.Models;

namespace SQMS.BLL
{
    public class BLLLog
    {
        public void SignalRBroadCastLogCreate(SignalRBroadcastLog log)
        {
            DALLog dal = new DALLog();
            int broadcast_log_id = dal.SignalRBroadcastLogInsert(log);
            log.broadcast_log_id = broadcast_log_id;
        }

        public void ApiRequestLogCreate(ApiRequestLog log)
        {
            DALLog dal = new DALLog();
            int request_log_id = dal.ApiRequestLogInsert(log);
            log.request_log_id = request_log_id;
        }

    }
}
