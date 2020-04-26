
namespace CertifyWPF.WPF_Status
{
    /// <summary>
    /// This class represents some "Status" of the Certify systems health - such as Overdue CAR's or Unread emails.
    /// </summary>
    
    public class Status
    {
        /// <summary>
        /// A count of the number of items that have this status.  For example, if there are 2 unread emails, then this will be 2.
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// The particular name of this status - such as "Unread Emails" or "Overdue CAR's".
        /// </summary>
        public string name { get; set; }


        /// <summary>
        /// Constructor.
        /// </summary>
        //-------------------------------------------------------------------------------------------------------------
        public Status()
        {
            count = 0;
            name = null;
        }

    }
}
