using F1Simulator.CompetitionService.Exceptions;
using F1Simulator.CompetitionService.Services.Interfaces;
using F1Simulator.Models.DTOs.CompetitionService.Request;
using F1Simulator.Models.DTOs.CompetitionService.Response;
using F1Simulator.Models.DTOs.CompetitionService.Update;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;

namespace F1Simulator.CompetitionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CircuitController : ControllerBase
    {
        private readonly ILogger<CircuitController> _logger;
        private readonly ICircuitService _circuitService;

        public CircuitController(ILogger<CircuitController> logger, ICircuitService circuitService)
        {
            _logger = logger;
            _circuitService = circuitService;
        }

        [HttpPost("Circuit")]
        public async Task<ActionResult> CreateCircuit(CreateCircuitRequestDTO createCircuit) { 
            try
            {
                 var circuit = await _circuitService.CreateCircuitAsync(createCircuit);
                if(circuit == null)
                {
                    return BadRequest("Error: There is already a circuit with this name and country.");
                }
                return Ok(circuit);
            }catch(BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error creating circuit");
                return BadRequest(bex.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating circuit");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the circuit.");
            }
        }

        [HttpPost("Circuits")]
        public async Task<ActionResult> CreateCircuits(CreateCircuitsRequestDTO createCircuit)
        {
            try
            {
                CreateCircuitsResponseDTO circuit = await _circuitService.CreateCircuitsAsync(createCircuit);
                if (circuit.circuits.IsNullOrEmpty())
                {
                    return BadRequest("Error: There is already a registration with the same name for all past circuits.");
                }
                return Ok(circuit);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error creating circuit");
                return BadRequest(bex.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating circuit");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the circuits.");
            }
        }

        [HttpPatch("Deactivate/{id}")]

        public async Task<ActionResult> DeactivateCircuitAsync(Guid id)
        {
            try
            {
                var( update, circuit) = await _circuitService.DeactivateCircuitAsync(id);
                if(circuit == null)
                {
                    return NotFound("Circuit not found");
                }
                if (!update)
                {
                    return Ok(new{ circuit, message = "The circuit was already inactive."});
                }

                return Ok(circuit);
            }catch(BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error deactivating circuit");
                return BadRequest(bex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when deactivating circuit");
                return StatusCode(StatusCodes.Status500InternalServerError,"An error occurred while deactivating the circuit.");
            }
        }

        [HttpPatch("Activate/{id}")]

        public async Task<ActionResult> ActivateCircuitAsync(Guid id)
        {
            try
            {
                var (update, circuit) = await _circuitService.ActivateCircuitAsync(id);
                if (circuit == null && update == false)
                {
                    return NotFound("Circuit not found");
                }
                if (!update)
                {
                    return Ok(new { circuit, message = "The circuit was already Active." });
                }
                if(circuit == null && update == true)
                {
                    return BadRequest("There are already 24 active circuits");
                }

                return Ok(circuit);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error activating circuit");
                return BadRequest(bex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when Activating circuit");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Activating the circuit.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<CreateCircuitResponseDTO>>> GetAllCircuits()
        {
            try
            {
                var circuits = await _circuitService.GetAllCircuits();
                return Ok(circuits);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when getting all circuits");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while getting all circuits.");
            }   
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CreateCircuitResponseDTO>> GetCircuitById(Guid id)
        {
            try
            {
                var circuit = await _circuitService.GetCircuitById(id);
                if(circuit == null)
                {
                    return NotFound("Circuit not found");
                }
                return Ok(circuit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when get circuit by id");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while get circuit by id.");
            }
        }

        [HttpDelete("{id}")] 
        public async Task<ActionResult> DeleteCircuit(Guid id)
        {
            try
            {
                var circuit = await _circuitService.DeleteCircuitAsync(id);
                if (circuit == false)
                {
                    return NotFound("Circuit not found");
                }
                return Ok();
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error deleting circuit");
                return BadRequest(bex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when get circuit by id");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while get circuit by id.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCircuitAsync(Guid id, UpdateCircuitDTO updateCircuit)
        {
            try
            {
                var (update, circuit) = await _circuitService.UpdateCircuitAsync(id, updateCircuit);
                if (circuit == null && update == false)
                {
                    return NotFound("Circuit not found.");
                }
                if (circuit == null && update == true)
                {
                    return BadRequest("there is already a circuit with this name");
                }
                return Ok(circuit);
            }catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error updating circuit");
                return BadRequest(bex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when updating circuit");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the circuit.");
            }
        }


    }
}
