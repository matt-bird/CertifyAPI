using System;
using System.Data;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;

using System.Text.RegularExpressions;
using System.Globalization;

namespace CertifyWPF.WPF_Utils
{
    /// <summary>
    /// A super generic series of static functions.  No ryhme or reason behind saving them here - they didn't really 
    /// fit anywhere else !
    /// </summary>

    public class Utils
    {

        /// <summary>
        /// Convert a string to a int if possible.
        /// </summary>
        /// <param name="numberAsString">The string representation of a number.</param>
        /// <returns>A (int) number if conversion was possible.  -1 if not.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static int getIntFromString(string numberAsString)
        {
            try
            {
                return Convert.ToInt32(numberAsString.Replace("$", "").Replace(",", ""));
            }

            catch (Exception)
            {
                return -1;
            }
        }


        /// <summary>
        /// Convert a string to a float if possible.
        /// </summary>
        /// <param name="numberAsString">The string representation of a number.</param>
        /// <returns>A (float) number if conversion was possible.  -1 if not.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static float getFloatFromString(string numberAsString)
        {
            try
            {
                return Convert.ToSingle(numberAsString.Replace("$","").Replace(",", ""));
            }

            catch (Exception)
            {
                return -1;
            }
        }


        /// <summary>
        /// Convert a string to a Big Int / Long if possible.
        /// </summary>
        /// <param name="numberAsString">The string representation of a number.</param>
        /// <returns>A (float) number if conversion was possible.  -1 if not.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static long getLongFromString(string numberAsString)
        {
            try
            {
                return Convert.ToInt64(numberAsString.Replace("$", "").Replace(",", ""));
            }

            catch (Exception)
            {
                return -1;
            }
        }


        /// <summary>
        /// Get the Date and Time from a string in a safe way.  This handles errors in the strings format properly.
        /// </summary>
        /// <param name="dateString">The string from which to extract the DateTime.</param>
        /// <returns></returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static DateTime getDateTime(string dateString)
        {
            if (DateTime.TryParse(dateString, out DateTime d)) return d;
            else return new DateTime(1970, 1, 1);
        }


        /// <summary>
        /// Execute a MS DOS command in the oeprating system.  Used by Certify Monitor.
        /// </summary>
        /// <param name="command">The MS DOS command to execute.</param>
        /// <param name="workingFolder">Optional: The working folder. If this is not set the current working folder will be used.
        /// Default = Null.</param>
        /// <returns></returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static string executeCommandLine(string command, string workingFolder = null)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            if (!String.IsNullOrEmpty(workingFolder)) process.StartInfo.WorkingDirectory = workingFolder;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();

            process.StandardInput.WriteLine(command);
            process.StandardInput.Flush();
            process.StandardInput.Close();
            process.WaitForExit();

            return process.StandardOutput.ReadToEnd();
        }


        /// <summary>
        /// Detemrine if we are in the production environment.  This uses the connection string settings to the database to determine.
        /// </summary>
        /// <returns>True if we are in the production environment. False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static bool isProduction()
        {
            ConnectionStringsSection config = (ConnectionStringsSection) ConfigurationManager.GetSection("connectionStrings");
            string connString = config.ConnectionStrings["CertifyWPF.Properties.Settings.CertifyConnectionString"].ConnectionString;
            return connString.Contains("SQLWeb\\SQL2017");
        }


        /// <summary>
        /// Determine if we are running as the Checklist Application.
        /// </summary>
        /// <returns>True if we are running as the Checklist Application.  False if we are not.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static bool isChecklistApplication()
        {
            ConnectionStringsSection config = (ConnectionStringsSection)ConfigurationManager.GetSection("connectionStrings");
            string connString = config.ConnectionStrings["CertifyWPF.Properties.Settings.CertifyConnectionString"].ConnectionString;
            return connString.Contains("CertifyChecklist - no connection to DBase possible");
        }


         /// <summary>
        /// Determine if a DataTable has a particular field.
        /// </summary>
        /// <param name="table">The DataTable to inspect.</param>
        /// <param name="name">The field name to look for.</param>
        /// <returns>True if a DataTable has a particular field.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static bool dataTableHasField(DataTable table, string name)
        {
            foreach(DataColumn col in table.Columns)
            {
                if (col.ColumnName == name) return true;
            }
            return false;
        }


         /// <summary>
        /// Determine if a file name has any invalid characters.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static bool isValidFilename(string str)
        {
            string sPattern = @"^(([a-zA-Z]:|\\)\\)?(((\.)|(\.\.)|([^\\/:\*\?""\|<>\. ](([^\\/:\*\?""\|<>\. ])|([^\\/:\*\?""\|<>]*[^\\/:\*\?""\|<>\. ]))?))\\)*[^\\/:\*\?""\|<>\. ](([^\\/:\*\?""\|<>\. ])|([^\\/:\*\?""\|<>]*[^\\/:\*\?""\|<>\. ]))?$";
        
            return (Regex.IsMatch(str, sPattern, RegexOptions.CultureInvariant));
        }


        public static string getTitleCase(string initial)
        {
            // Creates a TextInfo based on the "en-US" culture.
            TextInfo myTI = new CultureInfo("en-AU", false).TextInfo;
            return myTI.ToTitleCase(initial.ToLower());
        }
    }
}
