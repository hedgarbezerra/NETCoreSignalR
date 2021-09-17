using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCoreSignalR.Domain.Entities;
using NETCoreSignalR.Services.Data;
using NETCoreSignalR.Services.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace NETCoreSignalR.API.Controllers.v2
{
    [ApiVersion("2.0")]
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
        [Route("get/{id}")]
        [ProducesResponseType(typeof(EventLog), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            try
            {
                EventLog result = await _loggingService.GetAsync(id, cancellationToken);

                if (result is null)
                    return NoContent();

                return Ok(result);
            }
            catch (TaskCanceledException ex)
            {
                return Ok(ex);
            }

        }
    }
}