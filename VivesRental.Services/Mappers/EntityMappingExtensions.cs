using System;
using VivesRental.Model;
using VivesRental.Services.Results;

namespace VivesRental.Services.Mappers
{
    /// <summary>
    /// https://stackoverflow.com/questions/39585427/projection-of-single-entities-in-ef-with-extension-methods
    /// </summary>
    public static class EntityMappingExtensions
    {
        public static ArticleResult MapToResult(this Article article, DateTime fromDateTime, DateTime? untilDateTime)
        {
            if (article == null)
            {
                return null;
            }

            return ProjectionExpressions.ProjectToArticleResult(fromDateTime, untilDateTime).Compile()(article);
        }

        
       
        public static ArticleReservationResult MapToResult(this ArticleReservation articleReservation, DateTime fromDateTime, DateTime? untilDateTime)
        {
            if (articleReservation == null)
            {
                return null;
            }

            return ProjectionExpressions.ProjectToArticleReservationResult(fromDateTime, untilDateTime).Compile()(articleReservation);
        }

        

        public static CustomerResult MapToResult(this Customer customer)
        {
            if (customer == null)
            {
                return null;
            }

            return ProjectionExpressions.ProjectToCustomerResult().Compile()(customer);
        }

        public static OrderResult MapToResult(this Order order)
        {
            if (order == null)
            {
                return null;
            }

            return ProjectionExpressions.ProjectToOrderResult().Compile()(order);
        }

        public static OrderLineResult MapToResult(this OrderLine orderLine, DateTime fromDateTime, DateTime? untilDateTime)
        {
            if (orderLine == null)
            {
                return null;
            }

            return ProjectionExpressions.ProjectToOrderLineResult(fromDateTime, untilDateTime).Compile()(orderLine);
        }

        public static ProductResult MapToResult(this Product product, DateTime fromDateTime, DateTime? untilDateTime)
        {
            if (product == null)
            {
                return null;
            }

            return ProjectionExpressions.ProjectToProductResult(fromDateTime, untilDateTime).Compile()(product);
        }
    }
}
