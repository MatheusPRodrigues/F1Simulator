using Dapper;
using F1Simulator.CompetitionService.Exceptions;
using F1Simulator.CompetitionService.Repositories.Interfaces;
using F1Simulator.CompetitionService.Services.Interfaces;
using F1Simulator.Models.DTOs.CompetitionService.Request;
using F1Simulator.Models.DTOs.CompetitionService.Response;
using F1Simulator.Models.DTOs.CompetitionService.Update;
using F1Simulator.Models.Models;

namespace F1Simulator.CompetitionService.Services
{
    public class CircuitService : ICircuitService
    {
        private readonly ILogger<CircuitService> _logger;
        private readonly ICircuitRepository _circuitRepository;
        private readonly ICompetitionRepository _competitionRepository;
        public CircuitService(ILogger<CircuitService> logger,
            ICircuitRepository circuitRepository, 
            ICompetitionRepository competitionRepository)
            
        {
            _logger = logger;
           _circuitRepository = circuitRepository;
           _competitionRepository = competitionRepository;
        }

        public async Task<CreateCircuitsResponseDTO> CreateCircuitsAsync(CreateCircuitsRequestDTO circuits)
        {
            try
            {
                //verifica se a temporada já foi iniciada, se sim não permite criar novos circuitos
                var seasonStarted = await _competitionRepository.GetCompetionActiveAsync();
                if (seasonStarted != null)
                {
                    throw new BusinessException("Cannot create circuits when the season has already started.");
                }

                CreateCircuitsResponseDTO circuitsForController = new CreateCircuitsResponseDTO();

                foreach (var c in circuits.CircuitsRequest)
                {
                    // verifica se já não existe um circuito com o mesmo nome e país, se tiver ignora
                    if (!await _circuitRepository.CircuitExistsAsync(c.Name))
                    {
                        // verifica se já existem 24 circuitos 
                        bool IsActive = true;
                        int circuitsActive = await _circuitRepository.CircuitsActivatesAsync();
                        if (circuitsActive == 24)
                        {
                            break;
                        }
                        Circuit circuit = new Circuit(c.Name, c.Country, c.LapsNumber, IsActive);
                        await _circuitRepository.CreateCircuitAsync(circuit);

                        var circuitResponse = new CreateCircuitResponseDTO
                        {
                            Id = circuit.Id,
                            Name = circuit.Name,
                            Country = circuit.Country,
                            LapsNumber = circuit.LapsNumber,
                            IsActive = circuit.IsActive
                        };
                        circuitsForController.circuits.Add(circuitResponse);
                    }
                }
                return circuitsForController;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateCircuitsAsync");
                throw;
            }
        }
        public async Task<CreateCircuitResponseDTO> CreateCircuitAsync(CreateCircuitRequestDTO createCircuit)
        {
            try
            {
                //verifica se a temporada já foi iniciada, se sim não permite criar novos circuitos
                var seasonStarted = await _competitionRepository.GetCompetionActiveAsync();
                if (seasonStarted != null)
                {
                    throw new BusinessException("Cannot create circuits when the season has already started.");
                }

                // verifica se já existem 24 circuitos ativos, para os p´róximos serem criados como inativos
                bool IsActive = true;
                int circuitsActive = await _circuitRepository.CircuitsActivatesAsync();
                if (circuitsActive == 24)
                {
                    throw new BusinessException("You are only allowed to register 24 circuits.");
                }
                Circuit circuit = new Circuit(createCircuit.Name, createCircuit.Country, createCircuit.LapsNumber, IsActive);
                await _circuitRepository.CreateCircuitAsync(circuit);

                // verifica se já não existe um circuito com o mesmo nome e país, para evitar duplicidade
                bool retorno = await _circuitRepository.CircuitExistsAsync(createCircuit.Name);
                if (retorno == true)
                {
                    throw new BusinessException("Error: There is already a registration with the same name for all past circuits.");
                }

                

                // Retorna os dados do circuito criado
                return new CreateCircuitResponseDTO
                {
                    Id = circuit.Id,
                    Name = circuit.Name,
                    Country = circuit.Country,
                    LapsNumber = circuit.LapsNumber,
                    IsActive = circuit.IsActive
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateCircuitAsync");
                throw;
            }
        }


        public async Task<List<CreateCircuitResponseDTO>> GetAllCircuitsAsync()
        {

            try
            {
                return await _circuitRepository.GetAllCircuitsAsync();

            }catch(Exception ex)
            {
                _logger.LogError("Error in GetAllCircuits in CircuitService: " + ex.Message);
                throw;
            }

        }

        public async Task<CreateCircuitResponseDTO?> GetCircuitByIdAsync(Guid id)
        {

            try
            {
                return await _circuitRepository.GetCircuitByIdAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetCircuitById in CircuitService: " + ex.Message);
                throw;
            }

        }

        public async Task<bool> DeleteCircuitAsync(Guid id)
        {
            try
            {
                // verificar se a temporada já está ativa, se tiver não permite deletar
                var seasonStarted = await _competitionRepository.GetCompetionActiveAsync();
                if (seasonStarted != null)
                {
                    throw new BusinessException("Cannot delete circuits when the season has already started.");
                }

                return await _circuitRepository.DeleteCircuitAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in DeleteCircuitAsync in CircuitService: " + ex.Message);
                throw;
            }

        }

        public async Task<(bool Update, CreateCircuitResponseDTO? Circuit)> UpdateCircuitAsync(Guid id, UpdateCircuitDTO updateCircuit)
        {
            try
            {
                // verificar se a temporada já está ativa, se tiver não permite atualização
                var seasonStarted = await _competitionRepository.GetCompetionActiveAsync();
                if (seasonStarted != null)
                {
                    throw new BusinessException("Cannot update circuits when the season has already started.");
                }

                // verifica se existe algum circuito com o id informado

                var circuit = await _circuitRepository.GetCircuitByIdAsync(id);
                if (circuit == null)
                {
                    return (false, null);
                }

                var updates = new List<string>();
                var parameters = new DynamicParameters();
                parameters.Add("Id", id);

                if (!string.IsNullOrEmpty(updateCircuit.Name))
                {
                    // verifica se já existe um circuito com o mesmo nome, se sim não permite atualizar
                    bool existe = await _circuitRepository.CircuitExistsAsync(updateCircuit.Name);
                    if (existe is true)
                    {
                        return (true, null);
                    }

                    updates.Add("Name = @Name");
                    parameters.Add("Name", updateCircuit.Name);
                }

                if (!string.IsNullOrEmpty(updateCircuit.Country))
                {
                    updates.Add("Country = @Country");
                    parameters.Add("Country", updateCircuit.Country);
                }

                if (updateCircuit.LapsNumber.HasValue)
                {
                    updates.Add("LapsNumber = @LapsNumber");
                    parameters.Add("LapsNumber", updateCircuit.LapsNumber);
                }

                var result = await _circuitRepository.UpdateCircuitAsync(id, updates, parameters);
                circuit = await _circuitRepository.GetCircuitByIdAsync(id);

                return (result, circuit);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in UpdateCircuit in CircuitService: " + ex.Message);
                throw;
            }

        }


    }
}
