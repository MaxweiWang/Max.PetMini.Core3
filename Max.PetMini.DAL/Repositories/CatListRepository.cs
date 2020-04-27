using Max.PetMini.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Dapper.Contrib.Extensions;
using NAutowired.Core.Attributes;
using System.Linq;
using Max.PetMini.DAL.Extensions;

namespace Max.PetMini.DAL.Repositories
{
    [Repository]
    public class CatListRepository : Repository<CatList>
    {

        //public CatListRepository(DbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory.Master)
        //{
        //}

        public CatList GetById(int id)
        {
            var sqlbuilder = new SqlBuilderSingle();
            sqlbuilder.Equals(nameof(CatList.Id), id);
            return this.GetAll(sqlbuilder).FirstOrDefault();

            //string sql = string.Format(@"select * from CatList where Id = '{0}'", id);
            //return Master.Query<CatList>(sql).FirstOrDefault();
            //return Master.QueryFirst<CatList>(sql);
        }

        public List<CatList> GetCatListAll()
        {
            string sql = string.Format("select * from CatList ");
            return Master.Query<CatList>(sql).ToList();
        }

    }
}
