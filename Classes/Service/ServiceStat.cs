using System;
using CertifyWPF.WPF_Client;

namespace CertifyWPF.WPF_Service
{
    /// <summary>
    /// A class to collect various statistics about a clients service.
    /// </summary>
    public class ServiceStat
    {
        /// <summary>
        /// The client.
        /// </summary>
        public Client client { get; set; }

        /// <summary>
        /// The service.
        /// </summary>
        public Service service { get; set; }

        /// <summary>
        /// The number of certified items for this client and service.
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// The date of the initial audit for this client and service.
        /// </summary>
        public DateTime initialAuditDate { get; set; }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="_client">The client for which the stats are required.</param>
        /// <param name="_service">The service for which the stats are required.</param>
        //-------------------------------------------------------------------------------------------------------------------------
        public ServiceStat()
        {
            client = null;
            service = null;
            count = 0;
            initialAuditDate = new DateTime(1970,1,1);
        }
    }
}
