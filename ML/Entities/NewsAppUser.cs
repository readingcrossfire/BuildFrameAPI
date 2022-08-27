using Microsoft.AspNetCore.Identity;

namespace ML.Entities
{
    public class NewsAppUser : IdentityUser<Guid>
    {
        public string FullName { get; set; }
    }
}