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
    

        /// <summary>
        ///Password of the user to login to RedBracket
        /// </summary>
       


        
        public string password { set; get; } 
        public string email { set; get; } 
        
    }
    public class UserDetails
    {
        /// <summary>
        /// Email address of the user to login to RedBracket
        /// </summary>


        /// <summary>
        ///Password of the user to login to RedBracket
        /// </summary>



        public Int64 id { set; get; }
        public string firstName { set; get; }
        public string lastName { set; get; }
        public string password { set; get; }
        public string companyId { set; get; }
        public string isCompanyAdmin { set; get; }
        public string mobile { set; get; }
        public string email { set; get; }
        public string status { set; get; }
        public bool active { set; get; }
        public bool deleted { set; get; }

    }
}
