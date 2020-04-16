
using System;
using System.Data;

using CertifyWPF.WPF_Library;
using CertifyWPF.WPF_Utils;

namespace CertifyWPF.WPF_Client
{
    public class ClientAuditCycle
    {
        /// <summary>
        /// Primary key id of the Client Audit Cycle record
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// The primary key id of the cleint for this record
        /// </summary>
        public long clientId { get; set; }

        /// <summary>
        /// The primary key id of the user who is the preferred auditor for this client
        /// </summary>
        public long preferredAuditorUserId { get; set; }

        /// <summary>
        /// How long in between audits
        /// </summary>
        public int auditCycleFrequency { get; set; }

        /// <summary>
        /// THe month of the clients audit cycle
        /// </summary>
        public int auditCycleMonth { get; set; }

        /// <summary>
        /// The day of the moneth of the audit cycle
        /// </summary>
        public int auditCycleDay { get; set; }

        /// <summary>
        /// Constructor - new.
        /// </summary>
        //--------------------------------------------------------------------------------------------------------------------------
        public ClientAuditCycle()
        {
            id = -1;
            clientId = -1;
            preferredAuditorUserId = -1;
            auditCycleFrequency = -1;
            auditCycleMonth = -1;
            auditCycleDay = -1;
        }

        /// <summary>
        /// Constructor - Create a new cycle for a client.
        /// </summary>
        //--------------------------------------------------------------------------------------------------------------------------
        public ClientAuditCycle(Client client)
        {
            id = -1;
            clientId = client.id;
            preferredAuditorUserId = -1;
            auditCycleFrequency = 12;
            auditCycleMonth = -1;
            auditCycleDay = -1;
        }


        /// <summary>
        /// Constructor - existing.
        /// </summary>
        /// <param name="_id"></param>
        //--------------------------------------------------------------------------------------------------------------------------
        public ClientAuditCycle(long _id)
        {
            id = _id;
            clientId = -1;
            preferredAuditorUserId = -1;
            auditCycleFrequency = -1;
            auditCycleMonth = -1;
            auditCycleDay = -1;

            fetch();
        }


        /// <summary>
        /// Retrieve the record from the database.
        /// </summary>
        /// <returns>True if the record was retrieved successfully.  False otherwise.</returns>
        //-------------------------------------------------------------------------------------------------------------
        public bool fetch()
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM clientAuditCycle WHERE id = @id");

            if (records.Rows.Count == 1)
            {
                DataRow row = records.Rows[0];
                clientId = Utils.getLongFromString(row["clientId"].ToString());
                preferredAuditorUserId = Utils.getLongFromString(row["preferredAuditorUserId"].ToString());
                auditCycleFrequency = Utils.getIntFromString(row["auditCycleFrequency"].ToString());
                auditCycleMonth = Utils.getIntFromString(row["auditCycleMonth"].ToString());
                auditCycleDay = Utils.getIntFromString(row["auditCycleDay"].ToString());
                return true;
            }
            return false;
        }


        /// <summary>
        /// Save a new or Update and existing ClientGroup database record.  This is an upsert operation.
        /// </summary>
        /// <returns>True of the update was successful.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool save()
        {
            if (clientId == -1) return false;

            SQL mySql = new SQL();
            mySql.addParameter("clientId", clientId.ToString());

            if (preferredAuditorUserId != -1) mySql.addParameter("preferredAuditorUserId", preferredAuditorUserId.ToString());
            else mySql.addParameter("preferredAuditorUserId", null);

            if (auditCycleFrequency != -1) mySql.addParameter("auditCycleFrequency", auditCycleFrequency.ToString());
            else mySql.addParameter("auditCycleFrequency", null);

            if (auditCycleMonth != -1) mySql.addParameter("auditCycleMonth", auditCycleMonth.ToString());
            else mySql.addParameter("auditCycleMonth", null);

            if (auditCycleDay != -1) mySql.addParameter("auditCycleDay", auditCycleDay.ToString());
            else mySql.addParameter("auditCycleDay", null);


            // New Group
            if (id == -1)
            {
                mySql.setQuery(@"INSERT INTO clientAuditCycle 
                                (clientId, preferredAuditorUserId, auditCycleMonth, auditCycleDay, auditCycleFrequency) 
                                Values 
                                (@clientId, @preferredAuditorUserId, @auditCycleMonth, @auditCycleDay, @auditCycleFrequency)");

                if (mySql.executeSQL() == 1)
                {
                    id = mySql.getMaxId("clientAuditCycle");
                    return true;
                }
            }

            else
            {
                mySql.addParameter("id", id.ToString());

                mySql.setQuery(@"UPDATE clientAuditCycle SET 
                                clientId = @clientId, 
                                preferredAuditorUserId = @preferredAuditorUserId,
                                auditCycleFrequency = @auditCycleFrequency,
                                auditCycleMonth = @auditCycleMonth, 
                                auditCycleDay = @auditCycleDay
                                WHERE id = @id");

                if (mySql.executeSQL() == 1) return true;
            }
            return false;
        }


        /// <summary>
        /// Determine if the audit cycle has been set properly
        /// </summary>
        /// <returns>True if the audit cycle has been set properly.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool valid()
        {
            if (id ==-1 ||
                preferredAuditorUserId == -1 ||
                auditCycleDay == -1 ||
                auditCycleMonth == -1) return false;

            return true;
        }


        /// <summary>
        /// Retreieve a Client Audit cycle record for a client.
        /// </summary>
        /// <param name="client">The client</param>
        /// <returns>A Client Audit cycle record for a client.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static ClientAuditCycle get(Client client)
        {
            SQL mySql = new SQL();
            mySql.addParameter("clientId", client.id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM clientAuditCycle WHERE isDeleted = 0 AND clientId = @clientId");
            if (records.Rows.Count == 1) return new ClientAuditCycle(Convert.ToInt64(records.Rows[0]["id"].ToString()));
            return null;
        }
    }
}
