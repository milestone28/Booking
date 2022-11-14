using Booking.Api.Services.Abstraction;
using System;

namespace Booking.Api.Services
{
    public class ScopedOperation : IScopedOperation
    {
        public Guid Guid { get ; set; } = Guid.NewGuid();
    }
}
