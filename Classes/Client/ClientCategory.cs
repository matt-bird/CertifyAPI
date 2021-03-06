﻿using System;
using System.Collections.Generic;
using System.Data;

using CertifyWPF.WPF_Library;
using CertifyWPF.WPF_Utils;


namespace CertifyWPF.WPF_Client
{
    /// <summary>
    /// Client Category.  Client Categories are operational catgories such as "Producer - Dairy" or "Viticulture".  Client 
    /// Categories are stored in the <strong>clientCategory</strong> table.  Client Categories are used to filter web 
    /// application questions and checklist questions.
    /// </summary>
    public class ClientCategory
    {
        /// <summary>
        /// The primary key Id of the Client Category.
        /// </summary>
        public long id { get; set; }
        
        /// <summary>
        /// The primary key Id of the Client who owns this category.
        /// </summary>
        public long clientId { get; set; }

        /// <summary>The category.  Client categories such as "Produce - Livestock" are defined in 
        /// the <strong>list_clientCategory</strong> table.</summary>
        public string category { get; set; }


        /// <summary>
        /// Constructor.
        /// </summary>
        //----------------------------------------------------------------------------------------------------------------------------
        public ClientCategory()
        {
            id = -1;
            clientId = -1;
            category = null;
        }


        /// <summary>
        /// Constructor for an existing Client Category.
        /// </summary>
        /// <param name="_id">The primary key Id of the Client Category.</param>
        //----------------------------------------------------------------------------------------------------------------------------
        public ClientCategory(long _id)
        {
            id = _id;
            clientId = -1;
            category = null;

            fetch();
        }


        /// <summary>
        /// Fetch an existing Client Category record from the database using the primary key Id of the Client Category.
        /// </summary>
        /// <returns>True if the Client Category was found.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool fetch()
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM clientCategory WHERE isDeleted = 0 AND id = @id");

            if (records.Rows.Count == 1)
            {
                DataRow row = records.Rows[0];
                clientId = Convert.ToInt64(row["clientId"].ToString());

                long list_clientCategoryId = Convert.ToInt64(row["list_clientCategoryId"].ToString());
                category = UtilsList.getClientCategoryName(list_clientCategoryId);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Save the Client Category record to the database.  This is an Upsert operation.
        /// </summary>
        /// <returns>True if the Upsert was successful.  False otherwise.</returns>
        //----------------------------------------------------------------------------------------------------------------------------
        public bool save()
        {
            if (clientId == -1 || String.IsNullOrEmpty(category)) return false;
            long list_clientCategoryId = UtilsList.getClientCategoryId(category);

            SQL mySql = new SQL();
            mySql.addParameter("clientId", clientId.ToString());           
            mySql.addParameter("list_clientCategoryId",  list_clientCategoryId.ToString());

            // save
            if (id == -1)
            {
                // Make sure this category does not already exist for the client
                if (!exists(list_clientCategoryId))
                {
                    mySql.setQuery("INSERT INTO clientCategory (clientId, list_clientCategoryId) VALUES (@clientId, @list_clientCategoryId)");
                    if (mySql.executeSQL() == 1)
                    {
                        id = mySql.getMaxId("clientCategory");
                        return true;
                    }
                    else Log.write("An error ocurred while trying to save the client category.");
                }
            }

            // Update
            else
            {
                mySql.addParameter("id", id.ToString());
                mySql.setQuery(@"UPDATE clientCategory 
                                 SET 
                                 clientId = @clientId,
                                 list_clientCategoryId = @list_clientCategoryId
                                 WHERE id = @id");

                if (mySql.executeSQL() == 1) return true;
                else Log.write("An error ocurred while trying to update the client category.");
            }

            return false;
        }


        /// <summary>
        /// Determine if the Client already has a Category.
        /// </summary>
        /// <param name="categoryName">The Category name to search.</param>
        /// <returns>True if the Client has the Category.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool exists(string categoryName)
        {
            long categoryId = UtilsList.getClientCategoryId(categoryName);
            return exists(categoryId);
        }


        /// <summary>
        /// Determine if the Client already has a Category.
        /// </summary>
        /// <param name="categoryId">The primary key Id of the Client Category.</param>
        /// <returns>True if the Client has the Category.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool exists(long categoryId)
        {
            if (clientId == -1) return false;

            if (categoryId != -1)
            {
                SQL mySql = new SQL();
                mySql.addParameter("clientId", clientId.ToString());
                mySql.addParameter("list_clientCategoryId", categoryId.ToString());
                DataTable records = mySql.getRecords(@"SELECT * FROM clientCategory 
                                                       WHERE 
                                                       clientId = @clientId AND 
                                                       list_clientCategoryId = @list_clientCategoryId AND
                                                       isDeleted = 0");
                if (records.Rows.Count > 0) return true;
            }

            return false;
        }


        /// <summary>
        /// Mark a Client Category record as deleted.
        /// </summary>
        /// <returns>True if the Client Category was marked as deleted.  False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public bool delete()
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            mySql.setQuery("UPDATE clientCategory SET isDeleted = 1 WHERE id = @id"); // We only mark it as deleted
            if (mySql.executeSQL() == 1) return true;
            return false;
        }


        /// <summary>
        /// Get the Client Address use that is associated with this Client Category.  This is stored in a cross reference database table
        /// <strong>clientCategoryToClientAddressUse</strong>
        /// </summary>
        /// <returns>The primary key IDs of the Client Address uses that are associated with this Client Category</returns>
        ///--------------------------------------------------------------------------------------------------------------------------
        public List<string> getUses()
        {
            List<string> uses = new List<string>();

            SQL mySql = new SQL();
            mySql.addParameter("categoryName", category);

            DataTable records = mySql.getRecords(@"SELECT        
                                                   clientCategoryToClientAddressUse.id, 
                                                   list_clientCategory.name AS category, 
                                                   list_clientAddressUse.name AS addressUse
                                                   FROM            
                                                   clientCategoryToClientAddressUse INNER JOIN 
                                                   list_clientCategory ON clientCategoryToClientAddressUse.list_clientCategoryId = list_clientCategory.id INNER JOIN
                                                   list_clientAddressUse ON clientCategoryToClientAddressUse.list_clientAddressUseId = list_clientAddressUse.id 
                                                   WHERE 
                                                   list_clientCategory.name = @categoryName");
            foreach(DataRow row in records.Rows)
            {
                uses.Add(row["addressUse"].ToString());
            }
            return uses;
        }


        /// <summary>
        /// Get a list of client category strings.
        /// </summary>
        /// <returns>A list of client category strings.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static List<string> getList()
        {
            List<string> list = new List<string>();
            SQL mySql = new SQL();
            DataTable records = mySql.getRecords("SELECT * FROM list_ClientCategory ORDER BY name");
            foreach (DataRow row in records.Rows) list.Add(row["name"].ToString());
            return list;
        }
    }
}
