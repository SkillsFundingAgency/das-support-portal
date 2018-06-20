using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;

namespace SFA.DAS.Support.Deployment.Validator
{
    public class Program
    {

        private static StatusClient client = new StatusClient();

        static int Main(string[] args)
        {
            var defaultRequestMethod = "/api/status";
            var path = new DirectoryInfo(Environment.CurrentDirectory);

            client.Id = ConfigurationManager.AppSettings["Id"];
            client.AppKey = ConfigurationManager.AppSettings["AppKey"]; 
            client.ResourceId = ConfigurationManager.AppSettings["ResourceId"];
            client.Tenant = ConfigurationManager.AppSettings["Tenant"]; 
            client.Authority = ConfigurationManager.AppSettings["Authority"]; 

            client.Credential = new ClientCredential(client.Id, client.AppKey);
            client.Deployments = new List<Deployment>();

            List<Deployment> deployments = new List<Deployment>();

            foreach (var arg in args)
            {
                var fileInfo = new FileInfo(arg);
                if (!fileInfo.Exists) continue;
                var data = JsonConvert.DeserializeObject<List<Deployment>>(File.ReadAllText(fileInfo.FullName));
                deployments.AddRange(data);
            }

            if (!deployments.Any()) return (int)StatusClientResults.NothingToDo;

            client.Deployments = deployments;

            var result = client.Execute();

            File.WriteAllText($@"{path.FullName}\results.json", JsonConvert.SerializeObject(client.Deployments, Formatting.Indented));

            Console.WriteLine($"Processing Result: {result}");
            Console.ReadKey(false);
            return (int)result;
        }
    }
}