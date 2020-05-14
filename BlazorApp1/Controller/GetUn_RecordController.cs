using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SVW_BYSJ_WEB.Data;

namespace SVW_BYSJ_WEB.Controller
{
    [Route("api/RepairRecord/[controller]")]
    [ApiController]
    public class GetUn_RecordController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Models.repairRecord> Get()
        {
            IList<Models.repairRecord> unfinishedRecords = SqlServerCtl.ListRepairRecord(false);
            return unfinishedRecords.ToArray();
        }
    }
}