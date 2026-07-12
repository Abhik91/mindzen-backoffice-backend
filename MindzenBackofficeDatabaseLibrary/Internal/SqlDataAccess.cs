using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;



namespace MindzenBackofficeDatabaseLibrary.Internal
{
    internal class SqlDataAccess
    {
        public async Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionString)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    var result = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                    List<T> rows = result.ToList();
                    return rows;
                }
            }
            catch (Exception)
            {
                //throw new ExtractException("Database error in fetching records", $"{ex.Message}");
                //System.Diagnostics.Debug.WriteLine("Error in fetching query records - " + ex.Message);
                throw;
            }

            //return null;
        }

        public async Task<int> SaveData<T>(string storedProcedure, T parameters, string connectionString)
        {
            int result = 0;
            try
            {
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    result = await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                }


            }
            catch (Exception)
            {
                throw;
                //throw new ExtractException("Database error during insert / update / delete operation", $"{ex.Message}");
                //System.Diagnostics.Debug.WriteLine("Error in fetching query records - " + ex.Message);
                //result = 0;
            }
            return result;
        }

        public async Task<int> SaveScalarData<DynamicParameters>(string storedProcedure, DynamicParameters parameters, string connectionString)
        {
            int result = 0;
            try
            {
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                   result =  await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                }


            }
            catch (Exception)
            {
                throw;
                //throw new ExtractException("Database error during insert / update / delete operation", $"{ex.Message}");
                //System.Diagnostics.Debug.WriteLine("Error in fetching query records - " + ex.Message);
                //result = 0;
            }
            return result;
        }


    }
}
