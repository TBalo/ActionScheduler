using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TODO_LIST.Models;

namespace TODO_LIST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListController : ControllerBase
    {
        private readonly TodoListContext _context;

        public TodoListController(TodoListContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet("GetUserTasksAsc/{userId}")]
        public async Task<ActionResult<IEnumerable<TodoListResponseDto>>> GetTodoListsAscByUser(int userId)
        {
            if (_context.TodoLists == null)
            {
                return NotFound();
            }

            var userTodoLists = await _context.TodoLists
                .Where(t => t.UserId == userId)
                .OrderBy(m => m.DueDate)
                .ToListAsync();

            if (userTodoLists == null || !userTodoLists.Any())
            {
                return NotFound("No todo lists found for the specified user.");
            }

            var responseDtos = userTodoLists.Select(todoList => new TodoListResponseDto
            {
                ListId = todoList.ListId,
                Task = todoList.Task,
                Status = todoList.Status,
                DueDate = todoList.DueDate,
                UserId = todoList.UserId,
                UserName = _context.Users.FirstOrDefault(u => u.UserId == todoList.UserId)?.UserName ?? "Unknown",
                Email = _context.Users.FirstOrDefault(u => u.UserId == todoList.UserId)?.Email ?? "No Email"
            });

            return Ok(responseDtos);
        }

        [HttpGet("GetUserTasksDesc/{userId}")]
        public async Task<ActionResult<IEnumerable<TodoListResponseDto>>> GetTodoListsDescByUser(int userId)
        {
            if (_context.TodoLists == null)
            {
                return NotFound();
            }

            var userTodoLists = await _context.TodoLists
                .Where(t => t.UserId == userId)
                .OrderByDescending(m => m.DueDate)
                .ToListAsync();

            if (userTodoLists == null || !userTodoLists.Any())
            {
                return NotFound("No todo lists found for the specified user.");
            }

            var responseDtos = userTodoLists.Select(todoList => new TodoListResponseDto
            {
                ListId = todoList.ListId,
                Task = todoList.Task,
                Status = todoList.Status,
                DueDate = todoList.DueDate,
                UserId = todoList.UserId,
                UserName = _context.Users.FirstOrDefault(u => u.UserId == todoList.UserId)?.UserName ?? "Unknown",
                Email = _context.Users.FirstOrDefault(u => u.UserId == todoList.UserId)?.Email ?? "No Email"
            });

            return Ok(responseDtos);
        }

        [HttpGet("GetUserTasks/{userId}")]
        public async Task<ActionResult<IEnumerable<TodoListResponseDto>>> GetTodoListsByUser(int userId)
        {
            if (_context.TodoLists == null)
            {
                return NotFound();
            }

            var userTodoLists = await _context.TodoLists
                .Where(t => t.UserId == userId)
                .OrderBy(m => m.DueDate)
                .ToListAsync();

            if (userTodoLists == null || !userTodoLists.Any())
            {
                return NotFound("No todo lists found for the specified user.");
            }

            var responseDtos = userTodoLists.Select(todoList => new TodoListResponseDto
            {
                ListId = todoList.ListId,
                Task = todoList.Task,
                Status = todoList.Status,
                DueDate = todoList.DueDate,
                UserId = todoList.UserId,
                UserName = _context.Users.FirstOrDefault(u => u.UserId == todoList.UserId)?.UserName ?? "Unknown",
                Email = _context.Users.FirstOrDefault(u => u.UserId == todoList.UserId)?.Email ?? "No Email"
            });

            return Ok(responseDtos);
        }

        [HttpGet("GetTaskbyId{id}")]
        public async Task<ActionResult<TodoListResponseDto>> GetTodoList(int id)
        {
            if (_context.TodoLists == null)
            {
                return NotFound();
            }

            var todoList = await _context.TodoLists.FindAsync(id);

            if (todoList == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(todoList.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var responseDto = new TodoListResponseDto
            {
                ListId = todoList.ListId,
                Task = todoList.Task,
                Status = todoList.Status,
                DueDate = todoList.DueDate,
                UserId = todoList.UserId,
                UserName = user.UserName ?? "Unknown",
                Email = user.Email ?? "No Email"
            };

            return Ok(responseDto);
        }

        [HttpPut("UpdateTask{id}")]
        public async Task<IActionResult> PutTodoList(int id, TodoListDto todoListDto)
        {
            if (id != todoListDto.ListId)
            {
                return BadRequest();
            }

            var todoList = await _context.TodoLists.FindAsync(id);
            if (todoList == null)
            {
                return NotFound();
            }

            todoList.Task = todoListDto.Task ?? todoList.Task;
            todoList.Status = todoListDto.Status;
            todoList.DueDate = todoListDto.DueDate;
            todoList.UserId = todoListDto.UserId;

            _context.Entry(todoList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("CreateTask")]
        public async Task<ActionResult<TodoListResponseDto>> PostTodoList(TodoListDto todoListDto)
        {
            if (_context.TodoLists == null)
            {
                return Problem("Entity set 'TodoListContext.TodoLists' is null.");
            }

            var user = await _context.Users.FindAsync(todoListDto.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var todoList = new TodoList
            {
                Task = todoListDto.Task,
                Status = todoListDto.Status,
                DueDate = todoListDto.DueDate,
                UserId = todoListDto.UserId
            };

            try
            {
                _context.TodoLists.Add(todoList);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            var responseDto = new TodoListResponseDto
            {
                ListId = todoList.ListId,
                Task = todoList.Task,
                Status = todoList.Status,
                DueDate = todoList.DueDate,
                UserId = todoList.UserId,
                UserName = user.UserName ?? "Unknown",
                Email = user.Email ?? "No Email"
            };

            return CreatedAtAction("GetTodoList", new { id = todoList.ListId }, responseDto);
        }

        [HttpDelete("Delete{id}")]
        public async Task<IActionResult> DeleteTodoList(int id)
        {
            if (_context.TodoLists == null)
            {
                return NotFound();
            }

            var todoList = await _context.TodoLists.FindAsync(id);
            if (todoList == null)
            {
                return NotFound();
            }

            _context.TodoLists.Remove(todoList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoListExists(int id)
        {
            return (_context.TodoLists?.Any(e => e.ListId == id)).GetValueOrDefault();
        }
    }

    public class TodoListDto
    {
        public int ListId { get; set; }
        public string Task { get; set; } = string.Empty;
        public bool Status { get; set; }
        public DateTime DueDate { get; set; }
        public int UserId { get; set; }
    }

    public class TodoListResponseDto
    {
        public int ListId { get; set; }
        public string Task { get; set; } = string.Empty;
        public bool Status { get; set; }
        public DateTime DueDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = "Unknown";
        public string Email { get; set; } = "No Email";
    }

}
