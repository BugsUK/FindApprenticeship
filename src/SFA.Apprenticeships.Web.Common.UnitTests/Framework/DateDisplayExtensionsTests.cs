namespace SFA.Apprenticeships.Web.Common.UnitTests.Framework
{
    using System;
    using Common.Framework;
    using FluentAssertions;
    using Infrastructure.Presentation;
    using NUnit.Framework;

    [TestFixture]
    public class DateDisplayExtensionsTests
    {
        [TestCase(0, "today")]
        [TestCase(1, "tomorrow")]
        [TestCase(2, "in 2 days")]
        [TestCase(3, "in 3 days")]
        [TestCase(4, "in 4 days")]
        [TestCase(5, "in 5 days")]
        [TestCase(6, "in 6 days")]
        [TestCase(7, "in 7 days")]
        public void FriendlyClosingWeekWithinTheWeek(int daysToClosing, string expectedOutput)
        {
            //Test GMT and UTC dates
            var closingDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow.Date.AddDays(daysToClosing), TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
            var friendlyClosingDate = closingDate.ToFriendlyClosingWeek();
            Assert.AreEqual(friendlyClosingDate, expectedOutput);
            closingDate = DateTime.UtcNow.Date.AddDays(daysToClosing);
            friendlyClosingDate = closingDate.ToFriendlyClosingWeek();
            Assert.AreEqual(friendlyClosingDate, expectedOutput);
        }

        [Test]
        public void FriendlyClosingWeekGreaterThanAWeekInTheFuture()
        {
            //Test GMT and UTC dates
            var closingDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow.Date.AddDays(10), TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
            var friendlyClosingDate = closingDate.ToFriendlyClosingWeek();
            Assert.AreEqual(friendlyClosingDate, closingDate.ToString("dd MMM yyyy"));
            closingDate = DateTime.UtcNow.Date.AddDays(10);
            friendlyClosingDate = closingDate.ToFriendlyClosingWeek();
            Assert.AreEqual(friendlyClosingDate, closingDate.ToString("dd MMM yyyy"));
        }

        [Test]
        public void FriendlyClosingWeekInThePast()
        {
            //Test GMT and UTC dates
            var closingDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow.Date.AddDays(-10), TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
            var friendlyClosingDate = closingDate.ToFriendlyClosingWeek();
            Assert.AreEqual(friendlyClosingDate, closingDate.ToString("dd MMM yyyy"));
            closingDate = DateTime.UtcNow.Date.AddDays(-10);
            friendlyClosingDate = closingDate.ToFriendlyClosingWeek();
            Assert.AreEqual(friendlyClosingDate, closingDate.ToString("dd MMM yyyy"));
        }

        [TestCase(-20, false)]
        [TestCase(-1, false)]
        [TestCase(0, true)]
        [TestCase(1, false)]
        [TestCase(20, false)]
        public void FriendlyClosingToday(int daysToClosing, bool showToday)
        {
            //Test GMT and UTC dates
            var closingDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow.Date.AddDays(daysToClosing), TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
            var friendlyClosingDate = closingDate.ToFriendlyClosingToday();
            Assert.AreEqual(friendlyClosingDate, showToday ? "today" : closingDate.ToString("dd MMM yyyy"));
            closingDate = DateTime.UtcNow.Date.AddDays(daysToClosing);
            friendlyClosingDate = closingDate.ToFriendlyClosingToday();
            Assert.AreEqual(friendlyClosingDate, showToday ? "today" : closingDate.ToString("dd MMM yyyy"));
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(7)]
        [TestCase(28)]
        public void ToFriendlyDaysAgo(int daysAgo)
        {
            //Test GMT and UTC dates
            var dateDaysAgo = TimeZoneInfo.ConvertTime(DateTime.UtcNow.Date.AddDays(-daysAgo), TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
            var friendlyDaysAgo = dateDaysAgo.ToFriendlyDaysAgo();
            AssertToFriendlyDaysAgo(daysAgo, friendlyDaysAgo, dateDaysAgo);
            dateDaysAgo = DateTime.UtcNow.AddDays(-daysAgo);
            friendlyDaysAgo = dateDaysAgo.ToFriendlyDaysAgo();
            AssertToFriendlyDaysAgo(daysAgo, friendlyDaysAgo, dateDaysAgo);
        }

        private static void AssertToFriendlyDaysAgo(int daysAgo, string friendlyDaysAgo, DateTime dateDaysAgo)
        {
            if (daysAgo == 0)
            {
                friendlyDaysAgo.Should().Be("today");
            }
            else if (daysAgo == 1)
            {
                friendlyDaysAgo.Should().Be("yesterday");
            }
            else if (daysAgo > 7 || daysAgo < 0)
            {
                friendlyDaysAgo.Should().Be(dateDaysAgo.ToString("dd MMM yyyy"));
            }
            else
            {
                friendlyDaysAgo.Should().Be(daysAgo + " days ago");
            }
        }
    }
}