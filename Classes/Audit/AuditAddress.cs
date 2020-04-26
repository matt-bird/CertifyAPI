using System;
using System.Collections.Generic;
using System.Data;
using CertifyWPF.WPF_Client;
using CertifyWPF.WPF_Library;
using CertifyWPF.WPF_Utils;

namespace CertifyWPF.WPF_Audit
{

    /// <summary>
    /// Audit Address. The client addresses that the audit was conducted for.  This can be found in the <strong>auditAddress</strong> table.
    /// </summary>
    public class AuditAddress
    {
        /// <summary>
        /// The primary key Id of this Audit Address
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// The primary key Id of the audit for which the client address is to be applied.
        /// </summary>
        public long auditId { get; set; }

        /// <summary>
        /// The primary key Id of the client address to be applied to the audit.
        /// </summary>
        public long clientAddressId { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        //--------------------------------------------------------------------------------------------------------------------------
        public AuditAddress()
        {
            id = -1;
            auditId = -1;
            clientAddressId = -1;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        //--------------------------------------------------------------------------------------------------------------------------
        public AuditAddress(long _id)
        {
            id = _id;
            auditId = -1;
            clientAddressId = -1;

            fetch();
        }


        /// <summary>
        /// Retrieve an Audit Address record from the database.
        /// </summary>
        /// <returns>True if the record was retrieved successfully.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool fetch()
        {
            if (id == -1) return false;

            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM auditAddress WHERE id = @id");
            if (records.Rows.Count == 1)
            {
                auditId = Convert.ToInt64(records.Rows[0]["auditId"].ToString());
                clientAddressId = Convert.ToInt64(records.Rows[0]["clientAddressId"].ToString());
                return true;
            }
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
            mySql.addParameter("clientAddressId", clientAddressId.ToString());

            if(id == -1)
            {
                mySql.setQuery("INSERT INTO auditAddress (auditId, clientAddressId) VALUES (@auditId, @clientAddressId)");
                if (mySql.executeSQL() == 1)
                {
                    id = mySql.getMaxId("auditAddress");
                    return true;
                }
            }
            else
            {
                mySql.addParameter("id", id.ToString());
                mySql.setQuery(@"UPDATE auditAddress 
                                 SET
                                 auditId = @auditId,
                                 clientAddressId = @clientAddressId
                                 WHERE
                                 id = @id");

                if (mySql.executeSQL() == 1)return true;
            }
            return false;
        }


        /// <summary>
        /// Permanently delete and Audit Address.
        /// </summary>
        /// <returns>True if the record was deleted.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool delete()
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            mySql.setQuery("DELETE auditAddress WHERE id = @id");
            if (mySql.executeSQL() == 1) return true;
            return false;
        }



        //
        // Static Methods
        //

        /// <summary>
        /// Get a list of Audit Address for an audit.
        /// </summary>
        /// <param name="audit">The audit for which the client addresses are linked.</param>
        /// <returns>A list of Audit Addresses for an audit.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static List<ClientAddress> getAddresses(Audit audit)
        {
            List<ClientAddress> addresses = new List<ClientAddress>();

            SQL mySql = new SQL();
            mySql.addParameter("auditId", audit.id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM auditAddress WHERE auditId = @auditId");

            foreach(DataRow row in records.Rows)
            {
                long id = Utils.getLongFromString(row["clientAddressId"].ToString());
                if (id != -1)
                {
                    ClientAddress adr = new ClientAddress(id);
                    addresses.Add(adr);
                }
            }
            return addresses;
        }


        /// <summary>
        /// Delete all addresses associated with an audit.  Doing this as a static function here because at this stage, I see no 
        /// reason to add a list of addresses to the audit class.
        /// </summary>
        /// <param name="audit">THe aduit for which we want to delete the addresses</param>
        /// <returns>True if at least one address was deleted.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static void deleteAddresses(Audit audit)
        {
            SQL mySql = new SQL();
            mySql.addParameter("auditId", audit.id.ToString());
            mySql.setQuery("DELETE FROM auditAddress WHERE auditId = @auditId");
            mySql.executeSQL();            
        }


        /// <summary>
        /// Add a list of addresses to an Audit
        /// </summary>
        /// <param name="audit">The aduit to which to add the addresses.</param>
        /// <param name="addresseStrings">A list of address strings (that should be formated using the 'prettyAddressShort' format.</param>
        /// <returns>True if all addresses were saved.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static bool addAddresses(Audit audit, List<string> addresseStrings)
        {
            Client client = new Client(audit.clientId);
            List<ClientAddress> clientAddresses = ClientAddress.getClientAddresses(client, true);

            int counter = 0;

            foreach (ClientAddress clientAddress in clientAddresses)
            {
                foreach (string addressString in addresseStrings)
                {
                    if (clientAddress.prettyAddressShort == addressString)
                    {
                        AuditAddress newAdrress = new AuditAddress()
                        {
                            auditId = audit.id,
                            clientAddressId = clientAddress.id
                        };
                        if (newAdrress.save()) counter++;
                    }
                }
            }
            if (counter == addresseStrings.Count) return true;
            return false;
        }

    }
}
