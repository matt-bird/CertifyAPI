using System;
using System.Collections.Generic;
using System.Data;

using CertifyWPF.WPF_Library;
using CertifyWPF.WPF_Utils;
using CertifyWPF.WPF_Service;

namespace CertifyWPF.WPF_Client
{
    /// <summary>
    /// Client.  All informtion in Certify and CertifyWeb is in one way or another linked back to the Client class.  All Client Information is 
    /// stored in the <strong>client</strong> table.
    /// </summary>

    public class Client
    {
        /// <summary>
        /// The primary key Id of the Client.
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// The name of the company for this Client.
        /// </summary>
        public string company { get; set; }

        /// <summary>
        /// The ABN of the company for this Client.
        /// </summary>
        public string ABN { get; set; }

        /// <summary>
        /// The name of the consultant used by the Client - if any.
        /// </summary>
        public string consultantName { get; set; }

        /// <summary>
        /// The network path to the Certification Agreement for this Client.
        /// </summary>
        public string agreementFileLink { get; set; }

        /// <summary>
        /// The MD5 signature of the original agreement. We can use this to determine if an agreement has been signed.
        /// </summary>
        public string agreementHash{ get; set; }

        /// <summary>
        /// The Trading As name of the company (if one exists) for this Client.
        /// </summary>
        public string tradingAs { get; set; }

        /// <summary>Client notes - long lived notes that can be used to help Auditors understand the 
        /// clients operations and previous difficult decisions.</summary>
        public string notes { get; set; }

        /// <summary>
        /// Client Audit notes - notes that are to communicate with an auditor what needs to be followed up at the next audit
        /// </summary>
        public string auditNotes { get; set; }

        /// <summary>
        /// A flag to indicate wether or not this Client has been deleted.
        /// </summary>
        public bool isDeleted { get; set; }

        /// <summary>
        /// A flag to indicate wether or not this Clients data is valid.
        /// </summary>
        public bool dataOk { get; set; }

        /// <summary>
        /// A list of client categories for this Client.
        /// </summary>
        public List<ClientCategory> clientCategories { get; set; }

        /// <summary>
        /// A list of the Client's services.
        /// </summary>
        public List<ClientService> clientServices { get; set; }

        /// <summary>
        /// The Clients Audit Cycle information
        /// </summary>
        public ClientAuditCycle auditCycle { get; set; }

        /// <summary>
        /// A flag to indicate if this client is to be used only for testing.  Blurgh!
        /// </summary>
        public bool isTest { get; set; }


