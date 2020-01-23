using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VivesRental.Model;
using VivesRental.Repository.Includes;
using VivesRental.Services.Results;

namespace VivesRental.Services.Contracts
{
    public interface IArticleReservationService
    {
        Task<ArticleReservationResult> GetAsync(Guid id);
        Task<ArticleReservationResult> GetAsync(Guid id, ArticleReservationIncludes includes);
        Task<List<ArticleReservationResult>> AllAsync();
        Task<List<ArticleReservationResult>> AllAsync(ArticleReservationIncludes includes);
        Task<ArticleReservationResult> CreateAsync(Guid customerId, Guid articleId);
        Task<ArticleReservationResult> CreateAsync(ArticleReservation entity);
        Task<bool> RemoveAsync(Guid id);
    }
}
