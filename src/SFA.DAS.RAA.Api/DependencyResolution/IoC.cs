namespace SFA.DAS.RAA.Api.DependencyResolution
{
    using StructureMap;

    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c => c.AddRegistry<RaaRegistry>());
        }
    }
}