using Booking.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Dal
{
    public class DataContext : DbContext //snapshot of the database
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        //represent the table
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

    }
}
