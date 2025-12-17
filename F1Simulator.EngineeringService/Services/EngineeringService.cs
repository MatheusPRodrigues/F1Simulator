using F1Simulator.EngineeringService.Services.Interfaces;
using F1Simulator.Models.DTOs.EngineeringService;
using F1Simulator.Models.DTOs.TeamManegementService.CarDTO;
using F1Simulator.Models.DTOs.TeamManegementService.EngineerDTO;

namespace F1Simulator.EngineeringService.Services
{
    public class EngineeringService :  IEngineeringService
    {
		private readonly IHttpClientFactory _hhtpClientFactory;
        private readonly Random _random = Random.Shared;

        public EngineeringService(IHttpClientFactory hhtpClientFactory)
        {
            _hhtpClientFactory = hhtpClientFactory;
        }


        public async Task<CarResponseDTO> PutCarCoefficientsAsync(EngineersPutDTO engIds, string carId)
        {
            var httpClientCar = _hhtpClientFactory.CreateClient("Car");

            // looking for the car
            var car = await httpClientCar.GetFromJsonAsync<CarResponseDTO>($"car/{carId}");

            if (car is null)
                throw new KeyNotFoundException("Car not found.");

            var httpClientEngineer = _hhtpClientFactory.CreateClient("Engineer");

            EngineerResponseDTO? engineerCa = null, engineerCp = null;

            // Looking for engineers
            if (!string.IsNullOrWhiteSpace(engIds.EngineerCaId))
                engineerCa = await httpClientEngineer.GetFromJsonAsync<EngineerResponseDTO>($"engineer/{engIds.EngineerCaId}");

            if (!string.IsNullOrWhiteSpace(engIds.EngineerCpId))
                engineerCp = await httpClientEngineer.GetFromJsonAsync<EngineerResponseDTO>($"engineer/{engIds.EngineerCpId}");

            // Confirmar com a equipe se gera uma variação aleatória para cada engenheiro, ou se vai utilizar a mesma
            var randomFactor = Math.Round(
                    ((2.0 * _random.NextDouble()) - 1.0),
                    3);

            double caUpdated = car.Ca, cpUpdated = car.Cp;

            // Update Ca
            if (engineerCa is not null)
            {
                if (engineerCa.CarId != car.CarId)
                    throw new ArgumentException("The aerodynamic coefficient engineer is not associated with the reported car.");

                caUpdated = Math.Clamp(car.Ca + (engineerCa.ExperienceFactor * randomFactor), 0, 10);

            }

            // Update Cp
            if (engineerCp is not null)
            {
                if (engineerCp.CarId != car.CarId)
                    throw new ArgumentException("The power coefficient engineer is not associated with the specified car.");

                cpUpdated = Math.Clamp(car.Cp + (engineerCp.ExperienceFactor * randomFactor), 0, 10);
            }

            //If nothing has changed, error: bad request.
            if (caUpdated == car.Ca && cpUpdated == car.Cp)
                throw new ArgumentException("No coefficients were updated.");

            return new CarResponseDTO
            {
                CarId = car.CarId,
                TeamId = car.TeamId,
                Model = car.Model,
                WeightKg = car.WeightKg,
                Speed = car.Speed,
                Ca = caUpdated,
                Cp = cpUpdated,
                IsActive = car.IsActive,
            };
        }


        // Verificar com o time sobre a necessidade de dois endpoints de patch
        public async Task<CarResponseDTO> PatchCarAerodynamicCoefficientsAsync(EngineersPutDTO engIds, string carId)
        {
            return await PutCarCoefficientsAsync(new EngineersPutDTO 
                                                { 
                                                    EngineerCaId = engIds.EngineerCaId,
                                                    EngineerCpId = null
                                                }, 
                                                carId);
        }


        public async Task<CarResponseDTO> PatchCarPotentialCoefficientsAsync(EngineersPutDTO engIds, string carId)
        {
            return await PutCarCoefficientsAsync(new EngineersPutDTO
                                                {
                                                    EngineerCaId = engIds.EngineerCaId,
                                                    EngineerCpId = null
                                                },
                                                carId);
        }

    }
}