        /// <summary>
        /// Empty constructor - Needed for JSON Serialisation.
        /// </summary>       
        //--------------------------------------------------------------------------------------------------------------------------
        public Client()
        {
            id = -1;
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="_Id">Optional: The primary key of the client. Default = -1 (new client)</param>
        //--------------------------------------------------------------------------------------------------------------------------
        public Client(long _Id = -1)
        {
            // Note that JSON Deserialize sets a default ID of 0 - and we never want JSON deserialize trying to get to the database 
            // (becuase the user will not be connected to the database)
            if (_Id == -1 || _Id == 0) dataOk = setDefaultProperties();
            else dataOk = fetch(_Id);
        }


        /// <summary>
        /// Initialise all properties.
        /// </summary>
        /// <returns>True.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        private bool setDefaultProperties()
        {
            id = -1;
            company = null;
            ABN = null;
            consultantName = null;
            tradingAs = null;
            notes = null;
            auditNotes = null;
            agreementFileLink = null;
            agreementHash = null;
            isDeleted = false;
            isTest = false;
            return true;
        }


        /// <summary>
        /// Fetch the clients details from the database.  All Client Information is stored in the <strong>client</strong> table.
        /// </summary>
        /// <param name="inId"></param>
        /// <returns>True if the fetch was OK.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        private bool fetch(long inId)
        {
            SQL mySql = new SQL();
            DataTable records = mySql.getRecords("SELECT * FROM client WHERE id = " + Convert.ToString(inId));
            if (records.Rows.Count == 1)
            {
                DataRow row = records.Rows[0];

                id = Convert.ToInt64(row["id"].ToString());
                company = row["company"].ToString();
                ABN = row["ABN"].ToString();
                consultantName = row["consultantName"].ToString();
                agreementFileLink = row["agreementFileLink"].ToString();
                agreementHash = row["agreementHash"].ToString();
                tradingAs = row["tradingAs"].ToString();
                notes = row["notes"].ToString();
                auditNotes = row["auditNotes"].ToString();
                isTest = Convert.ToBoolean(row["isTest"].ToString());
                isDeleted = Convert.ToBoolean(row["isDeleted"].ToString());

                getClientCategories();
                getClientServices();
                getClientAuditCycle();

                return true;
            }
            return false;
        }


        /// <summary>
        /// Update the clients database record.
        /// </summary>
        /// <returns>True of the update was successful.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool save()
        {
            SQL mySql = new SQL();
            mySql.addParameter("company", company);
            mySql.addParameter("ABN", ABN);
            mySql.addParameter("tradingAs", tradingAs);
            mySql.addParameter("consultantName", consultantName);
            mySql.addParameter("notes", notes);
            mySql.addParameter("auditNotes", auditNotes);

            if (isTest) mySql.addParameter("isTest", "1");
            else mySql.addParameter("isTest", "0");

            // New Client
            if (id == -1)
            {
                mySql.setQuery(@"INSERT INTO client 
                                (company, ABN, tradingAs, consultantName, notes, auditNotes, isTest) 
                                Values 
                                (@company, @ABN, @tradingAs, @consultantName, @notes, @auditNotes, @isTest)");

                if (mySql.executeSQL() == 1)
                {
                    id = mySql.getMaxId("client");
                    return true;
                }
            }

            else
            {
                mySql.addParameter("id", id.ToString());

                mySql.addParameter("agreementFileLink", agreementFileLink);
                mySql.addParameter("agreementHash", agreementHash);

                mySql.setQuery(@"UPDATE client SET 
                                company = @company, 
                                ABN = @ABN, 
                                consultantName = @consultantName,
                                agreementFileLink = @agreementFileLink, 
                                agreementHash = @agreementHash, 
                                tradingAs = @tradingAs, 
                                notes = @notes, 
                                auditNotes = @auditNotes,
                                isTest = @isTest
                                WHERE id = @id");

                if (mySql.executeSQL() == 1)
                {
                    // Save the audit cycle
                    if(auditCycle != null) return auditCycle.save();
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Get the Client Categories that are associated with this Client.
        /// </summary>
        //-----------------------------------------------------------------------------------------------------------------------------
        public void getClientCategories()
        {
            clientCategories = new List<ClientCategory>();

            SQL mySql = new SQL();
            mySql.addParameter("clientId", id.ToString());
            DataTable records = mySql.getRecords("SELECT id FROM clientCategory WHERE IsDeleted = 0 AND clientId = @clientId");
            foreach (DataRow row in records.Rows)
            {
                ClientCategory cc = new ClientCategory(Convert.ToInt64(row["id"].ToString()));
                clientCategories.Add(cc);
            }
        }


        /// <summary>
        /// Get the services this Client currently has.
        /// </summary>
        //-----------------------------------------------------------------------------------------------------------------------------
        public void getClientServices()
        {
            clientServices = new List<ClientService>();

            SQL mySql = new SQL();
            mySql.addParameter("clientId", id.ToString());
            DataTable records = mySql.getRecords("SELECT id FROM clientService WHERE IsDeleted = 0 AND clientId = @clientId");
            foreach(DataRow row in records.Rows)
            {
                ClientService cs = new ClientService(Convert.ToInt64(row["id"].ToString()));
                clientServices.Add(cs);
            }
        }


        /// <summary>
        /// Get the Risk assessments for this Client.
        /// </summary>
        //-----------------------------------------------------------------------------------------------------------------------------
        public void getClientAuditCycle()
        {
            auditCycle = ClientAuditCycle.get(this);
            if (auditCycle == null)
            {
                // If an audit cycle does not exist - create one and save it.
                auditCycle = new ClientAuditCycle(this);
                auditCycle.save();
            }
        }



        /// <summary>
        /// Get the Clients main operation address.
        /// </summary>
        /// <returns>The Clients main operation address.</returns>
        //-----------------------------------------------------------------------------------------------------------------------------
        public ClientAddress getOperationAddress()
        {
            SQL mySql = new SQL();
            mySql.addParameter("clientId", id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM clientAddress WHERE clientId = @clientId AND isOperationAddress = 1 AND isDeleted = 0");
            if (records.Rows.Count > 0) return new ClientAddress(Convert.ToInt64(records.Rows[0]["id"].ToString()));
            return null;
        }


        /// <summary>
        /// Get the Clients postal address.
        /// </summary>
        /// <returns>The Clients postal address.</returns>
        //-----------------------------------------------------------------------------------------------------------------------------
        public ClientAddress getPostalAddress()
        {
            SQL mySql = new SQL();
            mySql.addParameter("clientId", id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM clientAddress WHERE clientId = @clientId AND isPostalAddress = 1 AND isDeleted = 0");
            if (records.Rows.Count > 0) return new ClientAddress(Convert.ToInt64(records.Rows[0]["id"].ToString()));
            return null;
        }


        /// <summary>
        /// Get a list of the clients addresses.
        /// </summary>
        /// <returns>A list of the clients addresses.</returns>
        //-----------------------------------------------------------------------------------------------------------------------------
        public List<ClientAddress> getAddressesForCertificate()
        {
            List<ClientAddress> list = new List<ClientAddress>();

            // Find the correct address for invoices and certificates
            SQL mySql = new SQL();
            mySql.addParameter("clientId", id.ToString());
            mySql.addParameter("privateModifierId", UtilsList.getClientAddressModifierId("Private Address").ToString());
            DataTable records = mySql.getRecords(@"SELECT * FROM clientAddress 
                                                   WHERE 
                                                   list_clientAddressModifierId <> @privateModifierId AND
                                                   isDeleted = 0 AND 
                                                   clientId = @clientId");

            foreach(DataRow row in records.Rows)
            {
                long id = Convert.ToInt64(row["id"].ToString());
                if(id != -1) list.Add(new ClientAddress(id));
            }

            return list;
        }


        /// <summary>
        /// Determine if the Client has a Service.
        /// </summary>
        /// <param name="service">The Service to look for.</param>
        /// <returns>True if the Client has a Service. False otherwise.</returns>
        //-----------------------------------------------------------------------------------------------------------------------------
        public bool hasService(Service service)
        {
            foreach(ClientService clientService in clientServices)
            {
                if (clientService.serviceId == service.id) return true;
            }
            return false;
        }


        /// <summary>
        /// Determine if the Client has a Service.
        /// </summary>
        /// <param name="serviceName">The name of the Service to look for.</param>
        /// <returns>True if the Client has a Service. False otherwise.</returns>
        //-----------------------------------------------------------------------------------------------------------------------------
        public bool hasService(string serviceName)
        {
            long id = Service.getId(serviceName);
            if (id == -1) return false;

            Service service = new Service(id);
            return hasService(service);
        }


        /// <summary>
        /// Determine if the Client has a Category.
        /// </summary>
        /// <param name="catName">The name of the Service to look for.</param>
        /// <returns>True if the Client has a Service. False otherwise.</returns>
        //-----------------------------------------------------------------------------------------------------------------------------
        public bool hasCategory(string catName)
        {
            long id = UtilsList.getClientCategoryId(catName);
            if (id == -1) return false;

            foreach (ClientCategory cat in clientCategories)
            {
                if (cat.list_clientCategoryId == id) return true;
            }
            return false;
        }



        /// <summary>
        /// Determine if the client is part of a Client Group.
        /// </summary>
        /// <returns>True if the client is part of a Client Group. False otherwise</returns>
        //-----------------------------------------------------------------------------------------------------------------------------
        public bool isGroupMember()
        {
            SQL mySql = new SQL();
            mySql.addParameter("clientId", id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM clientGroupMember WHERE clientId = @clientId");
            if (records.Rows.Count >= 1) return true;
            return false;
        }


        /// <summary>
        /// Get the date the last annual or Initial audit was due.
        /// </summary>
        /// <returns></returns>
        //-----------------------------------------------------------------------------------------------------------------------------
        public DateTime getLastAnnualAuditDueDate()
        {
            SQL mySql = new SQL();
            mySql.addParameter("clientId", id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM vw_clientLatestAnnualAudit WHERE clientId = @clientId");
            if(records.Rows.Count == 1) return Utils.getDateTime(records.Rows[0]["lastDueDate"].ToString());

            return new DateTime(1970, 1, 1);
        }


        /// <summary>
        /// Get the date of the next scheduled audit - that has not yet been inspected.
        /// </summary>
        /// <returns>The date of the next scheduled audit - that has not yet been inspected.</returns>
        //-----------------------------------------------------------------------------------------------------------------------------
        public DateTime getNextScheduledAnnualAuditDueDate()
        {
            SQL mySql = new SQL();
            mySql.addParameter("clientId", id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM vw_audits WHERE clientId = @clientId AND dateInspected is null");
            if (records.Rows.Count >= 1) return Utils.getDateTime(records.Rows[0]["dateDue"].ToString());

            return new DateTime(1970, 1, 1);
        }



        /// <summary>
        /// Determine if all the clients addresses appear in a list.
        /// </summary>
        /// <param name="list">A list of clients addresses</param>
        /// <param name="requiresAudit">A flag to indicate if we should only focus on those addresses that require an audit.</param>
        /// <returns>True if all the clients addresses appear in a list.</returns>
        //-----------------------------------------------------------------------------------------------------------------------------
        private bool compareClientAddresses(List<ClientAddress> list, bool requiresAudit = true)
        {
            // Get a list of all of our clients addresses
            List<ClientAddress> clientAddresses = ClientAddress.getClientAddresses(this);   

            // Go through this list, and see if it exists in the paramater list
            foreach (ClientAddress clientAddress in clientAddresses)
            {
                if(!requiresAudit || clientAddress.requiresAudit)
                {
                    bool found = false;
                    foreach (ClientAddress listAddress in list)
                    {
                        if (clientAddress.id == listAddress.id) found = true;
                    }

                    // If we did not find a match, it means this clients address has not been audited
                    if (!found) return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Get the state primary key id for the addresses of this client. 
        /// </summary>
        /// <returns>The state primary key id for the addresses of this client. If there is more than 1 stateId, return -1</returns>
        //-------------------------------------------------------------------------------------------------------------
        public long getStateId()
        {
            long stateId = -1;
            List<ClientAddress> addresses = ClientAddress.getClientAddresses(this);
            foreach (ClientAddress address in addresses)
            {
                if (address.list_stateId != -1)
                {
                    if (stateId == -1) stateId = address.list_stateId;
                }
                else if (address.list_stateId != stateId) return -1;
            }
            return stateId;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public List<string> getNationalStandardProduceCategories()
        {
            List<string> cats = new List<string>();

            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords(@"SELECT        
                                                   client.id, list_clientCategory.NationalStandardCategory
                                                   FROM 
                                                   client INNER JOIN 
                                                   clientCategory ON client.id = clientCategory.clientId INNER JOIN
                                                   list_clientCategory ON clientCategory.list_clientCategoryId = list_clientCategory.id
                                                   WHERE
                                                   (clientCategory.isDeleted = 0) AND(clientCategory.clientId = @id)
                                                   GROUP BY 
                                                   client.id, list_clientCategory.NationalStandardCategory");
            foreach(DataRow row in records.Rows)
            {
                cats.Add(row["NationalStandardCategory"].ToString());
            }

            return cats;
        }


        /// <summary>
        /// Determine if this client has addresses in multiple states.
        /// </summary>
        /// <returns>True if this client has addresses in multiple states. False otherwise.</returns>
        //-------------------------------------------------------------------------------------------------------------
        public bool isMultiState()
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            string query = @"SELECT        
                             clientAddress.clientId, 
                             list_state.name AS state
                             FROM            
                             clientAddress INNER JOIN list_state ON clientAddress.list_stateId = list_state.id
                             WHERE
                             clientId = @id";

            DataTable records = mySql.getRecords(query);
            if (records.Rows.Count > 1)
            {
                string initialState = records.Rows[0]["state"].ToString();
                foreach (DataRow row in records.Rows)
                {
                    if (row["state"].ToString() != initialState) return true;
                }

            }
            return false;
        }


        /// <summary>
        /// Get a list of clients.
        /// </summary>
        /// <returns>A list of clients.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static List<string[]> getClientList()
        {
            List<string[]> list = new List<string[]>();
            SQL mySql = new SQL();
            DataTable records = mySql.getRecords("SELECT * FROM client WHERE isDeleted = 0 AND isTest = 0 ORDER BY company");
            foreach (DataRow row in records.Rows)
            {
                string id = row["id"].ToString();
                string name = row["company"].ToString();                
                   list.Add(new string[] { id, name });              
            }
            return list;
        }


        /// <summary>
        /// Get the Clients company name without punctuation.
        /// </summary>
        /// <param name="inCompanyName">The fully punctuated Client's name.</param>
        /// <returns>The Clients company name without punctuation.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static string cleanClientName(string inCompanyName)
        {
            string name = inCompanyName;
            name = name.Replace(".", " ");
            name = name.Replace(",", " ");
            name = name.Replace("'", " ");
            name = name.Replace("/", " ");

            // Remove trailing spaces by splitting the string using spaces, and rebuilding
            string[] nameParts = name.Split(' ');

            name = "";
            int count = 0;
            for (int i = 0; i < nameParts.Length; i++)
            {
                if (!String.IsNullOrEmpty(nameParts[i]))
                {
                    if (count == 0)
                    {
                        name += nameParts[i];
                        count++;
                    }
                    else name += " " + nameParts[i];
                }
            }
            return name;
        }


        /// <summary>
        /// Get the primary key Id of a Client.
        /// </summary>
        /// <param name="name">The Client's company name.</param>
        /// <returns>The primary key Id of a client if it is found. -1 otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static long getId(string name)
        {
            SQL mySql = new SQL();
            mySql.addParameter("name", name);
            DataTable records = mySql.getRecords("SELECT id FROM client WHERE (company = @name OR tradingAs = @name)");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }

    }
}
