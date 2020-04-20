using Max.PetMini.DAL.Entities;
using Max.PetMini.DAL.Extensions;
using Dapper;
using Dapper.Contrib.Extensions;
using NAutowired.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Max.PetMini.DAL.Repositories
{
    [Repository]
    public class ExpressRepository : Repository<Express>
    {
        /// <summary>
        /// 根据快递名称获取快递数据
        /// </summary>
        /// <param name="expressName">expressName</param>
        /// <returns></returns>
        public Express GetExpressByExpressName(string expressName)
        {
            string sql = string.Format(@"select * from TExpress where Status !='{0}' and ExpressName = '{1}'", Express.StatusStruct.Delete,expressName);
            return Master.Query<Express>(sql).FirstOrDefault();
        }
    }
}
