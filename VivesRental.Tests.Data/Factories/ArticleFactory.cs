using VivesRental.Model;

namespace VivesRental.Tests.Data.Factories
{
    public static class ArticleFactory
    {
        public static Article CreateValidEntity(Product product)
        {
            return new Article
            {
                ProductId = product.Id,
                Product = product,
                Status = ArticleStatus.InRepair
            };
        }

        public static Article CreateInvalidEntity()
        {
            return new Article
            {
                Status = ArticleStatus.InRepair
            };
        }
    }
}
