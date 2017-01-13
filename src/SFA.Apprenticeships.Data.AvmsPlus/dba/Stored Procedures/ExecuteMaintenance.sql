CREATE PROCEDURE dba.ExecuteMaintenance
  @Databases NVARCHAR(MAX)
AS
BEGIN
EXECUTE dba.IndexOptimize
        @Databases=@Databases,
        @FragmentationLow='INDEX_REORGANIZE',
        @FragmentationMedium='INDEX_REORGANIZE,INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
        @FragmentationHigh='INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
        @FragmentationLevel1=5,
        @FragmentationLevel2=30,
        @UpdateStatistics='ALL',
        @LogToTable='Y',
        @Execute='Y';
END
GO