using Backend_for_angular_CRUD.EFContextSQLServer;
using Backend_for_angular_CRUD.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend_for_angular_CRUD
{
	[ApiController]
	[Route("api/users")]
	public class UserController : ControllerBase
	{
		private readonly CRUDService<User, UsersContext> _crudController;
		private readonly UsersContext _userContext;

		public UserController(CRUDService<User, UsersContext> crudController, UsersContext userContext)
		{
			_crudController = crudController;
			_userContext = userContext;
		}

		[HttpGet]
		public async Task<IEnumerable<User>> GetAllUsers()
		{
			var result = await _crudController.SelectAsync();
			System.Diagnostics.Debug.WriteLine("All users sent");
			return result;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<User>> GetUserById(string id)
		{
			var users = await _crudController.SelectAsync(id);
			var userToFind = users.FirstOrDefault();
			if (userToFind != null)
			{
				System.Diagnostics.Debug.WriteLine($"{userToFind.Name}: Sent");
				return userToFind;
			}
			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> AddUser([FromBody] User user)
		{
			if (user != null)
			{
				user.Id = Guid.NewGuid();
				await _crudController.ADD(user);
				return Ok();
			}
			return BadRequest("Invalid user data");
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUser(string id)
		{
			await _crudController.Remove(id);
			System.Diagnostics.Debug.WriteLine($"User with id: {id} removed");
			return NoContent();
		}

		[HttpPut]
		public async Task<IActionResult> UpdateUser([FromBody] User user)
		{
			if (user != null)
			{
				var foundUser = _userContext.Users.FirstOrDefault(x => x.Id == user.Id);
				if (foundUser != null)
				{
					await _crudController.AttachUpdate(user, foundUser);
					return Ok();
				}
				return NotFound();
			}
			return BadRequest("Invalid user data");
		}
	}
}
