using CertifyWPF.WPF_Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CertifyWPF.Controllers
{
    [Authorize(Roles = "admin, auditor")]
    public class ClientAddressController : ApiController
    {
        // GET: api/ClientAddress - Gets all client addresses
        public List<ClientAddress> Get()
        {
            List<ClientAddress> list = ClientAddress.getAddressList();
            return list;
        }

        // GET: api/ClientAddress/5 - Gest all client contacts for a client
        public List<ClientAddress> Get(long id)
        {
            List<ClientAddress> list = ClientAddress.getAddressList(id);
            return list;
        }

        // POST: api/ClientAddress
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ClientAddress/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ClientAddress/5
        public void Delete(int id)
        {
        }
    }
}
