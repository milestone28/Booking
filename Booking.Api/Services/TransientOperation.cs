using Booking.Api.Services.Abstraction;
using System;

namespace Booking.Api.Services
{
    public class TransientOperation : ITransientOperation
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
    }
}
