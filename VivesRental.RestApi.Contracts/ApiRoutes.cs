namespace VivesRental.RestApi.Contracts
{
    public static class ApiRoutes
    {
        private const string Version = "v1";
        private const string Root = "api";
        private const string Base = Root + "/" + Version;

        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            public const string Register = Base + "/identity/register";
            public const string Refresh = Base + "/identity/refresh";
        }

        public static class Customer
        {
            public const string GetAll = Base + "/customers";
            public const string Get = Base + "/customers/{id}";
            public const string Create = Base + "/customers";
            public const string Update = Base + "/customers/{id}";
        }

        public static class Item
        {
            public const string GetAll = Base + "/items";
            public const string Get = Base + "/items/{id}";
            public const string Create = Base + "/items";
            public const string Update = Base + "/items/{id}";
        }

        public static class RentalItem
        {
            public const string GetAll = Base + "/rentalitems";
            public const string Get = Base + "/rentalitems/{id}";
            public const string Create = Base + "/rentalitems";
            public const string Update = Base + "/rentalitems/{id}";
            public const string Delete = Base + "/rentalitems/{id}";
        }

        public static class RentalOrder
        {
            public const string GetAll = Base + "/rentalorders";
            public const string Get = Base + "/rentalorders/{id}";
            public const string Create = Base + "/rentalorders";
            public const string Update = Base + "/rentalorders/{id}";
            public const string Delete = Base + "/rentalorders/{id}";
        }

        public static class RentalOrderLine
        {
            public const string GetAll = Base + "/rentalorderlines";
            public const string Get = Base + "/rentalorderlines/{id}";
            public const string Create = Base + "/rentalorderlines";
            public const string Update = Base + "/rentalorderlines/{id}";
            public const string Delete = Base + "/rentalorderlines/{id}";
        }

    }
}
