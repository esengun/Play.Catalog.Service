﻿using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Play.Catalog.Service.Controllers
{
    // https://localhost:5001/items 
    [ApiController]
	[Route("items")]
	public class ItemsController : ControllerBase
	{
		private static readonly List<ItemDto> _items = new()
		{
			new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.UtcNow),
			new ItemDto(Guid.NewGuid(), "Antitode", "Cures poison", 7, DateTimeOffset.UtcNow),
			new ItemDto(Guid.NewGuid(), "Bronze sword", "Deals a small amount of damage", 20, DateTimeOffset.UtcNow)
        };

		[HttpGet]
		public IEnumerable<ItemDto> Get()
		{
			return _items;
		}


		// GET /items/{id}
		[HttpGet("{id}")]
		public ItemDto GetById(Guid id)
		{
			return _items.Where(x => x.Id == id).SingleOrDefault();
		}

		// POST /items
		[HttpPost]
		public ActionResult<ItemDto> Post(CreateItemDto createItemDto)
		{
			var item = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.UtcNow);
			_items.Add(item);

			return CreatedAtAction(nameof(GetById), new {id = item.Id}, item);
		}

		// PUT /items/{id}
		[HttpPut("{id}")]
		public IActionResult Put(Guid id, UpdateItemDto updateItemDto)
		{
			var existingItem = _items.Where(item => item.Id == id).SingleOrDefault();

			var updatedItem = existingItem with
			{
				Name = updateItemDto.Name,
				Description = updateItemDto.Description,
				Price = updateItemDto.Price
			};

			var index = _items.FindIndex(existingItem => existingItem.Id == id);
			_items[index] = updatedItem;

			return NoContent();
		}

		// DELETE /items/{id}
		[HttpDelete("{id}")]
		public IActionResult Delete(Guid id)
		{
			var index = _items.FindIndex(existingItem => existingItem.Id == id);
			_items.RemoveAt(index);

			return NoContent();
		}
	}
}
