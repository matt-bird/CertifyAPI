using System;
using System.Data;

using CertifyWPF.WPF_Library;
using CertifyWPF.WPF_Utils;

namespace CertifyWPF.WPF_Admin
{
    /// <summary>
    /// This class provides basic access to the Admin table for global system configuration settings   
    /// </summary>
    public class Admin
    {

        /// <summary>
        /// Constructor - no need to use as all methods are static
        /// </summary>
        //----------------------------------------------------------------------------------------------------------------------------
        public Admin()
        {
        }


        /// <summary>
        /// Provides quick access to a field value from within the Admin table
        /// </summary>
        /// <param name="fieldName">The field you wish to retrieve from the admin table</param>
        /// <returns>The field value from the Admin Table</returns>
        //----------------------------------------------------------------------------------------------------------------------------

        // TODO: Need to remove the S: from the folder paths in the admin table - and instead assume they all
        // hang off the masterFileRoot folder. Which they will !
        //
        // Once done, we can get rid of the hardcoded S:\ replace pattern below, and just always return masterfileRoot plus
        // desired folder. Quite a large impact in making this change with potentially many breaking issues - need to test heavily.
        public static string getAdminValue(string fieldName)
        {
            SQL mySql = new SQL();
            DataTable admin = mySql.getRecords("SELECT * FROM admin WHERE id = 1");
            if (admin.Rows.Count == 1) return admin.Rows[0][fieldName].ToString();
            return null;
        }


        /// <summary>
        /// Redirects the folder path to the development folder if we are in a Dev environment
        /// </summary>
        /// <param name="path">The original path - this will be the production path</param>
        /// <param name="forceDevMode">A flag to override and always use Dev mode paths</param>
        /// <returns>The path to the development environment folder if we are in a development environment, otherwise, the path as
        /// indicated by the Admin table value</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static string getSafePath(string path, bool forceDevMode = false)
        {
            // If we are in developmenet, make sure we add the devlopment sub folder to our path for safety.
            // This relies on a drive mapping on the development PC to the location where the client files are backed up / mirrored.
            if (!Utils.isProduction() || forceDevMode) return path.Replace("S:\\", "Z:\\");
            return path;
        }


        /// <summary>
        /// Gets the Certification Mark master folder from the Admin Table
        /// </summary>
        /// <returns>The Certification Mark master folder from the Admin Table</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static string getCertMarkFolderPath()
        {
            // This is the folder where we store all of our RDLC templates
            string path = getAdminValue("certificationMarkFolder");
            if (!String.IsNullOrEmpty(path)) return getSafePath(path);
            return null;
        }


        /// <summary>
        /// Gets the Report master folder from the Admin Table.  This folder has all RDLC report templates.
        /// </summary>
        /// <returns>The Report master folder from the Admin Table</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static string getReportFolderPath()
        {
            // This is the folder where we store all of our RDLC templates
            string path = getAdminValue("reportFolder");
            if (!String.IsNullOrEmpty(path)) return getSafePath(path);
            return null;
        }


        /// <summary>
        /// Gets the Document Template master folder from the Admin Table.  This folder has the email templates.
        /// </summary>
        /// <returns>The Document Template folder from the Admin Table</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static string getDocumentTemplateFolderPath()
        {
            // This is the folder where we keep email templates and some word templates (although that is becoming deprecated)
            string path = getAdminValue("documentTemplateFolder");
            if (!String.IsNullOrEmpty(path)) return getSafePath(path);
            return null;
        }


        /// <summary>
        /// Gets the Email master folder from the Admin Table.  This folder is the root folder for where all 
        /// emails are saved.
        /// </summary>
        /// <param name="forceDevMode">A flag to override and always use Dev mode paths</param>
        /// <returns>Email master folder from the Admin Table</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static string getEmailFolderPath(bool forceDevMode = false)
        {
            // This is the folder where we keep all emails
            string path = getAdminValue("emailFileRootFolder");
            if (!String.IsNullOrEmpty(path)) return getSafePath(path, forceDevMode);
            return null;
        }


        /// <summary>
        /// Gets the master folder from the Admin Table.  This is the base folder from which all other folders are
        /// sub folders.
        /// </summary>
        /// <returns>The master folder from the Admin Table  This is the base folder from which all other folders are
        /// sub folders.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static string getMasterFileRootFolder()
        {
            // This folder from which all others are based (except the HR folder)
            string path = getAdminValue("masterFileRootFolder");
            if (!String.IsNullOrEmpty(path)) return getSafePath(path);
            return null;
        }


