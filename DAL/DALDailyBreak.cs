using MySql.Data.MySqlClient;
using SQMS.Models;
using SQMS.Models.ViewModels;
using SQMS.Utility;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace SQMS.DAL
{
    public class DALDailyBreak
    {
        MySQLManager manager;
        public DataTable GetAll(int? branch_id, string user_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_BRANCH_ID", Value = branch_id });
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_USER_ID", Value = user_id });

                return manager.CallStoredProcedure_Select("USP_DailyBreak_SelectAll");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALDailyBreak",
                    procedure_name = "USP_DailyBreak_SelectAll",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public DataTable GetById(int id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_dailyBreak_id", id));

                return manager.CallStoredProcedure_Select("USP_DailyBreak_SelectList_ById");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALDailyBreak",
                    procedure_name = "USP_DailyBreak_SelectList_ById",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public int GetCountByUserId(string user_id)
        {
            manager = new MySQLManager();
            try
            {
                int poPkValue = 0;

                manager.AddParameter(new MySqlParameter("P_USER_ID", user_id));

                DataTable dataTable = manager.CallStoredProcedure_Select("USP_DAILYBREAK_COUNTBYUSERID");

                if(dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        poPkValue = Convert.ToInt32(row["PO_IS_BREAK_ASSIGNED"] == DBNull.Value ? 0 : row["PO_IS_BREAK_ASSIGNED"]);
                    }                    
                }
                return poPkValue;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALDailyBreak",
                    procedure_name = "USP_DAILYBREAK_COUNTBYUSERID",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        /// <summary>
        /// Call only for New Service Type Insert
        /// Return service_type_id
        /// </summary>
        /// <param name="serviceType">Service Type Object</param>
        /// <returns>Return service_type_id</returns>
        public int Insert(tblDailyBreak dailyBreak)
        {
            manager = new MySQLManager();
            try
            {
                MapParameters(dailyBreak);
                long? daily_break_id = manager.CallStoredProcedure_Insert("USP_DailyBreak_Insert");
                if (daily_break_id.HasValue) return (int)daily_break_id.Value;
                else return 0;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALDailyBreak",
                    procedure_name = "USP_DailyBreak_Insert",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public void Update(string user_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_user_id", user_id));
                manager.CallStoredProcedure_Update("USP_DailyBreak_Update");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALDailyBreak",
                    procedure_name = "USP_DailyBreak_Update",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        private void MapParameters(tblDailyBreak dailyBreak)
        {
            manager.AddParameter(new MySqlParameter("p_break_type_id", dailyBreak.break_type_id));
            manager.AddParameter(new MySqlParameter("p_counter_id", dailyBreak.counter_id));
            manager.AddParameter(new MySqlParameter("p_start_time", dailyBreak.start_time));
            manager.AddParameter(new MySqlParameter("p_end_time", dailyBreak.end_time));
            manager.AddParameter(new MySqlParameter("p_user_id", dailyBreak.user_id));
            manager.AddParameter(new MySqlParameter("p_remarks", dailyBreak.remarks));



        }
        public void Delete(int id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_dailyBreak_id", id));

                manager.CallStoredProcedure_Update("USP_DailyBreak_Delete");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALDailyBreak",
                    procedure_name = "USP_DailyBreak_Delete",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
    }
}
