using Dapper.Contrib.Extensions;

namespace Max.PetMini.DAL.Entities
{
    [Table("foo")]
    public class Foo : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        public long field { get; set; }

    }
}
