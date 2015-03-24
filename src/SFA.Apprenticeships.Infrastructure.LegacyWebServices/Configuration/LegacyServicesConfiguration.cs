﻿namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Configuration
{
    using System;
   
    public class LegacyServicesConfiguration
    {
        public const string ConfigurationName = "LegacyServicesConfiguration";

        public Guid SystemId { get; set; }

        public string PublicKey { get; set; }
    }
}