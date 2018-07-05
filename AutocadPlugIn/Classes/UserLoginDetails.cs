using System;
using System.Collections.Generic;
using System.Text;

namespace RedBracketConnector
{
    public class UserLoginDetails
    {
        /// <summary>
        /// Email address of the user to login to RedBracket
        /// </summary>
        public string email { set; get; }

        /// <summary>
        ///Password of the user to login to RedBracket
        /// </summary>
        public string password { set; get; }


    }

    /// <summary>
    /// Logged in user details will be sent as a response to the login request.
    /// Serialize the data Json format.
    /// </summary>
    public class UserDetails
    {
        /// <summary>
        /// User id of the user
        /// </summary>
        public Int64 id { set; get; }

        /// <summary>
        /// First Name of the user
        /// </summary>
        public string firstName { set; get; }

        /// <summary>
        /// Last name of the user
        /// </summary>
        public string lastName { set; get; }

        /// <summary>
        /// Hidden password of the user
        /// </summary>
        public string password { set; get; }

        /// <summary>
        /// Company id of the user
        /// </summary>
        public string companyId { set; get; }

        /// <summary>
        /// Is the user company admin.
        /// </summary>
        public string isCompanyAdmin { set; get; }

        /// <summary>
        /// Mobile number of the user
        /// </summary>
        public string mobile { set; get; }

        /// <summary>
        /// Email address of the user
        /// </summary>
        public string email { set; get; }

        /// <summary>
        /// Status of the user
        /// </summary>
        public string status { set; get; }

        /// <summary>
        /// Is an active user
        /// </summary>
        public bool active { set; get; }

        /// <summary>
        /// Is user got deleted from the service
        /// </summary>
        public bool deleted { set; get; }
    }
}
