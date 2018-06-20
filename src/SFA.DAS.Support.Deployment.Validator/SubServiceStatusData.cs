using System;

namespace SFA.DAS.Support.Deployment.Validator
{


    public partial class StatusClient
    {
        public class SubServiceStatusData
        {
            public int Result { get; set; }
            public int HttpStatusCode { get; set; }
            public Exception Exception { get; set; }
            public string Content { get; set; }

            /// <summary>
            /// The sub service Content string Deserialised
            /// </summary>
            public StatusData Status { get; set; }
        }
    }
}