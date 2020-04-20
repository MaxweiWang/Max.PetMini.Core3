using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Max.PetMini.BLL.Services;
using Max.PetMini.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NAutowired.Core.Attributes;

namespace Max.PetMini.WebAPI.Controllers.V1
{
    [Route("[controller]"),ApiController]
    public class PetTestController : BaseController
    {
        [Autowired]
        private readonly PetTestService petTestService;

        [HttpGet("")]
        public CatList GetById()
        {
           var catlist= petTestService.GetById(1);
            return catlist;
        }

    }
}