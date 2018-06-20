using System;

namespace SFA.DAS.Support.Deployment.Validator
{


    public partial class StatusClient
    {
        public class Service
        {
            /// <summary>
            /// The Name I give the service I will query
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The Address of the service I will query
            /// </summary>
            public Uri Address { get; set; }

            /// <summary>
            /// The raw response I recieve
            /// </summary>
            public string Response { get; set; }

            /// <summary>
            /// The Response string Deserialised
            /// </summary>
            public StatusData Status { get; set; }

            /// <summary>
            /// The exception recieved
            /// </summary>
            public string ExceptionReport { get; set; }

            /// <summary>
            /// The method to call for this service
            /// </summary>
            public string RequestMethod { get; set; }

        }
    }
}