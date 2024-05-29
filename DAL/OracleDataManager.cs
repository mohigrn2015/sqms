using Oracle.ManagedDataAccess.Client;
using SQMS.Utility;
using System.Data;

namespace SQMS.DAL
{
    internal class OracleDataManager
    {
        OracleConnection connectionSRDA;
        OracleCommand command;
        string connectionStringSRDA = String.Empty;

        public OracleDataManager()
        {
            connectionStringSRDA = StaticConfigValue.GetConnectionStringSRDA();
            connectionSRDA = new OracleConnection(connectionStringSRDA);
            command = new OracleCommand();
        }

        internal void AddParameter(OracleParameter param)
        {
            command.Parameters.Add(param);
        }

        public async Task CallStoredProcedureSRDA(string procedureName)
        {
            using (connectionSRDA)
            {
                try
                {
                    if (connectionSRDA.State != ConnectionState.Open) await connectionSRDA.OpenAsync();

                    command.Connection = connectionSRDA;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = procedureName;
                    OracleTransaction transaction = connectionSRDA.BeginTransaction();
                    command.Transaction = transaction;
                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (connectionSRDA.State != ConnectionState.Closed) await connectionSRDA.CloseAsync();
                }
            }
        }
        public async Task<long?> CallStoredProcedure_InsertSRDA(string procedureName)
        {
            long? pkValue = null;

            try
            {
                await CallStoredProcedureSRDA(procedureName);

                pkValue = 1;

                return pkValue;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
