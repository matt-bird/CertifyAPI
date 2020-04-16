using System;
using System.Data;

using CertifyWPF.WPF_Library;
using CertifyWPF.WPF_Client;
using CertifyWPF.WPF_Utils;
using System.Collections.Generic;

namespace CertifyWPF.WPF_Service
{
    /// <summary>A Service is another way of saying certification scheme or certification service offered by the Certification body.  
    /// Services are stored in the <strong>service</strong> table.  
    /// </summary>
    /// <seealso cref="ClientService"/>
    /// <seealso cref="ClientServiceHistory"/>

    public class Service
    {
        /// <summary>
        /// The primary key Id of the Service.
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// The short name of the Service.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// The full name of the Service.
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// The abbreviation of the Service name - one or two letters.
        /// </summary>
        public string abbreviation { get; set; }

        /// <summary>
        /// The name of the Services reference document.
        /// </summary>
        public string reference { get; set; }

        /// <summary>
        /// The Application Fee associated with this service
        /// </summary>
        public float applicationFee { get; set; }

        /// <summary>
        /// The Xero chart of accounts account code used for the application Fee
        /// </summary>
        public string applicationFeeXeroCode { get; set; }

        /// <summary>
        /// The Certification Fee associated with this Service
        /// </summary>
        public float certificationFee { get; set; }

        /// <summary>
        /// The Xero chart of accounts account code used for the certification Fee.
        /// </summary>
        public string certificationFeeXeroCode { get; set; }

        /// <summary>
        /// A flag to indicate if this service is currently active
        /// </summary>
        public bool active { get; set; }


        /// <summary>
        /// Constructor.
        /// </summary>
        //--------------------------------------------------------------------------------------------------------------------------
        public Service()
        {
            id = -1;
            name = null;
            title = null;
            abbreviation = null;
            reference = null;
            applicationFee = -1;
            certificationFee = -1;
            applicationFeeXeroCode = null;
            certificationFeeXeroCode = null;

            active = false;
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="_id">The primary key Id of the Service.</param>
        //--------------------------------------------------------------------------------------------------------------------------
        public Service(long _id)
        {
            id = _id;
            name = null;
            title = null;
            abbreviation = null;
            reference = null;
            applicationFee = -1;
            certificationFee = -1;
            applicationFeeXeroCode = null;
            certificationFeeXeroCode = null;
            active = false;

            fetch();
        }


        /// <summary>
        /// Fetch the Service from the Database.
        /// </summary>
        /// <returns>True if the fetch was successful.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool fetch()
        {
            if (id == -1) return false;

            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM service WHERE id = @id");
            if(records.Rows.Count == 1)
            {
                name = records.Rows[0]["name"].ToString();
                title = records.Rows[0]["title"].ToString();
                abbreviation = records.Rows[0]["abbreviation"].ToString();
                reference = records.Rows[0]["reference"].ToString();
                applicationFee = Utils.getFloatFromString(records.Rows[0]["applicationFee"].ToString());
                certificationFee = Utils.getFloatFromString(records.Rows[0]["certificationFee"].ToString());
                applicationFeeXeroCode = records.Rows[0]["applicationFeeXeroCode"].ToString();
                certificationFeeXeroCode = records.Rows[0]["certificationFeeXeroCode"].ToString();
                active = Convert.ToBoolean(records.Rows[0]["active"].ToString());
                return true;
            }
            return false;
        }


        //
        // Static Methods
        //

        /// <summary>
        /// Get the Service name.
        /// </summary>
        /// <param name="serviceId">The primary key Id of the Service.</param>
        /// <returns>The Service name.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static string getName(long serviceId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", serviceId.ToString());
            DataTable records = mySql.getRecords("SELECT name FROM service WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["name"].ToString();
            return null;
        }


        /// <summary>
        /// Get the Service Title.
        /// </summary>
        /// <param name="serviceId">The primary key Id of the Service.</param>
        /// <returns>The Service Title.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static string getTitle(long serviceId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", serviceId.ToString());
            DataTable records = mySql.getRecords("SELECT title FROM service WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["title"].ToString();
            return null;
        }


        /// <summary>
        /// Get the Service primary key Id.
        /// </summary>
        /// <param name="name">The name of the Service.</param>
        /// <returns>The Service primary key Id.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static long getId(string name)
        {
            SQL mySql = new SQL();
            mySql.addParameter("name", name);
            DataTable records = mySql.getRecords("SELECT id FROM service WHERE name = @name");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        /// <summary>
        /// Get the abbreviation of the Service name.
        /// </summary>
        /// <param name="name">The name of the Service.</param>
        /// <returns>The abbreviation of the Service name.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static string getAbbreviation(string name)
        {
            SQL mySql = new SQL();
            mySql.addParameter("name", name);
            DataTable records = mySql.getRecords("SELECT abbreviation FROM service WHERE name = @name");
            if (records.Rows.Count == 1) return records.Rows[0]["abbreviation"].ToString();
            return null;
        }


        /// <summary>
        /// Get the abbreviation of the Service name. Overload
        /// </summary>
        /// <param name="_id">The primary key Id of the Service.</param>
        /// <returns>The abbreviation of the Service name.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static string getAbbreviation(long _id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", _id.ToString());
            DataTable records = mySql.getRecords("SELECT abbreviation FROM service WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["abbreviation"].ToString();
            return null;
        }


        /// <summary>
        /// Get a list of client category strings.
        /// </summary>
        /// <returns>A list of client category strings.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static List<string> getServiceList()
        {
            List<string> list = new List<string>();
            SQL mySql = new SQL();
            DataTable records = mySql.getRecords("SELECT * FROM service WHERE active = 1");
            foreach (DataRow row in records.Rows) list.Add(row["name"].ToString());
            return list;
        }
    }
}
