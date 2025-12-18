using F1Simulator.Models.DTOs.RaceControlService;
using F1Simulator.RaceControlService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace F1Simulator.RaceControlService.Controllers
{
    [Route("api/race")]
    [ApiController]
    public class RaceControlController : ControllerBase
    {
        private readonly ILogger<RaceControlController> _logger;
        private readonly IRaceControlService _raceControlService;

        public RaceControlController
        (
            ILogger<RaceControlController> logger,
            IRaceControlService raceControlService
        )
        {
            _logger = logger;
            _raceControlService = raceControlService;
        }
    }
}
