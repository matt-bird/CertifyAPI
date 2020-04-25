using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Http;

using CertifyWPF.WPF_Client;
using CertifyWPF.WPF_Utils;

namespace CertifyWPF.Controllers
{
    [Authorize(Roles = "admin, auditor")]
    public class ClientController : ApiController
    {
        // GET: api/client?criteria=someName
        public List<string[]> Get(string criteria)
        {
            List<string[]> list = Client.getClientList(criteria);
            return list;
        }

        // GET: api/Client
        public List<string[]> Get()
        {
            List<string[]> list = Client.getClientList();
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
    }
}
