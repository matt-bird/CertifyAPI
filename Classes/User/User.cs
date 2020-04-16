using System;
using System.Data;
using System.IO;

using CertifyWPF.WPF_Library;
using CertifyWPF.WPF_Admin;

namespace CertifyWPF.WPF_User
{
    /// <summary>
    /// Certify Users.  Users can be general public (registered users from Certify Web), staff, auditors and laboratory staff.
    /// Users may also be deemed as "Admin" with very high levels of access.  All Users have an entry in the <strong>user</strong> 
    /// table.  Depending on the user, they may also have entries in the <strong>userAuditor, userStaff or userLab</strong> tables.
    /// Users may also have User Restricions - which are defined in the <strong>userRestrictions</strong> table.  These are 
    /// limitations place on certain types of users due to pecuniary interests.  Staff and Auditors also have entries in 
    /// the <strong>userTraining and userTrainingMatrix</strong> tables to assisst with training obligations.
    /// <para/>
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

                //Get related User details
                records = mySql.getRecords("SELECT * FROM userWeb WHERE isDeleted = 0 AND userId = @userId");
                if (records.Rows.Count == 1) webId = Convert.ToInt64(records.Rows[0]["id"].ToString());

                records = mySql.getRecords("SELECT * FROM userAuditor WHERE isDeleted = 0 AND userId = @userId");
                if (records.Rows.Count == 1) auditorId = Convert.ToInt64(records.Rows[0]["id"].ToString());

                records = mySql.getRecords("SELECT * FROM userStaff WHERE isDeleted = 0 AND userId = @userId");
                if (records.Rows.Count == 1) staffId = Convert.ToInt64(records.Rows[0]["id"].ToString());

