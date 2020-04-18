using CertifyWPF.WPF_Service;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace CertifyWPF.Controllers
{
    public class ServiceController : ApiController
    {
        // GET: api/Service
        public IEnumerable<string> Get()
        {
            return Service.getServiceList();
        }

        // GET: api/Service/5
        public Service Get(long id)
        {
            if (id != -1)
            {
                Service service = new Service(id);
                if (!String.IsNullOrEmpty(service.name)) return service;
            }
            return null;
        }

        // POST: api/Service
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Service/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Service/5
        public void Delete(int id)
        {
        }
    }
}
