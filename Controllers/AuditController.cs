using CertifyWPF.WPF_Audit;
using System.Collections.Generic;
using System.Web.Http;

namespace CertifyWPF.Controllers
{
    [Authorize(Roles = "admin, auditor")]

    public class AuditController : ApiController
    {
        /// <summary>
        /// GET: api/Audit 
        /// </summary>
        /// <param name="criteria">Can be open, created, allocated, docs sent, planned, processed</param>
        /// <returns></returns>
        public List<Audit> Get(string criteria)
        {
            return Audit.getAudits(criteria);
        }


        // GET: api/Audit/5
        public Audit Get(long id)
        {
            if (id != -1)
            {
                Audit audit = new Audit(id);
                if(audit.clientId != -1) return audit;
            }
            return null;
        }
    }
}
