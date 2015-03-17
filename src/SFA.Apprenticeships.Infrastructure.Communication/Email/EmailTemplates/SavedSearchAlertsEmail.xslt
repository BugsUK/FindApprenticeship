<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="html" indent="yes"/>
  <xsl:template match="savedSearchAlerts">
    <div>
      <xsl:apply-templates select="savedSearchAlert"/>
    </div>
  </xsl:template>

  <xsl:template match="savedSearchAlert">
    <table align="center" border="0" cellpadding="0" cellspacing="0" width="580">
      <tbody>
        <tr>
          <td border="0" cellpadding="0" cellspacing="0" style="font-family: Helvetica, Arial, sans-serif;color:#0b0c0c;" valign="top">
            <h2 style="font-size: 18px; margin: 0; padding: 0;">
              Top
              <span>
                <xsl:value-of select="resultsCount"/>
              </span> result(s) matching your search:
            </h2>
            <p>
              <xsl:value-of select="parameters/name"/>
              <br />
              <b>Apprenticeship level: </b>
              <xsl:value-of select="parameters/apprenticeshipLevel"/>
            </p>
            <br/>
          </td>
          <td border="0" cellpadding="0" cellspacing="0" style="font-family: Helvetica, Arial, sans-serif;" valign="top">
            <xsl:element name="a">
              <xsl:attribute name="href">
                <xsl:value-of select="parameters/url"/>
              </xsl:attribute>
              <xsl:text>Run search again</xsl:text>
            </xsl:element>
          </td>
        </tr>
      </tbody>
    </table>
    <table align="center" border="0" cellpadding="0" cellspacing="0" width="580">
      <tbody>
        <xsl:apply-templates select="results"/>
      </tbody>
    </table>
  </xsl:template>

  <xsl:template match="results">
    <tr>
      <td border="0" cellpadding="0" cellspacing="0" style="font-family: Helvetica, Arial, sans-serif;color:#0b0c0c; border-bottom: 1px solid #BFC1C3;" valign="top">
        <br/>
        <xsl:element name="a">
          <xsl:attribute name="href">
            <xsl:value-of select="url"/>
          </xsl:attribute>
          <xsl:attribute name="style">
            <xsl:text>font-size: 19px; font-weight: bold;</xsl:text>
          </xsl:attribute>
          <xsl:value-of select="title"/>
        </xsl:element>
        <br/>
        <span style="color: #6F777B;">
          <xsl:value-of select="employerName"/>
        </span>
        <br/>
        <p>
          <xsl:value-of select="description"/>
        </p>
        <b>Distance: </b>
        <span>
          <xsl:value-of select="distance"/>
        </span> mile(s)
        <br/>
        <b>Closing date: </b>
        <xsl:value-of select="closingDate"/>
        <br/>
        <br/>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>