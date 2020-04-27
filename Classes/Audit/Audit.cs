using System;
using System.Windows;
using System.Data;
using System.IO;
using System.Collections.Generic;

using CertifyWPF.WPF_Library;
using CertifyWPF.WPF_Client;
using CertifyWPF.WPF_Utils;
using CertifyWPF.WPF_Service;
using CertifyWPF.WPF_User;

namespace CertifyWPF.WPF_Audit
{
    /// <summary>
    /// Audits.  Audits are held in the <strong>audit</strong> table.
    /// </summary>
    public class Audit
    {
        /// <summary>
        /// The primary key Id.
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// The primary key Id of the client.
        /// </summary>
        public long clientId { get; set; }

        /// <summary>
        /// The primary key Id of an audit that this audit is linked to.
        /// </summary>
        public long clientGroupMemberId { get; set; }

        /// <summary>
        /// The primary key Id for the audits client.
        /// </summary>
        public string company { get; set; }

        /// <summary>
        /// The primary key Id of the web application, if this audit is associated with a web application. -1 otherwise.
        /// </summary>
        public long applicationId { get; set; }

        /// <summary>
        /// The primary key Id of the Invoice associated wit hthis audit.  This will be -1 until an Invoice has been raised for this audit.
        /// </summary>
        public long invoiceId { get; set; }

        /// <summary>
        /// The lead auditor.
        /// </summary>
        public string leadAuditor { get; set; }

        /// <summary>
        /// The trainee auditor if one exists. 
        /// </summary>
        public string traineeAuditor { get; set; }

        /// <summary>
        /// The primary key Id of the audit type.  Audit types (such as Annual or Initial) are specified in the <strong>list_auditType</strong> table.
        /// </summary>
        public string auditType { get; set; }

        /// <summary>
        /// The primary key Id of the audit status.  Audit statuses (such as Planned or Inspected) are specified in the <strong>list_auditStatus</strong> table.
        /// </summary>
        public string auditStatus { get; set; }

        /// <summary>
        /// The audt notes.  These can be updated by either certification staff or auditors during an audit.
        /// </summary>
        public string notes { get; set; }

        /// <summary>
        /// A list of people who were intervied during an audit.
        /// </summary>
        public string interviewees { get; set; }

        /// <summary>
        /// A list of documents reviewed during an audit.
        /// </summary>
        public string documentsReviewed { get; set; }

        /// <summary>
        /// The date the audit is due.
        /// </summary>
        public DateTime dateDue { get; set; }

        /// <summary>The date the audit has been planned.  This will be a date that is negotiated between the auditor and client.  
        /// This will be entered by the auditor via the auditors portal in CertifyWeb.</summary>
        public DateTime datePlanned { get; set; }

        /// <summary>The time the audit will start.  This will be negotiated between the auditor and client.
        /// This will be entered by the auditor via the auditors portal in CertifyWeb.</summary>
        public string plannedStartTime { get; set; }

        /// <summary>The date the audit actually ocurred.
        /// This will be entered by the auditor via the auditors portal in CertifyWeb.</summary>
        public DateTime dateInspected { get; set; }

        /// <summary>The time the audit started.  
        /// This is recorded by the auditor in the checklist, and captured when the audit checklist is processed.</summary>
        public string auditStart { get; set; }

        /// <summary>The time the audit finished.  
        /// This is recorded by the auditor in the checklist, and captured when the audit checklist is processed.</summary>
        public string auditFinish { get; set; }

        /// <summary>The duration of the audit.  This is not simply finish - start, because this has to allow for breaks and "chats" during an audit.
        /// This is recorded by the auditor in the checklist, and captured when the audit checklist is processed.</summary>
        public float auditDuration { get; set; }

        /// <summary>A flag to indicate if the issues identified during the audit have been discussed with the client.  This is so that issues raised 
        /// in an audit report are not surprises for the client. This is recorded by the auditor in the checklist, and captured when the audit 
        /// checklist is processed.</summary>
        public bool issuesDiscussed { get; set; }

