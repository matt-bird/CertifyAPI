
using CertifyWPF.WPF_Library;
using CertifyWPF.WPF_Utils;
using System.Collections.Generic;
using System.Data;

namespace CertifyWPF.WPF_Application
{
    /// <summary>
    /// Web Application responses view.
    /// </summary>
    public class WebApplicationResponseView
    {
        /// <summary>
        /// The primary key for the web application response.
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// The primary key id of the Web Appication this response is for.
        /// </summary>
        public long web_applicationId { get; set; }
        
        /// <summary>
        /// The title of the section.
        /// </summary>
        public string section { get; set; }

        /// <summary>
        /// The sub-title of the section.
        /// </summary>
        public string sectionSubTitle { get; set; }

        /// <summary>
        /// The question.
        /// </summary>
        public string question { get; set; }

        /// <summary>
        /// The primary key id of the application form question
        /// </summary>
        public long appFormQuestionId { get; set; }

        /// <summary>
        /// The response.
        /// </summary>
        public string response { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public WebApplicationResponseView()
        {

        }

        /// <summary>
        /// Retrieves the responses from the application.
        /// </summary>
        /// <returns>True if the fetch was successful. False otherwise.</returns>
        //--------------------------------------------------------------------------------------------------------------------------
        public static List<WebApplicationResponseView> fetchResponses(long webApplicationId)
        {
            List<WebApplicationResponseView> responses = new List<WebApplicationResponseView>();

            // Get the application information
            SQL mySql = new SQL();
            mySql.addParameter("web_applicationId", webApplicationId.ToString());
            DataTable records = mySql.getRecords("SELECT * FROM vw_applicationResponses WHERE web_applicationId = @web_applicationId");

            if (records.Rows.Count >= 1)
            {
                foreach (DataRow row in records.Rows)
                {
                    WebApplicationResponseView resp = new WebApplicationResponseView
                    {
                        id = Utils.getLongFromString(row["id"].ToString()),
                        web_applicationId = Utils.getLongFromString(row["web_applicationId"].ToString()),
                        appFormQuestionId = Utils.getLongFromString(row["appFormQuestionId"].ToString()),
                        question = row["question"].ToString(),
                        response = row["response"].ToString(),
                        section = row["section"].ToString(),
                        sectionSubTitle = row["sectionSubTitle"].ToString()
                    };
                    responses.Add(resp);
                }
            }
            return responses;
        }
    }
}
