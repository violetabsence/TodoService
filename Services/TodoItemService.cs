using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;
using TodoApiDTO.Repository;

namespace TodoApiDTO.Controllers
{
    public class TodoItemService : ITodoItemService
    {
        private readonly IRepository<TodoItem> _todoItemRepository;
        private readonly ILogger<TodoItemService> _logger;

        public TodoItemService(IRepository<TodoItem> todoItemRepository, ILogger<TodoItemService> logger)
        {
            if (todoItemRepository == null)
            {
                throw new ArgumentNullException("todoItemRepository");
            }

            _todoItemRepository = todoItemRepository;
            _logger = logger;
        }
        public async Task<IEnumerable<TodoItemDTO>> GetTodoItems()
        {
            var items = await _todoItemRepository.GetAllAsync();
            return items.Select(i => ItemToDTO(i));
        }

        public async Task<TodoItemDTO> GetTodoItem(long id)
        {
            var todoItem = await _todoItemRepository.GetByIdAsync(id);

            if (todoItem == null)
            {
                _logger.LogError(MyLogEvents.GetItemNotFound, "Get({Id}) NOT FOUND", id);
                return null;
            }

            return ItemToDTO(todoItem);
        }

        public async Task<TodoItemDTO> UpdateTodoItem(TodoItemDTO todoItemDto)
        {
            var todoItem = await _todoItemRepository.GetByIdAsync(todoItemDto.Id);
            if (todoItem == null)
            {
                _logger.LogError(MyLogEvents.UpdateItemNotFound, "Update({Id}) NOT FOUND", todoItem.Id);
                return null;
            }

            todoItem.Name = todoItemDto.Name;
            todoItem.IsComplete = todoItemDto.IsComplete;

            var todoItems = await _todoItemRepository.GetAllAsync();
            var todoItemExists = todoItems.Any(e => e.Id == todoItem.Id);

            try
            {
                await _todoItemRepository.CommitAsync();
            }
            catch (DbUpdateConcurrencyException) when (todoItemExists)
            {
                _logger.LogError(MyLogEvents.UpdateItemNotFound, "Update({Id}) NOT FOUND", todoItem.Id);
                return null;
            }

            return todoItemDto;
        }

        public async Task<TodoItemDTO> CreateTodoItem(TodoItemDTO todoItemDTO)
        {
            var todoItem = new TodoItem
            {
                IsComplete = todoItemDTO.IsComplete,
                Name = todoItemDTO.Name
            };

            await _todoItemRepository.AddAsync(todoItem);

            return ItemToDTO(todoItem);
        }

        public async Task DeleteTodoItem(long id)
        {
            var todoItem = await _todoItemRepository.GetByIdAsync(id);

            if (todoItem == null)
            {
                _logger.LogError(MyLogEvents.DeleteItemNotFound, "Delete({Id}) NOT FOUND", id);
                return;
            }

            await _todoItemRepository.Remove(todoItem);
        }

        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
            new TodoItemDTO
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };
    }
}
