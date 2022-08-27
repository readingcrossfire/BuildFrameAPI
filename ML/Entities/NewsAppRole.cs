using Microsoft.AspNetCore.Identity;

namespace ML.Entities
{
    public class NewsAppRole : IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}