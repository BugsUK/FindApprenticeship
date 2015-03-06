namespace SFA.Apprenticeships.Web.ContactForms.Mappers.Interfaces
{
    public interface IViewModelToDomainMapper<TSource, TDestination>
    {
        TDestination ConvertToDomain(TSource viewModel);                
    }
}