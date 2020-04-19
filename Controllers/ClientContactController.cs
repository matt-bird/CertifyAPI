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
    public class ClientContactController : ApiController
    {
        // GET: api/ClientContact - Gets all client contacts
        public List<ClientContactListItem> Get()
        {
            List<ClientContactListItem> list = ClientContact.getContactList();
            return list;
        }

        // GET: api/ClientContact/5 - Gest all client contacts for a client
        public List<ClientContactListItem> Get(long id)
        {
            List<ClientContactListItem> list = ClientContact.getContactList(id);
            return list;
        }

        // POST: api/ClientContact
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ClientContact/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ClientContact/5
        public void Delete(int id)
        {
        }
    }
}
