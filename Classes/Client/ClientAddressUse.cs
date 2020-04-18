using System;
using System.Collections.Generic;
using System.Data;

using CertifyWPF.WPF_Library;
using CertifyWPF.WPF_Utils;

namespace CertifyWPF.WPF_Client
{

    /// <summary>
    /// Client Address Use. The uses / activities carries out at a client address (such as Record Keeping, Livestock or Procssing).  This can be found in the <strong>clientAddressUse</strong> table.
    /// </summary>
    public class ClientAddressUse
    {
        /// <summary>
        /// The primary key Id of this Client Address Use.
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// The primary key Id of the client address this use applies to.
        /// </summary>
        public long clientAddressId { get; set; }

        /// <summary>
        /// The address use type such as "Records" or "Labelling".  This list is kept in the <strong>list_clientAddressUse</strong>database table.
        /// </summary>
        public string addressUse { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        //--------------------------------------------------------------------------------------------------------------------------
        public ClientAddressUse()
        {
            id = -1;
            clientAddressId = -1;
            addressUse = null;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        //--------------------------------------------------------------------------------------------------------------------------
        public ClientAddressUse(long _id)
        {
            id = _id;
            clientAddressId = -1;
            addressUse = null;

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
            DataTable records = mySql.getRecords("SELECT * FROM clientAddressUse WHERE id = @id");
            if (records.Rows.Count == 1)
            {
                clientAddressId = Convert.ToInt64(records.Rows[0]["clientAddressId"].ToString());
                long list_clientAddressUseId = Convert.ToInt64(records.Rows[0]["list_clientAddressUseId"].ToString());
                addressUse = UtilsList.getClientAddressUse(list_clientAddressUseId);
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
            mySql.addParameter("clientAddressId", clientAddressId.ToString());
            mySql.addParameter("list_clientAddressUseId", UtilsList.getClientAddressUseId(addressUse).ToString());

            if (id == -1)
            {
                mySql.setQuery("INSERT INTO clientAddressUse (clientAddressId, list_clientAddressUseId) VALUES (@clientAddressId, @list_clientAddressUseId)");
                if (mySql.executeSQL() == 1)
                {
                    id = mySql.getMaxId("clientAddressUse");
                    return true;
                }
            }
            else
            {
                mySql.addParameter("id", id.ToString());
                mySql.setQuery(@"UPDATE clientAddressUse 
                                 SET
                                 clientAddressId = @clientAddressId,
                                 list_clientAddressUseId = @list_clientAddressUseId
                                 WHERE
                                 id = @id");

                if (mySql.executeSQL() == 1)return true;
            }
            return false;
        }


        /// <summary>
        /// Permanently delete and Client Address use.
        /// </summary>
        /// <returns>True if the record was deleted.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool delete()
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            mySql.setQuery("DELETE clientAddressUse WHERE id = @id");
            if (mySql.executeSQL() == 1) return true;
            return false;
        }



        //
        // Static Methods
        //

        /// <summary>
        /// Get a list of Client Address uses for a Client Address.
        /// </summary>
        /// <param name="address">The Client Address for which the client addresse uses are linked.</param>
        /// <returns>A list of Client Address uses for a Client Address.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static List<ClientAddressUse> getUses(ClientAddress address)
        {
            List<ClientAddressUse> uses = new List<ClientAddressUse>();

            SQL mySql = new SQL();
            mySql.addParameter("clientAddressId", address.id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM clientAddressUse WHERE clientAddressId = @clientAddressId");

            foreach(DataRow row in records.Rows)
            {
                long id = Convert.ToInt64(row["id"].ToString());
                ClientAddressUse cau = new ClientAddressUse(id);
                uses.Add(cau);
            }
            return uses;
        }


        /// <summary>
        /// Get a list of Client address uses such as "Records" or "Packaging" etc.
        /// </summary>
        /// <returns>A list of Client address uses such as "Records" or "Packaging" etc.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static List<string> getAllUses()
        {
            List<string> list = new List<string>();

            SQL mySql = new SQL();
            DataTable records = mySql.getRecords("SELECT * FROM list_clientAddressUse");
            foreach (DataRow row in records.Rows) list.Add(row["name"].ToString());
            return list;
        }
    }
}
