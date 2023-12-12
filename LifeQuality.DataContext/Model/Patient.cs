using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuality.DataContext.Model
{
    public class Patient : User
    {
        public string? AdditioanlInfo { get; set; }
        public string? Address { get; set; }

        public Doctor Doctor { get; set; }
        public int DoctorId { get; set; }
        public Patron Patron { get; set; }
        public int PatronId { get; set; }

        public double Height { get; set; }
        public double Weight { get; set; }
        public string BloodType { get; set; }
    }
}
