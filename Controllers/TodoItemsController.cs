using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models;
using TodoApiDTO.Controllers;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemService _todoService;
        public TodoItemsController(ITodoItemService todoService)
        {
            if (todoService == null)
            {
                throw new ArgumentNullException("context");
            }

            _todoService = todoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            var todoList = await _todoService.GetTodoItems();
            return Ok(todoList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _todoService.GetTodoItem(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return Ok(todoItem);
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateTodoItem(TodoItemDTO todoItemDTO)
        {
            try
            {
                var todoItem = await _todoService.UpdateTodoItem(todoItemDTO);
            }
            catch (NullReferenceException ex)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> CreateTodoItem(TodoItemDTO todoItemDTO)
        {
            var todoItem = await _todoService.CreateTodoItem(todoItemDTO);

            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = todoItem.Id },
                todoItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _todoService.GetTodoItem(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            await _todoService.DeleteTodoItem(id);

            return NoContent();
        }
    }
}
