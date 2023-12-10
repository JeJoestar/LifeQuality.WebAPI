namespace LifeQuality.WebAPI.DTOs.Users
{
    public class PatientCreateDto
    {
        public string? ProfileImageUrl { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
