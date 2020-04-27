using System;
using System.Collections.Generic;
using System.Data;

using CertifyWPF.WPF_Library;
using CertifyWPF.WPF_Service;
using CertifyWPF.WPF_Utils;
using CertifyWPF.WPF_User;
using CertifyWPF.WPF_Client;

namespace CertifyWPF.WPF_Application
{
    /// <summary>
    /// The key Web Application interface.  Web Applications are held in the <strong>web_application</strong> table.
    /// </summary>
    /// <remarks>
    /// An application is designed in the Certify Desktop application using the Applications Designer. During this process, the designer will 
    /// create sections (logical groupings of questions), and then questions that belong to those sections. The sections have filters and 
    /// services which allow us to tailor an application based on the users details.
    ///
    /// <para/><para/>Users login to their online portal in the usual way and create a new application.
    /// 
    /// <para/><para/>An application is started by the user selecting the services they wish to be certified for, and any categories or other filters.  
    /// Based on this information, a series of sections (and corresponding questions) is built, and presented to the applicant for completing.  
    /// Once the applicant has finished entering all required responses, a declaration must be acknowledged by the applicant.
    /// 
    /// <para/><para/>Once a user has submitted an application (which may involve making a declaration and paying a fee), the 
    /// application will be available to SXC staff using the <strong>searchApplicationsForm</strong>.  An application form is then 
    /// processed according the various workflows assigned to the application form.
    /// </remarks>

    public class WebApplication
    {
        /// <summary>
        /// The primary key
        /// </summary>
        public long id { get; set; }

        /// <summary>The client for this application id one has been set</summary>
        public string company { get; set; }

        /// <summary>The foreign key to the user table.  This is the user who created the 
        /// application in using Certify Web.</summary>
        public string userName { get; set; }

        /// <summary>The foreign key to the workflow table.  This list is specified in the 
        /// list_appWorkFlow table.</summary>
        public string workFlow { get; set; }

        /// <summary>
        /// The applicant's name - Obtained via the applicants responses to questions.
        /// </summary>
        public string applicantName { get; set; }

        /// <summary>
        /// The applicant's email - Obtained via the applicants responses to questions.
        /// </summary>
        public string applicantEmail{ get; set; }

        /// <summary>
        /// The applicant's phone number - Obtained via the applicants responses to questions.
        /// </summary>
        public string applicantPhone { get; set; }

        /// <summary>
        /// The dat eand time the applicant declared that all responses are accurate.
        /// </summary>
        public string declarationDateTime { get; set; }

        /// <summary>
        /// The options of the application (standards, client categories, markets etc.)
        /// </summary>
        public string optionString { get; set; }

        /// <summary>The application notes (used by the reviewer to record questions / concerns 
        /// regarding the application.</summary>
        public string notes { get; set; }

        /// <summary>
        /// The application result (granted, denied etc.).
        /// </summary>
        public string result { get; set; }

        /// <summary>The type of application.  The application types are specified in the 
        /// list_appFormType table.</summary>
        public string type { get; set; }

        /// <summary>A flag to indicate if testing is required in order to complete the application.  
        /// This will be transferred to the linked audit.</summary>
        public bool requiresTests { get; set; }

        /// <summary>A flag to indicate if the processing of this application should be expedited.  
        /// This is only appropriate for Export applications.</summary>
        public bool expedite { get; set; }

        /// <summary>
        /// A flag to tell us if we should view this application as a system plan.
        /// </summary>
        public bool isPlan { get; set; }

        /// <summary>
        /// A list of Id's for the application's services.
        /// </summary>
        public List<long> serviceIds { get; set; }

        /// <summary>A list of Id's for the client categories that were chosen for the application.  
        /// Client Categories are specified in the list_clientCategory table.</summary>
        public List<long> clientCategoryIds { get; set; }

