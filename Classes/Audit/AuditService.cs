using System;
using System.Collections.Generic;
using System.Data;

using CertifyWPF.WPF_Library;

namespace CertifyWPF.WPF_Audit
{

    /// <summary>
    /// Audit Services. The services that the audit was conducted for.  This can be found in the <strong>auditService</strong> table.
    /// </summary>
    public class AuditService
    {
        /// <summary>
        /// The primary key Id of this Audit Services
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// The primary key Id of the audit for which the service is to be applied.
        /// </summary>
        public long auditId { get; set; }

        /// <summary>
        /// The primary key Id of the service to be applied to the audit.
        /// </summary>
        public long serviceId { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        //--------------------------------------------------------------------------------------------------------------------------
        public AuditService()
        {
            id = -1;
            auditId = -1;
            serviceId = -1;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        //--------------------------------------------------------------------------------------------------------------------------
        public AuditService(long _id)
        {
            id = _id;
            auditId = -1;
            serviceId = -1;

            fetch();
        }


        /// <summary>
        /// Constructor with initialisers.
        /// </summary>
        /// <param name="inAuditId">The primary key Id of the audit for which the service is to be applied.</param>
        /// <param name="inServiceId">The primary key Id of the service to be applied to the audit.</param>
        //--------------------------------------------------------------------------------------------------------------------------
        public AuditService(long inAuditId, long inServiceId)
        {
            auditId = inAuditId;
            serviceId = inServiceId;
            find();
        }


        /// <summary>
        /// Retrieve an Audit Service record from the database.
        /// </summary>
        /// <returns>True if the record was retrieved successfully.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool fetch()
        {
            if (id == -1) return false;

            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM auditService WHERE id = @id");
            if (records.Rows.Count == 1)
            {
                auditId = Convert.ToInt64(records.Rows[0]["auditId"].ToString());
                serviceId = Convert.ToInt64(records.Rows[0]["serviceId"].ToString());
                return true;
            }
            return false;
        }


        /// <summary>
        /// Retrieve an Audit Service record from the database, based on an audit and service ID.
        /// </summary>
        /// <returns>True if the record was retrieved successfully.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool find()
        {
            if (auditId == -1 || serviceId == -1) return false;

            SQL mySql = new SQL();
            mySql.addParameter("auditId", auditId.ToString());
            mySql.addParameter("serviceId", serviceId.ToString());
            DataTable records = mySql.getRecords("SELECT id FROM auditService WHERE IsDeleted = 0 AND auditId = @auditId AND serviceId = @serviceId");

            if (records.Rows.Count == 1)
            {
                id = Convert.ToInt64(records.Rows[0]["id"].ToString());
                return true;
            }
            return false;
        }


        /// <summary>
        /// Determine if the audit already has a particular service.
        /// </summary>
        /// <returns>True if the audit has the service.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool auditHasService()
        {
            if (auditId == -1 || serviceId == -1) return false;

            SQL mySql = new SQL();
            mySql.addParameter("auditId", auditId.ToString());
            mySql.addParameter("serviceId", serviceId.ToString());
            DataTable dataTable = mySql.getRecords("SELECT id FROM auditService WHERE IsDeleted = 0 AND auditId = @auditId AND serviceId = @serviceId");

            // If we have more than o rows, the client has the service
            if (dataTable.Rows.Count > 0) return true;

            // Client does not have the service
            return false;
        }


        /// <summary>
        /// Saves the record in the database. This is an upsert operation.
        /// </summary>
        /// <returns>True if the record was created / saved correctly.  False otherwsie.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool save()
        {
            // Form Query
            SQL mySql = new SQL();
            mySql.addParameter("auditId", auditId.ToString());
            mySql.addParameter("serviceId", serviceId.ToString());

            if(id == -1)
            {
                mySql.setQuery("INSERT INTO auditService (auditId, serviceId) VALUES (@auditId, @serviceId)");
                if (mySql.executeSQL() == 1)
                {
                    id = mySql.getMaxId("auditService");
                    return true;
                }
            }
            else
            {
                mySql.addParameter("id", id.ToString());
                mySql.setQuery(@"UPDATE auditService 
                                 SET
                                 auditId = @auditId,
                                 serviceId = @serviceId
                                 WHERE
                                 id = @id");

                if (mySql.executeSQL() == 1)return true;
            }
            return false;
        }


        /// <summary>
        /// Permanently delete and Audit Service.
        /// </summary>
        /// <returns>True if the record was deleted.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool delete()
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            mySql.setQuery("DELETE auditService WHERE id = @id");
            if (mySql.executeSQL() == 1) return true;
            return false;
        }



        //
        // Static Methods
        //

        /// <summary>
        /// Get a list of Audit Services for an audit.
        /// </summary>
        /// <param name="audit">The audit for which the services are linked.</param>
        /// <returns>A list of Audit Services for an audit.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static List<AuditService> getServices(Audit audit)
        {
            List<AuditService> services = new List<AuditService>();

            SQL mySql = new SQL();
            mySql.addParameter("auditId", audit.id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM auditService WHERE IsDeleted = 0 AND auditId = @auditId");
            foreach(DataRow row in records.Rows)
            {
                long id = Convert.ToInt64(row["id"].ToString());
                AuditService ser = new AuditService(id);
                services.Add(ser);
            }

            return services;
        }
    }
}
