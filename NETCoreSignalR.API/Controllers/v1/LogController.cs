using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCoreSignalR.Domain.Entities;
using NETCoreSignalR.Domain.Model;
using NETCoreSignalR.Services.Data;
using NETCoreSignalR.Services.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreSignalR.API.Controllers
{
    //[ApiVersion("1.0", Deprecated = true)]
    //[Authorize(Roles = "Administrator")]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        public IUriService _uriService { get; }
        public ILoggingService _loggingService { get; }

        public LogController(IUriService uriService, ILoggingService loggingService)
        {
            _uriService = uriService;
            _loggingService = loggingService;
        }

        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(PaginatedList<EventLog>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult GetLog([FromQuery] PaginationFilter query)
        {
            var route = Request.Path.Value;
            var paginatedList = _loggingService.GetPaginatedList(route, query.PageIndex, query.PageSize);

            if (paginatedList.TotalCount <= 0)
                return NotFound();

            return Ok(paginatedList);
        }

        [HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(PaginatedList<EventLog>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult GetById([FromQuery] int id)
        {
            EventLog result = _loggingService.Get(id);

            if (result is null)
                return NotFound();

            return Ok(result);
        }
    }
}