        /// <summary>
        /// A flag to indicate if this application gets charged an application Fee.  This 
        /// can be set by certification staff at any time before the application is submitted.
        /// </summary> 
        public bool applicationFeeExempt { get; set; }

        /// <summary>
        /// The last date this application was checked for staleness.  
        /// </summary>
        public DateTime checkDate { get; set; }


        /// <summary>
        /// Constructor. Use this to create anew web application
        /// </summary>
        //--------------------------------------------------------------------------------------------------------------------------
        public WebApplication()
        {
            setDefaults();     
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_id">The Id of the web application.</param>
        //--------------------------------------------------------------------------------------------------------------------------
        public WebApplication(long _id)
        {
            setDefaults();
            id = _id;

            // Get the application response information
            fetch();
            getServices();
            getCategories();
        }


        /// <summary>
        /// Set the defaults fro all properties.
        /// </summary>
        //--------------------------------------------------------------------------------------------------------------------------
        public void setDefaults()
        {
            id = -1;
            userName = null;
            company = null;
            workFlow = null;

            applicantName = null;
            applicantEmail = null;
            applicantPhone = null;

            declarationDateTime = null;
            optionString = null;
            notes = null;
            result = null;
            type = null;

            requiresTests = false;
            expedite = false;

            applicationFeeExempt = false;

            checkDate = new DateTime(1970, 1, 1);
            serviceIds = new List<long>();
            clientCategoryIds = new List<long>();

            isPlan = false;
        }


        /// <summary>
        /// Retrieves the data from the database.  The data is retrieved using the vw_applications view.
        /// </summary>
        /// <returns>True if the fetch was successful. False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool fetch()
        {
            // Get the application information
            SQL mySql = new SQL();
            DataTable records = mySql.getRecords("SELECT * FROM vw_applications WHERE id = " + id.ToString());

            if (records.Rows.Count == 1)
            {
                DataRow row = records.Rows[0];
                if (!String.IsNullOrEmpty(row["clientId"].ToString()))
                {
                    long clientId = Utils.getLongFromString(row["clientId"].ToString());
                    Client client = new Client(clientId);
                    company = client.company;
                }
                
                long userId = Utils.getLongFromString(row["userId"].ToString());
                User user = new User(userId);
                userName = user.fullName;
                
                long list_appWorkFlowId = Utils.getLongFromString(row["list_appWorkFlowId"].ToString());
                workFlow = UtilsList.getWorkFlowName(list_appWorkFlowId);

                declarationDateTime = row["declarationDate"].ToString();
                applicantName = row["fullName"].ToString();
                applicantEmail = row["email"].ToString();
                applicantPhone = row["phone"].ToString();
                optionString = row["optionString"].ToString();
                notes = row["notes"].ToString();
                result = row["result"].ToString();
                type = row["type"].ToString();
                requiresTests = Convert.ToBoolean(row["requiresTests"].ToString());
                expedite = Convert.ToBoolean(row["expedite"].ToString());
                applicationFeeExempt = Convert.ToBoolean(row["applicationFeeExempt"].ToString());
                checkDate = Utils.getDateTime(row["checkDate"].ToString());
                isPlan = Convert.ToBoolean(row["isPlan"].ToString());
                return true;
            }
            else
            {
                Log.write("Error retrieving Application from database");
            }

            return false;
        }


        /// <summary>
        /// Gets the services that the applicant applied for.  This information is sourced from the applications Options string.
        /// </summary>
        /// <returns>True if the fetch was successful. False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        private bool getServices()
        {
            if (String.IsNullOrEmpty(optionString)) return false;
            try
            {
                int startPos = optionString.IndexOf("*Standards");
                if(startPos != -1)
                {
                    string standardsString = optionString.Substring(startPos + 11);
                    string[] values = standardsString.Split(',');
                    foreach (string value in values)
                    {
                        if (!String.IsNullOrEmpty(value))
                        {
                            long serviceId = UtilsList.getServiceId(value);
                            if (serviceId != -1) serviceIds.Add(serviceId);
                        }
                    }
                    return true;
                }
            }
            catch(Exception err)
            {
                Log.write("Error getting services from the option string: " + err.Message);
            }
            return false;
        }


        /// <summary>
        /// Gets the client categories that the applicant applied for.  This information is sourced from the applications Options string.
        /// </summary>
        /// <returns>True if the fetch was successful. False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        private bool getCategories()
        {
            if (String.IsNullOrEmpty(optionString)) return false;
            try
            {
                int startPos = optionString.IndexOf("*Categories");

                // Make sure we found our categories section
                if (startPos == -1) return false;

                string categoriesString = optionString.Substring(startPos + 12);
                string[] values = categoriesString.Split(',');
                foreach (string value in values)
                {
                    if (!String.IsNullOrEmpty(value))
                    {
                        long catId = UtilsList.getClientCategoryId(value);
                        if (catId != -1) clientCategoryIds.Add(catId);
                    }
                }
                return true;
            }
            catch (Exception err)
            {
                Log.write("Error getting categories from the option string: " + err.Message);
            }
            return false;
        }


        /// <summary>
        /// Gets the Id of the audit linked to this application.  Ausdits contain an "applicationId" field that when set, 
        /// indicate that the audit is linked to an application.
        /// </summary>
        /// <returns>The Id of the linked audit.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public long getLinkedAuditId()
        {
            SQL mySql = new SQL();
            mySql.addParameter("applicationId", id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM audit WHERE applicationId = @applicationId");
            if (records.Rows.Count > 0) return Convert.ToInt64(records.Rows[records.Rows.Count-1]["id"].ToString());
            return -1;
        }


        /// <summary>
        /// Counts the number of open CAR's that are associated with this application.  CAR's have a list_CARSourceId and an itemId field 
        /// that are used to refer the CAR to various other types such as applications and audits.
        /// </summary>
        /// <returns>The number of open CAR's that are associated with this application.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public int countOpenCARs()
        {
            SQL mySql = new SQL();
            mySql.addParameter("itemId", id.ToString());
            mySql.addParameter("source", "Document Review");
            DataTable records = mySql.getRecords("SELECT * FROM vw_CARs WHERE " +
                                                 "currentStatus <> 'Closed' AND " +
                                                 "currentStatus <> 'Cancelled' AND " +
                                                 "severity = 'Major (30 Day)' AND " +
                                                 "source = @source AND " +
                                                 "itemId = @itemId");
            return records.Rows.Count;
        }



        /// <summary>
        /// Mark an application as deleted.
        /// </summary>
        /// <returns>True if the application was updated as deleted. False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool delete()
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            mySql.setQuery("UPDATE web_application SET isDeleted = 1 WHERE id = @id");
            if (mySql.executeSQL() == 1) return true;
            return false;
        }


        /// <summary>
        /// Reinstate a previously deleted application.
        /// </summary>
        /// <returns>True if the application was updated as deleted. False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool unDelete()
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            mySql.setQuery("UPDATE web_application SET isDeleted = 0 WHERE id = @id");
            if (mySql.executeSQL() == 1) return true;
            return false;
        }


