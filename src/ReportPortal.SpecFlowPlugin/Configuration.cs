﻿using System;
using System.Configuration;

namespace ReportPortal.SpecFlowPlugin
{
    public static class Configuration
    {
        static readonly Lazy<ReportPortalSection> ReportPortalSection = new Lazy<ReportPortalSection>(() => LoadConfigurationFromPluginAssembly() ?? LoadConfigurationFromAssemblyThatUsingThePlugin());

        private static ReportPortalSection LoadConfigurationFromAssemblyThatUsingThePlugin()
        {
            return ConfigurationManager.GetSection("reportPortal") as ReportPortalSection;
        }

        private static ReportPortalSection LoadConfigurationFromPluginAssembly()
        {
            var exeConfigPath = new Uri(typeof(Configuration).Assembly.CodeBase).LocalPath;
            return ConfigurationManager.OpenExeConfiguration(exeConfigPath).GetSection("reportPortal") as ReportPortalSection;
        }

        private static void ThrowAnErrorIfReportalPortalConfigurationSectionIsNull()
        {
            if (ReportPortalSection.Value == null)
                throw new ConfigurationErrorsException("No ReportPortalSection in config file!");
        }

        public static ReportPortalSection ReportPortal
        {
            get {
                    ThrowAnErrorIfReportalPortalConfigurationSectionIsNull();
                    return ReportPortalSection.Value; 
                }
        }
    }

    public class ReportPortalSection : ConfigurationSection
    {
        [ConfigurationProperty("enabled")]
        public bool Enabled
        {
            get { return bool.Parse(this["enabled"].ToString()); }
        }

        [ConfigurationProperty("server")]
        public ServerConfigurationElement Server
        {
            get { return this["server"] as ServerConfigurationElement; }
        }

        [ConfigurationProperty("launch")]
        public LaunchConfigurationElement Launch
        {
            get { return this["launch"] as LaunchConfigurationElement; }
        }

        public class ServerConfigurationElement : ConfigurationElement
        {
            [ConfigurationProperty("url")]
            public string Url
            {
                get { return this["url"].ToString(); }
            }

            [ConfigurationProperty("project")]
            public string Project
            {
                get { return this["project"].ToString(); }
            }

            [ConfigurationProperty("authentication")]
            public AuthenticationConfigurationElement Authentication
            {
                get { return this["authentication"] as AuthenticationConfigurationElement; }
            }

            [ConfigurationProperty("proxy")]
            public ProxyConfigurationElement Proxy
            {
                get { return this["proxy"] as ProxyConfigurationElement; }
            }

            public class AuthenticationConfigurationElement : ConfigurationElement
            {
                [ConfigurationProperty("username")]
                public string Username
                {
                    get { return this["username"].ToString(); }
                }

                [ConfigurationProperty("password")]
                public string Password
                {
                    get { return this["password"].ToString(); }
                }
            }

            public class ProxyConfigurationElement : ConfigurationElement
            {
                [ConfigurationProperty("server")]
                public string Server
                {
                    get { return this["server"].ToString(); }
                }
            }
        }

        public class LaunchConfigurationElement : ConfigurationElement
        {
            [ConfigurationProperty("name")]
            public string Name
            {
                get { return this["name"].ToString(); }
            }

            [ConfigurationProperty("debugMode")]
            public bool DebugMode
            {
                get { return bool.Parse(this["debugMode"].ToString()); }
            }

            [ConfigurationProperty("tags")]
            public string Tags
            {
                get { return this["tags"].ToString(); }
            }
        }
    }
}
