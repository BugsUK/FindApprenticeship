namespace FrameworkDataImport.Entities
{
    using Nest;

    public class Framework
    {
        public string Name { get; set; }
        [ElasticProperty(Store = true, Index = FieldIndexOption.NotAnalyzed)]
        public int Number { get; set; }
        public string IssuingAuthority { get; set; }
        public string Sector { get; set; }
        public int[] Levels { get; set; }
    }
}