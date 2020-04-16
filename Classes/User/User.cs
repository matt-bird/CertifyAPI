using System;
using System.Data;
using System.IO;

using CertifyWPF.WPF_Library;
using CertifyWPF.WPF_Admin;
using CertifyWPF.WPF_Utils;

namespace CertifyWPF.WPF_User
{
    /// <summary>
    /// Certify Users.  Users can be general public (registered users from Certify Web), staff, auditors and laboratory staff.
    /// Users may also be deemed as "Admin" with very high levels of access.  All Users have an entry in the <strong>user</strong> 
    /// table.  Users may also have User Restricions - which are defined in the <strong>userRestrictions</strong> table.  These are 
    /// limitations placed on certain types of users due to pecuniary interests. 
    /// <para/> Web Users must be authorised for a Client in order for the Web User to view the Clients information.  Authorised user
    /// information is stored in the <strong>userWebClient</strong> table.
    /// via Certify Web.
    /// </summary>


    public class User
    {
        /// <summary>
        /// The primary key Id of this User.
        /// </summary>
        public long userId { get; set; }

        /// <summary>
        /// The primary key Id of this Users entry in the <strong>userWeb</strong> table.
        /// </summary>
        public long webId { get; set; }

        /// <summary>
        /// The primary key Id of this Users entry in the <strong>userAuditor</strong> table.
        /// </summary>
        public long auditorId { get; set; }

        /// <summary>
        /// The primary key Id of this Users entry in the <strong>userStaff</strong> table.
        /// </summary>
        public long staffId { get; set; }

        /// <summary>
        /// The full name of the User.
        /// </summary>
        public string fullName { get; set; }

        /// <summary>
        /// The position this user has within our certification body.  Null if there is none.
        /// </summary>
        public string userPosition { get; set; }

        /// <summary>
        /// The email address of the User.
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// The salted and hashed version of this users password.
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// The salt used for hashing this users password.
        /// </summary>
        public string passwordSalt { get; set; }

        /// <summary>
        /// The phone number of the User.
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// The secondary phone number of this user if one exists. Empty otherwise.
        /// </summary>
        public string phoneSecondary { get; set; }

        /// <summary>The domain user name of the user if they have one.  Usually, this will be for staff members 
        /// only.  A user must have a domainUserName if they are tu use the Certify Desktop application.</summary>
        public string domainUserName { get; set; }

        /// <summary>
        /// A brief description of the training for the auditor / staff member
        /// </summary>
        public string trainingPlan { get; set; }

        /// <summary>
        /// A flag to indicate if the user should have administrator privelages - use wisely !
        /// </summary>
        public bool isAdmin { get; set; }

        /// <summary>
        /// A flag to indicate if the user is active
        /// </summary>
        public bool active { get; set; }

        /// <summary>
        /// A flag to indicate if we should email the user a Newsletter
        /// </summary>
        public bool sendNewsletter { get; set; }

        /// <summary>
        /// A flag to indicate if we should email the user promotions
        /// </summary>
        public bool sendPromotions { get; set; }



        /// <summary>
        /// Constructor.  Use for a new User.
        /// </summary>
        //----------------------------------------------------------------------------------------------------------------------------
        public User()
        {
            userId = -1;
            webId = -1;
            auditorId = -1;
            staffId = -1;
            email = null;
            password = "6732F21FF7E8A7A76CE0426C76BE0B0155352D67C588608C8BF8D7FF4AB6B0A6"; // Default Password = h0neyB33
            passwordSalt = "cRdWiWpJEjPZ8gjq6P1SQLkHTuM=";
            fullName = null;
            userPosition = null;
            phone = null;
            phoneSecondary = null;
            domainUserName = null;
            trainingPlan = null;

            isAdmin = false;
            active = false;
            sendNewsletter = false;
            sendPromotions = false;
        }


        /// <summary>
        /// Constructor.  Use to open an existing User.
        /// </summary>
        /// <param name="_userId">The primary key Id of this User.</param>
        //----------------------------------------------------------------------------------------------------------------------------
        public User(long _userId)
        {
            userId = _userId;
            webId = -1;
            auditorId = -1;
            staffId = -1;
            email = null;
            password = null;
            passwordSalt = null;
            fullName = null;
            userPosition = null;
            phone = null;
            phoneSecondary = null;
            domainUserName = null;
            trainingPlan = null;

            isAdmin = false;
            active = false;
            sendNewsletter = false;
            sendPromotions = false;

            fetch();
        }


