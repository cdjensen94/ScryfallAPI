using ScryfallAPI.Models;

namespace ScryfallAPI.Utilities
{
	public class AuthenticateUser
	{
		public ScryfallContext _context {  get; set; }
				
		public AuthenticateUser(ScryfallContext context)
		{
			_context = context;
		}

		public async Task<User> AuthenticateUsers(string email)
		{
			using ILoggerFactory loggerFactory = LoggerFactory.Create(b => b.AddConsole());
			ILogger logger = loggerFactory.CreateLogger<AuthenticateUser>();
			User newUser = new User
			{
				Email = ""
			};

			var searchingForUser = _context.Users
								   .Select(u => u)
								   .ToList();
			
			if(!searchingForUser.Select(u => u.Email).Contains(email)) 
			{
				newUser.Email = email;
				await _context.Users.AddAsync(newUser);
				await _context.SaveChangesAsync();
				logger.LogInformation($"User {newUser.Email} didn't exist, added to DB");
			}

			else
				searchingForUser.ForEach(u =>
				{
					if (u.Email == email)
					{
						newUser.Email = u.Email;
						newUser.Id = u.Id;
					}
				});

			return newUser;
			

		}
	}

}
