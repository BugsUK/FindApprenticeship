namespace SFA.Apprenticeships.Web.Raa.Common.Mappers.Resolvers
{
    using AutoMapper;

    public class NullableIntToNullableDecimalResolver : ValueResolver<int?, decimal?>
    {
        protected override decimal? ResolveCore(int? source)
        {
            return source.HasValue ? (decimal?)source.Value : null;
        }
    }
}
