using System;

namespace VivesRental.Repository.Includes
{
    public class ArticleIncludes
    {
        public bool Product { get; set; }
        [Obsolete("Collection includes are not an efficient way to retrieve data. Will be removed in the near future.")]
		public bool OrderLines { get; set; }
    }
}
