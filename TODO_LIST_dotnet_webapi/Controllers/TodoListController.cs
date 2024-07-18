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

        // GET: api/TodoList/Asc
        [Route("GetTodoListsAsc")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoList>>> GetTodoListsAsc()
        {
          if (_context.TodoLists == null)
          {
              return NotFound();
          }
            return await _context.TodoLists.OrderBy(m=>m.DueDate).ToListAsync();
        }

        // GET: api/TodoList/desc
        [Route("GetTodoListsDesc")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoList>>> GetTodoListsDesc()
        {
            if (_context.TodoLists == null)
            {
                return NotFound();
            }
            return await _context.TodoLists.OrderByDescending(m => m.DueDate).ToListAsync();
        }

        // GET: api/TodoList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoList>> GetTodoList(int id)
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

            return todoList;
        }

        // PUT: api/TodoList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoList(int id, TodoList todoList)
        {
            if (id != todoList.ListId)
            {
                return BadRequest();
            }

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
        public async Task<ActionResult<TodoList>> PostTodoList(TodoList todoList)
        {
          if (_context.TodoLists == null)
          {
              return Problem("Entity set 'TodoListContext.TodoLists'  is null.");
          }
            _context.TodoLists.Add(todoList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoList", new { id = todoList.ListId }, todoList);
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
