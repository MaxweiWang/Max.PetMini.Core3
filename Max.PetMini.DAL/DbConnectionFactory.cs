using Microsoft.Extensions.Options;
using NAutowired.Core.Attributes;
using System.Data;
using System.Data.SqlClient;

namespace Max.PetMini.DAL
{
    /// <summary>
    /// 
    /// </summary>
    [Component]
    public class DbConnectionFactory
    {

        [Autowired]
        private IOptions<DbConnectionConfig> config { get; set; }

        private IDbConnection master;

        //private IDbConnection slave;


        /// <summary>
        /// 主
        /// </summary>
        public IDbConnection Master
        {
            get
            {
                if (master == null)
                {
                    master = new SqlConnection(config.Value.Master);
                }
                return master;
            }
        }

        ///// <summary>
        ///// 从数据库
        ///// </summary>
        //public IDbConnection Slave
        //{
        //    get
        //    {
        //        if (slave == null)
        //        {
        //            slave = new SqlConnection(config.Value.Slave);
        //        }
        //        return slave;
        //    }
        //}

    }
}
