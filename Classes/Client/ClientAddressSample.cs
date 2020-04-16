using System;
using System.Collections.Generic;
using System.Data;

using CertifyWPF.WPF_Library;
using CertifyWPF.WPF_Utils;

namespace CertifyWPF.WPF_Client
{

    /// <summary>
    /// Client Address Sample. This is used for audit planning, and determining if a sample must be taken the next time a clients address is audited.  
    /// This can be found in the <strong>clientAddressSample</strong> table.
    /// </summary>
    public class ClientAddressSample
    {
        /// <summary>
        /// The primary key Id of this Client Address Sample.
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// The primary key Id of the client address this use applies to.
        /// </summary>
        public long clientAddressId { get; set; }

        /// <summary>
        /// The next year a sample must be taken.
        /// </summary>
        public int nextSampleYear { get; set; }

        /// <summary>
        /// Instructions for when the next sample is taken
        /// </summary>
        public string nextSampleInstructions { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        //--------------------------------------------------------------------------------------------------------------------------
        public ClientAddressSample()
        {
            id = -1;
            clientAddressId = -1;
            nextSampleYear = -1;
            nextSampleInstructions = null;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        //--------------------------------------------------------------------------------------------------------------------------
        public ClientAddressSample(long _id)
        {
            id = _id;
            clientAddressId = -1;
            nextSampleYear = -1;
            nextSampleInstructions = null;

            fetch();
        }


        /// <summary>
        /// Retrieve an Client Address Sample record from the database.
        /// </summary>
        /// <returns>True if the record was retrieved successfully.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool fetch()
        {
            if (id == -1) return false;

            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM clientAddressSample WHERE id = @id");
            if (records.Rows.Count == 1)
            {
                clientAddressId = Utils.getLongFromString(records.Rows[0]["clientAddressId"].ToString());
                nextSampleYear = Utils.getIntFromString(records.Rows[0]["nextSampleYear"].ToString());
                nextSampleInstructions = records.Rows[0]["nextSampleInstructions"].ToString();
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
            mySql.addParameter("nextSampleYear", nextSampleYear.ToString());
            mySql.addParameter("nextSampleInstructions", nextSampleInstructions);

            if (id == -1)
            {
                mySql.setQuery("INSERT INTO clientAddressSample (clientAddressId, nextSampleYear, nextSampleInstructions) VALUES (@clientAddressId, @nextSampleYear, @nextSampleInstructions)");
                if (mySql.executeSQL() == 1)
                {
                    id = mySql.getMaxId("clientAddressSample");
                    return true;
                }
            }
            else
            {
                mySql.addParameter("id", id.ToString());
                mySql.setQuery(@"UPDATE clientAddressSample 
                                 SET
                                 clientAddressId = @clientAddressId,
                                 nextSampleYear = @nextSampleYear,
                                 nextSampleInstructions = @nextSampleInstructions
                                 WHERE
                                 id = @id");

                if (mySql.executeSQL() == 1)return true;
            }
            return false;
        }


        /// <summary>
        /// Permanently delete and Client Address Sample.
        /// </summary>
        /// <returns>True if the record was deleted.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool delete()
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            mySql.setQuery("DELETE clientAddressSample WHERE id = @id");
            if (mySql.executeSQL() == 1) return true;
            return false;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static ClientAddressSample getClientAddressSample(ClientAddress address)
        {
            SQL mySql = new SQL();
            mySql.addParameter("clientAddressId", address.id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM clientAddressSample WHERE clientAddressId = @clientAddressId");

            if(records.Rows.Count == 1)
            {
                long id = Utils.getLongFromString(records.Rows[0]["id"].ToString());
                if (id != -1) return new ClientAddressSample(id);
            }

            return null;
        }
    }
}
