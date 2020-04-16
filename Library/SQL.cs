using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;


namespace CertifyWPF.WPF_Library
{
    /// <summary>
    /// The class used for interacting with MS SQL server that contains the tables.  Connection strings are defined in the 
    /// properties if the project.  When compiled, the connection strings are defined in the CertifyWPF.exe.config.
    /// </summary>

    public class SQL
    {
        /// <summary>
        /// The query to use for executing the SQL statement.
        /// </summary>
        public string query { get; set; }

        /// <summary>
        /// The SQL Server connection string.
        /// </summary>
        public SqlConnection sqlConnect { get; set; }

        /// <summary>
        /// The SQL command object.
        /// </summary>
        public SqlCommand cmd { get; set; }


        /// <summary>
        /// Constructor.
        /// </summary>
        //----------------------------------------------------------------------------------------------------------------------------
        public SQL()
        {
            query = "";
            sqlConnect = new SqlConnection(CertifyWPF.Properties.Settings.Default.CertifyConnectionString);
            cmd = new SqlCommand();
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="_query">The query to use for executing the SQL statement.</param>
        //----------------------------------------------------------------------------------------------------------------------------
        public SQL(string _query)
        {
            query = _query;
            sqlConnect = new SqlConnection(CertifyWPF.Properties.Settings.Default.CertifyConnectionString);
            cmd = new SqlCommand();
        }


        /// <summary>
        /// Get the current date and time in a string formatted for use in a query.
        /// </summary>
        /// <returns>The current date and time in a string formatted for use in a query.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        // Gets the current date and time in a format that SQL server can use in Queries
        public string getDateTimeForSQLServer()
        {
            DateTime myDateTime = DateTime.Now;
            return  myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }


        /// <summary>
        /// Add a parameter to the SQL Query.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        //----------------------------------------------------------------------------------------------------------------------------
        public void addParameter(string name, string value)
        {
            if(value == null) cmd.Parameters.AddWithValue("@" + name, DBNull.Value);
            else cmd.Parameters.AddWithValue("@" + name, value);
        }


        /// <summary>
        /// Add a parameter to the SQL Query.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        //----------------------------------------------------------------------------------------------------------------------------
        public void addParameter(string name, int value)
        {
            if (value == -1) cmd.Parameters.AddWithValue("@" + name, DBNull.Value);
            else cmd.Parameters.AddWithValue("@" + name, value);
        }

        /// <summary>
        /// Delete all parameters.
        /// </summary>
        //----------------------------------------------------------------------------------------------------------------------------
        public void clearParameters()
        {
            cmd.Parameters.Clear();
        }


        /// <summary>
        /// Set the query to be used for executing the SQL statement.
        /// </summary>
        /// <param name="_query">The query to use for executing the SQL statement.</param>
        //----------------------------------------------------------------------------------------------------------------------------
        public void setQuery(string _query)
        {
            query = _query;
        }


        /// <summary>
        /// Execute an "Action" type SQL command such as Insert or Update or Delete.
        /// </summary>
        /// <returns>The number of records affected by the command.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public int executeSQL()        
        {            
            try
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnect;
                sqlConnect.Open();
                int numRows = cmd.ExecuteNonQuery();
                sqlConnect.Close();
                return numRows;
            }
            catch (Exception ex)
            {
                Log.write("SQL Error - " + ex.Message);
                Log.write("SQL Error Query - " + query);
                string str = "";
                foreach(SqlParameter sparam in cmd.Parameters)
                {
                    if(sparam.Value != null) str += sparam.ParameterName + " = " + sparam.Value.ToString() + "\n";
                    else str += sparam.ParameterName + " = NULL\n";
                }

                Log.write("SQL Error Params - " + str);
            }
            return 0;
        }


        /// <summary>
        /// Execute a scalar type of command such as a Select statement.
        /// </summary>
        /// <returns>The number of records returned by the query.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        // Returns the first row in a "Select" type query - returns an integer
        public int executeScalarSQL()
        {
            int retVal = -1;
            try
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnect;
                sqlConnect.Open();
                retVal = (Int32)cmd.ExecuteScalar();
                sqlConnect.Close();
            }
            catch (Exception ex)
            {
                Log.write("SQL Error - " + ex.Message);
            }
            return retVal;
        }


        /// <summary>
        /// Gets the value of a field from a table based on search criteria.  This is an MS Access style DLookup function.
        /// </summary>
        /// <param name="tableName">The name table to search.</param>
        /// <param name="fieldName">The field tht contains the value required.</param>
        /// <param name="criteria">The criteria that is used to find the correct row(s).</param>
        /// <returns>The value of a field from a table based on search criteria.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        // Implements an MS Access style DLookup function
        public string lookup(string tableName, string fieldName, string criteria)
        {
            string retVal = null;
            query = "SELECT " + fieldName + " FROM " + tableName + " WHERE " + criteria;
            try
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnect;
                sqlConnect.Open();
                retVal = Convert.ToString(cmd.ExecuteScalar());
                sqlConnect.Close();
            }
            catch (Exception ex)
            {
                Log.write("SQL Error - " + ex.Message);
                return "error";
            }

