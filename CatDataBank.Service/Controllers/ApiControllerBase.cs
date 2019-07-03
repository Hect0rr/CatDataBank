using System;
using CatDataBank.Helper;
using CatDataBank.Model;
using CatDataBank.Model.Dto;
using CatDataBank.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CatDataBank.Controllers
{
    public class ApiControllerBase : Controller
    {
        public virtual IActionResult Success(object response = null)
        {
            if (response == null)
                return Ok();
            return Ok(response);
        }

        public virtual IActionResult Error(object response = null)
        {
            if (response == null)
                return BadRequest();
            return BadRequest(response);
        }

        public virtual StatusCodeResult InternalError()
        {
            return StatusCode(500);
        }

        public virtual StatusCodeResult Created()
        {
            return StatusCode(201);
        }
    }
}