using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Http;

using CertifyWPF.WPF_Client;

namespace CertifyWPF.Controllers
{
    [Authorize]
    public class ClientController : ApiController
    {
        // GET: api/Client
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Client/5
        public string Get(long id)
        {
            if (id != -1)
            {
                Client client = new Client(id);
                if (!String.IsNullOrEmpty(client.company))
                    return JsonConvert.SerializeObject(client, Newtonsoft.Json.Formatting.Indented);
            }
            return "Error - Client Not Found - Fuck off";
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
