using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCoreSignalR.Domain.Entities;
using NETCoreSignalR.Domain.Model.PokeAPI;
using NETCoreSignalR.Services.Data;
using NETCoreSignalR.Services.External.PokeAPI;
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
        private readonly ILoggingService _loggingService;

        public LogController(ILoggingService loggingService)
        {
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
                var logOption = await _loggingService.GetAsync(id, cancellationToken);

                return logOption.Match<IActionResult>(log => Ok(log), () => NoContent());
            }
            catch (TaskCanceledException ex)
            {
                return Ok(ex);
            }
        }
    }
}