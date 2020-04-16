using System;
using System.Collections.Generic;
using System.Data;

using CertifyWPF.WPF_Library;
using CertifyWPF.WPF_Utils;

namespace CertifyWPF.WPF_Client
{
    /// <summary>
    /// Client Address.  Client Addresses are stored in the <strong>clientAddress</strong> table.
    /// </summary>
    public class ClientAddress
    {

        /// <summary>
        /// The primary key Id of this address.
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// The primary key Id of the Client who owns this address.
        /// </summary>
        public long clientId { get; set; }

        /// <summary>
        /// The property name - if applicable.
        /// </summary>
        public string propertyName { get; set; }

        /// <summary>
        /// The first line of the street address.  Example 12 Somestreet Rd.
        /// </summary>
        public string street1 { get; set; }

        /// <summary>
        /// The second line of the street address.  This is usually empty.
        /// </summary>
        public string street2 { get; set; }

        /// <summary>
        /// The town or suburb name.
        /// </summary>
        public string town { get; set; }

        /// <summary>
        /// The primary key Id of the Australian state.  States are defined in the <strong>list_state</strong> table.
        /// </summary>
        public long list_stateId { get; set; }

        /// <summary>
        /// The primary key Id of the country.  Countries are defined in the <strong>list_country</strong> table.
        /// </summary>
        public long list_countryId { get; set; }

        /// <summary>
        /// The region of the address - this is currently not being used.
        /// </summary>
        public string region { get; set; }

        /// <summary>
        /// The Australian Post Code.
        /// </summary>
        public string postcode { get; set; }

        /// <summary>
        /// The pretty, formatted version of the complete address string.
        /// </summary>
        public string prettyAddress { get; set; }

        /// <summary>
        /// The shorter, pretty, formatted version of the complete address string.
        /// </summary>
        public string prettyAddressShort { get; set; }

        /// <summary>
        /// Any restrictions applied to this address - such as In-Conversion or Quarantine
        /// </summary>
        public string restriction { get; set; }

        /// <summary>
        /// Why the restriction exists - such as "Used prohibited input" or similar
        /// </summary>
        public string restrictionReason { get; set; }

        /// <summary>
        /// When the restriction ends.
        /// </summary>
        public DateTime restrictionEndDate { get; set; }

        /// <summary>
        /// What is the current modifier primary key ID for the Client Address.
        /// </summary>
        public long list_clientAddressModifierId { get; set; }

        /// <summary>
        /// What is the text description of the current modifier for certified items coming from this address.
        /// </summary>
        public string modifier { get; set; }

        /// <summary>
        /// A flag to indicate if this is the clients main operation address.
        /// </summary>
        public bool isOperationAddress { get; set; }

        /// <summary>
        /// A flag to indicate if this is the clients postal address.
        /// </summary>
        public bool isPostalAddress { get; set; }

        /// <summary>
        /// A flag to indicate that this property requires and audit.
        /// </summary>
        public bool requiresAudit { get; set; }

        /// <summary>
        /// When did the client first apply fir ths property. 
        /// </summary>
        public DateTime appliedDate { get; set; }

        /// <summary>
        /// When did this property achieve full organic certification.
        /// </summary>
        public DateTime certifiedOrganicDate { get; set; }

        /// <summary>
        /// The addresses latitude
        /// </summary>
        public string latitude { get; set; }

        /// <summary>
        /// The addresses longitude
        /// </summary>
        public string longitude { get; set; }

        /// <summary>
        /// A list of uses for this Client Address.
        /// </summary>
        public List<ClientAddressUse> uses { get; set; }

        public ClientAddressSample sampling { get; set; }

        /// <summary>
        /// A flag to indicate if this address should be audited with the main address
        /// </summary>
        public bool auditedWithMain { get; set; }


