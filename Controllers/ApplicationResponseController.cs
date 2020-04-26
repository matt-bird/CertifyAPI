
using System;
using System.Collections.Generic;
using System.Web.Http;

using CertifyWPF.WPF_Application;

namespace CertifyWPF.Controllers
{
    [Authorize(Roles = "admin, auditor")]
    public class ApplicationResponseController : ApiController
    {
        // GET: api/ApplicationResponse/5
        public List<WebApplicationResponseView> Get(long id)
        {
            if (id != -1)
            {
                List<WebApplicationResponseView> list = WebApplicationResponseView.fetchResponses(id);
                if(list.Count > 0) return list;
            }
            return null;   
        }

    }
}
