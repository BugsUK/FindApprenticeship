namespace SFA.Apprenticeships.Web.Common.Extensions
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    public static class MetadataExtensions
    {
        public static ModelMetadata GetMetadata<TViewModel, TValue>(this Type viewModel, Expression<Func<TViewModel, TValue>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(
                expression,
                new ViewDataDictionary<TViewModel>()
            );

            return metadata;
        }
        public static ModelMetadata GetMetadata<TViewModel, TValue>(this TViewModel viewModel, Expression<Func<TViewModel, TValue>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(
                expression,
                new ViewDataDictionary<TViewModel>()
            );

            return metadata;
        }
    }
}