        /// <summary>
        /// Gets the Human Resources master folder from the Admin Table.  This is the folder where all files 
        /// relating to sdtaff and contractors are located.
        /// </summary>
        /// <returns>The Human Resources master folder from the Admin Table</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static string getHumanResourcesRootFolder()
        {
            // This is the folder where all data about staff and contractors is kept
            SQL mySql = new SQL();
            DataTable admin = mySql.getRecords("SELECT * FROM admin WHERE id = 1");
            return admin.Rows[0]["humanResourcesRootFolder"].ToString();
        }


        /// <summary>
        /// Gets the Web Upload master folder from the Admin Table.  This is the folder where all files uploaded from 
        /// CertifyWeb are stored.  This is a staging folder.
        /// </summary>
        /// <returns>The Web Upload master folder from the Admin Table</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static string getWebUploadFolder()
        {
            // This is the folder where all uploads associated with web applications are sent
            string path = getAdminValue("webUploadFolder");
            if (!String.IsNullOrEmpty(path)) return getSafePath(path);
            return null;
        }


        /// <summary>
        /// Gets the Ingredients master folder from the Admin Table.  This is the folder where all supporting evidence 
        /// for Ingredients are located.
        /// </summary>
        /// <returns>The Ingredients master folder from the Admin Table.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static string getIngredientsFolder()
        {
            string path = getAdminValue("ingredientsFolder");
            if (!String.IsNullOrEmpty(path)) return getSafePath(path);
            return null;
        }


        /// <summary>
        /// Gets the Organic Inputs master folder from the Admin Table.  This is the folder where all supporting evidence 
        /// for organic inputs and assessments are located.
        /// </summary>
        /// <returns>The Organic Inputs master folder from the Admin Table.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static string getOrganicInputsFolder()
        {
            string path = getAdminValue("organicInputsFolder");
            if (!String.IsNullOrEmpty(path)) return getSafePath(path);
            return null;
        }


        /// <summary>
        /// Gets the Web Users folder from the Admin Table.  This is the folder where all invoices and supporting
        /// evidence (such as export applications) is saved for users without clients.
        /// </summary>
        /// <returns>The Web Users master folder from the Admin Table.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static string getWebUsersFolder()
        {
            string path = getAdminValue("webUsersFolder");
            if (!String.IsNullOrEmpty(path)) return getSafePath(path);
            return null;
        }


        /// <summary>
        /// Returns a string indicating which Bank Account Xero should use when we send Xero notification of a payment.
        /// </summary>
        /// <returns>The Bank Account Xero should use when we send Xero notification of a payment.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static string getXeroBankAccount()
        {
            if(Utils.isProduction()) return getAdminValue("xeroBankAccount");

            // Xero Demo Company bank account used for development only
            return "Business Bank Account";
        }


        /// <summary>
        /// Returns the date and time of the last run OPC report.
        /// </summary>
        /// <returns>The date and time of the last run OPC report.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static DateTime getMonthlyReportsLastRunDateTime()
        {
            SQL mySql = new SQL();
            DataTable admin = mySql.getRecords("SELECT * FROM admin WHERE id = 1");
            return Convert.ToDateTime(admin.Rows[0]["monthlyReportsLastRunDateTime"].ToString());
        }


        /// <summary>
        /// Updates the date and time of the last run OPC report in the Admin table.
        /// </summary>
        /// <returns>true if the update executed correctly, false otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static bool updateMonthlyReportsLastRunDateTime(DateTime newDateTime)
        {
            SQL mySql = new SQL();
            mySql.addParameter("monthlyReportsLastRunDateTime", newDateTime.ToString("yyyy-MM-dd"));

            string query = "UPDATE Admin SET monthlyReportsLastRunDateTime = @monthlyReportsLastRunDateTime WHERE id = 1";
            mySql.setQuery(query);

            if (mySql.executeSQL() == 1) return true;
            return false;
        }


        /// <summary>
        /// Returns the date and time of the last audit scheduling run.
        /// </summary>
        /// <returns>The date and time of the last audit scheduling run.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static DateTime getAuditSchedulingLastRunDateTime()
        {
            SQL mySql = new SQL();
            DataTable admin = mySql.getRecords("SELECT * FROM admin WHERE id = 1");
            return Utils.getDateTime(admin.Rows[0]["auditSchedulingLastRunDateTime"].ToString());
        }


        /// <summary>
        /// Returns the date and time of the last audit nags run.
        /// </summary>
        /// <returns>The date and time of the last audit nags run.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static DateTime getAuditNagsLastRunDateTime()
        {
            SQL mySql = new SQL();
            DataTable admin = mySql.getRecords("SELECT * FROM admin WHERE id = 1");
            return Utils.getDateTime(admin.Rows[0]["auditNagsLastRunDateTime"].ToString());
        }


