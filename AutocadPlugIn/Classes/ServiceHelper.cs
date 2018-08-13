using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.Win32;
using RestSharp;
using RBAutocadPlugIn;
namespace RBAutocadPlugIn
{
    public static class ServiceHelper
    {
        /// <summary>
        /// Posts the data to the server and sends the Rest response back to the caller
        /// </summary>
        /// <param name="relevantAddress">Enter the address needs to be apended to the base address. For ex, "/Login/login"</param>
        /// <param name="dataFormat">Format of the data, Json/XML. For ex, DataFormat.Json</param>
        /// <param name="postData">Class object of the data to be posted. Class object.</param>
        /// <returns>Rest response, Service response</returns>
        public static IRestResponse PostData(string baseAddress, string relevantAddress, DataFormat dataFormat = DataFormat.Json, object postData = null, bool addUserNametoUrl = true, List<KeyValuePair<string, string>> urlParameters = null)
        {
            try
            {
                //var restClient = new RestClient("https://test.redbracket.in:8090");
                var restClient = new RestClient(baseAddress);
                int count = 0;
                if (addUserNametoUrl)
                {
                    relevantAddress += "?userName=" + Helper.GetValueRegistry("LoginSettings", "UserName").ToString();
                }

                if (urlParameters != null)
                {
                    foreach (KeyValuePair<string, string> urlParameter in urlParameters)
                    {
                        if (count == 0 && !addUserNametoUrl)
                        {
                            relevantAddress += ("?" + urlParameter.Key + "=" + urlParameter.Value);
                        }
                        else
                        {
                            relevantAddress += ("&" + urlParameter.Key + "=" + urlParameter.Value);
                        }

                        count++;
                    }
                }

                var request = new RestRequest(relevantAddress, Method.POST);
                request.RequestFormat = dataFormat;

                request.AddBody(postData);
                return restClient.Execute(request);
            }
            catch (WebException webException)
            {
                throw new WebException("Invalid response from the server.", webException.InnerException);
            }
        }

        public static IRestResponse GetData(string baseAddress, string relevantAddress, bool addUserNametoUrl = true, List<KeyValuePair<string, string>> urlParameters = null)
        {
            try
            {
                string delimeter = "?";
                //var restClient = new RestClient("https://test.redbracket.in:8090");
                var restClient = new RestClient(baseAddress);

                if (addUserNametoUrl)
                {
                    relevantAddress += "?userName=" + Helper.GetValueRegistry("LoginSettings", "UserName");
                }

                if (urlParameters != null)
                {
                    foreach (KeyValuePair<string, string> urlParameter in urlParameters)
                    {
                        if (!relevantAddress.Contains(delimeter))
                        {
                            delimeter = "?";
                        }
                        else
                        {
                            delimeter = "&";
                        }

                        relevantAddress += (delimeter + urlParameter.Key + "=" + urlParameter.Value);
                    }
                }

                return restClient.Execute(new RestRequest(relevantAddress, Method.GET));
            }
            catch (WebException webException)
            {
                throw new WebException("Invalid response from the server.", webException.InnerException);
            }
        }

        /// <summary>
        /// Saves the object to redbracket server.
        /// </summary>
        /// <param name="baseAddress">Base address of the server to save the object.</param>
        /// <param name="relevantAddress">Relevant address of the path.</param>
        /// <param name="filePath">Physical, absolute path of the file.</param>
        /// <param name="fileName">Name of the file to save that in the server.</param>
        /// <param name="addUserNametoUrl">Username should be added to URL?</param>
        /// <param name="urlParameters">Specify list of parameters if any.</param>
        /// <returns>Response of the server in IRestResponse format.</returns>
        public static IRestResponse SaveObject(string baseAddress, string relevantAddress, string filePath, bool addUserNametoUrl = true,
            List<KeyValuePair<string, string>> urlParameters = null, string PreFix = null, bool IsFileUpload = true)
        {
            try
            {
                //var restClient = new RestClient("https://test.redbracket.in:8090");
                string fileName = "";
                var restClient = new RestClient(baseAddress);
                int count = 0;
                if (addUserNametoUrl)
                {
                    relevantAddress += "?userName=" + Helper.GetValueRegistry("LoginSettings", "UserName").ToString();
                }

                if (urlParameters != null)
                {
                    foreach (KeyValuePair<string, string> urlParameter in urlParameters)
                    {

                        if (count == 0 && !addUserNametoUrl)
                        {
                            relevantAddress += ("?" + urlParameter.Key + "=" + urlParameter.Value);
                        }
                        else
                        {
                            relevantAddress += ("&" + urlParameter.Key + "=" + urlParameter.Value);
                        }

                        count++;
                    }
                }

                var request = new RestRequest(relevantAddress, Method.POST);

                fileName = Helper.RemovePreFixFromFileName(Path.GetFileName(filePath), PreFix);
          
                if (IsFileUpload)
                    request.AddFile("files", Helper.GetFileDataBytes(filePath), fileName);
                //else
                //    request.AddFile("files", "");


                return restClient.Execute(request);
            }
            catch (WebException webException)
            {
                throw new WebException("Invalid response from the server.", webException.InnerException);
            }
        }


        public static IRestResponse UpdateObject(string baseAddress, string relevantAddress, string filePath, bool addUserNametoUrl = true, List<KeyValuePair<string, string>> urlParameters = null, String PreFix = null)
        {
            try
            {
                //var restClient = new RestClient("https://test.redbracket.in:8090");
                string fileName = "";
                string FIleid = "";
                var restClient = new RestClient(baseAddress);
                int count = 0;
                if (addUserNametoUrl)
                {
                    relevantAddress += "?userName=" + Helper.GetValueRegistry("LoginSettings", "UserName").ToString();
                }

                if (urlParameters != null)
                {
                    foreach (KeyValuePair<string, string> urlParameter in urlParameters)
                    {
                        if (urlParameter.Key == "")
                        {
                            FIleid = urlParameter.Value;
                        }

                        if (count == 0 && !addUserNametoUrl)
                        {
                            relevantAddress += ("?" + urlParameter.Key + "=" + urlParameter.Value);
                        }
                        else
                        {
                            relevantAddress += ("&" + urlParameter.Key + "=" + urlParameter.Value);
                        }

                        count++;
                    }
                }

                var request = new RestRequest(relevantAddress, Method.POST);

                fileName = Helper.RemovePreFixFromFileName(Path.GetFileName(filePath), PreFix);
                 
                request.AddFile("files", Helper.GetFileDataBytes(filePath), fileName);

                return restClient.Execute(request);
            }
            catch (WebException webException)
            {
                throw new WebException("Invalid response from the server.", webException.InnerException);
            }
        }
    }
}
