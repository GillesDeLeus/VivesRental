using System;

namespace VivesRental.Model.Contracts
{
    public interface IIdentifiable
    {
        Guid Id { get; set; }
    }
}
