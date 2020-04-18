using System.Windows;

using CertifyWPF.WPF_Library;
using CertifyWPF.WPF_Service;
using CertifyWPF.WPF_User;

namespace CertifyWPF.WPF_Client
{
    /// <summary>
    /// Client Service History.  This represents the history of the various stages that a service for a client can go 
    /// through - such as "Applied" or "Suspended". Client Service History entries are stored in the 
    /// <strong>clientServiceHistory</strong> table.
    /// </summary>

    public class ClientServiceHistory
    {
        /// <summary>
        /// The primary key Id of the Client Service History entry.
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// The primary key Id of the Client Service.
        /// </summary>
        public long clientServiceId { get; set; }

        /// <summary>
        /// The primary key Id of the Service Status.  Service statuses such as "Applied" or "Suspended" are defined in 
        /// the <strong>serviceStatus</strong> table.
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// The primary key Id of the user who added the Client Service History entry.
        /// </summary>
        public long userId { get; set; }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="_clientServiceId">The primary key Id of the Client Service.</param>
        public ClientServiceHistory(long _clientServiceId)
        {
            id = -1;
            clientServiceId = _clientServiceId;
            status = null;
            userId = -1;
        }
    }
}
