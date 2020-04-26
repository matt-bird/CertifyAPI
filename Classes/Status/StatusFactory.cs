using CertifyWPF.WPF_Library;
using System.Collections.Generic;
using System.Data;

namespace CertifyWPF.WPF_Status
{
    public class StatusFactory
    {
        public enum StatusType 
        {
            NEW_PRODUCT, 
            NEW_INGREDIENT, 
            NEW_RECIPE, 
            NEW_LABEL, 
            NEW_APPLICATION, 
            STALE_APPLICATION, 
            APPLICATION_READY_FOR_DECISION,
            DEROGATION,
            INSPECTED_AUDIT,
            AUDIT_WARNING,
            REJECTED_AUDIT,
            OVERDUE_CAR,
            ENDING_RESTRICTION,
            OVERDUE_QUALITY_CAR,
            CERTIFICATE_WARNING,
            BUSINESS_DEVELOPMENT_REMINDER,
            CLIENT_UPDATE
        };


        //-------------------------------------------------------------------------------------------------------------
        public static Status getStatus(StatusType type)
        {
            if (type == StatusType.NEW_PRODUCT) return getNewProducts();
            if (type == StatusType.NEW_INGREDIENT) return getNewIngredients();
            if (type == StatusType.NEW_RECIPE) return getNewRecipes();
            if (type == StatusType.NEW_LABEL) return getNewLabels();
            if (type == StatusType.NEW_APPLICATION) return getNewApplications();
            if (type == StatusType.STALE_APPLICATION) return getStaleApplications();
            if (type == StatusType.APPLICATION_READY_FOR_DECISION) return getApplicationsReadyForDecision();
            if (type == StatusType.DEROGATION) return getDerogations();
            if (type == StatusType.INSPECTED_AUDIT) return getInspectedAudits();
            if (type == StatusType.AUDIT_WARNING) return getAuditWarnings();
            if (type == StatusType.REJECTED_AUDIT) return getRejectedAudits();
            if (type == StatusType.OVERDUE_CAR) return getOverdueCARs();
            if (type == StatusType.ENDING_RESTRICTION) return getEndingRestrictions();
            if (type == StatusType.OVERDUE_QUALITY_CAR) return getOverdueQualityCARs();
            if (type == StatusType.CERTIFICATE_WARNING) return getCertificateWarnings();
            if (type == StatusType.BUSINESS_DEVELOPMENT_REMINDER) return getBDReminders();
            if (type == StatusType.CLIENT_UPDATE) return getClientUpdates();
            return null;
        }


        //-------------------------------------------------------------------------------------------------------------
        public static List<Status> getStatuses()
        {
            List<Status> list = new List<Status>();
            list.Add(getNewProducts());
            list.Add(getNewIngredients());
            list.Add(getNewRecipes());
            list.Add(getNewLabels());
            list.Add(getNewApplications());
            list.Add(getStaleApplications());
            list.Add(getApplicationsReadyForDecision());
            list.Add(getDerogations());
            list.Add(getInspectedAudits());
            list.Add(getAuditWarnings());
            list.Add(getRejectedAudits());
            list.Add(getOverdueCARs());
            list.Add(getEndingRestrictions());
            list.Add(getOverdueQualityCARs());
            list.Add(getCertificateWarnings());
            list.Add(getBDReminders());
            list.Add(getClientUpdates());

            return list;
        }


