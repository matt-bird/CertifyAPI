using System;
using System.Windows;
using System.Data;

using CertifyWPF.WPF_Library;
using CertifyWPF.WPF_Service;
using CertifyWPF.WPF_Utils;

namespace CertifyWPF.WPF_Client
{
    /// <summary>
    /// Client Services.  The services to which the Client has applied for or are certified to.  Client Services have histories.
    /// </summary>
    /// <seealso cref="ClientServiceHistory"/>
    /// <seealso cref="Service"/>

    public class ClientService
    {
        /// <summary>
        /// The primary key Id of the Client Service.
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// The primary key Id of the Client.
        /// </summary>
        public long clientId { get; set; }

        /// <summary>
        /// The primary key Id of the Service.
        /// </summary>
        public long serviceId { get; set; }

        /// <summary>
        /// The Service Status.  Service statuses such as "Applied" and "Suspended" 
        /// are defined in the <strong>serviceStatus</strong> table.
        /// </summary>
        public string status { get; set; }


        /// <summary>
        /// Constructor.  Use this for a new Client Service.
        /// </summary>
        //----------------------------------------------------------------------------------------------------------------------------
        public ClientService()
        {
            id = -1;
            clientId = -1;
            serviceId = -1;
            status = null;
        }


        /// <summary>
        /// Constructor.  Use this to open an Existing Client Service.
        /// </summary>
        /// <param name="_id">The primary key Id of the Client Service to open.</param>
        //----------------------------------------------------------------------------------------------------------------------------
        public ClientService(long _id)
        {
            id = _id;
            clientId = -1;
            serviceId = -1;
            status = null;

            fetch();
        }


        /// <summary>
        /// Constructor.  Use this to open an Existing Client Service.
        /// </summary>
        /// <param name="_clientId">The primary key Id of the Client.</param>
        /// <param name="_serviceId">The primary key Id of the Service.</param>
        //----------------------------------------------------------------------------------------------------------------------------
        public ClientService(long _clientId, long _serviceId)
        {
            clientId = _clientId;
            serviceId = _serviceId;
            status = null;

            id = find();
        }


