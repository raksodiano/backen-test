using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApplicationApi.Data;
using TodoApplicationApi.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApplicationApi.Common.Enums;
using TodoApplicationApi.DTOs;


namespace TodoApplicationApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TodoController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly ILogger<TodoController> _logger;

		public TodoController(ApplicationDbContext context, ILogger<TodoController> logger)
		{
			_context = context;
			_logger = logger;
		}

		// GET: api/todo
		[HttpGet]
		public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll()
		{
			var todoItems = await _context.TodoItems
																			.OrderBy(item => item.CreatedAt)
																			.ToListAsync();
			return Ok(todoItems);
		}

		// GET: api/todo/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<TodoItem>> GetOne(Guid id)
		{
			var item = await _context.TodoItems.FindAsync(id);
			if (item == null)
			{
				return NotFound();
			}
			return item;
		}

		// POST: api/todo
		[HttpPost]
		public async Task<ActionResult<TodoItem>> Create([FromBody] TodoItem item)
		{
			try
			{
				_context.TodoItems.Add(item);
				await _context.SaveChangesAsync();

				return CreatedAtAction(nameof(GetOne), new { id = item.Id }, item);
			}
			catch (Exception ex)
			{
				_logger.LogError("Error while creating the item", ex);
				return StatusCode(StatusCodes.Status500InternalServerError, "Error while creating the item.");
			}
		}

		// PUT Helper Function
		private async Task<IActionResult> UpdateItem(Guid id, Action<TodoItem> updateAction)
		{
			var existingItem = await _context.TodoItems.FindAsync(id);
			if (existingItem == null)
			{
				return NotFound();
			}

			updateAction(existingItem);

			try
			{
				await _context.SaveChangesAsync();
				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError("Error while updating the item", ex);
				return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating the item.");
			}
		}

		// PUT: api/todo/update-details/{id}
		[HttpPut("update/{id}")]
		public async Task<IActionResult> UpdateDetails(Guid id, [FromBody] TodoItem item)
		{
			return await UpdateItem(id, existingItem =>
			{
				existingItem.Title = item.Title;
				existingItem.Description = item.Description;
			});
		}

		// PUT: api/todo/mark-in-progress/{id}
		[HttpPut("mark-in-progress/{id}")]
		public async Task<IActionResult> MarkInProgress(Guid id)
		{
			return await UpdateItem(id, existingItem =>
			{
				existingItem.Status = Status.Doing;
			});
		}

		// PUT: api/todo/admin/approved/{id}
		[HttpPut("admin/approved/{id}")]
		public async Task<IActionResult> MarkAsComplete(Guid id)
		{
			return await UpdateItem(id, existingItem =>
			{
				existingItem.Status = Status.Done;
			});
		}

		// DELETE: api/todo/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var item = await _context.TodoItems.FindAsync(id);
			if (item == null)
			{
				return NotFound();
			}

			try
			{
				_context.TodoItems.Remove(item);
				await _context.SaveChangesAsync();

				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError("Error while deleting the item", ex);
				return StatusCode(StatusCodes.Status500InternalServerError, "Error while deleting the item.");
			}
		}
	}
}
