using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SFA.DAS.Support.Indexer.Worker.DependencyResolution
{
    [ExcludeFromCodeCoverage]
    public class LoggingPropertyFactory : ILoggingPropertyFactory
    {
        public IDictionary<string, object> GetProperties()
        {
            var properties = new Dictionary<string, object>();
            try
            {
                properties.Add("Version", GetVersion());
                properties.Add("RequestCtx.User.SupportTier", "ConsoleUser-Worker Role");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return properties;
        }


        private static string GetVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileVersionInfo.ProductVersion;
        }
    }
}