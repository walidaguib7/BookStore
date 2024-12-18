

using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace api.models
{
    public class EmailVerification
    {
        public int Id { get; set; }
        public int code { get; set; }
        public string userId { get; set; }
        public User user { get; set; }


    }
}