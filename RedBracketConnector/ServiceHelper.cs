using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.Win32;
using RestSharp;

namespace RedBracketConnector
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
        public static IRestResponse SaveObject(string baseAddress, string relevantAddress, string filePath, string  fileName, bool addUserNametoUrl = true, List<KeyValuePair<string, string>> urlParameters = null)
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

                //request.AddFile("files", File.ReadAllBytes(filePath), fileName);
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