        /// <summary>
        /// Constructor. Use this for a new Address.
        /// </summary>
        public ClientAddress()
        {
            id = -1;
            clientId = -1;
            propertyName = null;
            street1 = null;
            street2 = null;
            town = null;
            list_stateId = -1;
            list_countryId = -1;
            region = null;
            postcode = null;
            prettyAddress = null;
            restriction = null;
            restrictionReason = null;
            restrictionEndDate = new DateTime(1970, 1, 1);
            appliedDate = new DateTime(1970, 1, 1);
            certifiedOrganicDate = new DateTime(1970, 1, 1);
            list_clientAddressModifierId = -1;
            modifier = null;
            isOperationAddress = false;
            isPostalAddress = false;
            requiresAudit = true;
            auditedWithMain = true;
            latitude = null;
            longitude = null;

            uses = new List<ClientAddressUse>();
            sampling = null;
        }


        /// <summary>
        /// Constructor. Use this for an existing Address.
        /// </summary>
        /// <param name="_id">The primary key Id of the the address to get.</param>
        public ClientAddress(long _id)
        {
            id = _id;
            clientId = -1;
            propertyName = null;
            street1 = null;
            street2 = null;
            town = null;
            list_stateId = -1;
            list_countryId = -1;
            region = null;
            postcode = null;
            prettyAddress = null;
            restriction = null;
            restrictionReason = null;
            restrictionEndDate = new DateTime(1970, 1, 1);
            appliedDate = new DateTime(1970, 1, 1);
            certifiedOrganicDate = new DateTime(1970, 1, 1);
            list_clientAddressModifierId = -1;
            modifier = null;
            isOperationAddress = false;
            isPostalAddress = false;
            requiresAudit = true;
            auditedWithMain = true;
            latitude = null;
            longitude = null;

            uses = new List<ClientAddressUse>();
            longitude = null;

            fetch();
        }


        /// <summary>
        /// Fetch an existing Address record from the database.
        /// </summary>
        /// <returns>True if the Address was found.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool fetch()
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM clientAddress WHERE isDeleted = 0 AND id = @id");

            if (records.Rows.Count == 1)
            {
                DataRow row = records.Rows[0];
                clientId = Convert.ToInt64(row["clientId"].ToString());
                propertyName = row["propertyName"].ToString();
                street1 = row["street1"].ToString();
                street2 = row["street2"].ToString();

                string stateIdAsString = row["list_stateId"].ToString();
                if(!String.IsNullOrEmpty(stateIdAsString)) list_stateId = Convert.ToInt64(stateIdAsString);

                list_countryId = Convert.ToInt64(row["list_countryId"].ToString());
                town = row["town"].ToString();
                region = row["region"].ToString();
                postcode = row["postcode"].ToString();
                restriction = row["restriction"].ToString();
                restrictionReason = row["restrictionReason"].ToString();

                restrictionEndDate = Utils.getDateTime(row["restrictionEndDate"].ToString());
                appliedDate = Utils.getDateTime(row["appliedDate"].ToString());
                certifiedOrganicDate = Utils.getDateTime(row["certifiedOrganicDate"].ToString());

                string modifierIdAsString = row["list_clientAddressModifierId"].ToString();
                if (!String.IsNullOrEmpty(modifierIdAsString))
                {
                    list_clientAddressModifierId = Convert.ToInt64(modifierIdAsString);
                    modifier = UtilsList.getClientAddressModifierName(list_clientAddressModifierId);
                }

                latitude = row["latitude"].ToString();
                longitude = row["longitude"].ToString();
                isOperationAddress = Convert.ToBoolean(row["isOperationAddress"].ToString());
                isPostalAddress = Convert.ToBoolean(row["isPostalAddress"].ToString());
                requiresAudit = Convert.ToBoolean(row["requiresAudit"].ToString());
                auditedWithMain = Convert.ToBoolean(row["auditedWithMain"].ToString());

                prettyAddress = pretty();
                prettyAddressShort = pretty(true, true);

                // Now retreive all of this addresses uses
                uses = ClientAddressUse.getUses(this);

                // Get any sampling requirments for this address
                sampling = ClientAddressSample.getClientAddressSample(this);

                // All good
                return true;
            }
            return false;
        }


