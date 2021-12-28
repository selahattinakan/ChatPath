namespace ChatPath.Entity
{
    public class Channel
    {
        public int ChannelID { get; set; }
        public string ChannelName { get; set; }
        public string ChannelDescription { get; set; }
        public string AvatarFileName { get; set; }
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Kanal id'lerinin htmlde görünmesi problem teşkil etseydi id'nin şifrelenmiş halini Enc property'inde tutacaktım,
        /// işlemler bunun üstünden yapılacaktı
        /// </summary>
        public string Enc { get; set; }
    }
}
