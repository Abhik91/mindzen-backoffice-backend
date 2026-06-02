using System;
using System.Data;
using Dapper;

using MySql.Data.MySqlClient;

namespace MindzenBackofficeDatabaseLibrary.Internal
{
    internal class MySqlDataAccess
    {
        public static async Task<List<T>> LoadData<T,U>(string storedProcedure, U parameters, string connectionString){

            try{
                using (IDbConnection connection = new MySqlConnection(connectionString)){
                    var result = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                    List<T> rows = result.ToList();
                    return rows;                    
                }

            }
            catch(Exception){
                throw;
            }

        }

        public static async Task<int> SaveData<T>(string storedProcedure, T parameters, string connectionString){
            int result = 0;
            try{
                using (IDbConnection connection = new MySqlConnection(connectionString)){
                    result = await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch(Exception){
                throw;
            }

            return result;
        }
        
    }
}
