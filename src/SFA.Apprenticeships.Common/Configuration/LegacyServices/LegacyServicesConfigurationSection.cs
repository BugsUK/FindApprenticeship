﻿namespace SFA.Apprenticeships.Common.Configuration.LegacyServices
{
    using System;
    using System.Configuration;
    using SFA.Apprenticeships.Common.Configuration;

    public class LegacyServicesConfigurationSection : SecureConfigurationSection<LegacyServicesConfigurationSection>, ILegacyServicesConfiguration
    {
        private const string SystemIdConst = "SystemId";
        private const string PublicKeyConst = "PublicKey";

        public LegacyServicesConfigurationSection()
            : base("LegacyServicesConfiguration")
        {
        }

        [ConfigurationProperty(SystemIdConst, IsRequired = true, IsKey = true)]
        public Guid SystemId
        {
            get { return (Guid)this[SystemIdConst]; }
            set { this[SystemIdConst] = value; }
        }

        [ConfigurationProperty(PublicKeyConst, IsRequired = true, IsKey = false)]
        public string PublicKey
        {
            get { return (string)this[PublicKeyConst]; }
            set { this[PublicKeyConst] = value; }
        }
    }
}