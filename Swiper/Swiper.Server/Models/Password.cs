namespace Swiper.Server.Models
{
    public class Password
    {
        public string Hash { get; set; }
        public string Salt { get; set; }

        public Password(string Hash, string Salt)
        {
            this.Hash = Hash;
            this.Salt = Salt;
        }
    }
}