        /// <summary>
        /// A flag to indicate if test samples are required for the audit.
        /// </summary>
        public bool requiresTesting { get; set; }

        /// <summary>
        /// The total($) of all audit expenses for this audit.
        /// </summary>
        public float auditExpenses { get; set; }

        /// <summary>
        /// The total($) of all costs relating to testing for this audit.
        /// </summary>
        public float testingCosts { get; set; }

        /// <summary>
        /// A description of where the test samples were taken from during the audit.
        /// </summary>
        public string testSampleLocation { get; set; }

        /// <summary>
        /// A list of services ofr this audit
        /// </summary>
        public List<AuditService> services { get; set; }


        /// <summary>
        /// Constructor.  Use this for creating a new audit.
        /// </summary>
        //--------------------------------------------------------------------------------------------------------------------------
        public Audit()
        {
            id = -1;
            clientId = -1;
            clientGroupMemberId = -1;
            company = null;
            applicationId = -1;
            invoiceId = -1;
            leadAuditor = null;
            traineeAuditor = null;
            auditType = null;
            auditStatus = null;
            plannedStartTime = null;
            auditStart = null;
            auditFinish = null;
            auditDuration = -1;
            issuesDiscussed = false;
            requiresTesting = false;
            auditExpenses = -1;
            testingCosts = -1;
            testSampleLocation = null;
            dateDue = new DateTime(1970, 1, 1);
            datePlanned = new DateTime(1970, 1, 1);
            dateInspected = new DateTime(1970, 1, 1);

            notes = null;
            interviewees = null;
            documentsReviewed = null;
        }


        /// <summary>
        /// Constructor.  Use this for opening an excisting audit
        /// </summary>
        /// <param name="_Id">The primary key of the audit to be opened.</param>
        //--------------------------------------------------------------------------------------------------------------------------
        public Audit(long _Id)
        {
            id = _Id;
            clientId = -1;
            clientGroupMemberId = -1;
            company = null;
            applicationId = -1;
            invoiceId = -1;
            leadAuditor = null;
            traineeAuditor = null;
            auditType = null;
            auditStatus = null;
            plannedStartTime = null;
            auditStart = null;
            auditFinish = null;
            auditDuration = -1;
            issuesDiscussed = false;
            requiresTesting = false;
            auditExpenses = -1;
            testingCosts = -1;
            testSampleLocation = null;
            dateDue = new DateTime(1970, 1, 1);
            datePlanned = new DateTime(1970, 1, 1);
            dateInspected = new DateTime(1970, 1, 1);

            notes = null;
            interviewees = null;
            documentsReviewed = null;

            fetch();

            getServices();
        }


