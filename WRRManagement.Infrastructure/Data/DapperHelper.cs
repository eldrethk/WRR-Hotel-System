using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using WRRManagement.Domain.Base;
using WRRManagement.Domain.Hotels;

namespace WRRManagement.Infrastructure.Data
{
    public static class DapperHelper
    {
        public static List<T> GetRecords<T>(string spName, List<ParameterInfo> parameters, WRRContext context)
        {
            
            List<T>
                records = new List<T>();
            using (var conn = context.CreateConnection())
            {
                conn.Open();
                DynamicParameters dynamicParameters = GetDynamicParameters(parameters);
                records = conn.Query<T>(spName, dynamicParameters, commandType: CommandType.StoredProcedure).ToList();
               
               
                conn.Close();
                //int b = p.Get<int>("@b");
                //int c = p.Get<int>("@c");
            }
            return records;
        }

        public static (List<T1>, List<T2>) GetMultipleHorizontalRecords<T1, T2>(string spName, List<ParameterInfo> parameters, WRRContext context, Func<T1, T2, T1> map, string splitOn)
        {
            List<T1> records1 = new List<T1>();
            List<T2> records2 = new List<T2>();

            try
            {
                using (var conn = context.CreateConnection())
                {
                    conn.Open();
                    DynamicParameters dynamicParameters = GetDynamicParameters(parameters);
                    
                    var result = conn.Query<T1, T2, T1>(
                        spName, 
                        (T1, T2) =>
                        {
                            if (!records2.Any(r => r.Equals(T2)))
                            {
                                records2.Add(T2);
                            }
                            return map(T1, T2);
                        },
                        dynamicParameters, 
                        splitOn: splitOn,
                        commandType: CommandType.StoredProcedure);

                    records1 = result.ToList();

                    /*using (var results = conn.QueryMultiple(spName, dynamicParameters, commandType: CommandType.StoredProcedure))
                    {
                        // Read result sets
                        records1 = results.Read<T1>().ToList();
                        records2 = results.Read<T2>().ToList();
                    }*/
                }
            }
            catch (Exception ex)
            {
                // Log the exception (replace with your preferred logging framework)
                Console.WriteLine($"Error in GetMultipleRecords: {ex.Message}");
                // Optionally rethrow or handle the exception
                throw;
            }

            return (records1, records2);
        }


        public static T GetRecord<T>(string spName, List<ParameterInfo> parameters, WRRContext context)
        {
            T? record = default;
            using(var conn = context.CreateConnection())
            {
                conn.Open();
                DynamicParameters dynamicParameters = GetDynamicParameters(parameters);
                
                record = conn.QueryFirst<T>(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
                conn.Close();
            }
            return record;
        }

        public static int ExecuteQuery(string spName, List<ParameterInfo> parameters, WRRContext context)
        {
            int result = 0;
            DynamicParameters dynamicParameters = GetDynamicParameters(parameters);
            
            using (var conn = context.CreateConnection())
            {
                try
                {
                    conn.Open();
                    //result = conn.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
                    //result = SqlMapper.Execute(conn, spName, dynamicParameters, commandType: CommandType.StoredProcedure);
                    result = conn.ExecuteScalar<int>(spName, dynamicParameters, commandType:CommandType.StoredProcedure);
                    conn.Close();
                }catch (Exception ex) { }

            }
            return result;
        }

        public static T ExecuteWithTransaction<T>(Func<IDbConnection, IDbTransaction, T> query, WRRContext context)
        {
            using (var conn = context.CreateConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        T result = query(conn, transaction);                        
                        transaction.Commit();
                        return result;                       
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Transaction rolled back due to error: {ex.Message}");
                        return default;
                    }
                }              
            }          
        }


        public static int ExecuteQueryInt(string spName, List<ParameterInfo> parameters, WRRContext context, string output)
        {
            int result = 0;
            DynamicParameters dynParameters = GetDynamicParameters(parameters);
            dynParameters.Add(output, null, dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
           
            using (var conn = context.CreateConnection())
            {
               
                conn.Open(); 
                try
                {
                    
                    conn.Execute(spName, dynParameters, commandType: CommandType.StoredProcedure);
                    
                }
                catch(Exception ex) { }
                result = dynParameters.Get<int>(output);
                conn.Close();
            }
           
          
            return result;
        }

        public static string ExecuteQueryString(string spName, List<ParameterInfo> parameters, WRRContext context, string output)
        {
            string result = string.Empty;
            DynamicParameters dynParameters = GetDynamicParameters(parameters);
            dynParameters.Add(output, null, dbType: DbType.String, direction: ParameterDirection.ReturnValue);
            using(var conn = context.CreateConnection()) {
                conn.Open();
                try
                {
                    result = conn.ExecuteScalar<string>(spName, dynParameters, commandType: CommandType.StoredProcedure);
                }
                catch(Exception ex) { }
               // result = dynParameters.Get<string>(output);
                conn.Close();
            }
            return result;

        }

        public static bool ExecuteQueryBool(string spName, List<ParameterInfo>parameters, WRRContext context, string output)
        {
            bool result = false;
            DynamicParameters dynamicParameters = GetDynamicParameters(parameters);
            dynamicParameters.Add(output, null, dbType: DbType.Boolean, direction: ParameterDirection.ReturnValue);
            using(var conn = context.CreateConnection()) {
                conn.Open();
                try
                {
                    result = conn.ExecuteScalar<bool>(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
                }
                catch(Exception ex) { }
                //result = dynamicParameters.Get<bool>(output);
                conn.Close();
            }
            return result;

        }

        public static DateTime ExecuteQueryDateTime(string spName, List<ParameterInfo> parameters, WRRContext context, string output)
        {
            DateTime result = DateTime.MinValue;
            DynamicParameters dynParameters = GetDynamicParameters(parameters);
            dynParameters.Add(output, null, dbType: DbType.DateTime, direction: ParameterDirection.ReturnValue);
            using(var connection = context.CreateConnection())
            {
                connection.Open();
                try
                {
                    connection.Execute(spName, dynParameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex) { }
                result = dynParameters.Get<DateTime>(output);
                connection.Close();
            }
            return result;
        }

        public static DynamicParameters GetDynamicParameters(List<ParameterInfo> parameters)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add("@" + parameter.ParameterName, parameter.ParameterValue);
            }
            return dynamicParameters;

            /* p.Add("@a", 11);
               p.Add("@b", dbType: DbType.Int32, direction: ParameterDirection.Output);
               p.Add("@c", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
               */
        }
    }
}
