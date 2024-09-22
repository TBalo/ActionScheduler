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
            _context = context;
        }

        // GET: api/TodoList/user/asc/{userId}
        // GET: api/TodoList/user/asc/{userId}
        [HttpGet("user/asc/{userId}")]
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

            // Map to response DTO
            var responseDtos = userTodoLists.Select(todoList => new TodoListResponseDto
            {
                ListId = todoList.ListId,
                TaskDescription = todoList.TaskDescription,
                Status = todoList.Status,
                DueDate = todoList.DueDate,
                UserId = todoList.UserId,
                UserName = _context.Users.FirstOrDefault(u => u.UserId == todoList.UserId)?.UserName,
                Email = _context.Users.FirstOrDefault(u => u.UserId == todoList.UserId)?.Email
            });

            return Ok(responseDtos);
        }

        // GET: api/TodoList/user/desc/{userId}
        [HttpGet("user/desc/{userId}")]
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

            // Map to response DTO
            var responseDtos = userTodoLists.Select(todoList => new TodoListResponseDto
            {
                ListId = todoList.ListId,
                TaskDescription = todoList.TaskDescription,
                Status = todoList.Status,
                DueDate = todoList.DueDate,
                UserId = todoList.UserId,
                UserName = _context.Users.FirstOrDefault(u => u.UserId == todoList.UserId)?.UserName,
                Email = _context.Users.FirstOrDefault(u => u.UserId == todoList.UserId)?.Email
            });

            return Ok(responseDtos);
        }

        // GET: api/TodoList/user/{userId}
        [HttpGet("user/{userId}")]
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

            // Map to response DTO
            var responseDtos = userTodoLists.Select(todoList => new TodoListResponseDto
            {
                ListId = todoList.ListId,
                TaskDescription = todoList.TaskDescription,
                Status = todoList.Status,
                DueDate = todoList.DueDate,
                UserId = todoList.UserId,
                UserName = _context.Users.FirstOrDefault(u => u.UserId == todoList.UserId)?.UserName,
                Email = _context.Users.FirstOrDefault(u => u.UserId == todoList.UserId)?.Email
            });

            return Ok(responseDtos);
        }


        // GET: api/TodoList/5
        [HttpGet("{id}")]
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

            // Get the associated user
            var user = await _context.Users.FindAsync(todoList.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Map the TodoList entity to TodoListResponseDto
            var responseDto = new TodoListResponseDto
            {
                ListId = todoList.ListId,
                TaskDescription = todoList.TaskDescription,
                Status = todoList.Status,
                DueDate = todoList.DueDate,
                UserId = todoList.UserId,
                UserName = user.UserName,  // Include necessary user fields only
                Email = user.Email
            };

            return Ok(responseDto);
        }


        // PUT: api/TodoList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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

            // Map the DTO to the actual TodoList entity
            todoList.TaskDescription = todoListDto.TaskDescription;
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


        // POST: api/TodoList
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoListResponseDto>> PostTodoList(TodoListDto todoListDto)
        {
            if (_context.TodoLists == null)
            {
                return Problem("Entity set 'TodoListContext.TodoLists' is null.");
            }

            // Check if the user exists
            var user = await _context.Users.FindAsync(todoListDto.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Map the DTO to the actual TodoList entity
            var todoList = new TodoList
            {
                TaskDescription = todoListDto.TaskDescription,
                Status = todoListDto.Status,
                DueDate = todoListDto.DueDate,
                UserId = todoListDto.UserId
            };

            _context.TodoLists.Add(todoList);
            await _context.SaveChangesAsync();

            // Prepare the response DTO, exclude the password
            var responseDto = new TodoListResponseDto
            {
                ListId = todoList.ListId,
                TaskDescription = todoList.TaskDescription,
                Status = todoList.Status,
                DueDate = todoList.DueDate,
                UserId = todoList.UserId,
                UserName = user.UserName,  // Include necessary user fields only
                Email = user.Email
            };

            return CreatedAtAction("GetTodoList", new { id = todoList.ListId }, responseDto);
        }



        // DELETE: api/TodoList/5
        [HttpDelete("{id}")]
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
}


public class TodoListDto
{
    public int ListId { get; set; }
    public string TaskDescription { get; set; }
    public bool Status { get; set; }
    public DateTime DueDate { get; set; }
    public int UserId { get; set; }
}

public class TodoListResponseDto
{
    public int ListId { get; set; }
    public string TaskDescription { get; set; }
    public bool Status { get; set; }
    public DateTime DueDate { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
}
