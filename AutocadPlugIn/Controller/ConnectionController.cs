using System;
using System.Collections; 

using System.Windows.Forms; 
using RestSharp;
using System.Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using AutocadPlugIn;
namespace AutocadPlugIn
{
    public class ConnectionController : BaseController
    {
        public bool isConnect = false;
        public UserDetails loggedUserDetails;

        #region "public Methods"

        public override void Execute(Command command)
        {
            ConnectionCommand cmd = (ConnectionCommand)command;

            try
            { 
                //isConnect = ArasConnector.ArasConnector.Isconnected;
                 
                isConnect = System.Net.HttpStatusCode.OK== Helper.objRBC.LoginValidation(cmd.UserName, cmd.Passwd, out loggedUserDetails);

                

                if(isConnect)
                { 
                    Helper.UserName = cmd.UserName;
                    Helper.UserFullName = Convert.ToString(loggedUserDetails.firstName) + " " + Convert.ToString(loggedUserDetails.lastName);
                    Helper.FirstName = Convert.ToString(loggedUserDetails.firstName);
                    Helper.LastName = Convert.ToString(loggedUserDetails.lastName);
                    Helper.UserID = Convert.ToString(loggedUserDetails.id);
                }
                 
                return;
            }
            catch (System.Exception ex)
            {
                errorString = ex.Message;
                return;
            }
        }

        #endregion

    }
}
