using System;
using System.Collections; 

using System.Windows.Forms;
using RedBracketConnector;
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
                //objConnector.Connect(cmd.Url, cmd.DbName, cmd.UserName, cmd.Passwd, cmd.AuthoringTool);

                RestResponse restResponse = (RestResponse)ServiceHelper.PostData(cmd.Url, "/Login/login", DataFormat.Json, new UserLoginDetails
                {
                    email = cmd.UserName,
                    password = cmd.Passwd
                }, false);

                //isConnect = ArasConnector.ArasConnector.Isconnected;
                if(restResponse.StatusCode==System.Net.HttpStatusCode.OK)
                {
                    loggedUserDetails = JsonConvert.DeserializeObject<UserDetails>(restResponse.Content);
                }
                else if(restResponse.StatusCode==System.Net.HttpStatusCode.Unauthorized)
                {
                    errorString = "Unauthorize access.";
                }
                else
                {
                    errorString = restResponse.Content;
                }
                isConnect = restResponse.StatusCode == System.Net.HttpStatusCode.OK;

                

                if(isConnect)
                {
                    UserDetails UserRecords = JsonConvert.DeserializeObject<UserDetails>(restResponse.Content);
                    Helper.UserName = cmd.UserName;
                    Helper.UserFullName = Convert.ToString(UserRecords.firstName) + " " + Convert.ToString(UserRecords.lastName);
                    Helper.FirstName = Convert.ToString(UserRecords.firstName);
                    Helper.LastName = Convert.ToString(UserRecords.lastName);
                    Helper.UserID = Convert.ToString(UserRecords.id);
                }


                //var dataSet = JsonConvert.DeserializeObject<DataSet>(restResponse.Content);
                //var table = dataSet.Tables[0];
                //DataTable dtUserInfo = (DataTable)JsonConvert.DeserializeObject(restResponse.Content, (typeof(DataTable)));


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