        /// <summary>
        /// Save the Address record to the database.  This is an Upsert operation.
        /// </summary>
        /// <returns>True if the Upsert was successful.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool save()
        {
            SQL mySql = new SQL();
            mySql.addParameter("clientId", clientId.ToString());
            mySql.addParameter("propertyName", propertyName);
            mySql.addParameter("street1", street1);
            mySql.addParameter("street2", street2);
            mySql.addParameter("town", town);

            if(list_stateId == -1) mySql.addParameter("list_stateId", null);
            else mySql.addParameter("list_stateId", list_stateId.ToString());

            mySql.addParameter("list_countryId", list_countryId.ToString());
            mySql.addParameter("region", region);
            mySql.addParameter("postcode", postcode);
            mySql.addParameter("restriction", restriction);
            mySql.addParameter("restrictionReason", restrictionReason);

            if (restrictionEndDate != new DateTime(1970,1,1)) mySql.addParameter("restrictionEndDate", restrictionEndDate.ToString("yyyy-MM-dd"));
            else mySql.addParameter("restrictionEndDate", null);

            if (appliedDate != new DateTime(1970, 1, 1)) mySql.addParameter("appliedDate", appliedDate.ToString("yyyy-MM-dd"));
            else mySql.addParameter("appliedDate", null);

            if (certifiedOrganicDate != new DateTime(1970, 1, 1)) mySql.addParameter("certifiedOrganicDate", certifiedOrganicDate.ToString("yyyy-MM-dd"));
            else mySql.addParameter("certifiedOrganicDate", null);

            mySql.addParameter("list_clientAddressModifierId", list_clientAddressModifierId.ToString());

            if (isOperationAddress) mySql.addParameter("isOperationAddress", "1");
            else mySql.addParameter("isOperationAddress", "0");

            if (isPostalAddress) mySql.addParameter("isPostalAddress", "1");
            else mySql.addParameter("isPostalAddress", "0");

            if (requiresAudit) mySql.addParameter("requiresAudit", "1");
            else mySql.addParameter("requiresAudit", "0");

            if (auditedWithMain) mySql.addParameter("auditedWithMain", "1");
            else mySql.addParameter("auditedWithMain", "0");

            mySql.addParameter("latitude", latitude);
            mySql.addParameter("longitude", longitude);

            // save
            if (id == -1)
            {
                mySql.setQuery(@"INSERT INTO 
                                clientAddress 
                                (clientId, propertyName, street1, street2, town, list_stateId, list_countryId, postcode, region, 
                                 restriction, restrictionReason, restrictionEndDate, appliedDate, certifiedOrganicDate, 
                                 list_clientAddressModifierId, isOperationAddress, isPostalAddress, requiresAudit, auditedWithMain, latitude, longitude) 
                                VALUES 
                                (@clientId, @propertyName, @street1, @street2, @town, @list_stateId, @list_countryId, @postcode, @region, 
                                 @restriction, @restrictionReason, @restrictionEndDate, @appliedDate, @certifiedOrganicDate, 
                                 @list_clientAddressModifierId, @isOperationAddress, @isPostalAddress, @requiresAudit, @auditedWithMain, @latitude, @longitude)");

                if (mySql.executeSQL() == 1)
                {

                    id = mySql.getMaxId("clientAddress");
                    return saveUses();
                }
                else Log.write("An error ocurred while trying to add an address to the client. The address was not added to the client properly.");
            }

            // Update
            else
            {
                mySql.addParameter("id", id.ToString());
                mySql.setQuery(@"UPDATE clientAddress
                                 SET 
                                 clientId = @clientId,
                                 propertyName = @propertyName,
                                 street1 = @street1, 
                                 street2 = @street2, 
                                 town = @town, 
                                 list_stateId = @list_stateId,
                                 list_countryId = @list_countryId,
                                 postcode = @postcode,
                                 region = @region,
                                 restriction = @restriction,
                                 restrictionReason = @restrictionReason,
                                 restrictionEndDate = @restrictionEndDate,
                                 appliedDate = @appliedDate,
                                 certifiedOrganicDate = @certifiedOrganicDate,
                                 list_clientAddressModifierId = @list_clientAddressModifierId,
                                 isOperationAddress = @isOperationAddress,
                                 isPostalAddress = @isPostalAddress,
                                 requiresAudit = @requiresAudit,
                                 auditedWithMain = @auditedWithMain,
                                 latitude = @latitude,
                                 longitude = @longitude
                                 WHERE id = @id");

                if (mySql.executeSQL() == 1) return saveUses();
                else Log.write("There was an error while trying to update this client address. The status has not changed properly.");
            }
            return false;
        }


        /// <summary>
        /// Save all of this clients address uses.  
        /// </summary>
        /// <returns>True if all uses were saved, false otherwise.</returns>
        ///--------------------------------------------------------------------------------------------------------------------------
        public bool saveUses()
        {
            // Save current list
            int successCount = 0;
            foreach (ClientAddressUse use in uses)
            {
                if (use.save()) successCount++;
            }

            if (successCount == uses.Count) return true;
            return false;
        }


        ///--------------------------------------------------------------------------------------------------------------------------
        private void clearUses()
        {
            // First, blow away all existing uses.
            SQL mySql = new SQL();
            mySql.addParameter("clientAddressId", id.ToString());
            mySql.setQuery(@"DELETE FROM clientAddressUse WHERE clientAddressId = @clientAddressId");
            mySql.executeSQL();

            uses.Clear();
        }


        /// <summary>
        /// Get a pretty, formatted version of the full address.
        /// </summary>
        /// <param name="includePropertyName">A flag to determine if we include the property name in brackets as a suffix.</param>
        /// <param name="shorter">A flag to indicate if we should use a shorter address output</param>
        /// <returns>A pretty, formatted version of the full address.</returns>
        ///--------------------------------------------------------------------------------------------------------------------------
        public string pretty(bool includePropertyName = true, bool shorter = false)
        {
            string str = street1;
            if (!String.IsNullOrEmpty(street2)) str += ", " + street2;
            if (!String.IsNullOrEmpty(town)) str += ", " + town;
            if (list_stateId != -1) str += ", " + UtilsList.getStateName(list_stateId, shorter);
            if (list_countryId != -1 && !shorter) str += ", " + UtilsList.getCountryName(list_countryId);
            if (!String.IsNullOrEmpty(postcode)) str += ", " + postcode;
            if (!String.IsNullOrEmpty(propertyName) && includePropertyName) str += " (" + propertyName + ")";

            return str;
        }


        /// <summary>
        /// Mark an address as deleted.
        /// </summary>
        /// <returns>True if the Address was marked as deleted.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool delete()
        {
            if (id == -1) return false;

            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            mySql.setQuery(@"UPDATE clientAddress SET isDeleted = 1 WHERE id = @id");
            if (mySql.executeSQL() == 1) return true;
            return false;
        }


        /// <summary>
        /// Determine if the Address exists.
        /// </summary>
        /// <returns>True if the Address exists.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool exists()
        {
            if (clientId == -1) return false;

            SQL mySql = new SQL();
            mySql.addParameter("clientId", clientId.ToString());
            mySql.addParameter("street1", street1);
            mySql.addParameter("town", town);
            mySql.addParameter("list_stateId", list_stateId.ToString());
            mySql.addParameter("list_countryId", list_countryId.ToString());
            mySql.addParameter("postcode", postcode);

            DataTable records = mySql.getRecords(@"SELECT * FROM clientAddress WHERE 
                                                  clientId = @clientId AND 
                                                  street1 = @street1 AND 
                                                  town = @town AND 
                                                  list_stateId = @list_stateId AND
                                                  list_countryId = @list_countryId AND
                                                  postcode = @postcode");

            if (records.Rows.Count > 0) return true;
            return false;
        }


        /// <summary>
        /// Determine the Certified Item modifier, based on the address modifier for this address
        /// </summary>
        /// <returns>The Certified Item modifier if one can be found, -1 otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public long getCertifiedItemModifierId()
        {
            SQL mySql = new SQL();
            mySql.addParameter("list_clientAddressModifierId", list_clientAddressModifierId.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM list_clientAddressModifier WHERE id = @list_clientAddressModifierId");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["list_certifiedItemModifieId"].ToString());
            return -1;
        }


        /// <summary>
        /// Get the standard client address uses for this address, based on the Clients categories.
        /// </summary>
        /// <returns></returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool getUsesFromCategories(bool deleteExisting = true)
        {
            if (clientId == -1) return false;

            Client client = new Client(clientId);
            if (client.clientCategories.Count <= 0) return false;

            if (deleteExisting) clearUses();
            foreach (ClientCategory cat in client.clientCategories)
            {
                List<long> useIds = cat.getUses();
                foreach(long useId in useIds)
                {
                    ClientAddressUse newUse = new ClientAddressUse()
                    {
                        clientAddressId = id,
                        list_clientAddressUseId = useId
                    };
                    addUse(newUse);
                }
            }
            return true;
        }


        /// <summary>
        /// Updates the uses for this cleint address
        /// </summary>
        /// <param name="newUses">A list of cleint address use names.</param>
        /// <returns>True if the list was updated and saved.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool updateUses(List<string> newUses)
        {
            clearUses();

            foreach (string newUse in newUses)
            {
                long newUseId = UtilsList.getClientAddressUseId(newUse);
                ClientAddressUse use = new ClientAddressUse()
                {
                    clientAddressId = id,
                    list_clientAddressUseId = newUseId
                };
                addUse(use);
            }
            return save();
        }


        /// <summary>
        /// Add a new use to the uses for this client address.  This protects against duplication.
        /// </summary>
        /// <param name="newUse">The use to add.</param>
        /// <returns>True if the use was added.  False otherwise</returns>
        ///--------------------------------------------------------------------------------------------------------------------------
        public bool addUse(ClientAddressUse newUse)
        {
            // Only add if the use has not already been added
            foreach(ClientAddressUse use in uses)
            {
                if (use.list_clientAddressUseId == newUse.list_clientAddressUseId) return false;
            }
            uses.Add(newUse);
            return true;
        }


        /// <summary>
        /// Set the initial client addres suse for an address.
        /// </summary>
        //--------------------------------------------------------------------------------------------------------------------------
        public void setDefaultUses()
        {
            if (id == -1) return;

            uses.Clear();

            if(isOperationAddress)
            {
                ClientAddressUse recordsUse = new ClientAddressUse()
                {
                    clientAddressId = id,
                    list_clientAddressUseId = UtilsList.getClientAddressUseId("Records")
                };
                uses.Add(recordsUse);

                ClientAddressUse labellingUse = new ClientAddressUse()
                {
                    clientAddressId = id,
                    list_clientAddressUseId = UtilsList.getClientAddressUseId("Labelling")
                };
                uses.Add(labellingUse);

                ClientAddressUse packagingUse = new ClientAddressUse()
                {
                    clientAddressId = id,
                    list_clientAddressUseId = UtilsList.getClientAddressUseId("Packaging")
                };
                uses.Add(packagingUse);
            }
            
            getUsesFromCategories(false);          
        }


        /// <summary>
        /// Count the number of groups that this Client Address is associated with.
        /// </summary>
        /// <returns>The number of groups that this Client Address is associated with.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public int countGroups()
        {
            SQL mySql = new SQL();
            mySql.addParameter("clientAddressId", id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM clientGroupMemberAddress WHERE clientAddressId = @clientAddressId");
            return records.Rows.Count;
        }


        //
        // Static Methods
        //

        /// <summary>
        /// Get all Client Addresses for a Client.
        /// </summary>
        /// <param name="clientId">The primary key Id of the Client who owns this address.</param>
        /// <returns>A DataTable of all Client Addresses.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static DataTable getClientAddresses(long clientId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", clientId.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM clientAddress WHERE clientId = @id");
            return records;
        }

      
        /// <summary>
        /// Get a list of all Addresses for a Client.
        /// </summary>
        /// <param name="client">The Client.</param>
        /// <returns>A list of all Addresses for a Client.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static List<ClientAddress> getClientAddresses(Client client, bool requiringAudit = false)
        {
            List<ClientAddress> addresses = new List<ClientAddress>();

            SQL mySql = new SQL();
            mySql.addParameter("id", client.id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM clientAddress WHERE isDeleted = 0 AND clientId = @id");
            foreach (DataRow row in records.Rows)
            {
                long id = Utils.getLongFromString(row["id"].ToString());
                if(id != -1)
                {
                    ClientAddress address = new ClientAddress(id);
                    if(!requiringAudit || (requiringAudit && address.requiresAudit)) addresses.Add(address);
                }
                
            }
            return addresses;
        }


        /// <summary>
        /// Check to see if the client addresses are complete for a Client.
        /// </summary>
        /// <param name="client">The Client to check.</param>
        /// <returns>True if all Client addresses are complete.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static bool checkClientAddresses(Client client)
        {
            List<ClientAddress> addresses = getClientAddresses(client);
            foreach(ClientAddress address in addresses)
            {
                if (address.list_clientAddressModifierId == -1) return false;
            }
            return true;
        }


        /// <summary>
        /// Determine a list of Certified Item modifiers, based on the statuses of client's addresses
        /// </summary>
        /// <param name="client">The client that has the addresses</param>
        /// <returns>A list of Certified Item modifiers, based on the statuses of client's addresses.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static List<long> getCertifiedItemModifiersFromAddressModifiers(Client client)
        {
            List<long> modifiers = new List<long>();
            List<ClientAddress> addresses = getClientAddresses(client);
            foreach (ClientAddress address in addresses)
            {
                long certifiedItemModifierId = address.getCertifiedItemModifierId();
                if(UtilsList.getCertifiedItemModifierName(certifiedItemModifierId) != "Unknown")
                {
                    bool found = false;
                    foreach (long modifierId in modifiers)
                    {
                        if (modifierId == certifiedItemModifierId) found = true;
                    }
                    if (!found) modifiers.Add(certifiedItemModifierId);
                }
            }
            return modifiers;
        }


        /// <summary>
        /// Counts how many Client Addresses have restrictions ending today (or earlier).
        /// </summary>
        /// <returns>The number of Client Services that have restrictions ending today (or earlier).</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static int countEndingRestrictions()
        {
            SQL mySql = new SQL();
            mySql.addParameter("today", DateTime.Now.ToString("yyyy-MM-dd"));
            DataTable records = mySql.getRecords(@"SELECT
                                                   clientAddress.id, clientAddress.clientId, clientAddress.propertyName, clientAddress.street1, 
                                                   clientAddress.street2, clientAddress.town, clientAddress.list_stateId, clientAddress.list_countryId, 
                                                   clientAddress.postcode, clientAddress.region, clientAddress.restriction, clientAddress.restrictionReason, 
                                                   clientAddress.list_clientAddressModifierId, clientAddress.appliedDate, clientAddress.certifiedOrganicDate, 
                                                   clientAddress.isOperationAddress,clientAddress.isPostalAddress, clientAddress.requiresAudit, 
                                                   clientAddress.latitude, clientAddress.longitude, clientAddress.auditedWithMain, clientAddress.restrictionEndDate, 
                                                   clientAddress.isDeleted
                                                   FROM            
                                                   clientAddress 
                                                   INNER JOIN clientService ON clientAddress.clientId = clientService.clientId 
                                                   INNER JOIN serviceStatus ON clientService.serviceStatusId = serviceStatus.id
                                                   WHERE
                                                   (clientAddress.isDeleted = 0) AND
                                                   (clientAddress.restrictionEndDate <= GETDATE()) AND
                                                   (serviceStatus.name = N'Active')");
            return records.Rows.Count;
        }
    }
}
