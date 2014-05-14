﻿namespace SFA.Apprenticeships.Services.Common.Tests.Configuration
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Common.Configuration;
    using SFA.Apprenticeships.Services.Common.ActiveDirectory;

    [TestFixture]
    public class ActiveDirectoryConfigurationTests
    {
        [TestCase]
        public void GetConfigSectionForAd()
        {
            var test = ActiveDirectoryConfigurationSection.Instance;
           
            test.DistinguishedName.Should().Be("distinguishedname");
            test.Server.Should().Be("server");
            test.Username.Should().Be("username");
            test.Password.Should().Be("password");
        }
    }
}
