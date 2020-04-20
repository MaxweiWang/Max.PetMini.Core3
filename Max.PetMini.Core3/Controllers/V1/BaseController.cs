using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NAutowired.Core.Attributes;

namespace Max.PetMini.WebAPI.Controllers.V1
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseController : ControllerBase
    {

        /// <summary>
        /// 
        /// </summary>
        [Autowired]
        protected readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// 日志
        /// </summary>
        protected ILogger logger { get { return loggerFactory.CreateLogger(GetType().FullName); } }
    }
}