        //-------------------------------------------------------------------------------------------------------------
        public static Status getClientUpdates()
        {
            SQL mySql = new SQL();
            DataTable records = mySql.getRecords(@"SELECT id
                                                   FROM clientUpdate
                                                   WHERE 
                                                   processedDateTime IS NULL AND 
                                                   isDeleted = 0");

            return getStatus(records.Rows.Count, "Client Update");
        }


        //-------------------------------------------------------------------------------------------------------------
        public static Status getBDReminders()
        {
            SQL mySql = new SQL();
            DataTable records = mySql.getRecords(@"SELECT * 
                                                   FROM businessDevelopment
                                                   WHERE 
                                                   isDeleted = 0 AND 
                                                   reminderDate IS NOT NULL AND 
                                                   reminderDate <= GETDATE()");

            return getStatus(records.Rows.Count, "BD Reminder");
        }


        //-------------------------------------------------------------------------------------------------------------
        public static Status getCertificateWarnings()
        {
            SQL mySql = new SQL();
            DataTable records = mySql.getRecords("SELECT * FROM vw_clientCertificate_Expiring WHERE expiryStatus <> 'OK'");

            return getStatus(records.Rows.Count, "Cert Warning");
        }


        //-------------------------------------------------------------------------------------------------------------
        public static Status getOverdueQualityCARs()
        {
            SQL mySql = new SQL();
            DataTable records = mySql.getRecords("SELECT * FROM vw_qualityCARs WHERE Status = 'Overdue' AND isClosed = 0");

            return getStatus(records.Rows.Count, "Quality CAR");
        }


        //-------------------------------------------------------------------------------------------------------------
        public static Status getEndingRestrictions()
        {
            SQL mySql = new SQL();
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

            return getStatus(records.Rows.Count, "Restriction End");
        }


        //-------------------------------------------------------------------------------------------------------------
        public static Status getOverdueCARs()
        {
            SQL mySql = new SQL();
            DataTable records = mySql.getRecords(@"SELECT *
                                                   FROM vw_CARs
                                                   WHERE 
                                                   isDeleted = 0 AND
                                                   currentStatus <> 'Closed' AND
                                                   isResolved = 0 AND
                                                   dueDate IS NOT NULL AND     
                                                   dueDate <= getDate() AND 
                                                   company <> 'Test Client'");

            return getStatus(records.Rows.Count, "Overdue CAR");
        }


        //-------------------------------------------------------------------------------------------------------------
        public static Status getRejectedAudits()
        {
            SQL mySql = new SQL();
            DataTable records = mySql.getRecords(@"SELECT id
                                                   FROM vw_audits
                                                   WHERE 
                                                   isDeleted = 0 AND
                                                   status = 'Rejected'");

            return getStatus(records.Rows.Count, "Rejected Audit");
        }


        //-------------------------------------------------------------------------------------------------------------
        public static Status getAuditWarnings()
        {
            SQL mySql = new SQL();
            DataTable records = mySql.getRecords(@"SELECT id
                                                   FROM vw_audits
                                                   WHERE 
                                                   isDeleted = 0 AND
                                                   datePlanned IS NULL AND
                                                   dateDue <= @dateDue AND 
                                                   isTest = 0 AND
                                                   (status = 'Created' OR 
                                                    status = 'Allocated' OR 
                                                    status = 'Rejected')");

            return getStatus(records.Rows.Count, "Audit Warning");
        }



        //-------------------------------------------------------------------------------------------------------------
        public static Status getInspectedAudits()
        {
            SQL mySql = new SQL();
            DataTable records = mySql.getRecords(@"SELECT id
                                                   FROM vw_audits
                                                   WHERE status = 'Inspected' AND
                                                   company <> 'Test Client'");

            return getStatus(records.Rows.Count, "Inspected Audit");
        }


        //-------------------------------------------------------------------------------------------------------------
        public static Status getDerogations()
        {
            SQL mySql = new SQL();
            DataTable records = mySql.getRecords(@"SELECT id
                                                   FROM vw_derogations
                                                   WHERE 
                                                   isDeleted = 0 AND
                                                   decision IS NULL");

            return getStatus(records.Rows.Count, "New Derogation");
        }


        //-------------------------------------------------------------------------------------------------------------
        public static Status getApplicationsReadyForDecision()
        {
            SQL mySql = new SQL();
            DataTable records = mySql.getRecords(@"SELECT id
                                                    FROM vw_applications
                                                    WHERE 
                                                    isDeleted = 0 AND
                                                    workFlow = 'Ready for Decision' AND
                                                    company IS NOT NULL AND
                                                    isTestCompany = 0");
            return getStatus(records.Rows.Count, "Decision");
        }


        //-------------------------------------------------------------------------------------------------------------
        public static Status getStaleApplications()
        {
            SQL mySql = new SQL();
            DataTable records = mySql.getRecords(@"SELECT * FROM vw_web_applicationStaleCheck");
            return getStatus(records.Rows.Count, "Stale Applications");
        }


        //-------------------------------------------------------------------------------------------------------------
        public static Status getNewApplications()
        {
            SQL mySql = new SQL();
            DataTable records = mySql.getRecords(@"SELECT id
                                                   FROM vw_applications
                                                   WHERE 
                                                   isDeleted = 0 AND
                                                   workFlow = 'Ready for Initial Review' AND
                                                   company IS NULL AND
                                                   (isTestCompany = 0 OR isTestCompany IS NULL)");
            return getStatus(records.Rows.Count, "New Application");
        }


        //-------------------------------------------------------------------------------------------------------------
        public static Status getNewProducts()
        {
            SQL mySql = new SQL();
            DataTable records = mySql.getRecords(@"SELECT *
                                                   FROM vw_certifiedItems
                                                   WHERE status = 'Applied' AND 
                                                   certificateNumber IS NOT NULL AND
                                                   certificateNumber <> '' AND
                                                   isDeleted = 0");
            return getStatus(records.Rows.Count, "New Product");
        }


        //-------------------------------------------------------------------------------------------------------------
        public static Status getNewIngredients()
        {
            SQL mySql = new SQL();
            DataTable records = mySql.getRecords(@"SELECT id
                                                   FROM ingredient
                                                   WHERE dateTimeAssessed IS NULL
                                                   GROUP BY id");
            return getStatus(records.Rows.Count, "New Ingredient");
        }


        //-------------------------------------------------------------------------------------------------------------
        public static Status getNewRecipes()
        {
            SQL mySql = new SQL();
            DataTable records = mySql.getRecords(@"SELECT id
                                                   FROM certifiedOrganicRecipe
                                                   WHERE 
                                                   approvalDateTime IS NULL
                                                   AND isDeleted = 0");
            return getStatus(records.Rows.Count, "New Recipe");
        }


        //-------------------------------------------------------------------------------------------------------------
        public static Status getNewLabels()
        {
            SQL mySql = new SQL();
            DataTable records = mySql.getRecords(@"SELECT id
                                                   FROM vw_labels
                                                   WHERE status = 'Applied' AND 
                                                   decisionDateTime IS NULL");
            return getStatus(records.Rows.Count, "New Label");
        }


        /// <summary>
        /// We have a non-zero or new status we need to add to our on screen display.
        /// </summary>
        /// <param name="_count">The number of items with this status.</param>
        /// <param name="_name">The name for this status - such as "New Emails" or "Overdue CAR".</param>
        //-------------------------------------------------------------------------------------------------------------
        public static Status getStatus(int _count, string _name)
        {
            string newName;
            if (_count == 1) newName = _count.ToString() + " " + _name;
            else newName = _count.ToString() + " " + _name + "s";

            return new Status
            {
                count = _count,
                name = newName
            };
        }
    }
}