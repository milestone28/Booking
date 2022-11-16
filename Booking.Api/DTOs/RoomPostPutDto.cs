namespace Booking.Api.DTOs
{
    public class RoomPostPutDto
    {
        public int RoomNumber { get; set; }
        public double Surface { get; set; }
        public bool NeedsRepair { get; set; }
    }
}
