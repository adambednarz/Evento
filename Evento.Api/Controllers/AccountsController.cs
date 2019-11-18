﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evento.Infrastructure.Commands.Users;
using Evento.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Evento.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountsController : Controller
    {
        private readonly IUserService _userService;
        public AccountsController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _userService.BrowseAsync();

            return Json(user);
        }

        [HttpGet("{tickets}")]
        public async Task<IActionResult> GetTickets()
        {
            var user = await _userService.BrowseAsync();

            return Json(user);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> Get(string email)
        {
            var user = await _userService.GetAsync(email);

            return Json(user);
        }

        [HttpPost("register")]
        //[HttpPost]
        public async Task<IActionResult> Post([FromBody] Register command)
        {
            command.Id = Guid.NewGuid();
            await _userService.RegisterAsync(command.Id, command.Name, 
                command.Email, command.Password, command.Role);

            return Created($"/accounts/{command.Email}", null);
        }

        [HttpPost("login")]
        //[HttpPost]
        public async Task<IActionResult> Post([FromBody] Login command)
        {
            await _userService.LoginAsync(command.Email, command.Password);

            return NoContent();
        }
    }
}