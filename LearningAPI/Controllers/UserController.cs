using System.Threading.Tasks;
using LearningAPI.Models;
using LearningAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LearningAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("/{id}")]
        public async Task<ActionResult<User>> GetById(int id) => Ok(await _userService.GetById(id));

        [HttpGet("/role/{role}")]
        public async Task<ActionResult<List<User>>> GetByRole(String role) =>
            Ok(await _userService.GetByRole(role));

        [HttpPost]
        public async Task<ActionResult> Add(User user)
        {
            if (user == null)
                return Ok(
                    new
                    {
                        status = 500,
                        type = "error",
                        message = "user did not added",
                        data = new { },
                    }
                );

            await _userService.Add(user);

            return Ok(
                new
                {
                    status = 200,
                    type = "sucess",
                    message = "user has been added successfully",
                    data = user,
                }
            );
        }

        [HttpPut]
        public async Task<ActionResult> Update(User user)
        {
            try
            {
                await _userService.Update(user);
                return Ok(
                    new
                    {
                        status = 200,
                        type = "sucess",
                        message = "user has been updated successfully",
                        data = user,
                    }
                );
            }
            catch (Exception e)
            {
                return StatusCode(
                    500,
                    new
                    {
                        status = 500,
                        type = "error",
                        message = e.Message,
                        data = new { },
                    }
                );
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _userService.Delete(id);
            return Ok($"User with id = {id} has been deleted");
        }

        [HttpPatch]
        public async Task<ActionResult> UpdateRole(int id, String role)
        {
            await _userService.UpdateRole(id, role);
            return Ok($"User with id = {id} role has been updated to the role of {role}");
        }

        [HttpDelete("deleteUsers")]
        public async Task<ActionResult> DeleteUsers([FromBody] int[] userIds)
        {
            if (userIds == null || userIds.Length == 0)
            {
                return BadRequest();
            }

            await _userService.DeleteUsers(userIds);
            return Ok();
        }
    }
}
