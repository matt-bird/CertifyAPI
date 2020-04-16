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
    /// <seealso cref="ClientService"/>
    /// <seealso cref="Client"/>
    /// <seealso cref="Service"/>

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
        public long serviceStatusId { get; set; }

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
            serviceStatusId = -1;
            userId = -1;
        }


        /// <summary>
        /// Add a Client Service History entry.
        /// </summary>
        /// <param name="_serviceStatusId">The primary key Id of the Service Status.  Service statuses such as "Applied" or "Suspended" are defined in 
        /// the <strong>serviceStatus</strong> table.</param>
        /// <returns>True if the Client Service History entry was added successfully.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool addHistoryEntry(long _serviceStatusId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("clientServiceId", clientServiceId.ToString());
            mySql.addParameter("serviceStatusId", _serviceStatusId.ToString());
            mySql.addParameter("userId", User.getUserIdFromSession().ToString());

            mySql.setQuery("INSERT INTO clientServiceHistory (clientServiceId, serviceStatusId, userId) " +
                            "VALUES (@clientServiceId, @serviceStatusId, @userId)");

            // Make sure we got exactly 1 row 
            if (mySql.executeSQL() != 1)
            {
                Log.write("An error ocurred while trying to add a history entry.");
                return false;
            }

            return true;
        }
    }
}