                active = Convert.ToBoolean(row["active"].ToString());
                isAdmin = Convert.ToBoolean(row["isAdmin"].ToString());
                sendNewsletter = Convert.ToBoolean(row["sendNewsletter"].ToString());
                sendPromotions = Convert.ToBoolean(row["sendPromotions"].ToString());
            }
        }


        /// <summary>
        /// Determine if a User has a particular role such as "Auditor" or "Staff"
        /// </summary>
        /// <param name="role">"userWeb", "userAuditor", "userStaff", "userLab"</param>
        /// <returns>True if the User has the role.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool isUserRole(string role)
        {
            if (userId == -1) return false;

            SQL mySql = new SQL();
            mySql.addParameter("userId", userId.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM " + role + " WHERE isDeleted = 0 AND userId = @userId");
            if (records.Rows.Count == 1) return true;
            return false;
        }


        /// <summary>
        /// Determine if a User's role has been marked as deleted. 
        /// </summary>
        /// <param name="role">"userWeb", "userAuditor", "userStaff", "userLab"</param>
        /// <returns>True if the User's role has been deleted.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool isUserRoleDeleted(string role)
        {
            if (userId == -1) return false;

            SQL mySql = new SQL();
            mySql.addParameter("userId", userId.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM " + role + " WHERE isDeleted = 1 AND userId = @userId");
            if (records.Rows.Count == 1) return true;
            return false;
        }


        /// <summary>
        /// Create a role for a User. 
        /// </summary>
        /// <param name="role">"userWeb", "userAuditor", "userStaff", "userLab"</param>
        /// <returns>True if the role was successfully created for the User.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public long createUserRole(string role)
        {
            SQL mySql = new SQL();
            mySql.addParameter("userId", userId.ToString());

            bool userHasRole = isUserRole(role);
            bool userIsDeleted = isUserRoleDeleted(role);

            // If the user does not already have this role - create it
            if (!userHasRole)
            {
                string fields = " (userId";
                string values = "(@userId";
                if (role == "userWeb")
                {
                    fields += ", isConfirmed)";
                    values += ", 1)";
                }
                else
                {
                    fields += ")";
                    values += ")";
                }
                // Create the role
                long id = -1;
                mySql.setQuery("INSERT INTO " + role + fields + " VALUES " + values);
                if (mySql.executeSQL() == 1)
                {
                    id = mySql.getMaxId(role);
                }
                return id;
            }

            else
            {
                // User exists and has the role, but the role is marked as deleted
                if (userIsDeleted)
                {
                    mySql.setQuery("UPDATE " + role + " SET isDeleted = 0 WHERE userId = @userId");
                    if (mySql.executeSQL() != 1) return -1;
                }

                //User already has this role and is not already deleted - get that roles id
                DataTable records = mySql.getRecords("SELECT id from " + role + " WHERE userId = @userId");
                if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
                return -1;
            }
        }


        /// <summary>
        /// Mark a User's role as deleted.
        /// </summary>
        /// <param name="role">"userWeb", "userAuditor", "userStaff", "userLab"</param>
        /// <returns>True if the role was successfully marked as deleted for the User.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool deleteUserRole(string role)
        {
            // User does not have role - return true to indicate the user is deleted
            if (!isUserRole(role)) return true;

            // User has role, and is currently not deleted - so delete
            if (!isUserRoleDeleted(role))
            {
                SQL mySql = new SQL();
                mySql.addParameter("userId", userId.ToString());
                mySql.setQuery("UPDATE " + role + " SET isDeleted = 1 WHERE userId = @userId");
                if (mySql.executeSQL() == 1) return true;
                return false;
            }

            return false;
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
        /// Determine if a Web User was once assigned as an authorised user for a Client, but this has been marked 
        /// as Deleted.
        /// </summary>
        /// <param name="clientId">The primary key Id of the Client.</param>
        /// <returns>True if the Web User was once assigned as an authorised user for a Client, but this has been marked 
        /// as Deleted. False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool isUserWebClientDeleted(long clientId)
        {
            // If user or climent ID == -1 retun true, as, well, I courldnt think of anything else to do at this point
            if (webId == -1 || clientId == -1) return true;

            // If user doesn;t blong to the client, then return true - again - not sure what else to do at this point.
            if (!doesUserBelongToClient(clientId)) return true;
            
            SQL mySql = new SQL();
            mySql.addParameter("userWebId", webId.ToString());
            mySql.addParameter("clientId", clientId.ToString());

            // If the user belongs to the client and is not deleted, return false
            DataTable records = mySql.getRecords("SELECT * FROM userWebClient WHERE (userWebId = @userWebId AND clientId = @clientId AND isDeleted = 0)");
            if (records.Rows.Count == 1) return false;

            // If we get here, user belongs to client, and is deleted
            return true;
        }


        /// <summary>
        /// Add a Web User as an authorised User for a Client.
        /// </summary>
        /// <param name="clientId">The primary key Id of the Client.</param>
        /// <returns>True if the Web User was successfully authorised.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool addWebUserToClient(long clientId)
        {
            if (webId == -1 || clientId == -1) return false;

            SQL mySql = new SQL();
            mySql.addParameter("userWebId", webId.ToString());
            mySql.addParameter("clientId", clientId.ToString());

            bool userBelongs = doesUserBelongToClient(clientId);
            bool isDeleted = isUserWebClientDeleted(clientId);

            // user already belongs to cleint and is not deleted
            if (userBelongs && !isDeleted) return true;

            // User does belong, but this has been previously deleted
            if(userBelongs && isDeleted)
            {
                mySql.setQuery("UPDATE userWebClient SET isDeleted = 0 WHERE userWebId = @userWebId AND clientId = @clientId");
                if (mySql.executeSQL() == 1) return true;
            }

            // User does not belong to the client so create
            if(!userBelongs)
            {
                mySql.setQuery("INSERT INTO userWebClient (userWebId, clientId, admin) VAlUES (@userWebId, @clientId, 0)");
                if (mySql.executeSQL() == 1) return true;
            }
            return false;
        }


        /// <summary>
        /// Mark an authorised Web User for a Client as deleted.
        /// </summary>
        /// <param name="clientId">The primary key Id of the Client.</param>
        /// <returns>True if the Web User was successfully marked as deleted.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool deleteWebUserFromClient(long clientId)
        {
            if (webId == -1 || clientId == -1) return true;

            // user already belongs to Client and is not deleted
            if (doesUserBelongToClient(clientId) && !isUserWebClientDeleted(clientId))
            {
                SQL mySql = new SQL();
                mySql.addParameter("userWebId", webId.ToString());
                mySql.addParameter("clientId", clientId.ToString());
                mySql.setQuery("UPDATE userWebClient SET isDeleted = 1 WHERE userWebId = @userWebId AND clientId = @clientId");
                if (mySql.executeSQL() == 1) return true;
            }
            return true;
        }



        /// <summary>
        /// Reset a Users password to the default password and password salt.
        /// </summary>
        /// <returns>True if the password was reset.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool resetPassword()
        {
            SQL mySql = new SQL();
            mySql.addParameter("userId", userId.ToString());

            // Default Password = h0neyB33 = 6732F21FF7E8A7A76CE0426C76BE0B0155352D67C588608C8BF8D7FF4AB6B0A6
            // Default Salt = cRdWiWpJEjPZ8gjq6P1SQLkHTuM=
            password = "6732F21FF7E8A7A76CE0426C76BE0B0155352D67C588608C8BF8D7FF4AB6B0A6";
            passwordSalt = "cRdWiWpJEjPZ8gjq6P1SQLkHTuM=";

            mySql.addParameter("password", password);
            mySql.addParameter("passwordSalt", passwordSalt);

            mySql.setQuery("UPDATE [user] Set password = @password, passwordSalt = @passwordSalt WHERE id = @userId");
            if (mySql.executeSQL() == 1) return true;

            return false;
        }


        /// <summary>
        /// Unlock a Users Web Access if the Access had been previously locked due to too many login attempts.
        /// </summary>
        /// <returns>True if the access was unlocked.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool unlockWebAccess()
        {
            SQL mySql = new SQL();
            mySql.addParameter("userId", userId.ToString());
            mySql.addParameter("lockoutEnd", null);
            mySql.setQuery("UPDATE [userWeb] Set lockoutEndDateTime = @lockoutEnd, failedLoginCount = 0 WHERE userId = @userId");
            if (mySql.executeSQL() == 1) return true;
            return false;
        }


        /// <summary>
        /// Confirm a Users Email address manually.  Ordinarily, this is done by the user who clicks a link in their Welcome email.  
        /// But for users who are created using the desktop application, this must be done manually by certification staff.
        /// </summary>
        /// <returns>True if the User's Email was confirmed.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool confirmAccount()
        {
            SQL mySql = new SQL();
            mySql.addParameter("userId", userId.ToString());
            mySql.setQuery("UPDATE [userWeb] Set isConfirmed = 1 WHERE userId = @userId");
            if (mySql.executeSQL() == 1) return true;
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
        /// Get a User.
        /// </summary>
        /// <param name="userId">The primary key Id of the User.  Set to -1 if you want to use the currently logged in user.</param>
        /// <returns>A User if one was found.  Null otherwise.</returns>
        public static User getUser(long userId = -1)
        {
            if (userId == -1) userId = getLoggedInUserId();
            if (userId == -1) return null;
            return new User(userId);
        }


        /// <summary>
        /// Get the primary key Id of the User based on a User's role.
        /// </summary>
        /// <param name="role">"userWeb", "userAuditor", "userStaff", "userLab"</param>
        /// <param name="roleId">The primary key Id of the userWeb, userAuditor, userStaff or userLab table.</param>
        /// <returns>The primary key Id of the User</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static long getUserIdFromRole(string role, long roleId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", roleId.ToString());
            DataTable records = mySql.getRecords("SELECT userId FROM " + role + " WHERE id = @id");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["userId"].ToString());
            return -1;
        }


        /// <summary>
        /// Get the currently logged in Users primary key Id.  This also supports impersonation for testing.
        /// </summary>
        /// <returns>The currently logged in Users primary key Id. </returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static long getLoggedInUserId()
        {
            // Check for impersonated user - only to be used by Developer when testing !!!!
            long impersonatedUserId = getImpersonatedId();
            if (impersonatedUserId != -1) return impersonatedUserId;

            else
            {
                SQL mySql = new SQL();
                mySql.addParameter("domainUserName", System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                DataTable records = mySql.getRecords("SELECT id FROM [user] WHERE domainUserName = @domainUserName");
                if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            }
            return -1;
        }


        /// <summary>
        /// Get the impersonated Users Id - if one is being used.
        /// </summary>
        /// <returns>The impersonated Users Id - if one is being used. -1 otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static long getImpersonatedId()
        {
            // Check for impersonated user - only to be used by Developer when testing !!!!
            string impersonatedUserIdAsString = Admin.getAdminValue("impersonateUserId");
            if (!String.IsNullOrEmpty(impersonatedUserIdAsString))
            {
                return Convert.ToInt64(impersonatedUserIdAsString);
            }
            return -1;
        }


        /// <summary>
        /// Get the Users Full Name.
        /// </summary>
        /// <param name="userId">The primary key Id of the User.  Set to -1 if you want to use the currently logged in user.</param>
        /// <returns>The Users Full Name if found.  Null otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static string getUserFullName(long userId = -1)
        {
            if (userId == -1) userId = getLoggedInUserId();

            SQL mySql = new SQL();
            mySql.addParameter("id", userId.ToString());
            DataTable records = mySql.getRecords("SELECT fullName FROM [user] WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["fullName"].ToString();
            return null;
        }


        /// <summary>
        /// Get the Users Full Name.
        /// </summary>
        /// <param name="email">The email address of the User.</param>
        /// <returns>The Users Full Name if found.  Null otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static string getUserFullName(string email)
        {
            SQL mySql = new SQL();
            mySql.addParameter("email", email);
            DataTable records = mySql.getRecords("SELECT fullName FROM [user] WHERE email = @email");
            if (records.Rows.Count == 1) return records.Rows[0]["fullName"].ToString();
            return null;
        }


        /// <summary>
        /// Get the primary key Id of the User.
        /// </summary>
        /// <param name="fullName">The User's full name.</param>
        /// <returns>The primary key Id of the User if found.  -1 otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static long getUserId(string fullName)
        {
            SQL mySql = new SQL();
            mySql.addParameter("fullName", fullName.ToString());
            DataTable records = mySql.getRecords("SELECT id FROM [user] WHERE fullName = @fullName");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        /// <summary>
        /// Get the Users Full Name if they are an Auditor.
        /// </summary>
        /// <param name="auditorId">The primary key Id of the UserAuditor.</param>
        /// <returns>The Users Full Name if found.  Null otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static string getAuditorFullName(long auditorId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("auditorId", auditorId.ToString());
            DataTable records = mySql.getRecords("SELECT [user].fullName FROM [user] " + 
                                                 "INNER JOIN userAuditor ON [user].id = userAuditor.userId " + 
                                                 "WHERE userAuditor.id = @auditorId");

            if (records.Rows.Count == 1) return records.Rows[0]["fullName"].ToString();
            return null;
        }


        /// <summary>
        /// Get the userAuditor primary key Id of a User if they are an Auditor.
        /// </summary>
        /// <param name="fullName">The User's full name.</param>
        /// <returns>The userAuditor primary key Id of a User if they are an Auditor. -1 otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static long getAuditorId(string fullName)
        {
            SQL mySql = new SQL();
            mySql.addParameter("fullName", fullName);
            DataTable records = mySql.getRecords("SELECT userAuditor.id FROM [user] " +
                                                 "INNER JOIN userAuditor ON [user].id = userAuditor.userId " +
                                                 "WHERE [user].fullName = @fullName");

            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }



        /// <summary>
        /// Get the userAuditor primary key Id of a User if they are an Auditor.
        /// </summary>
        /// <param name="userId">The primary key id of the User.</param>
        /// <returns>The userAuditor primary key Id of a User if they are an Auditor. -1 otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static long getAuditorIdFromUserId(long userId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("userId", userId.ToString());
            DataTable records = mySql.getRecords("SELECT userAuditor.id FROM [user] " +
                                                 "INNER JOIN userAuditor ON [user].id = userAuditor.userId " +
                                                 "WHERE [user].id = @userId");

            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
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


        /// <summary>
        /// Get the name of the User from the User's userAuditor primary key Id.
        /// </summary>
        /// <param name="auditorId">The primary key Id of the UserAuditor.</param>
        /// <returns>The name of the User if found.  Null otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static string getUserNameFromAuditorId(long auditorId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("auditorId", auditorId.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM userAuditor WHERE id = @auditorId");
            if (records.Rows.Count == 1)
            {
                User user = new User (Convert.ToInt64(records.Rows[0]["userId"].ToString()));
                return user.fullName;
            }
            return null;
        }


        /// <summary>
        /// Get the primary key Id of the User from the User's userWeb primary key Id.
        /// </summary>
        /// <param name="webUserId">The primary key Id of the UserWeb.</param>
        /// <returns>The primary key Id of the User if found.  -1 otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static long getUserIdFromWebUserId(long webUserId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("webUserId", webUserId.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM userWeb WHERE id = @webUserId");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["userId"].ToString());
            return -1;
        }


        /// <summary>
        /// Get a User's email address.
        /// </summary>
        /// <param name="userId">The primary key Id of the User.  Set to -1 if you want to use the currently logged in user.</param>
        /// <returns>The User's email address if found.  Null otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static string getUserEmailAddress(long userId = -1)
        {
            if (userId == -1) userId = getLoggedInUserId();

            SQL mySql = new SQL();
            mySql.addParameter("id", userId.ToString());
            DataTable records = mySql.getRecords("SELECT email FROM [user] WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["email"].ToString();
            return null;
        }


        /// <summary>
        /// Determine if an Email address is already being used by a User.
        /// </summary>
        /// <param name="email">The Email address to search for.</param>
        /// <returns>True if an Email address is already being used by a User. False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static bool emailExists(string email)
        {
            SQL mySql = new SQL();
            mySql.addParameter("email", email);
            DataTable records = mySql.getRecords("SELECT * FROM [user] WHERE email = @email");
            if (records.Rows.Count == 1) return true;
            return false;
        }


        /// <summary>
        /// Determine if an User fullname is already being used by a User.
        /// </summary>
        /// <param name="fullName">The Full Name to search for.</param>
        /// <returns>True if the fullname is already being used by a User. False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static bool fullNameExists(string fullName)
        {
            SQL mySql = new SQL();
            mySql.addParameter("fullName", fullName);
            DataTable records = mySql.getRecords("SELECT * FROM [user] WHERE fullName = @fullName");
            if (records.Rows.Count > 0) return true;
            return false;
        }


        /// <summary>
        /// Get the User primary key Id from the <strong>userSession</strong> table.  Everytime someone using the desktop 
        /// application starts the application, a Session entry is created in the userSession table.  The entry is deleted on log off.
        /// </summary>
        /// <returns>The User primary key Id from the userSession table.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static long getUserIdFromSession()
        {
            SQL mySql = new SQL();
            string sessionIdAsString = mySql.lookup("userSession", "userId", "machine = '" + Environment.MachineName + "'");
            if (!String.IsNullOrEmpty(sessionIdAsString)) return Convert.ToInt64(sessionIdAsString);
            return -1;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static int countActiveWebUsers()
        {
            SQL mySql = new SQL();
            DataTable records = mySql.getRecords("SELECT * FROM userWeb WHERE isDeleted = 0");
            return records.Rows.Count;
        }
    }
}
