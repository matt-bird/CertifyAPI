using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Http;

using CertifyWPF.WPF_Client;

namespace CertifyWPF.Controllers
{
    [Authorize(Roles = "admin, auditor")]
    public class ClientController : ApiController
    {
        // GET: api/Client
        public List<string[]> Get(int maxCount)
        {
            List<string[]> list = Client.getClientList(maxCount);
            return list;
        }

        // GET: api/Client/5
        public Client Get(long id)
        {
            if (id != -1)
            {
                Client client = new Client(id);
                if (!String.IsNullOrEmpty(client.company))return client;
            }
            return null;
        }

        // POST: api/Client
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Client/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Client/5
        public void Delete(int id)
        {
        }
    }
}
