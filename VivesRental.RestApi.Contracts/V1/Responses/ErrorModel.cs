using System;
using System.Collections.Generic;
using System.Text;

namespace VivesRental.RestApi.Contracts.V1.Responses
{
    public class ErrorModel
    {
        public string FieldName { get; set; }
        public string Message { get; set; }
    }
}
