<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="html" indent="yes"/>
  <xsl:template match="savedSearchAlerts">
    <div>
      <xsl:apply-templates select="savedSearchAlert"/>
    </div>
  </xsl:template>

  <xsl:template match="savedSearchAlert">
    <br/>
    <table align="center" border="0" cellpadding="0" cellspacing="0" width="580">
      <colgroup>
        <col style="width:70%"/>
        <col style="width:30%"/>
        <col/>
      </colgroup>
      <tbody>
        <tr>
          <td border="0" cellpadding="0" cellspacing="0" style="font-family: Helvetica, Arial, sans-serif;color:#0b0c0c;" valign="top">
            <h2 style="font-size: 18px; margin: 0; padding: 0;">
              Top <span>
                <xsl:value-of select="resultsCount"/>
              </span> result(s) matching your search:
            </h2>
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
        <tr>
          <td border="0" cellpadding="0" cellspacing="0" style="font-family: Helvetica, Arial, sans-serif;color:#0b0c0c;" valign="top">
            <p>
              <xsl:value-of select="parameters/name"/>
              <xsl:if test="parameters/subCategoriesFullName != ''">
                <br />
                <b>Sub-categories: </b>
                <xsl:value-of select="parameters/subCategoriesFullName"/>
              </xsl:if>
              <br />
              <b>Apprenticeship level: </b>
              <xsl:value-of select="parameters/apprenticeshipLevel"/>
            </p>
            <br/>
          </td>
        </tr>
        <xsl:apply-templates select="results"/>
      </tbody>
    </table>
  </xsl:template>

  <xsl:template match="results">
    <tr>
      <td border="0" cellpadding="0" cellspacing="0" style="font-family: Helvetica, Arial, sans-serif;color:#0b0c0c; border-bottom: 1px solid #BFC1C3;" valign="top">
        <xsl:element name="a">
          <xsl:attribute name="href">
            <xsl:value-of select="url"/>
          </xsl:attribute>
          <xsl:attribute name="style">
            <xsl:text>font-size: 19px; font-weight: bold;</xsl:text>
          </xsl:attribute>
          <xsl:value-of select="title"/>
          <xsl:if test="displaySubCategory='true'">
            <span style="font-size: 19px;">
              (<xsl:value-of select="subCategory"/>)
            </span>
          </xsl:if>
        </xsl:element>
        <br/>
        <span style="color: #6F777B;">
          <xsl:value-of select="employerName"/><span style="font-size: 12px;"> (Added <xsl:value-of select="postedDate"/>)</span>
        </span>
        <br/>
        <xsl:if test="displayDescription='true'">
          <p>
            <xsl:value-of select="description"/>
            <xsl:if test="numberOfPositions > 1">
              <span style="font-size: 19px;">
                (<xsl:value-of select="numberOfPositions"/> positions available)
              </span>
            </xsl:if>
          </p>
        </xsl:if>
        <xsl:if test="displayDistance='true'">
          <b>Distance: </b>
          <span>
            <xsl:value-of select="distance"/>
          </span> mile(s)
          <br/>
        </xsl:if>
        <xsl:if test="displayClosingDate='true'">
          <b>Closing date: </b>
          <xsl:value-of select="closingDate"/>
          <br/>
        </xsl:if>
        <xsl:if test="displayStartDate='true'">
          <b>Possible start date: </b>
          <xsl:value-of select="startDate"/>
          <br/>
        </xsl:if>
        <xsl:if test="displayApprenticeshipLevel='true'">
          <b>Apprenticeship level: </b>
          <xsl:value-of select="apprenticeshipLevel"/>
          <br/>
        </xsl:if>
        <xsl:if test="displayWage='true'">
          <b>Wage: </b>
          <xsl:value-of select="wage"/> p/week
          <br/>
        </xsl:if>
        <br/>
      </td>
    </tr>
    <tr>
      <td>
        <xsl:text disable-output-escaping="yes">&amp;</xsl:text>nbsp;
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>