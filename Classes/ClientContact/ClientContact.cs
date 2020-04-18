using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using CertifyWPF.WPF_Library;
using CertifyWPF.WPF_Utils;

namespace CertifyWPF.WPF_Client
{
    /// <summary>
    /// Client Contacts.  Client Contacts can be default (automated Emails go to these clients), or not.  Client Contacts are stored in
    /// the <strong>clientContact</strong> table.  Client Contacts have different types such as "Email - Work" or "Phone - Home" that 
    /// are defined in the <strong>list_contactType</strong> table.
    /// </summary>
    public class ClientContact
    {
        /// <summary>
        /// The primary key Id for this contact.
        /// </summary>
        public long id;

        /// <summary>
        /// The primary key Id of the Client who owns this Contact.
        /// </summary>
        public long clientId { get; set; }

        /// <summary>
        /// The primary key Id of the Contact Type.  Contact Types such as "Email - Work" or "Phone - Home" are defined in 
        /// the <strong>list_contactType</strong> table.
        /// </summary>
        public long list_contactTypeId { get; set; }

        /// <summary>
        /// The name of the Client Contact.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// The preferred name of the Client Contact.
        /// </summary>
        public string preferredName { get; set; }

        /// <summary>
        /// The contact details (such as the actual phone number or email address) of the Client Contact.
        /// </summary>
        public string details { get; set; }

        /// <summary>
        /// A flag to indicate if the Client COntact is the Default contact for the Client.
        /// </summary>
        public bool isDefault { get; set; }


        /// <summary>
        /// Constructor. Use this for a new client contact.
        /// </summary>
        public ClientContact()
        {
            id = -1;
            clientId = -1;
            list_contactTypeId = -1;
            name = null;
            preferredName = null;
            details = null;
            isDefault = false;
        }


        /// <summary>
        /// Constructor. Use this for a new client contact.
        /// </summary>
        /// <param name="_id">The primary key Id of the Client who owns this Contact.</param>
        public ClientContact(long _id)
        {
            id = _id;
            clientId = -1;
            list_contactTypeId = -1;
            name = null;
            preferredName = null;
            details = null;
            isDefault = false;
            fetch();
        }


        /// <summary>
        /// Fetch an existing Client Contact record from the database.
        /// </summary>
        /// <returns>True if the Client Contact was found.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool fetch()
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM clientContact WHERE isDeleted = 0 AND id = @id");

            if (records.Rows.Count == 1)
            {
                DataRow row = records.Rows[0];
                clientId = Convert.ToInt64(row["clientId"].ToString());
                list_contactTypeId = Convert.ToInt64(row["list_contactTypeId"].ToString());
                name = row["name"].ToString();
                preferredName = row["preferredName"].ToString();
                details = row["details"].ToString();
                isDefault = Convert.ToBoolean(row["isDefault"].ToString());
                return true;
            }
            return false;
        }


