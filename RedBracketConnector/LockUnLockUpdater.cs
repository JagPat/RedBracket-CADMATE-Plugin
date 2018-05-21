using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace RedBracketConnector
{
    public  class LockUnLockUpdater
    {
        public void LockObject(List<PLMObject> plmObjs)
        {
            try
            {


                bool IsUpdated = true;
                    foreach (PLMObject plmObj in plmObjs)
                    {
                    //Item drawingQuery = myInnovator.newItem(plmObj.ItemType, "get");
                    //Item drawingQueryRes = null;

                    //drawingQuery.setProperty("id", plmObj.ObjectId);
                    //drawingQueryRes = drawingQuery.apply();
                    //bool is_need = true;
                    //if (drawingQueryRes.getProperty("is_current") != "1")
                    //{
                    //    if (MessageBox.Show("Aras has updated Version of Drawing " + drawingQueryRes.getProperty("name").ToString() + ", Eventhough Would you like to update it?", "Aras has updated Version of this Drawing", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    //        is_need = true;
                    //    else
                    //        is_need = false;
                    //}
                    //if (is_need)
                    //{
                    //    if (drawingQueryRes.lockItem().isError())
                    //    {
                    //        throw (new Exceptions.ConnectionException("Exception occured in 'LockObject' method.\n Error string is :" + drawingQueryRes.lockItem().getErrorString()));
                    //    }
                    //}
                    //else
                    //{
                    //    throw (new Exceptions.ConnectionException("Please download latest Version of Drawing from Aras!!!"));
                    //}


                    // KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileid", plmObj.ObjectId);
                    List<KeyValuePair<string, string>> urlParameters = new List<KeyValuePair<string, string>>();
                    KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileid", "11760c31-d3fb-4acb-9675-551915493fd5");                                   
                    urlParameters.Add(L);
                    L = new KeyValuePair<string, string>("userName", Helper.UserName);
                    urlParameters.Add(L);

                    RestResponse restResponse = (RestResponse)ServiceHelper.PostData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                        "/AutocadFiles/lockfile", DataFormat.Json,
                        null, false, urlParameters);
                    if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        MessageBox.Show("Some error occurred while locking file.");
                    }


                }
                
            }
            catch (Exception ex)
            {
                throw (new Exceptions.ConnectionException("RedBracketConnectorS Exception Message : " + ex.Message));
            }
        }

        public void UnlockObject(List<PLMObject> plmObjs)
        {
            try
            {
                
                    

                    foreach (PLMObject plmObj in plmObjs)
                    {
                    //Item drawingQuery = myInnovator.newItem(plmObj.ItemType, "get");
                    //Item drawingQueryRes = null;

                    //drawingQuery.setProperty("id", plmObj.ObjectId);
                    //drawingQueryRes = drawingQuery.apply();
                    //if (drawingQueryRes.unlockItem().isError())
                    //{
                    //    throw (new Exceptions.ConnectionException("Exception occured in 'UnlockObject' method.\n Error string is :" + drawingQueryRes.unlockItem().getErrorString()));
                    //}

                    // KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileid", plmObj.ObjectId);
                    List<KeyValuePair<string, string>> urlParameters = new List<KeyValuePair<string, string>>();
                    KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileid", "11760c31-d3fb-4acb-9675-551915493fd5");
                    urlParameters.Add(L);
                    L = new KeyValuePair<string, string>("userName", Helper.UserName);
                    urlParameters.Add(L);

                    RestResponse restResponse = (RestResponse)ServiceHelper.PostData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                        "/AutocadFiles/unlockfile", DataFormat.Json,
                        null, false, urlParameters);
                    if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        MessageBox.Show("Some error occurred while locking file.");
                    }


                }
                
            }
            catch (Exception ex)
            {
                throw (new Exceptions.ConnectionException("RedBracketConnector Exception Message : " + ex.Message));

            }
        }

        public void LockStatus(ref DataTable itemInfo)
        {
            try
            {
                foreach (DataRow rw in itemInfo.Rows)
                {
                    if (rw["drawingid"].ToString() != "")
                    {
                        //Item lockStatusQry = null;
                        //Item lockStatusRes = null;
                        //lockStatusQry = myInnovator.newItem(rw["type"].ToString(), "get");
                        //lockStatusQry.setProperty("id", rw["drawingid"].ToString());
                        //lockStatusQry.setAttribute("select", "locked_by_id, id");
                        //lockStatusRes = lockStatusQry.apply();

                        //if (lockStatusRes.isError())
                        //{
                        //    throw (new Exceptions.ConnectionException("Exception occured in 'LockStatus' method.\n Error string is :" + lockStatusRes.getErrorString()));
                        //}
                        //rw["lockstatus"] = lockStatusRes.getLockStatus().ToString();
                        //rw["drawingid"] = (String)lockStatusRes.getProperty("id");
                        //if (lockStatusRes.getLockStatus() == 2)
                        //    rw["lockby"] = GetLockBy(lockStatusRes.getProperty("locked_by_id"));
                        //else
                        //    rw["lockby"] = " ";

                        // KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileId", rw["drawingid"].ToString());
                        KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileId", "11760c31-d3fb-4acb-9675-551915493fd5");

                        List<KeyValuePair<string, string>> urlParameters = new List<KeyValuePair<string, string>>();
                        urlParameters.Add(L);
                        RestResponse restResponse = (RestResponse)ServiceHelper.PostData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                            "/AutocadFiles/fetchFileInfo", DataFormat.Json,
                            null, true, urlParameters);

                        //try
                        //{
                        //    var resultSearchCriteriaResponseList = JsonConvert.DeserializeObject<List<ResultSearchCriteria>>(restResponse.Content);
                        //}
                        //catch { }

                        if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            MessageBox.Show("Some error occurred while fetching file status.");
                        }
                        else
                        {
                            string Response = restResponse.Content;

                            if(Response.Contains("11760c31-d3fb-4acb-9675-551915493fd5") && Response.Contains("filelock") && 
                                Response.Contains("updatedBy"))
                            {
                               int i= Response.IndexOf("updatedBy");
                                string s = Response.Substring(Response.LastIndexOf("updatedby") +12, 50);
                                string s1 = Response.Substring(Response.IndexOf("filelock") + 10, 5);
                                if(s1!="false")
                                {
                                    s1 = s1.Substring(0, 4);
                                }
                                bool FileLock = Convert.ToBoolean(Convert.ToString(s1));
                                string UpdatedBy =  s.Substring(0, s.IndexOf('"'));

                                if (FileLock)
                                {
                                    if (UpdatedBy == Helper.UserFullName)
                                    {
                                        rw["lockstatus"] = "1";
                                        rw["lockby"] = UpdatedBy;
                                    }
                                    else
                                    {
                                        rw["lockstatus"] = "2";
                                        rw["lockby"] = UpdatedBy;
                                    }
                                }
                                else
                                {
                                    rw["lockstatus"] = "0";
                                    rw["lockby"] = UpdatedBy;
                                }
                            }
                            //DataTable dataTableFileInfo = (DataTable)JsonConvert.DeserializeObject(restResponse.Content, (typeof(DataTable)));
                            //if(dataTableFileInfo.Rows.Count>0)
                            //{
                            //    bool FileLock = Convert.ToBoolean(Convert.ToString(dataTableFileInfo.Rows[0]["filelock"]));
                            //    string UpdatedBy = Convert.ToString(dataTableFileInfo.Rows[0]["updatedBy"]);

                            //    if(FileLock)
                            //    {
                            //        if(UpdatedBy== Helper.UserName)
                            //        {
                            //            rw["lockstatus"] = "1";
                            //            rw["lockby"] = dataTableFileInfo.Rows[0]["updatedBy"];
                            //        }
                            //        else
                            //        {
                            //            rw["lockstatus"] = "2";
                            //            rw["lockby"] = dataTableFileInfo.Rows[0]["updatedBy"];
                            //        }
                            //    }
                            //    else
                            //    {

                            //    }
                            //}

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (new Exceptions.ConnectionException("Exception Message :" + ex.Message));

            }
        }
    }
}
