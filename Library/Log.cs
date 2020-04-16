

using System;
using System.Data;

namespace CertifyWPF.WPF_Library
{
    /// <summary>
    /// Very simple logger class.  Logs to the <strong>log</strong> and <strong>logEmail</strong> tables.
    /// </summary>
    

    class Log
    {
        /// <summary>
        /// Make a log entry.
        /// </summary>
        /// <param name="entry">The text body of the log entry.</param>
        /// <param name="isEmailLog">Optional: A flag to indicate if this should logged as an email Log entry or not.  Default = False.</param>
        /// <param name="appType">Optional: The application type that is logging the enrty.  Use "App" for the desktop 
        /// application (default) or "Monitor" for the Certify Montor application.</param>
        //-------------------------------------------------------------------------------------------------------------------------
        public static void write(string entry, bool isEmailLog = false, string appType = "API")
        {
            SQL mySql = new SQL();
            
            //Maximum log entry can be only 500, so limit the length of the entry, in case we get a stack dump or similar
            if (entry.Length > 450) entry = entry.Substring(0, 450);

            mySql.addParameter("entry", "Certify " + appType + ": " + entry);
            if (isEmailLog) mySql.setQuery("INSERT INTO logEmail (entry) VALUES (@entry)");
            else mySql.setQuery("INSERT INTO log (entry) VALUES (@entry)");
            mySql.executeSQL();
        }


        /// <summary>
        /// Count the number of errors recorded in the logs since a certain date.
        /// </summary>
        /// <returns>The number of errors recorded in the logs since a certain date.</returns>
        //-------------------------------------------------------------------------------------------------------------------------
        public static int logErrorCount(DateTime since)
        {
            SQL mySql = new SQL();
            mySql.addParameter("startDate", since.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            // Set the Datasources
            DataTable logSource = mySql.getRecords(@"SELECT * FROM log 
                                                     WHERE 
                                                     entry LIKE '%error%' AND 
                                                     entry NOT LIKE '%Error Log%' AND
                                                     entry NOT LIKE '%No Errors%' AND
                                                     entryDateTime > @startDate 
                                                     ORDER BY 
                                                     id DESC");

            DataTable logEmailSource = mySql.getRecords(@"SELECT * FROM logEmail 
                                                          WHERE 
                                                          entry LIKE '%error%' AND 
                                                          entry NOT LIKE '%Error Log%' AND
                                                          entry NOT LIKE '%Log Error%' AND
                                                          entry NOT LIKE '%No Errors%' AND
                                                          entryDateTime > @startDate
                                                          ORDER BY
                                                          id DESC");

            return logSource.Rows.Count + logEmailSource.Rows.Count;
        }
    }
}
