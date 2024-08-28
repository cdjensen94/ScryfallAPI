using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScryfallAPI.Models;
using ScryfallAPI.Utilities;
using System.Linq.Expressions;
using System.Security.Claims;

namespace ScryfallAPI.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
		public ScryfallContext _context { get; set; }
	
		public IndexModel(ScryfallContext context) 
		{
			_context = context;
		}

        public void OnGet()
        {

        }

		public User GetUser(int  id) 
		{ 
			var searching =  _context.Users.Where(u => u.Id == id)
										   .FirstOrDefault();

			_context.Entry(searching).State = EntityState.Detached;

			return searching ?? new User { Email = ""};
		}

		public void OnPostSend(string name, int pennyRank, string releasedDate)
		{
			using ILoggerFactory loggerFactory = LoggerFactory.Create(b => b.AddConsole());
			ILogger logger = loggerFactory.CreateLogger<IndexModel>();
			var retrievingClaims = User.Claims.FirstOrDefault(c => c.Type == "UserId");
			string returnUrl = "https://localhost:7224/index";
			var searchingForUser = GetUser(Convert.ToInt32(retrievingClaims.Value));
			User user = new()
			{
				Email = ""
			};

			if(searchingForUser is not null )
			{
				user.Email = searchingForUser.Email;
				user.Id = searchingForUser.Id;
			}

			FavoriteCards favoriteCards = new FavoriteCards()
			{
				IsFavorite = true,
				Name = name,
				PennyRank = pennyRank,
				ReleasedAt = releasedDate,
				UserId = Convert.ToInt32(retrievingClaims?.Value),
				User = user
			};

			user.Favorites = new List<FavoriteCards> { favoriteCards };
			favoriteCards.User.Favorites = user.Favorites;
			_context.Entry(favoriteCards).State = EntityState.Modified;
			_context.Favorites.Add(favoriteCards);
			_context.SaveChanges();

			logger.LogInformation($"{favoriteCards.Name} marked as favorite in the db");

			Redirect(returnUrl);



		}

	}
}
