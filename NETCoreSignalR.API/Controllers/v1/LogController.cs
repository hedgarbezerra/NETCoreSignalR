using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCoreSignalR.Domain.Entities;
using NETCoreSignalR.Domain.Interfaces;
using NETCoreSignalR.Domain.Model;
using NETCoreSignalR.Services.Data;
using NETCoreSignalR.Services.Pagination;

namespace NETCoreSignalR.API.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
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

        [HttpGet, MapToApiVersion("2.0")]
        [Route("get")]
        [ProducesResponseType(typeof(PaginatedList<EventLog>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult GetLog([FromQuery] PaginationFilter query)
        {
            var route = Request.Path.Value;
            var paginatedList = _loggingService.GetPaginatedList(route, query.PageIndex, query.PageSize);

            if (paginatedList.TotalCount <= 0)
                return NoContent();

            return Ok(paginatedList);
        }

        [HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(EventLog), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 204)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult GetById(int id)
        {
            var logOption = _loggingService.Get(id);

            return logOption.Match<IActionResult>(log => Ok(log), () => NoContent());
        }
    }
}
