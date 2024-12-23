using Microsoft.EntityFrameworkCore;

namespace Allison.Model
{
    public class AllisonContext : DbContext
    {
        public DbSet<MessageBubble> MessageBubble { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=AllisonData.db");
        }
    }

    public class MessageBubble
    {
        public int MessageBubbleId { get; set; }

        public bool To { get; set; }

        public bool Yo { get; set; }

        public bool Select { get; set; }

        public bool Youtube { get; set; }

        public bool News { get; set; }

        public string Message { get; set; }

        public string LiveTime { get; set; }

        //////Youtube

        public string VideoTitle { get; set; }

        public string VideoUrl { get; set; }

        public string VideoImage { get; set; }

        //////News

        public string NewsTitle { get; set; }

        public string NewsTime { get; set; }

        public string NewsUrl { get; set; }

        public string NewsImage { get; set; }
    }
}
