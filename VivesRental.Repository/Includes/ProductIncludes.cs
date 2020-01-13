using System;

namespace VivesRental.Repository.Includes
{
	public class ProductIncludes
	{
        [Obsolete("Collection includes are not an efficient way to retrieve data. Will be removed in the near future.")]
		public bool Articles { get; set; }
        [Obsolete("Collection includes are not an efficient way to retrieve data. Will be removed in the near future.")]
		public bool ArticleOrderLines { get; set; }
	}
}
