using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SFA.DAS.Support.Shared
{
    public static class AssemblyExtensions
    {
        //General Information about an assembly is controlled through the following
        // set of attributes.Change these attribute values to modify the information
        // associated with an assembly.
        //[assembly: AssemblyTitle("whatever")]
        //[assembly: AssemblyDescription("")]
        //[assembly: AssemblyConfiguration("")]
        //[assembly: AssemblyCompany("")]
        //[assembly: AssemblyProduct("whatever")]
        //[assembly: AssemblyCopyright("Copyright ©  2018 whatever")]
        //[assembly: AssemblyTrademark("whatever ™ ®")]
        //[assembly: AssemblyCulture("")]

        // Setting ComVisible to false makes the types in this assembly not visible
        // to COM components.  If you need to access a type in this assembly from
        // COM, set the ComVisible attribute to true on that type.
        //[assembly: ComVisible(false)]

        // The following GUID is for the ID of the typelib if this project is exposed to COM
        //[assembly: Guid("7fb46d94-bc1e-476f-a52b-9f3ecd292a0b")]

        // Version information for an assembly consists of the following four values:


        //      Major Version
        //      Minor Version
        //      Build Number
        //      Revision

        // You can specify all the values or you can default the Build and Revision Numbers
        // by using the '*' as shown below:
        // [assembly: AssemblyVersion("1.0.*")]
        //[assembly: AssemblyVersion("1.0.0.0")]
        //[assembly: AssemblyFileVersion("1.0.0.0")]


        public static string Id(this Assembly source)
        {
            var attribute = (GuidAttribute) source
                .GetCustomAttributes(typeof(GuidAttribute), true)
                .SingleOrDefault();
            return attribute == null ? string.Empty : attribute.Value;
        }


        public static string Configuration(this Assembly source)
        {
            var attribute = (AssemblyConfigurationAttribute) source
                .GetCustomAttributes(typeof(AssemblyConfigurationAttribute), true)
                .SingleOrDefault();
            return attribute == null ? string.Empty : attribute.Configuration;
        }

        public static string Description(this Assembly source)
        {
            var attribute = (AssemblyDescriptionAttribute) source
                .GetCustomAttributes(typeof(AssemblyDescriptionAttribute), true)
                .SingleOrDefault();
            return attribute == null ? string.Empty : attribute.Description;
        }


        public static string Title(this Assembly source)
        {
            var attribute = (AssemblyTitleAttribute) source
                .GetCustomAttributes(typeof(AssemblyTitleAttribute), true)
                .SingleOrDefault();
            return attribute == null ? string.Empty : attribute.Title;
        }


        public static string FileVersion(this Assembly source)
        {
            var attribute = (AssemblyFileVersionAttribute) source
                .GetCustomAttributes(typeof(AssemblyFileVersionAttribute), true)
                .SingleOrDefault();
            return attribute == null ? string.Empty : attribute.Version;
        }

        public static string Version(this Assembly source)
        {
            var attribute = (AssemblyVersionAttribute) source
                .GetCustomAttributes(typeof(AssemblyVersionAttribute), true)
                .SingleOrDefault();
            return attribute == null ? string.Empty : attribute.Version;
        }

        public static string Company(this Assembly source)
        {
            var attribute = (AssemblyCompanyAttribute) source
                .GetCustomAttributes(typeof(AssemblyCompanyAttribute), true)
                .SingleOrDefault();
            return attribute == null ? string.Empty : attribute.Company;
        }

        public static string Copyright(this Assembly source)
        {
            var attribute = (AssemblyCopyrightAttribute) source
                .GetCustomAttributes(typeof(AssemblyCopyrightAttribute), true)
                .SingleOrDefault();
            return attribute == null ? string.Empty : attribute.Copyright;
        }

        public static string Trademark(this Assembly source)
        {
            var attribute = (AssemblyTrademarkAttribute) source
                .GetCustomAttributes(typeof(AssemblyTrademarkAttribute), true)
                .SingleOrDefault();
            return attribute == null ? string.Empty : attribute.Trademark;
        }

        public static string Culture(this Assembly source)
        {
            var attribute = (AssemblyCultureAttribute) source
                .GetCustomAttributes(typeof(AssemblyCultureAttribute), true)
                .SingleOrDefault();
            return attribute == null ? string.Empty : attribute.Culture;
        }

        public static string Product(this Assembly source)
        {
            var attribute = (AssemblyProductAttribute) source
                .GetCustomAttributes(typeof(AssemblyProductAttribute), true)
                .SingleOrDefault();
            return attribute == null ? string.Empty : attribute.Product;
        }
    }
}