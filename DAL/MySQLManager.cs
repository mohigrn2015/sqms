using MySql.Data.MySqlClient;
using SQMS.Utility;
using System.Data;

namespace SQMS.DAL
{
    public class MySQLManager 
    {
        private readonly string connectionStringRead = string.Empty;
        private readonly string connectionStringReadWrite = string.Empty;
        MySqlCommand cmd = new MySqlCommand();
        public MySQLManager()
        {
            connectionStringRead = StaticConfigValue.GetConnectionStringRead();
            connectionStringReadWrite = StaticConfigValue.GetConnectionStringReadWrite();
            cmd = new MySqlCommand();
        }
        public void AddParameter(MySqlParameter param)
        {
            cmd.Parameters.Add(param);
        }
        public void CallStoredProcedure(string storedProcedureName)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringReadWrite))
            {
                using (cmd)
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                            connection.Open();

                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = storedProcedureName;

                        MySqlTransaction transaction = connection.BeginTransaction();
                        cmd.Transaction = transaction;

                        try
                        {
                            cmd.ExecuteNonQuery();
                            transaction.Commit();
                        }
                        catch (MySqlException ex)
                        {
                            transaction.Rollback();
                            throw new Exception(ex.Message.ToString());
                        }
                    }
                    catch (Exception ex2)
                    {
                        throw new Exception(ex2.Message);
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                            connection.Close();
                    }
                }
            }
        }

        public DataTable CallStoredProcedure_TokenInsert(string storedProcedureName)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringReadWrite))
            {
                using (cmd)
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                            connection.Open();

                        cmd.Connection = connection;
                        cmd.CommandText = storedProcedureName;
                        cmd.CommandType = CommandType.StoredProcedure;

                        DataTable dt = new DataTable(storedProcedureName);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }

                        return dt;
                    }
                    catch (MySqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    catch (Exception ex2)
                    {
                        throw new Exception(ex2.Message);
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                            connection.Close();
                    }
                }
            }
        }

        public DateTime CallStoredProcedure_DBDate()
        {
            DateTime pkValue = DateTime.Now;

            using (MySqlConnection connection = new MySqlConnection(connectionStringRead))
            {
                using (cmd)
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                            connection.Open();

                        cmd.Connection = connection;
                        cmd.CommandText = "SSP_GetDBDate";
                        cmd.CommandType = CommandType.StoredProcedure;

                        DataTable dt = new DataTable(cmd.CommandText);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }

                        if(dt != null)
                        {
                            foreach (DataRow item in dt.Rows)
                            {
                                pkValue = Convert.ToDateTime(item["DB_DATE"]);
                            }
                        }

                        return pkValue;
                    }
                    catch (MySqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    catch (Exception ex2)
                    {
                        throw new Exception(ex2.Message);
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                            connection.Close();
                    }
                }
            }
        }

        public DataSet CallStoredProcedure_SelectDS(string storedProcedureName)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringRead))
            {
                using (cmd)
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                            connection.Open();

                        cmd.Connection = connection;
                        cmd.CommandText = storedProcedureName;
                        cmd.CommandType = CommandType.StoredProcedure;

                        DataSet dt = new DataSet(storedProcedureName);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }

                        return dt;
                    }
                    catch (MySqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    catch (Exception ex2)
                    {
                        throw new Exception(ex2.Message);
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                            connection.Close();
                    }
                }
            }
        }

        public DataTable CallStoredProcedure_Select(string storedProcedureName)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringRead))
            {
                using (cmd)
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                            connection.Open();

                        cmd.Connection = connection;
                        cmd.CommandText = storedProcedureName;
                        cmd.CommandType = CommandType.StoredProcedure;

                        DataTable dt = new DataTable(storedProcedureName);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }

                        return dt;
                    }
                    catch (MySqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    catch (Exception ex2)
                    {
                        throw new Exception(ex2.Message);
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                            connection.Close();
                    }
                }
            }
        }
         
        public DataSet CallStoredProcedure_SelectDataSet(string storedProcedureName)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringRead))
            {
                using (cmd)
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                            connection.Open();

                        cmd.Connection = connection;
                        cmd.CommandText = storedProcedureName;
                        cmd.CommandType = CommandType.StoredProcedure;

                        DataSet dataSet = new DataSet();

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataSet);
                        }

                        return dataSet;
                    }
                    catch (MySqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    catch (Exception ex2)
                    {
                        throw new Exception(ex2.Message);
                    }
                }
            }
        }

        public long? CallStoredProcedure_Insert(string storedProcedureName)
        {
            long? pkValue = null;

            using (MySqlConnection connection = new MySqlConnection(connectionStringReadWrite))
            {
                using (cmd)
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                            connection.Open();

                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = storedProcedureName;

                        MySqlTransaction transaction = connection.BeginTransaction();
                        cmd.Transaction = transaction;

                        try
                        {
                            cmd.ExecuteNonQuery();
                            transaction.Commit();

                            //if (param.Value != DBNull.Value)
                                //pkValue = (long)param.Value;
                                pkValue = 1;
                        }
                        catch (MySqlException ex)
                        {
                            transaction.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                            connection.Close();
                    }
                }
            }

            return pkValue;
        }

        public void CallStoredProcedure_Update(string storedProcedureName)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringReadWrite))
            {
                using (cmd)
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                            connection.Open();

                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = storedProcedureName;

                        MySqlTransaction transaction = connection.BeginTransaction();
                        cmd.Transaction = transaction;

                        try
                        {
                            cmd.ExecuteNonQuery();
                            transaction.Commit();
                        }
                        catch (MySqlException ex)
                        {
                            transaction.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                    catch (Exception ex2)
                    {
                        throw new Exception(ex2.Message);
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                            connection.Close();
                    }
                }
            }
        }

        public void CallStoredProcedure_Delete(string storedProcedureName)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringReadWrite))
            {
                using (cmd)
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                            connection.Open();

                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = storedProcedureName;

                        MySqlTransaction transaction = connection.BeginTransaction();
                        cmd.Transaction = transaction;

                        try
                        {
                            cmd.ExecuteNonQuery();
                            transaction.Commit();
                        }
                        catch (MySqlException ex)
                        {
                            transaction.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                    catch (Exception ex2)
                    {
                        throw new Exception(ex2.Message);
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                            connection.Close();
                    }
                }
            }
        }
    }

}
