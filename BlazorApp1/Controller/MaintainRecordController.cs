using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SVW_BYSJ_WEB.Data;

namespace SVW_BYSJ_WEB.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintainRecordController : ControllerBase
    {
        // GET: api/MaintainRecord
        [HttpGet]
        public IEnumerable<Models.repairRecord> Get()
        {
            IList<Models.repairRecord> unfinishedRecords = SqlServerCtl.ListRepairRecord(false);
            return unfinishedRecords.ToArray();
        }

        // GET: api/MaintainRecord/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/MaintainRecord
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/MaintainRecord/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