        /// <summary>
        /// Find an existing Client Service record from the database using the clientId and serviceId.
        /// </summary>
        /// <returns>True if the Client Service was found.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public long find()
        {
            SQL mySql = new SQL();
            mySql.addParameter("clientId", clientId.ToString());
            mySql.addParameter("serviceId", serviceId.ToString());
            DataTable records = mySql.getRecords(@"SELECT * FROM clientService 
                                                   WHERE 
                                                   isDeleted = 0 AND 
                                                   clientId = @clientId AND
                                                   serviceId = @serviceId");

            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        /// <summary>
        /// Fetch an existing Client Service record from the database using the primary key Id of the Client Service.
        /// </summary>
        /// <returns>True if the Client Service was found.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool fetch()
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM clientService WHERE isDeleted = 0 AND id = @id");

            if (records.Rows.Count == 1)
            {
                DataRow row = records.Rows[0];
                clientId = Convert.ToInt64(row["clientId"].ToString());
                serviceId = Convert.ToInt64(row["serviceId"].ToString());
                long serviceStatusId = Convert.ToInt64(row["serviceStatusId"].ToString());
                status = UtilsList.getServiceStatus(serviceStatusId);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Save the Client Service record to the database.  This is an Upsert operation.
        /// </summary>
        /// <returns>True if the Upsert was successful.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool save()
        {
            SQL mySql = new SQL();
            mySql.addParameter("clientId", clientId.ToString());
            mySql.addParameter("serviceId", serviceId.ToString());
            mySql.addParameter("serviceStatusId", UtilsList.getServiceStatusId(status).ToString());

            // save
            if (id == -1)
            {
                // Make sure we do not already have this service for this client
                if (!ClientService.clientHasService(clientId, serviceId))
                {
                    mySql.setQuery(@"INSERT INTO clientService 
                                (clientId, serviceId, serviceStatusId) 
                                VALUES 
                                (@clientId, @serviceId, @serviceStatusId)");

                    if (mySql.executeSQL() == 1)
                    {
                        // Insert worked - now add a history entry
                        id = mySql.getMaxId("clientService");
                        return true;
                    }
                    else Log.write("An error ocurred while trying to add a service to the client. The service was not added to the client properly.");
                }
                return true; // Just return true of the client already has this service

            }

            // Update
            else
            {
                mySql.addParameter("id", id.ToString());
                mySql.setQuery(@"UPDATE clientService 
                                 SET 
                                 serviceStatusId = @serviceStatusId
                                 WHERE id = @id");

                if (mySql.executeSQL() == 1) return true;

                else Log.write("There was an error while trying to update this service. The status has not changed properly.");
            }

            return false;
        }


        /// <summary>
        /// Mark the Client Service as Deleted, and add a Client Service History entry for this.
        /// </summary>
        /// <returns>True if the Client Service was marked as deleted.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool delete()
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            mySql.setQuery("UPDATE clientService SET isDeleted = 1 WHERE id = @id"); // We only mark it as deleted
            if (mySql.executeSQL() == 1) return true;
            return false;
        }


        /// <summary>
        /// Permanently delete the Client Service, and add a Client Service History entry for this.
        /// </summary>
        /// <returns>True if the Client Service was deleted.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool deleteWithMalice()
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            mySql.setQuery("DELETE clientService WHERE id = @id"); 
            if (mySql.executeSQL() == 1) return true;
            return false;
        }


        /// <summary>
        /// Determine if a service was active during a time period.  Used for annual reports.
        /// </summary>
        /// <param name="periodStart">The time period start.</param>
        /// <param name="periodEnd">The time period end.</param>
        /// <returns>True if this client service was active during a time period.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool activeDuringPeriod(DateTime periodStart, DateTime periodEnd)
        {
            bool nonActiveStatusDetected = false;

            SQL mySql = new SQL();
            mySql.addParameter("clientServiceId", id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM clientServiceHistory WHERE clientServiceId = @clientServiceId ORDER BY historyDateTime DESC");
            foreach (DataRow row in records.Rows)
            {
                long statusId = Utils.getLongFromString(row["serviceStatusId"].ToString());
                if (statusId == UtilsList.getServiceStatusId("Active") ||
                    statusId == UtilsList.getServiceStatusId("Intent to Suspend"))
                {
                    DateTime entryDateTime = Utils.getDateTime(row["historyDateTime"].ToString());
                    if(nonActiveStatusDetected)
                    {
                        if (entryDateTime >= periodStart) return true;
                    }
                    else
                    {
                        if (entryDateTime <= periodEnd) return true;
                    }
                    
                }
                else nonActiveStatusDetected = true;
            }
            return false;
        }


        //
        // Static Methods
        //

        /// <summary>
        /// Determine if a Client has a Service.
        /// </summary>
        /// <param name="clientId">The primary key Id of the Client.</param>
        /// <param name="serviceId">The primary key Id of the Service.</param>
        /// <returns>True if the Client has the Service.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static bool clientHasService(long clientId, long serviceId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("clientId", clientId.ToString());
            mySql.addParameter("serviceId", serviceId.ToString());
            DataTable dataTable = mySql.getRecords("SELECT id FROM clientService WHERE IsDeleted = 0 AND clientId = @clientId AND serviceId = @serviceId");

            // If we have more than o rows, the client has the service
            if (dataTable.Rows.Count > 0) return true;

            // Client does not have the service
            return false;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static DateTime getEarliestCertificationDate(Client client)
        {
            SQL mySql = new SQL();
            mySql.addParameter("clientId", client.id.ToString());
            mySql.addParameter("activeStatusId", Convert.ToString(UtilsList.getServiceStatusId("Active")));

            DataTable records = mySql.getRecords(@"SELECT
                                                   clientService.clientId, MIN(clientServiceHistory.historyDateTime) AS earliestActiveDate
                                                   FROM 
                                                   clientService 
                                                   INNER JOIN clientServiceHistory ON clientService.id = clientServiceHistory.clientServiceId 
                                                   INNER JOIN serviceStatus ON clientServiceHistory.serviceStatusId = serviceStatus.id
                                                   WHERE    
                                                   clientId = @clientId AND 
                                                   clientServiceHistory.isDeleted = 0 AND 
                                                   serviceStatus.name = N'Active'
                                                   GROUP BY 
                                                   clientService.clientId");

            if (records.Rows.Count == 1) return Utils.getDateTime(records.Rows[0]["earliestActiveDate"].ToString());
            return new DateTime(1970, 1, 1);
        }

    } 

} 