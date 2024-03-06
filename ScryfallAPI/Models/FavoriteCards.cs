namespace ScryfallAPI.Models
{
    public class FavoriteCards
    {
        public int Id { get; set; }
        public bool IsFavorite { get; set; } 
        public string? Name{ get; set; }
        public string? ReleasedAt { get; set; }
        public int PennyRank { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
