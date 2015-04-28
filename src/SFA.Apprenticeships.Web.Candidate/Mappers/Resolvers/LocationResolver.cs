namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using AutoMapper;
    using Domain.Entities.Locations;
    using ViewModels.VacancySearch;
    internal class LocationResolver : ITypeConverter<VacancySearchViewModel, Location>
    {
        public Location Convert(ResolutionContext context)
        {
            var viewModel = (VacancySearchViewModel)context.SourceValue;

            if (string.IsNullOrWhiteSpace(viewModel.Location) && !viewModel.Latitude.HasValue && !viewModel.Longitude.HasValue)
            {
                return null;
            }

            var location = new Location
            {
                Name = viewModel.Location,
                GeoPoint =
                    new GeoPoint
                    {
                        Latitude = viewModel.Latitude.GetValueOrDefault(),
                        Longitude = viewModel.Longitude.GetValueOrDefault()
                    }
            };

            return location;
        }
    }
}