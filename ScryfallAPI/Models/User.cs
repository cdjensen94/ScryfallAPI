namespace ScryfallAPI.Models
{
	public class User
	{
		public int Id { get; set; }
		public required string Email { get; set; }
		public List<FavoriteCards>? Favorites { get; set; }
	}
}
