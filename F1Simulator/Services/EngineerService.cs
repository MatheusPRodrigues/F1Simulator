using F1Simulator.Models.DTOs.TeamManegementService.EngineerDTO;
using F1Simulator.Models.Models.TeamManegement;
using F1Simulator.TeamManagementService.Repositories.Interfaces;
using F1Simulator.TeamManagementService.Services.Interfaces;
using System;

namespace F1Simulator.TeamManagementService.Services
{
    public class EngineerService : IEngineerService
    {
        private readonly Random _random = Random.Shared;
        private readonly IEngineerRepository _engineerRepository;

        public EngineerService(IEngineerRepository engineerRepository)
        {
            _engineerRepository = engineerRepository;
        }

        public async Task<EngineerResponseDTO> CreateEngineerAsync(EngineerRequestDTO engineerRequestDTO)
        {
            try
            {
                //Validar se já existe um engenheiro na equipe

                var experienceEngineer = Math.Round((10.0 * _random.NextDouble()), 3);

                var engineer = new Engineer(
                    engineerRequestDTO.TeamId,
                    engineerRequestDTO.CarId,
                    engineerRequestDTO.FirstName,
                    engineerRequestDTO.FullName,
                    engineerRequestDTO.EngineerSpecialization,
                    experienceEngineer
                    );

                return await _engineerRepository.CreateEngineerAsync( engineer );

            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
