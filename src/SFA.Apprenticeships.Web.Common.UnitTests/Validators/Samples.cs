﻿namespace SFA.Apprenticeships.Web.Common.UnitTests.Validators
{
    public static class Samples
    {
        public const string ValidFreeText =
            "Excellent opportunities for Pre School Apprentice&#39;s are " +
            "available in this established, " +
            "friendly nursery.  You will be caring for and meeting the needs of children aged between " +
            "4 months and 5 years, creating a safe environment and providing stimulating \r\n";

        public const string ValidFreeHtmlText =
            "<h1>Excellent op<h2>portunities</h2> for Pre School Apprentice&#39;s are " +
            "available in this established, " +
            "friendly nursery.  You will be caring for and meeting <br>the needs of <br><br>children aged between " +
            "4 months and 5 years, creating a safe environment and providing stimulating </h1>\r\n<br></br> <br></br>" +
            "<br></br>";

        public const string InvalidHtmlTextWithScript =
            "<h1>Excellent op<h2>portunities</h2> for Pre School Apprentice&#39;s are available in this established, \" " +
            "+\r\n                  \"friendly nursery.  " +
            "You will be caring for and meeting <br>the needs of <br><br>children aged between " +
            "\" +\r\n                  \"4 " +
            "months and 5 years, creating a safe environment and providing stimulating </h1>\\r\\n<br></br> <br>" +
            "</br>\" +\r\n                  \"<br>" +
            "</br><script>alert(\'Hello World\')</script>\"</br>";

        public const string InvalidHtmlTextWithInput =
            "<h1>Excellent op<h2>portunities</h2> for Pre School Apprentice&#39;s are available in this established, \" " +
            "+\r\n                  \"friendly nursery.  " +
            "You will be caring for and meeting <br>the needs of <br><br>children aged between " +
            "\" +\r\n                  \"4 " +
            "months and 5 years, creating a safe environment and providing stimulating </h1>\\r\\n<br></br> <br>" +
            "</br>\" +\r\n                  \"<br>" +
            "<input>alert(\\\'Hello World\\\')</input>\"\"</br>";

        public const string InvalidHtmlTextWithObject =
            "<h1>Excellent op<h2>portunities</h2> for Pre School Apprentice&#39;s are available in this established, \" " +
            "+\r\n                  \"friendly nursery.  " +
            "You will be caring for and meeting <br>the needs of <br><br>children aged between " +
            "\" +\r\n                  \"4 " +
            "months and 5 years, creating a safe environment and providing stimulating </h1>\\r\\n<br></br> <br>" +
            "</br>\" +\r\n                  \"<br>" +
            "<object>alert(\\\'Hello World\\\')</object>\"";
    }
}