using System;
using System.Collections.Generic;
using System.Web.Http;

using CertifyWPF.WPF_Status;

namespace CertifyWPF.Controllers
{
    [Authorize(Roles = "admin, auditor")]
    public class SXCStatusController : ApiController
    {
        public List<Status> Get()
        {
            List<Status> list = StatusFactory.getStatuses();
            List<Status> listToReturn = new List<Status>();
            foreach(Status status in list)
            {
                if (status.count > 0) listToReturn.Add(status);
            }

            return listToReturn;
        }


        //-------------------------------------------------------------------------------------------------------------
        public Status Get(StatusFactory.StatusType type)
        {
            return StatusFactory.getStatus(type);
        }
    }
}
