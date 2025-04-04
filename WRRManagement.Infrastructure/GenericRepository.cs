using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using WRRManagement.Domain.Base;
using WRRManagement.Infrastructure.Data;

namespace WRRManagement.Infrastructure
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class 
    {
        protected WRRContext context;

        public GenericRepository(WRRContext context)
        {
            this.context = context;
        }
        public int Add(string spName, List<ParameterInfo> p)
        {
            int id = 0;
            try
            {
                id = DapperHelper.ExecuteQuery(spName, p, context);
            }
            catch (Exception ex) { }
            return id;
                  
        }

        public void Delete(string spName, List<ParameterInfo> p)
        {
            try
            {
                DapperHelper.ExecuteQuery(spName, p, context);
            }
            catch(Exception ex) { } 
        }

        public IEnumerable<T> GetAll(string spName, List<ParameterInfo> parameters)
        {
            IEnumerable<T> results = new List<T>();
            try
            {
                results = DapperHelper.GetRecords<T>(spName, parameters, context);
            }
            catch (Exception ex) { }
            return results;
        }

        public T GetById(string spName, List<ParameterInfo> parameters)
        {
            T? result = null;       
            try
            {
                result = DapperHelper.GetRecord<T>(spName, parameters, context);
            }
            catch(Exception ex) { }
          
            return result;
        }

        public void Update(string spName, List<ParameterInfo> p)
        {
            try
            {
                DapperHelper.ExecuteQuery(spName, p, context);
            }
            catch (Exception e) { }
        }

    }
}