        /// <summary>
        /// Fetch the audit information from the database record.
        /// </summary>
        /// <returns>A DataTable that contains all information of the audit.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public DataTable fetch()
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM Audit WHERE id = @id");
            if(records.Rows.Count == 1)
            {
                DataRow row = records.Rows[0];

                clientId = Utils.getLongFromString(row["clientId"].ToString());
                if(clientId != -1)
                {
                    Client client = new Client(clientId);
                    company = client.company;
                }

                clientGroupMemberId = Utils.getLongFromString(row["clientGroupMemberId"].ToString());
                applicationId = Utils.getLongFromString(row["applicationId"].ToString());
                invoiceId = Utils.getLongFromString(row["invoiceId"].ToString());

                long list_auditTypeId = Utils.getLongFromString(row["list_auditTypeId"].ToString());
                auditType = UtilsList.getAuditType(list_auditTypeId);

                long list_auditStatusId = Utils.getLongFromString(row["list_auditStatusId"].ToString());
                auditStatus = UtilsList.getAuditStatus(list_auditStatusId);

                long leadUserAuditorId = Utils.getLongFromString(row["leadUserAuditorId"].ToString());
                if(leadUserAuditorId != -1)
                {
                    long userId = User.getUserIdFromAuditorId(leadUserAuditorId);
                    if(userId != -1)
                    {
                        User user = new User(userId);
                        leadAuditor = user.fullName;
                    }
                }

                long traineeUserAuditorId = Utils.getLongFromString(row["traineeUserAuditorId"].ToString());
                if (traineeUserAuditorId != -1)
                {
                    long userId = User.getUserIdFromAuditorId(traineeUserAuditorId);
                    if (userId != -1)
                    {
                        User user = new User(userId);
                        traineeAuditor = user.fullName;
                    }
                }

                auditDuration = Utils.getFloatFromString(row["auditDuration"].ToString());
                auditExpenses = Utils.getFloatFromString(row["auditExpenses"].ToString());
                testingCosts = Utils.getFloatFromString(row["testingCosts"].ToString());

                issuesDiscussed = Convert.ToBoolean(row["issuesDiscussed"].ToString());
                requiresTesting = Convert.ToBoolean(row["requiresTesting"].ToString());

                dateDue = Utils.getDateTime(row["dateDue"].ToString());
                datePlanned = Utils.getDateTime(row["datePlanned"].ToString());
                dateInspected = Utils.getDateTime(row["dateInspected"].ToString());

                auditStart = row["auditStart"].ToString();
                auditFinish = row["auditFinish"].ToString();               
                testSampleLocation = row["testSampleLocation"].ToString();
                notes = row["notes"].ToString();
                interviewees = row["interviewees"].ToString();
                documentsReviewed = row["documentsReviewed"].ToString();

                if (!String.IsNullOrEmpty(row["plannedStartTime"].ToString())) plannedStartTime = row["plannedStartTime"].ToString();
            }

            return records;
        }


        /// <summary>
        /// Get a list of all services for this Audit.
        /// </summary>
        /// <returns>A list of all services for this Audit.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public void getServices()
        {
            services = new List<AuditService>();
            services = AuditService.getServices(this);
        }


        /// <summary>
        /// Add a service to this audit's list of services
        /// </summary>
        /// <param name="service"></param>
        //--------------------------------------------------------------------------------------------------------------------------
        public void addService(Service service)
        {
            if(services == null) services = new List<AuditService>();
            AuditService serv = new AuditService()
            {
                auditId = id,
                serviceId = service.id
            };
            services.Add(serv);
        }


        /// <summary>
        /// Add the default services to this audit - these will be the same services as the clients.
        /// </summary>
        //--------------------------------------------------------------------------------------------------------------------------
        public void addDefaultServices()
        {
            services = new List<AuditService>();

            Client client = new Client(clientId);

            foreach (ClientService clientService in client.clientServices)
            {
                AuditService serv = new AuditService()
                {
                    auditId = id,
                    serviceId = clientService.serviceId
                };
                services.Add(serv);
            }
        }


        /// <summary>
        /// Refresh the audits information from the database.
        /// </summary>
        //--------------------------------------------------------------------------------------------------------------------------
        public void refresh()
        {
            fetch();
        }


        /// <summary>
        /// For a string that describes the audit.
        /// </summary>
        /// <returns>A string that describes the audit.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public string getAuditTitle()
        {
            SQL mySql = new SQL();
            string clientName = mySql.lookup("client", "company", "id = " + clientId.ToString());

            string title;
            if (id != -1) title = "Audit for " + clientName + ", Audit ID: " + id.ToString() + ", Status: " + getAuditStatus(id);
            else title = "New Audit for " + clientName;
            return title;
        }