        /// <summary>
        /// Updates the date and time of the last run of an Audit Scheduling process.
        /// </summary>
        /// <returns>true if the update executed correctly, false otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static bool updateAuditSchedulingLastRunDateTime(DateTime newDateTime)
        {
            SQL mySql = new SQL();
            mySql.addParameter("auditSchedulingLastRunDateTime", newDateTime.ToString("yyyy-MM-dd"));

            string query = "UPDATE Admin SET auditSchedulingLastRunDateTime = @auditSchedulingLastRunDateTime WHERE id = 1";
            mySql.setQuery(query);

            if (mySql.executeSQL() == 1) return true;
            return false;
        }


        /// <summary>
        /// Gets the email of address of where the OPC report should be sent.  Set to null in the Admin table if Matt should get it.
        /// </summary>
        /// <returns>The email of address of where the OPC report should be sent</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static string getOPCReportEmail()
        {
            SQL mySql = new SQL();
            DataTable admin = mySql.getRecords("SELECT * FROM admin WHERE id = 1");

            string emailAddress = admin.Rows[0]["OPCReportEmail"].ToString();

            if (String.IsNullOrEmpty(emailAddress)) return "matt@sxcertified.com.au";

            // Always copy Matt so we can see what was sent to them
            return emailAddress + ",matt@sxcertified.com.au";
        }


        /// <summary>
        /// Gets the last used OPC number which is stored in the Admin table.
        /// </summary>
        /// <returns>The last used OPC number.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static int getLatestOPCCertNumber()
        {
            SQL mySql = new SQL();
            DataTable admin = mySql.getRecords("SELECT * FROM admin WHERE id = 1");
            return Convert.ToInt32(admin.Rows[0]["OPCCertificate"].ToString());
        }


        /// <summary>
        /// Updates the last used OPC number in the Admin table.
        /// </summary>
        /// <param name="newNum">The new OPC number to be saved.</param>
        /// <returns>true if the update executed correctly, false otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static bool setOPCCertNumber(int newNum)
        {
            SQL mySql = new SQL();
            mySql.addParameter("OPCCertificate", newNum.ToString());

            string query = "UPDATE Admin SET OPCCertificate = @OPCCertificate WHERE id = 1";
            mySql.setQuery(query);

            if (mySql.executeSQL() == 1) return true;
            return false;
        }


        /// <summary>
        /// Updates the "impersonateUserId" field in the Admin table.   
        /// This will be used when identifying the logged in user, and must be used for testing / debugging only. 
        /// </summary>
        /// <param name="userId">The user ID of the person who will be doing the impersonating.</param>
        /// <returns>true if the update executed correctly, false otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static bool setImpersonateId(long userId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("impersonateUserId", userId.ToString());

            string query = "UPDATE Admin SET impersonateUserId = @impersonateUserId WHERE id = 1";
            mySql.setQuery(query);

            if (mySql.executeSQL() == 1) return true;
            return false;
        }


        /// <summary>
        /// Clears the "impersonateUserId" field in the Admin table.   
        /// </summary>
        /// <returns>true if the update executed correctly, false otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static bool clearImpersonateId()
        {
            SQL mySql = new SQL();
            mySql.addParameter("impersonateUserId", null);

            string query = "UPDATE Admin SET impersonateUserId = @impersonateUserId WHERE id = 1";
            mySql.setQuery(query);

            if (mySql.executeSQL() == 1) return true;
            return false;
        }


        /// <summary>
        /// Gets the email address(es) of where all reports generated by the Certify Monitor should be sent.  
        /// </summary>
        /// <returns>The email address(es) of where all reports generated by the Certify Monitor should be sent.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static string getMonitorReportRecipients()
        {
            SQL mySql = new SQL();
            DataTable admin = mySql.getRecords("SELECT * FROM admin WHERE id = 1");
            return admin.Rows[0]["monitorReportRecipients"].ToString();
        }


        /// <summary>
        /// Get the path to the template to be used for viewing the client maps.
        /// </summary>
        /// <returns>The path to the template to be used for viewing the client maps.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static string getClientMapTemplate()
        {
            return getSafePath(@"S:\00_admin_Templates\clientMapTemplate.html");
        }


        /// <summary>
        /// Get the path to the generated Client Map HTML.
        /// </summary>
        /// <returns>The path to the generated Client Map HTML.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static string getClientMapPath()
        {
            return getSafePath(@"S:\00_admin_Templates\clientMap.html");
        }
    }
}
