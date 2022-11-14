using Booking.Api.Services.Abstraction;
using System;

namespace Booking.Api.Services
{
    public class SingletonOperation : ISingletonOperation
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
    }
}