            return retVal;
        }


        /// <summary>
        /// Get a DataTable of all records that are returned from an SQL query.  This is the most commonly used of all interface funtions.
        /// </summary>
        /// <param name="query">The query to use to retrieve the data.</param>
        /// <returns>A DataTable of all records that are returned from an SQL query.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public DataTable getRecords(string query)
        {
            DataTable dataTable = new DataTable();
            this.query = query;

            try
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnect;
                sqlConnect.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);    // create data adapter                
                da.Fill(dataTable);                             // this will query your database and return the result to your datatable
                sqlConnect.Close();
                da.Dispose();
            }
            catch (Exception ex)
            {
                Log.write("SQL Error - " + ex.Message);
            }

            return dataTable;
        }


        /// <summary>
        /// Get a DataTable of all records that are returned from an SQL query.  This is the most commonly used of all interface funtions.
        /// </summary>
        /// <param name="tableName">The table name that contains the records.</param>
        /// <param name="criteria">The criteria to use.</param>
        /// <returns>A DataTable of all records that are returned from an SQL query.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public DataTable getRecords(string tableName, string criteria = "")
        {
            string query = "SELECT * FROM " + tableName;
            if (criteria != "")
            {
                query += " WHERE " + criteria;
            }

            return getRecords(query);
        }


        /// <summary>
        /// Build a "WHERE" clause to add to an SQL query.
        /// </summary>
        /// <param name="whereClause">The existing WHERE clause.</param>
        /// <param name="clauseToAdd">The clause to add to the existing WHERE clause.</param>
        /// <param name="isAnd">Optional: Set to True to make this additiona an "AND".  Set to false for "OR".</param>
        /// <returns>The updated WHERE clause.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public string addToWhereClause(string whereClause, string clauseToAdd, bool isAnd = true)
        {
            string newWhereClause;

            if (String.IsNullOrEmpty(whereClause))
            {
                newWhereClause = " WHERE " + clauseToAdd;
            }

            else
            {
                if (isAnd) newWhereClause = whereClause + " AND " + clauseToAdd;
                else newWhereClause = whereClause + " OR " + clauseToAdd;
            }

            return newWhereClause;
        }


        /// <summary>
        /// Get the maximum ID of a table.  This is used after inserting a new record to get the newly inserted records ID.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <returns>The maximum ID of a table.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public long getMaxId(string tableName)
        {
            DataTable t = getRecords("SELECT MAX(id) As MaxOfId FROM " + tableName);
            if (DBNull.Value.Equals(t.Rows[0]["MaxOfId"])) return 1;
            return Convert.ToInt64(t.Rows[0]["MaxOfId"]); 
        }


        /// <summary>
        /// Determine if records in a table contain a field with a specific value.
        /// </summary>
        /// <param name="tableName">The table to search.</param>
        /// <param name="fieldName">The field name that contains the target string.</param>
        /// <param name="targetString">The string to search for.</param>
        /// <returns>True if records in a table contain a field with a specific value.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool stringExists(string tableName, string fieldName, string targetString)
        {
            DataTable t = getRecords("SELECT COUNT(" + fieldName + ") AS countOfRecords" +
                                          " FROM " + tableName + 
                                          " WHERE " + fieldName + " = '" + targetString + "'");

            if (DBNull.Value.Equals(t.Rows[0]["countOfRecords"])) return false;
            if (Convert.ToInt64(t.Rows[0]["countOfRecords"]) == 0) return false;

            return true;
        }


        /// <summary>
        /// Determine if a primary key Id exists for a table.
        /// </summary>
        /// <param name="tableName">The table to search.</param>
        /// <param name="targetId">The primary key Id to find.</param>
        /// <returns>True if the primary key Id exists.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool idExists(string tableName, long targetId)
        {
            DataTable t = getRecords("SELECT COUNT(id) AS countOfRecords" +
                                     " FROM " + tableName +
                                     " WHERE id = " + targetId);

            if (DBNull.Value.Equals(t.Rows[0]["countOfRecords"])) return false;
            if (Convert.ToInt64(t.Rows[0]["countOfRecords"]) == 0) return false;

            return true;
        }


        /// <summary>
        /// Get a list of all tables in the database.
        /// </summary>
        /// <param name="filter">Optional: Any filters required,  Default = Null.</param>
        /// <returns>A list of all tables in the database.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public List<string> getTableList(string filter = null)
        {
            List<string> tableNames = new List<string>();

            string query = "SELECT * FROM sys.objects WHERE ([type] = 'U')";
            if (!String.IsNullOrEmpty(filter)) query += " AND " + filter;
            
            DataTable dataTable = getRecords(query);
            foreach (DataRow row in dataTable.Rows)
            {
                tableNames.Add(row["name"].ToString());
            }

            return tableNames;
        }


        //
        // Static Methods
        //

        /// <summary>
        /// Get a DataTable from a list of strings.
        /// </summary>
        /// <param name="list">The list of strings.</param>
        /// <param name="columnName">The name of the column in the DataTable.</param>
        /// <returns>A DataTable formed from a list of strings.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static DataTable listToDataTable(List<string> list, string columnName)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(columnName, typeof(string)));
            table.Rows.Add(list.ToArray());
            return table;
        }


        /// <summary>
        /// Get a DataTable from a list of (Int64) numbers.
        /// </summary>
        /// <param name="list">The list of strings.</param>
        /// <param name="columnName">The name of the column in the DataTable.</param>
        /// <returns>A DataTable formed from a list of (Int64) numbers.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public static DataTable listToDataTable(List<long> list, string columnName)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(columnName, typeof(long)));
            
            foreach(long item in list)
            {
                DataRow row = table.NewRow();
                row[columnName] = item;
                table.Rows.Add(row);
            }
            return table;
        }

    }
}
