using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;

namespace SFA.DAS.Support.Deployment.Validator
{
    public partial class StatusClient
    {
        public List<Deployment> Deployments = new List<Deployment>();
        public string Id { get; set; }
        public string AppKey { get; set; }
        public string ResourceId { get; set; }
        public string Tenant { get; set; }
        public ClientCredential Credential { get; set; }
        public string Authority { get; set; }

        public StatusClientResults Execute()
        {

            var statusResult = StatusClientResults.Failed;

            //********************************************************
            // Get a client access token from Active Directory
            //********************************************************
            var context = new AuthenticationContext(Authority, true);
            var result = context.AcquireTokenAsync(ResourceId, Credential).Result;
            var token = result.AccessToken;
            if (token == null)
            {
                Console.WriteLine($"The subSite Access token was not obtained.");
            }
            else
            {
                //********************************************************
                // Create an Http Client and set the bearer token
                //********************************************************
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
                //********************************************************
                // Define the deployments to poke
                //********************************************************
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                };
                statusResult = StatusClientResults.Success;
                foreach (var deployment in Deployments)
                    //		$"Processing environment '{deployment.Environment}'".Dump();
                    foreach (var site in deployment.Services)
                    {
                        //			$"Querying site '{site.Name}' at '{site.Address}'...".Dump();
                        HttpResponseMessage response = null;
                        var data = string.Empty;
                        StatusData statusData = null;
                        try
                        {
                            var uri = new Uri(site.Address, site.RequestMethod);
                            response = client.GetAsync(uri).Result;

                            data = response.Content.ReadAsStringAsync().Result;
                            site.Response = data;
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                statusData = JsonConvert.DeserializeObject<StatusData>(data, settings);
                                if (statusData != null)
                                {
                                    site.Status = statusData;
                                    var allWork = true;
                                    if (statusData.SubSites != null)
                                        foreach (var subSite in statusData.SubSites)
                                        {
                                            allWork &= subSite.Value.HttpStatusCode == 200;
                                            allWork &= subSite.Value.Exception == null;
                                            if (allWork)
                                                subSite.Value.Status =
                                                    JsonConvert.DeserializeObject<StatusData>(
                                                        subSite.Value.Content, settings);
                                        }

                                    if (!allWork)
                                    {
                                        statusResult = StatusClientResults.BadSecondary;
                                    }
                                }
                            }
                            else
                            {
                                statusResult = StatusClientResults.BadPrimary;
                            }
                        }
                        catch (Exception ex)
                        {
                            var index = 0;
                            var exceptionReport = $"{index++}: {ex.Message}";
                            var innerEx = ex.InnerException;
                            while (innerEx != null)
                            {
                                exceptionReport += $"\r\n{index++}: {innerEx.Message}";
                                innerEx = innerEx.InnerException;
                            }

                            site.ExceptionReport = exceptionReport;
                            statusResult = StatusClientResults.Failed;
                        }
                    }

                //// For a data dump 
                //	foreach (var deployment in deployments)
                //	{
                //		foreach (var service in deployment.Services)
                //		{
                //			$"Environment {deployment.Environment} Service: {service.Name} Address: {service.Address} Responded: {service.Response!=null}".Dump();
                //			if (service.ExceptionReport != null)
                //			{
                //				service.ExceptionReport.Dump();
                //			}
                //			else if (service.Status != null)
                //			{
                //				service.Status.Dump();
                //			}
                //		}
                //	}

                // For Version assessments
                foreach (var deployment in Deployments)
                {
                    Console.WriteLine($"Environment {deployment.Environment}");
                    Console.WriteLine("Version     Service");
                    foreach (var service in deployment.Services)
                        if (service.Status != null)
                        {
                            Console.WriteLine($"{service.Status.ServiceVersion} {service.Status.ServiceName}");
                            foreach (var subSite in service.Status.SubSites)
                                try
                                {
                                    var status =
                                        JsonConvert.DeserializeObject<StatusData>(subSite.Value.Content,
                                            settings);
                                    Console.WriteLine($"{status.ServiceVersion} {status.ServiceName}");
                                }
                                catch (Exception ex)
                                {
                                    var report = $"{subSite.Value.Content}";

                                    if (report.Contains("<title>Runtime Error</title>"))
                                        Console.WriteLine($"*ERR* {subSite.Key}");
                                    else
                                        Console.WriteLine(report);
                                }
                        }
                }
            }

            return statusResult;
        }
    }
}