        /// <summary>
        /// Save the Client Contact record to the database.  This is an Upsert operation.
        /// </summary>
        /// <returns>True if the Upsert was successful.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool save()
        {
            SQL mySql = new SQL();
            mySql.addParameter("clientId", clientId.ToString());
            mySql.addParameter("list_contactTypeId", list_contactTypeId.ToString());
            mySql.addParameter("name", name);
            mySql.addParameter("preferredName", preferredName);
            mySql.addParameter("details", details);

            if (isDefault)mySql.addParameter("isDefault", "1");
            else mySql.addParameter("isDefault", "0");

            // save
            if (id == -1)
            {
                mySql.setQuery(@"INSERT INTO clientContact 
                                (clientId, list_contactTypeId, name, details, preferredName, isDefault) 
                                VALUES 
                                (@clientId, @list_contactTypeId, @name, @details, @preferredName, @isDefault)");

                if (mySql.executeSQL() == 1)
                {
                    id = mySql.getMaxId("clientContact");
                    return true;
                }
                else Log.write("An error ocurred while trying to add a service to the client. The service was not added to the client properly.");
            }

            // Update
            else
            {
                mySql.addParameter("id", id.ToString());
                mySql.setQuery(@"UPDATE clientContact 
                                 SET 
                                 clientId = @clientId,
                                 list_contactTypeId = @list_contactTypeId,
                                 name = @name, 
                                 details = @details,
                                 preferredName = @preferredName, 
                                 isDefault = @isDefault
                                 WHERE id = @id");

                if (mySql.executeSQL() == 1) return true;
                else Log.write("There was an error while trying to update this service. The status has not changed properly.");
            }

            return false;
        }


        /// <summary>
        /// Mark a Client Contact as deleted.
        /// </summary>
        /// <returns>True iof the record was successfully marked as deleted.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool delete()
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            mySql.setQuery("UPDATE clientContact SET isDeleted = 1 WHERE id = @id");
            if (mySql.executeSQL() == 1) return true;
            return false;
        }


        /// <summary>
        /// Get a list of all Client Contacts for a Client.
        /// </summary>
        /// <param name="clientId">The primary key Id of the Client who owns this Contact.</param>
        /// <returns>A list (DataTable) of all Client Contacts for a Client.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static List<ClientContact> getClientContacts(Client client, bool onlyDefault = true, bool onlyPhoneNumber = true)
        {
            List<ClientContact> contacts = new List<ClientContact>();

            SQL mySql = new SQL();
            mySql.addParameter("id", client.id.ToString());
            
            string query = "SELECT * FROM clientContact WHERE clientId = @id";
            
            if (onlyDefault) query += " AND isDefault = 1";
            
            if (onlyPhoneNumber) query += " AND (list_contactTypeId = " + 
                                          UtilsList.getContactTypeId("Phone - Work") +
                                          " OR list_contactTypeId = " + 
                                          UtilsList.getContactTypeId("Phone - Home") + ")";
            
            DataTable records = mySql.getRecords(query);
            foreach(DataRow row in records.Rows)
            {
                long id = Utils.getLongFromString(row["id"].ToString());
                if (id != -1) contacts.Add(new ClientContact(id));
            }
            return contacts;
        }


        /// <summary>
        /// Get a list of all Client Contacts for a Client.
        /// </summary>
        /// <param name="client">The primary key Id of the Client who owns this Contact.</param>
        /// <returns>A list of all Client Contacts for a Client.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static List<ClientContact> getClientContacts(Client client)
        {
            List<ClientContact> contacts = new List<ClientContact>();

            SQL mySql = new SQL();
            mySql.addParameter("id", client.id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM clientContact WHERE isDeleted = 0 AND clientId = @id");
            foreach(DataRow row in records.Rows)
            {
                contacts.Add(new ClientContact(Convert.ToInt64(row["id"].ToString())));
            }

            return contacts;
        }


        /// <summary>
        /// Get a list of all contacts.
        /// </summary>
        /// <returns>A list of clients.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static List<ClientContactListItem> getContactList(long clientId = -1)
        {
            List<ClientContactListItem> list = new List<ClientContactListItem>();
            SQL mySql = new SQL();
            DataTable records;
            if (clientId != -1)
            {
                mySql.addParameter("clientId", clientId.ToString());
                records = mySql.getRecords(@"SELECT        
                                             clientContact.id, 
                                             clientContact.clientId, 
                                             clientContact.name, 
                                             clientContact.preferredName, 
                                             list_contactType.type, 
                                             clientContact.details, 
                                             clientContact.isDefault
                                             FROM            
                                             clientContact INNER JOIN list_contactType ON clientContact.list_contactTypeId = list_contactType.id
                                             WHERE
                                             clientId = @clientId AND
                                             clientContact.isDeleted = 0");
            }
            else
            {
                records = mySql.getRecords(@"SELECT        
                                             clientContact.id, 
                                             clientContact.clientId, 
                                             clientContact.name, 
                                             clientContact.preferredName, 
                                             list_contactType.type, 
                                             clientContact.details, 
                                             clientContact.isDefault
                                             FROM            
                                             clientContact INNER JOIN list_contactType ON clientContact.list_contactTypeId = list_contactType.id
                                             WHERE
                                             clientContact.isDeleted = 0");
            }
            foreach (DataRow row in records.Rows)
            {
                ClientContactListItem item = new ClientContactListItem()
                {
                    id = Utils.getLongFromString(row["id"].ToString()),
                    clientId = Utils.getLongFromString(row["clientId"].ToString()),
                    name = row["name"].ToString(),
                    preferredName = row["preferredName"].ToString(),
                    type = row["type"].ToString(),
                    details = row["details"].ToString(),
                    isDefault = Convert.ToBoolean(row["isDefault"].ToString())
                };
                list.Add(item);
            }
            return list;
        }
    }
}
