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
                return NotFound(new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "User not found.",
                    Data = null
                });
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

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteTodoList([FromQuery] int id)
        {
            if (_context.TodoLists == null)
            {
                return NotFound(new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "No item has been added Todo.",
                    Data = null
                });
            }

            var todoList = await _context.TodoLists.FindAsync(id);
            if (todoList == null)
            {
                return NotFound(new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Task does not exist.",
                    Data = null
                });
            }

            _context.TodoLists.Remove(todoList);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<string>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Task deleted successfully.",
                Data = null
            });

        }
        
        [HttpPut("UpdateTask")]
        public async Task<ActionResult<TodoListResponseDto>> PutTodoList([FromQuery] int id, [FromBody] TodoListDto todoListDto)
        {
            if (id != todoListDto.ListId)
            {
                return BadRequest();
            }

            var todoList = await _context.TodoLists.FindAsync(id);
            if (todoList == null)
            {
                return NotFound(new ApiResponse<IEnumerable<TodoListResponseDto>>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "No item has been added Todo.",
                    Data = null
                });
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
                    return NotFound(new ApiResponse<IEnumerable<TodoListResponseDto>>
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Task does not exist.",
                        Data = null
                    });
                }
                else
                {
                    throw;
                }
            }

            var user = await _context.Users.FindAsync(todoList.UserId);
            var responseDto = new TodoListResponseDto
            {
                ListId = todoList.ListId,
                Task = todoList.Task,
                Status = todoList.Status,
                DueDate = todoList.DueDate,
                UserId = todoList.UserId,
                UserName = user?.UserName ?? "Unknown",
                Email = user?.Email ?? "No Email"
            };

            return Ok(new ApiResponse<TodoListResponseDto>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Task Updated successfully",
                Data = responseDto
            });
        }


        [HttpGet("GetUserTasksAsc")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TodoListResponseDto>>>> GetTodoListsAscByUser([FromQuery] int userId)
        {
            if (_context.TodoLists == null)
            {
                return NotFound(new ApiResponse<IEnumerable<TodoListResponseDto>>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Task not found.",
                    Data = new List<TodoListResponseDto>()
                });
            }

            var userTodoLists = await _context.TodoLists
                .Where(t => t.UserId == userId)
                .OrderBy(m => m.DueDate)
                .ToListAsync();

            if (userTodoLists == null || !userTodoLists.Any())
            {
                return NotFound(new ApiResponse<IEnumerable<TodoListResponseDto>>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "No Task found for the specified user.",
                    Data = null
                });
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

            return Ok(new ApiResponse<IEnumerable<TodoListResponseDto>>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Task retrieved successfully.",
                Data = responseDtos
            });
        }


        [HttpGet("GetUserTasksDesc")]
        public async Task<ActionResult<IEnumerable<TodoListResponseDto>>> GetTodoListsDescByUser([FromQuery] int userId)
        {
            if (_context.TodoLists == null)
            {
                return NotFound(new ApiResponse<IEnumerable<TodoListResponseDto>>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Task not found.",
                    Data = new List<TodoListResponseDto>()
                });
            }

            var userTodoLists = await _context.TodoLists
                .Where(t => t.UserId == userId)
                .OrderByDescending(m => m.DueDate)
                .ToListAsync();

            if (userTodoLists == null || !userTodoLists.Any())
            {
                return NotFound(new ApiResponse<IEnumerable<TodoListResponseDto>>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "No Task found for the specified user.",
                    Data = null
                });
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

            return Ok(new ApiResponse<IEnumerable<TodoListResponseDto>>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Task retrieved successfully.",
                Data = responseDtos
            });
        }

        [HttpGet("GetUserTasks")]
        public async Task<ActionResult<IEnumerable<TodoListResponseDto>>> GetTodoListsByUser([FromQuery] int userId)
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
                return NotFound("No Task found for the specified user.");
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

            return Ok(new ApiResponse<IEnumerable<TodoListResponseDto>>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Task retrieved successfully.",
                Data = responseDtos
            });
        }

        [HttpGet("GetTaskbyId{id}")]
        public async Task<ActionResult<TodoListResponseDto>> GetTodoList(int id)
        {
            if (_context.TodoLists == null)
            {
                return NotFound(new ApiResponse<IEnumerable<TodoListResponseDto>>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "No task has been added here.",
                    Data = null
                });
            }

            var todoList = await _context.TodoLists.FindAsync(id);

            if (todoList == null)
            {
                return NotFound(new ApiResponse<IEnumerable<TodoListResponseDto>>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Task does not exist.",
                    Data = null
                });
            }

            var user = await _context.Users.FindAsync(todoList.UserId);
            if (user == null)
            {
                return NotFound(new ApiResponse<IEnumerable<TodoListResponseDto>>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "User Not found.",
                    Data = new List<TodoListResponseDto>()
                });
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

            return Ok(new ApiResponse<TodoListResponseDto>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Task retrieved successfully.",
                Data = responseDto
            });
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

    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; } = default!;
    }

}
