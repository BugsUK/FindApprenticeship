namespace SFA.Apprenticeships.Web.ContactForms.Mappers.Interfaces
{
    public interface IDomainToViewModelMapper<TSource, TDestination>
    {
        TDestination ConvertToViewModel(TSource domain);        
    }
}