        /// <summary>
        /// Determine if the audit has been marked as deleted.
        /// </summary>
        /// <returns>True if the audit has been marked as deleted.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool isDeleted()
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM audit WHERE id = @id AND isDeleted = 1");
            if (records.Rows.Count > 0) return true;
            return false;
        }


        /// <summary>
        /// Validate the audit information.
        /// </summary>
        /// <returns>True if the validation passes.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        private bool validate()
        {
            if (String.IsNullOrEmpty(leadAuditor))
            {
                Log.write("Cannot save audit as no lead auditor has been set");
                return false;
            }

            if (String.IsNullOrEmpty(auditStatus))
            {
                Log.write("Cannot save audit as no audit status has been set");
                return false;
            }

            if (String.IsNullOrEmpty(auditType))
            {
                Log.write("Cannot save audit as no audit type has been set");
                return false;
            }

            return true;
        }


        /// <summary>
        /// Get the planned, due or inspected date of the audit.
        /// </summary>
        /// <param name="type">"dateDue", "datePlanned" or "dateInspected"</param>
        /// <returns>The date requested.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public DateTime getAuditDate(string type)
        {
            if (id == -1) return new DateTime(1970, 1, 1);

            SQL mySql = new SQL();
            string dueDateAsString = mySql.lookup("audit", type, "id = " + id.ToString());

            // Set return value to Epoch when there is no date entered
            if (String.IsNullOrEmpty(dueDateAsString)) return new DateTime(1970, 1, 1);
            return Convert.ToDateTime(dueDateAsString);
        }


        /// <summary>
        /// Count the number of audit expenses for this audit.  This uses the <strong>vw_auditExpenses</strong> view.
        /// </summary>
        /// <returns>The number of audit expenses for this audit.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public int countAuditExpenses()
        {
            SQL mySql = new SQL();
            mySql.addParameter("auditId", id.ToString());
            DataTable expenses = mySql.getRecords("SELECT * FROM vw_auditExpenses WHERE auditId = @auditId");
            return expenses.Rows.Count;
        }


        /// <summary>
        /// Count the number of test results that have been linked to this audit.  Test results are linked to an audit by setting the testResult itemId field to the audit Id.  
        /// </summary>
        /// <returns>The number of test results that are linked to this audit.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public int countTestResults()
        {
            SQL mySql = new SQL();
            mySql.addParameter("itemId", id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM testResult WHERE itemId = @itemId");
            return records.Rows.Count;
        }


        /// <summary>
        /// Get a list of client address uses for al laddresses that this audit is for.  If the audit contains the main operation address
        /// then all client address uses are returned.
        /// </summary>
        /// <returns>A list of client address uses for al laddresses that this audit is for.  If the audit contains the main operation address
        /// then all client address uses are returned.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public List<ClientAddressUse> getUses()
        {
            List<ClientAddressUse> uses = new List<ClientAddressUse>();

            // Get address uses for all addresses in this audit
            List<ClientAddress> addresses = AuditAddress.getAddresses(this);
            foreach(ClientAddress address in addresses)
            {
                foreach(ClientAddressUse addressUse in address.uses)
                {
                    // Make sure this use does not already exist
                    bool found = false;
                    foreach(ClientAddressUse use in uses)
                    {
                        if (addressUse.addressUse == use.addressUse) found = true;
                    }

                    // If not already exists, add it.
                    if (!found) uses.Add(addressUse);
                }               
            }
            return uses;
        }

        
        /// <summary>
        /// Determine if one of the addresses being audited is the main operation address.
        /// </summary>
        /// <returns>True if one of the addresses being audited is the main operation address.  False otherwise</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool includesOperationAddress()
        {
            // Get a list of addresses
            List<ClientAddress> addresses = AuditAddress.getAddresses(this);
            foreach (ClientAddress address in addresses)
            {
                if (address.isOperationAddress) return true;
            }

            return false;
        }



        //
        // Static Methods
        //


        /// <summary>
        /// Get an audits list of service primary key Id's.
        /// </summary>
        /// <param name="auditId">The audit's primary key Id.</param>
        /// <returns>An audits list of service primary key Id's.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static DataTable getAuditServiceIds(long auditId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", auditId.ToString());
            return mySql.getRecords("SELECT serviceId FROM auditService WHERE auditId = @id");
        }


        /// <summary>
        /// Get an audits information from the <strong>vw_audits</strong> view.
        /// </summary>
        /// <param name="auditId">The audit's primary key Id.</param>
        /// <returns>An audits information from the vw_audits view.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static DataTable getAuditViewDetails(long auditId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", auditId.ToString());
            return mySql.getRecords("SELECT * FROM vw_audits WHERE id = @id");
        }


        /// <summary>
        /// Get the primary key Id of a client for which an audit has ocurred.
        /// </summary>
        /// <param name="auditId">The audit's primary key Id.</param>
        /// <returns>The primary key Id of a client for which an audit has ocurred.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static long getClientIdFromAudit(long auditId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", auditId.ToString());
            DataTable records = mySql.getRecords("SELECT clientId FROM audit WHERE id = @id");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["clientId"].ToString());
            return -1;
        }


        /// <summary>
        /// Get the planned, due or inspected date of the audit.
        /// </summary>
        /// <param name="auditId">The audit's primary key Id.</param>
        /// <param name="type">"dateDue", "datePlanned" or "dateInspected"</param>
        /// <returns>The date requested.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static DateTime getAuditDate(long auditId, string type)
        {
            if (auditId == -1) return new DateTime(1970, 1, 1);

            SQL mySql = new SQL();
            string dateAsString = mySql.lookup("audit", type, "id = " + auditId.ToString());

            // Set return value to Epoch when there is no date entered
            if (String.IsNullOrEmpty(dateAsString)) return new DateTime(1970, 1, 1);
            return Convert.ToDateTime(dateAsString);
        }


        /// <summary>
        /// Get an audits current status.  Audit statuses (such as Planned or Inspected) are specified in the <strong>list_auditStatus</strong> table.
        /// </summary>
        /// <param name="auditId">The audit's primary key Id.</param>
        /// <returns>The audit's status.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static string getAuditStatus(long auditId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", auditId.ToString());

            string query = "SELECT list_auditStatus.name FROM audit INNER JOIN " +
                           "list_auditStatus ON audit.list_auditStatusId = list_auditStatus.id WHERE audit.id = @id";
            DataTable records = mySql.getRecords(query);
            if (records.Rows.Count == 1) return records.Rows[0]["name"].ToString();
            return null;
        }

        /// <summary>
        /// Get the inspected audits for a client for a particular year.
        /// </summary>
        /// <param name="client">The client ot investigate</param>
        /// <param name="yearInspected">The year the audits should be inspected in.</param>
        /// <returns>The clients recent audits.  There will be X number of audits if available, or fewer if there were not that many</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static List<Audit> getClientInspectedAudits(Client client, int yearInspected)
        {
            List<Audit> audits = new List<Audit>();

            SQL mySql = new SQL();
            mySql.addParameter("clientId", client.id.ToString());
            mySql.addParameter("yearInspected", yearInspected.ToString());
            DataTable records = mySql.getRecords(@"SELECT id
                                                   FROM vw_audits
                                                   WHERE 
                                                   clientId = @clientId AND
                                                   isDeleted = 0 AND
                                                   YEAR(dateInspected) = @yearInspected
                                                   ORDER BY id DESC");
            foreach(DataRow row in records.Rows)
            {
                long id = Utils.getLongFromString(row["id"].ToString());
                if(id != -1) audits.Add(new Audit(id));
            }
            return audits;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria">Can be open, created, allocated, docs sent, planned, processed</param>
        /// <returns></returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static List<Audit> getAudits(string criteria)
        {
            List<Audit> list = new List<Audit>();
            SQL mySql = new SQL();

            // Get the query
            string query;
            if (criteria == "open") query = "SELECT * FROM vw_audits WHERE isTest = 0 AND isDeleted = 0 AND status <> 'Finished' AND status <> 'Cancelled' ";
            else 
            {
                mySql.addParameter("status", criteria);
                query = "SELECT * FROM vw_audits WHERE isTest = 0 AND isDeleted = 0 AND status = @status";
            }

            DataTable records = mySql.getRecords(query);
            foreach(DataRow row in records.Rows)
            {
                long id = Utils.getLongFromString(row["id"].ToString());
                if (id != -1) list.Add(new Audit(id));
            }
            
            return list;
        }
    }
}
