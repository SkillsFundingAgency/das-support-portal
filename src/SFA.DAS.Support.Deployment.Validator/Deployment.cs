using System.Collections.Generic;

namespace SFA.DAS.Support.Deployment.Validator
{
  public class Deployment
        {
            public string Environment { get; set; }
            public List<StatusClient.Service> Services { get; set; } = new List<StatusClient.Service>();
        }

   
}