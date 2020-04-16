using System;
using System.Data;

using CertifyWPF.WPF_Library;

namespace CertifyWPF.WPF_Utils
{
    /// <summary>
    /// A Series of static functions that allow interaction with the various "list" tables of Certify.  Most of these functions 
    /// will convert from a primary key Id to a name, or from a name to primary key Id.
    /// </summary>


    public class UtilsList
    {

        //
        // Static Methods
        //
        //----------------------------------------------------------------------------------------------------------------------------
        public static long getStateId(string stateName)
        {
            SQL mySql = new SQL();
            mySql.addParameter("name", stateName);
            DataTable records = mySql.getRecords("SELECT id FROM list_state WHERE name = @name");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static string getStateName(long id, bool useAbbreviation = false)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM list_state WHERE id = @id");
            if (records.Rows.Count == 1)
            {
                if(useAbbreviation) return records.Rows[0]["abbreviation"].ToString();
                else return records.Rows[0]["name"].ToString();
            }
            return null;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static long getCountryId(string country)
        {
            SQL mySql = new SQL();
            mySql.addParameter("name", country);
            DataTable records = mySql.getRecords("SELECT id FROM list_country WHERE name = @name");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static string getCountryName(long id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString()); ;
            DataTable records = mySql.getRecords("SELECT name FROM list_country WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["name"].ToString();
            return null;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static long getServiceId(string name)
        {
            SQL mySql = new SQL();
            mySql.addParameter("name", name);
            DataTable records = mySql.getRecords("SELECT id FROM service WHERE name = @name");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static long getServiceStatusId(string name)
        {
            SQL mySql = new SQL();
            mySql.addParameter("name", name);
            DataTable records = mySql.getRecords("SELECT id FROM serviceStatus WHERE name = @name");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static string getServiceStatus(long id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT name FROM serviceStatus WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["name"].ToString();
            return null;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static string getClientCategoryName(long inClientCategoryId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", inClientCategoryId.ToString());
            DataTable records = mySql.getRecords("SELECT name FROM list_clientCategory WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["name"].ToString();
            return null;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static long getClientCategoryId(string name)
        {
            SQL mySql = new SQL();
            mySql.addParameter("name", name);
            DataTable records = mySql.getRecords("SELECT id FROM list_clientCategory WHERE name = @name");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1; 
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static long getContactTypeId(string type)
        {
            SQL mySql = new SQL();
            mySql.addParameter("type", type);
            DataTable records = mySql.getRecords("SELECT id FROM list_contactType WHERE type = @type");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static string getContactType(long _typeId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", _typeId.ToString());
            DataTable records = mySql.getRecords("SELECT type FROM list_contactType WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["type"].ToString();
            return null;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static long getInvoiceItemTypeId(string type)
        {
            SQL mySql = new SQL();
            mySql.addParameter("type", type);
            DataTable records = mySql.getRecords("SELECT id FROM list_invoiceItemType WHERE type = @type");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static string getInvoiceItemType(long _typeId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", _typeId.ToString());
            DataTable records = mySql.getRecords("SELECT type FROM list_invoiceItemType WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["type"].ToString();
            return null;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static string getChecklistTypeName(long _typeId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", _typeId.ToString());
            DataTable records = mySql.getRecords("SELECT name FROM list_checklistType WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["name"].ToString();
            return null;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static long getChecklistTypeId(string name)
        {
            SQL mySql = new SQL();
            mySql.addParameter("name", name);
            DataTable records = mySql.getRecords("SELECT id FROM list_checklistType WHERE name = @name");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static string getChecklistTypeAbbreviation(long _typeId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", _typeId.ToString());
            DataTable records = mySql.getRecords("SELECT abbreviation FROM list_checklistType WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["abbreviation"].ToString();
            return null;
        }

        //----------------------------------------------------------------------------------------------------------------------------
        public static long getChecklistStatusId(string name)
        {
            SQL mySql = new SQL();
            mySql.addParameter("name", name);
            DataTable records = mySql.getRecords("SELECT id FROM list_checklistStatus WHERE name = @name");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static string getChecklistStatusName(long id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT name FROM list_checklistStatus WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["name"].ToString();
            return null;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static long getEmailTypeId(string typeName)
        {
            SQL mySql = new SQL();
            mySql.addParameter("type", typeName);
            DataTable records = mySql.getRecords("SELECT id FROM list_emailType WHERE type = @type");
            return Convert.ToInt64(records.Rows[0]["id"].ToString());
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static string getEmailType(long id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT type FROM list_emailType WHERE id = @id");
            return records.Rows[0]["type"].ToString();
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static long getAuditStatusId(string status)
        {
            SQL mySql = new SQL();
            mySql.addParameter("name", status);
            DataTable records = mySql.getRecords("SELECT id FROM list_auditStatus WHERE name = @name");
            if(records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static string getAuditStatus(long id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT name FROM list_auditStatus WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["name"].ToString();
            return null;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static int getAuditStage(long statusId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", statusId.ToString());
            DataTable records = mySql.getRecords("SELECT stage FROM list_auditStatus WHERE id = @id");
            if (records.Rows.Count == 1) return Convert.ToInt32(records.Rows[0]["stage"].ToString());
            return -1;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static int getAuditStage(string name)
        {
            SQL mySql = new SQL();
            mySql.addParameter("name", name);
            DataTable records = mySql.getRecords("SELECT stage FROM list_auditStatus WHERE name = @name");
            if (records.Rows.Count == 1) return Convert.ToInt32(records.Rows[0]["stage"].ToString());
            return -1;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static string getAuditType(long id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT name FROM list_auditType WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["name"].ToString();
            return null;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static long getAuditTypeId(string name)
        {
            SQL mySql = new SQL();
            mySql.addParameter("name", name.ToString());
            DataTable records = mySql.getRecords("SELECT id FROM list_auditType WHERE name = @name");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static string getAuditExpenseType(long id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT type FROM list_auditExpenseType WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["type"].ToString();
            return null;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static long getAuditExpenseTypeId(string type)
        {
            SQL mySql = new SQL();
            mySql.addParameter("type", type);
            DataTable records = mySql.getRecords("SELECT id FROM list_auditExpenseType WHERE type = @type");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //----------------------------------------------------------------------------------------------------------------------------
        public static long getCARStatusId(string status)
        {
            SQL mySql = new SQL();
            string id = mySql.lookup("list_CARStatus", "id", "status = '" + status + "'");
            return Convert.ToInt64(id);
        }

        //----------------------------------------------------------------------------------------------------------------------------
        public static string getCARStatusName(long id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT status FROM list_CARStatus WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["status"].ToString();
            return null;
        }


        //--------------------------------------------------------------------------------------------------------------------------        
        public static long getCARSourceId(string source)
        {
            SQL mySql = new SQL();
            string id = mySql.lookup("list_CARSource", "id", "source = '" + source + "'");
            return Convert.ToInt64(id);
        }


        //--------------------------------------------------------------------------------------------------------------------------        
        public static string getCARSourceName(long sourceId)
        {
            SQL mySql = new SQL();
            return mySql.lookup("list_CARSource", "source", "id = " + sourceId.ToString());
        }


        //--------------------------------------------------------------------------------------------------------------------------        
        public static long getCARSeverityId(string severity)
        {
            SQL mySql = new SQL();

            // Backwards compatability - There used to be an "Opportunity for Improvement" severity - this should now always be "Observation"
            if (severity == "Opportunity for Improvement") severity = "Observation";
            string id = mySql.lookup("list_CARSeverity", "id", "name = '" + severity + "'");

            if (!String.IsNullOrEmpty(id)) return Convert.ToInt64(id);
            else return -1;
        }


        //--------------------------------------------------------------------------------------------------------------------------        
        public static string getCARSeverityName(long severityId)
        {
            SQL mySql = new SQL();
            return mySql.lookup("list_CARSeverity", "name", "id = " + severityId.ToString());
        }


        //--------------------------------------------------------------------------------------------------------------------------    
        public static long getCertifiedItemTypeId(string inName)
        {
            SQL mySql = new SQL();
            mySql.addParameter("name", inName);
            DataTable records = mySql.getRecords("SELECT id FROM list_certifiedItemType WHERE name = @name");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //--------------------------------------------------------------------------------------------------------------------------    
        public static long getCertifiedItemModifierId(string modifier)
        {
            SQL mySql = new SQL();
            mySql.addParameter("name", modifier);
            DataTable records = mySql.getRecords("SELECT id FROM list_certifiedItemModifer WHERE name = @name");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //--------------------------------------------------------------------------------------------------------------------------    
        public static string getCertifiedItemModifierName(long modifierId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", modifierId.ToString());
            DataTable records = mySql.getRecords("SELECT name FROM list_certifiedItemModifer WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["name"].ToString();
            return null;
        }


        //--------------------------------------------------------------------------------------------------------------------------    
        public static long getClientAddressModifierId(string modifier)
        {
            SQL mySql = new SQL();
            mySql.addParameter("name", modifier);
            DataTable records = mySql.getRecords("SELECT id FROM list_clientAddressModifier WHERE name = @name");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //--------------------------------------------------------------------------------------------------------------------------    
        public static string getClientAddressModifierName(long modifierId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", modifierId.ToString());
            DataTable records = mySql.getRecords("SELECT name FROM list_clientAddressModifier WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["name"].ToString();
            return null;
        }

        //--------------------------------------------------------------------------------------------------------------------------
        public static string getCertifiedItemTypeName(long inId)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", inId.ToString());
            DataTable records = mySql.getRecords("SELECT name FROM list_certifiedItemType WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["name"].ToString();
            return null;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static long getCertificationBodyId(string shortName)
        {
            SQL mySql = new SQL();
            mySql.addParameter("shortName", shortName);
            DataTable records = mySql.getRecords("SELECT id FROM certificationBodies WHERE shortName = @shortName");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static string getCertificationBodyName(long id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT shortName FROM certificationBodies WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["shortName"].ToString();
            return null;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static string getWorkFlowName(long id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT name FROM list_appWorkFlow WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["name"].ToString();
            return null;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static string getWorkFlowName(int stage)
        {
            SQL mySql = new SQL();
            mySql.addParameter("stage", stage.ToString());
            DataTable records = mySql.getRecords("SELECT name FROM list_appWorkFlow WHERE stage = @stage");
            if (records.Rows.Count == 1) return records.Rows[0]["name"].ToString();
            return null;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static int getWorkFlowStage(long id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT stage FROM list_appWorkFlow WHERE id = @id");
            if (records.Rows.Count == 1) return Convert.ToInt32(records.Rows[0]["stage"].ToString());
            return -1;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static int getWorkFlowStage(string name)
        {
            SQL mySql = new SQL();
            mySql.addParameter("name", name.ToString());
            DataTable records = mySql.getRecords("SELECT stage FROM list_appWorkFlow WHERE name = @name");
            if (records.Rows.Count == 1) return Convert.ToInt32(records.Rows[0]["stage"].ToString());
            return -1;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static long getWorkFlowId(int stage)
        {
            SQL mySql = new SQL();
            mySql.addParameter("stage", stage.ToString());
            DataTable records = mySql.getRecords("SELECT id FROM list_appWorkFlow WHERE stage = @stage");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static string getWorkFlowDescription(long id, string applicationType)
        {
            string columnName = null;
            if (applicationType == "Organic Application") columnName = "description";
            if (applicationType == "Organic Export") columnName = "descriptionExport";
            if (!String.IsNullOrEmpty(columnName))
            {
                SQL mySql = new SQL();
                mySql.addParameter("id", id.ToString());
                DataTable records = mySql.getRecords("SELECT * FROM list_appWorkFlow WHERE id = @id");
                if (records.Rows.Count == 1) return records.Rows[0][columnName].ToString();
            }
            return null;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static long getWorkFlowId(string name)
        {
            SQL mySql = new SQL();
            mySql.addParameter("name", name);
            DataTable records = mySql.getRecords("SELECT id FROM list_appWorkFlow WHERE name = @name");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static long getTestId(string name)
        {
            SQL mySql = new SQL();
            mySql.addParameter("testName", name);
            DataTable records = mySql.getRecords("SELECT id FROM test WHERE testName = @testName");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static string getApplicationQuestionControlTypeName(long id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT name FROM list_appFormControlType WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["name"].ToString();
            return null;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static long getApplicationQuestionControlTypeId(string name)
        {
            SQL mySql = new SQL();
            mySql.addParameter("name", name.ToString());
            DataTable records = mySql.getRecords("SELECT id FROM list_appFormControlType WHERE name = @name");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static string getApplicationQuestionValidationTypeName(long id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT type FROM list_appFormValidationType WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["type"].ToString();
            return null;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static long getApplicationQuestionValidationTypeId(string type)
        {
            SQL mySql = new SQL();
            mySql.addParameter("type", type.ToString());
            DataTable records = mySql.getRecords("SELECT id FROM list_appFormValidationType WHERE type = @type");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static long getApplicationTypeId(string type)
        {
            SQL mySql = new SQL();
            mySql.addParameter("type", type);
            DataTable records = mySql.getRecords("SELECT id FROM list_appFormType WHERE type = @type");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static string getApplicationTypeName(long id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT type FROM list_appFormType WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["type"].ToString();
            return null;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static long getPaymentTypeId(string name)
        {
            SQL mySql = new SQL();
            mySql.addParameter("type", name);
            DataTable records = mySql.getRecords("SELECT id FROM list_paymentType WHERE type = @type");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static string getPaymentTypeName(long id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT type FROM list_paymentType WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["type"].ToString();
            return null;
        }

        //--------------------------------------------------------------------------------------------------------------------------
        public static long getRiskCategoryId(string name)
        {
            SQL mySql = new SQL();
            mySql.addParameter("title", name);
            DataTable records = mySql.getRecords("SELECT id FROM riskCategory WHERE title = @title");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static string getRiskCategoryName(long id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT title FROM riskCategory WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["title"].ToString();
            return null;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static long getRiskAssessmentId(string name)
        {
            SQL mySql = new SQL();
            mySql.addParameter("name", name);
            DataTable records = mySql.getRecords("SELECT id FROM list_riskAssessment WHERE name = @name");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static string getRiskAssessmentName(long id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT name FROM list_riskAssessment WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["name"].ToString();
            return null;
        }        
        
        //--------------------------------------------------------------------------------------------------------------------------
        public static string getDerogationType(long id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT type FROM list_derogationType WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["type"].ToString();
            return null;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static string getTestSampleType(long id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT type FROM list_testSampleType WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["type"].ToString();
            return null;
        }

        //--------------------------------------------------------------------------------------------------------------------------
        public static long getTestSampleTypeId(string name)
        {
            SQL mySql = new SQL();
            mySql.addParameter("type", name);
            DataTable records = mySql.getRecords("SELECT id FROM list_testSampleType WHERE type = @type");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        public static string getTestSource(long id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT name FROM list_testSource WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["name"].ToString();
            return null;
        }

        //--------------------------------------------------------------------------------------------------------------------------
        public static long getTestSourceId(string name)
        {
            SQL mySql = new SQL();
            mySql.addParameter("name", name);
            DataTable records = mySql.getRecords("SELECT id FROM list_testSource WHERE name = @name");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }

        //--------------------------------------------------------------------------------------------------------------------------
        public static string getClientAddressUse(long id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT name FROM list_clientAddressUse WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["name"].ToString();
            return null;
        }

        //--------------------------------------------------------------------------------------------------------------------------
        public static long getClientAddressUseId(string name)
        {
            SQL mySql = new SQL();
            mySql.addParameter("name", name);
            DataTable records = mySql.getRecords("SELECT id FROM list_clientAddressUse WHERE name = @name");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }

        //--------------------------------------------------------------------------------------------------------------------------
        public static string getClientUpdateType(long id)
        {
            SQL mySql = new SQL();
            mySql.addParameter("id", id.ToString());
            DataTable records = mySql.getRecords("SELECT type FROM list_clientUpdateType WHERE id = @id");
            if (records.Rows.Count == 1) return records.Rows[0]["type"].ToString();
            return null;
        }

        //--------------------------------------------------------------------------------------------------------------------------
        public static long getClientUpdteTypeId(string type)
        {
            SQL mySql = new SQL();
            mySql.addParameter("type", type);
            DataTable records = mySql.getRecords("SELECT id FROM list_clientUpdateType WHERE type = @type");
            if (records.Rows.Count == 1) return Convert.ToInt64(records.Rows[0]["id"].ToString());
            return -1;
        }
    }
}
