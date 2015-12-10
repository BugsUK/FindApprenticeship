MERGE WebProxy.WebProxyConsumer AS dest
USING (
	SELECT '14950389-76FB-4429-956E-188A35220019', '', '', '') AS src (
		ExternalSystemId,
		ShortDescription,
		FullDescription,
		RouteToCompatabilityWebServiceRegex
	)
ON (dest.ExternalSystemId = src.ExternalSystemId)
WHEN MATCHED THEN
	UPDATE SET
		ExternalSystemId = src.ExternalSystemId,
		ShortDescription = src.ShortDescription,
		FullDescription = src.FullDescription,
		RouteToCompatabilityWebServiceRegex = src.RouteToCompatabilityWebServiceRegex
WHEN NOT MATCHED THEN
	INSERT (
		ExternalSystemId,
		ShortDescription,
		FullDescription,
		RouteToCompatabilityWebServiceRegex
	)
	VALUES (
		src.ExternalSystemId,
		src.ShortDescription,
		src.FullDescription,
		src.RouteToCompatabilityWebServiceRegex
	)
;

SELECT * FROM WebProxy.WebProxyConsumer