        /// <summary>
        /// Determine if the application has been marked as deleted.
        /// </summary>
        /// <returns>True if the application has been marked as deleted.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool isDeleted()
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM web_application WHERE id = @id AND isDeleted = 1");
            if (records.Rows.Count > 0) return true;
            return false;
        }


        /// <summary>
        /// Permanently deleted an application.  This will delete the record form the datatabse.
        /// </summary>
        /// <returns>True if the application was deleted. False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool deletePermanently()
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            mySql.setQuery("DELETE FROM web_application WHERE id = @id");
            if (mySql.executeSQL() == 1) return true;
            return false;
        }


        /// <summary>
        /// Get the date the application was first created.
        /// </summary>
        /// <returns>The date the application was first created.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public DateTime getWorkflowDate(string workflowName)
        {
            long newAppWorkflowId = UtilsList.getWorkFlowId(workflowName);

            SQL mySql = new SQL();
            mySql.addParameter("web_applicationId", id.ToString());
            mySql.addParameter("list_appWorkflowId", newAppWorkflowId.ToString());
            DataTable records = mySql.getRecords(@"SELECT * FROM 
                                                   web_applicationHistory 
                                                   WHERE 
                                                   web_applicationId = @web_applicationId AND 
                                                   list_appWorkflowId = @list_appWorkflowId AND 
                                                   isDeleted = 0");
            if (records.Rows.Count > 0)
            {
                return Utils.getDateTime(records.Rows[0]["historyDateTime"].ToString());
            }

            return new DateTime(1970,1,1);
        }

      
        /// <summary>
        /// Determine if a user can make a decision - only users that have not been involved with the application process 
        /// can make a decision
        /// </summary>
        /// <param name="user">The user to check.</param>
        /// <returns>True if the user can make a decison.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool canMakeDecision(User user)
        {
            SQL mySql = new SQL();
            mySql.addParameter("web_applicationId", id.ToString());
            DataTable records = mySql.getRecords(@"SELECT * FROM web_applicationHistory WHERE web_applicationId = @web_applicationId");
            foreach(DataRow row in records.Rows)
            {
                long userId = Convert.ToInt64(row["userId"].ToString());
                if (userId == user.userId) return false;
            }

            return true;
        }


        /// <summary>
        /// Get the user who started the document review for this web application.
        /// </summary>
        /// <returns>The user who started the document review for this web application.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public User getDocumentReviewer()
        {
            long docReviewId = UtilsList.getWorkFlowId("Doc Review Underway");

            SQL mySql = new SQL();
            mySql.addParameter("web_applicationId", id.ToString());
            mySql.addParameter("list_appWorkflowId", docReviewId.ToString());

            DataTable records = mySql.getRecords(@"SELECT * FROM web_applicationHistory 
                                                   WHERE
                                                   web_applicationId = @web_applicationId AND 
                                                   list_appWorkflowId = @list_appWorkflowId");
            if(records.Rows.Count == 1)
            {
                long userId = Convert.ToInt64(records.Rows[0]["userId"].ToString());
                return new User(userId);
            }
            return null;
        }


        /// <summary>
        /// Determine if it is OK for a user to process an Audit.
        /// </summary>
        /// <param name="user">The user to check.</param>
        /// <returns>True if it is OK for a user to process an Audit.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool canProcessAudit(User user)
        {
            User docReviewer = getDocumentReviewer();
            if (docReviewer != null)
            {
                if (user.userId != docReviewer.userId) return false;
            }
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria">Can be : open, new Application, application finished, awaiting audit, application underway, 
        ///                                 documents sent, ready for decision
        /// </param>
        /// <returns></returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static List<WebApplication> getApplications(string criteria)
        {
            List<WebApplication> list = new List<WebApplication>();
            SQL mySql = new SQL();

            // Get the query
            string query;
            if (criteria == "open") query = @"SELECT * FROM vw_applications 
                                              WHERE 
                                              (isTestCompany = 0 OR isTestCompany IS NULL) AND 
                                              isDeleted = 0 AND 
                                              workflow <> 'Application Finished' AND 
                                              workflow <> 'Application Cancelled'";
            else
            {
                mySql.addParameter("workflow", criteria);
                query = "SELECT * FROM vw_applications WHERE (isTestCompany = 0 OR isTestCompany IS NULL) AND isDeleted = 0 AND workflow = @workflow";
            }

            DataTable records = mySql.getRecords(query);
            foreach (DataRow row in records.Rows)
            {
                long id = Utils.getLongFromString(row["id"].ToString());
                if (id != -1) list.Add(new WebApplication(id));
            }

            return list;
        }
    }
}
