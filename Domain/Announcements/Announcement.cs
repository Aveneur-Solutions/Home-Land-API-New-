using System;

namespace Domain.Announcements
{
    public class Announcement
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FileLocation { get; set; }
        public DateTime TimeUploaded { get; set; }
    }
}