using System;
using System.Collections;
using CADController.Commands;
using CADController.Controllers;
using ArasConnector;
using ArasConnector.Exceptions;

using ArasConnector.Configuration;

using System.Windows.Forms;
using RedBracketConnector;
using RestSharp;
using System.Data;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CADController.Controllers
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

                loggedUserDetails = JsonConvert.DeserializeObject<UserDetails>(restResponse.Content);
                isConnect = restResponse.StatusCode == System.Net.HttpStatusCode.OK;

                //DataTable dtUserDetail = new DataTable();
                //dtUserDetail.Rows.Add();
                //var list = restResponse.Content.Split(',');
                //foreach(string s in list)
                //{
                //    string s2 = s.Replace('{',' ').Trim();
                //    s2 = s2.Replace('{', ' ').Trim();
                //    s2 = s2.Replace('"', ' ').Trim();
                //    //string s1 = s2.Substring( s2.IndexOf(@":")+1 );
                //    string ColumnName = s2.Substring(0, s2.IndexOf(@":") - 1);
                //    string Value = s2.Substring(s2.IndexOf(@":") + 1);
                //    dtUserDetail.Columns.Add(ColumnName);
                //    dtUserDetail.Rows[0][ColumnName] = Value;

                //}

                //if(dtUserDetail.Rows.Count>0)
                //{
                //    Helper.UserFullName = Convert.ToString(dtUserDetail.Rows[0]["firstName"]) + " " + Convert.ToString(dtUserDetail.Rows[0]["lastName"]);
                //    Helper.FirstName = Convert.ToString(dtUserDetail.Rows[0]["firstName"]);
                //    Helper.LastName = Convert.ToString(dtUserDetail.Rows[0]["lastName"]);
                //    Helper.UserID = Convert.ToString(dtUserDetail.Rows[0]["id"]);
                //}

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