        /// <summary>
        /// Save the users details in the database.  This is an upsert
        /// </summary>
        /// <returns>True if the User was saved in the Database successfully.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool save()
        {
            if (String.IsNullOrEmpty(fullName) || String.IsNullOrEmpty(email))
            {
                Log.write("Please ensure the Users full name and email address have been entered.");
                return false;
            }

            SQL mySql = new SQL();

            mySql.addParameter("fullName", fullName);
            mySql.addParameter("userPosition", userPosition);
            mySql.addParameter("email", email);
            mySql.addParameter("phone", phone);
            mySql.addParameter("phoneSecondary", phoneSecondary);
            mySql.addParameter("password", password);
            mySql.addParameter("passwordSalt", passwordSalt);

            mySql.addParameter("domainUserName", domainUserName);
            mySql.addParameter("trainingPlan", trainingPlan);

            if (isAdmin) mySql.addParameter("isAdmin", "1");
            else mySql.addParameter("isAdmin", "0");

            if (active) mySql.addParameter("active", "1");
            else mySql.addParameter("active", "0");

            if (sendNewsletter) mySql.addParameter("sendNewsletter", "1");
            else mySql.addParameter("sendNewsletter", "0");

            if (sendPromotions) mySql.addParameter("sendPromotions", "1");
            else mySql.addParameter("sendPromotions", "0");

            if (userId == -1)
            {
                mySql.setQuery(@"INSERT INTO 
                             [user] 
                             (fullName, userPosition, email, phone, phoneSecondary, password, passwordSalt, 
                              domainUserName, trainingPlan, isAdmin, active, sendNewsletter, sendPromotions) 
                             VALUES 
                             (@fullName, @userPosition, @email, @phone, @phoneSecondary, @password, @passwordSalt, 
                              @domainUserName, @trainingPlan, @isAdmin, @active, @sendNewsletter, @sendPromotions)");

                if (mySql.executeSQL() == 1)
                {
                    // Get the latest User ID, then add a record in the userWeb table
                    userId = mySql.getMaxId("[user]");
                    mySql.clearParameters();
                    mySql.addParameter("userId", userId.ToString());
                    mySql.setQuery("INSERT INTO userWeb (userId, isConfirmed) VALUES (@userId, 1)");
                    if (mySql.executeSQL() == 1) return true;
                }
            }
            else
            {
                mySql.addParameter("id", userId.ToString());
                mySql.setQuery(@"UPDATE [user] SET
                                fullName = @fullName,
                                userPosition = @userPosition,
                                email = @email, 
                                phone = @phone,
                                phoneSecondary = @phoneSecondary, 
                                password = @password,
                                passwordSalt = @passwordSalt,
                                domainUserName = @domainUserName, 
                                trainingPlan = @trainingPlan, 
                                isAdmin = @isAdmin,
                                active = @active, 
                                sendNewsletter = @sendNewsletter, 
                                sendPromotions = @sendPromotions
                                WHERE 
                                id = @id");
                if (mySql.executeSQL() == 1) return true;
            }

            return false;
        }


        /// <summary>
        /// Fetch the User from the Database.
        /// </summary>
        //----------------------------------------------------------------------------------------------------------------------------
        private void fetch()
        {
            if (userId == -1) return;

            DataTable records;

            // Get base details
            SQL mySql = new SQL();
            mySql.addParameter("userId", userId.ToString());
            records = mySql.getRecords("SELECT * FROM [user] WHERE id = @userId");
            if(records.Rows.Count == 1)
            {
                DataRow row = records.Rows[0];

                email = row["email"].ToString();
                fullName = row["fullName"].ToString();
                userPosition = row["userPosition"].ToString();
                phone = row["phone"].ToString();
                phoneSecondary = row["phoneSecondary"].ToString();
                domainUserName = row["domainUserName"].ToString();
                trainingPlan = row["trainingPlan"].ToString();
                password = row["password"].ToString();
                passwordSalt = row["passwordSalt"].ToString();
                active = Convert.ToBoolean(row["active"].ToString());
                isAdmin = Convert.ToBoolean(row["isAdmin"].ToString());
                sendNewsletter = Convert.ToBoolean(row["sendNewsletter"].ToString());
                sendPromotions = Convert.ToBoolean(row["sendPromotions"].ToString());
            }
        }


        /// <summary>
        /// Determine if a Web User has been assigned to a Client (that is, the Web User is an authorised User for a Client).
        /// </summary>
        /// <param name="clientId">The primary key Id of the Client.</param>
        /// <returns>True if the Web User is an authroised user for the Client.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool doesUserBelongToClient(long clientId)
        {
            if (webId == -1 || clientId == -1) return false;

            SQL mySql = new SQL();
            mySql.addParameter("userWebId", webId.ToString());
            mySql.addParameter("clientId", clientId.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM userWebClient WHERE (userWebId = @userWebId AND clientId = @clientId)");
            if (records.Rows.Count == 1) return true;
            return false;
        }


        /// <summary>
        /// Get the users first name.
        /// </summary>
        /// <returns>The users first name.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public string getFirstName()
        {
            string[] names = fullName.Split(' ');
            return names[0];
        }


        /// <summary>
        /// Get the "UID" string used in Email Subject's.  The UID allows the Certify Monitor robot to properly file an 
        /// incoming email into the correct Users email folder.
        /// </summary>
        /// <returns>The "UID" string used in Email Subject's.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public string getUserIDString()
        {
            return "UID" + userId.ToString().PadLeft(5, '0');
        }


        /// <summary>
        /// Gets the users network folder.
        /// </summary>
        /// <returns>The users network folder</returns>
        //-----------------------------------------------------------------------------------------------------------------------------------------
        public string getFolder(bool autoCreate = false)
        {
            string userFolder = Admin.getWebUsersFolder() + fullName.ToUpper()[0] + "\\" + fullName + "_" + userId.ToString();

            if(autoCreate && !Directory.Exists(userFolder)) Directory.CreateDirectory(userFolder);

            return userFolder;
        }


        /// <summary>
        /// Gets the users email network folder.
        /// </summary>
        /// <returns>The users email network folder</returns>
        //-----------------------------------------------------------------------------------------------------------------------------------------
        public string getEmailFolder(bool autoCreate = true)
        {
            string emailFolder = getFolder(true) + "\\Email";
            if (autoCreate && !Directory.Exists(emailFolder)) Directory.CreateDirectory(emailFolder);

            return emailFolder;
        }


        /// <summary>
        /// Gets the users email attachments network folder.
        /// </summary>
        /// <returns>The users email attachments network folder</returns>
        //-----------------------------------------------------------------------------------------------------------------------------------------
        public string getEmailAttachmentsFolder(bool autoCreate = true)
        {
            string attachmentsFolder = getFolder(true) + "\\Email\\Attachments";
            if (autoCreate && !Directory.Exists(attachmentsFolder)) Directory.CreateDirectory(attachmentsFolder);

            return attachmentsFolder;
        }



        //
        // Static Methods
        //


        /// <summary>
        /// Get the user based on an email address.
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <returns>The user based on an email address.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static User getFromEmailAddress(string email)
        {
            SQL mySql = new SQL();
            mySql.addParameter("email", email);
            DataTable records = mySql.getRecords("SELECT * FROM [user] WHERE email = @email");
            if (records.Rows.Count == 1)
            {
                long id = Utils.getLongFromString(records.Rows[0]["id"].ToString());
                if (id != -1) return new User(id);
            }
            return null;
        }



        /// <summary>
        /// Get the primary key Id of the User from the User's userAuditor primary key Id.
        /// </summary>
        /// <param name="auditorId">The primary key Id of the UserAuditor.</param>
        /// <returns>The primary key Id of the User if found.  -1 otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static long getUserIdFromAuditorId(long auditorId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("auditorId", auditorId.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM userAuditor WHERE id = @auditorId");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["userId"].ToString());
            return -1;
        }
    }
}
