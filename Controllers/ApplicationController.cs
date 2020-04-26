using System;
using System.Collections.Generic;
using System.Web.Http;

using CertifyWPF.WPF_Application;

namespace CertifyWPF.Controllers
{
    [Authorize(Roles = "admin, auditor")]

    public class ApplicationController : ApiController
    {
        /// <summary>
        /// GET: api/Audit 
        /// </summary>
        /// <param name="criteria">Can be : open, new application, application finished, awaiting audit, application underway, 
        ///                                 documents sent, ready for decision
        /// <returns></returns>
        public List<WebApplication> Get(string criteria)
        {
            return WebApplication.getApplications(criteria);
        }


        // GET: api/Audit/5
        public WebApplication Get(long id)
        {
            if (id != -1)
            {
                WebApplication webApp = new WebApplication(id);
                if(!String.IsNullOrEmpty(webApp.optionString)) return webApp;
            }
            return null;
        }
    }
}
