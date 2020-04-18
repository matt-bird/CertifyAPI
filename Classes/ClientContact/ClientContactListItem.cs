using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CertifyWPF.WPF_Client
{
    public class ClientContactListItem
    {
        public long id { get; set; }
        public long clientId { get; set; }
        public string name { get; set; }
        public string preferredName { get; set; }
        public string type { get; set; }
        public string details { get; set; }
        public bool isDefault { get; set; }

        public ClientContactListItem()
        {
            id = -1;
            clientId = -1;
            name = null;
            preferredName = null;
            type = null;
            details = null;
            isDefault = false;
        }
    }
}