using F1Simulator.EngineeringService.Services.Interfaces;
using F1Simulator.Models.DTOs.EngineeringService;
using F1Simulator.Models.DTOs.TeamManegementService.CarDTO;
using F1Simulator.Models.DTOs.TeamManegementService.EngineerDTO;

namespace F1Simulator.EngineeringService.Services
{
    public class EngineeringService :  IEngineeringService
    {
		private readonly IHttpClientFactory _httpClientFactory;
        private readonly Random _random = Random.Shared;

        public EngineeringService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<CarResponseDTO> PutCarCoefficientsAsync(EngineersPutDTO engIds, string carId)
        {
            var httpClientCar = _httpClientFactory.CreateClient("Car");

            // looking for the car
            var car = await httpClientCar.GetFromJsonAsync<CarResponseDTO>($"{carId}");

            if (car is null)
                throw new KeyNotFoundException("Car not found.");

            var httpClientEngineer = _httpClientFactory.CreateClient("Engineer");

            EngineerResponseDTO? engineerCa = null, engineerCp = null;

            // Looking for engineers
            if (!string.IsNullOrWhiteSpace(engIds.EngineerCaId))
                engineerCa = await httpClientEngineer.GetFromJsonAsync<EngineerResponseDTO>($"{engIds.EngineerCaId}");

            var raw = await httpClientEngineer.GetStringAsync($"{engIds.EngineerCaId}");
            Console.WriteLine(raw);


            if (!string.IsNullOrWhiteSpace(engIds.EngineerCpId))
                engineerCp = await httpClientEngineer.GetFromJsonAsync<EngineerResponseDTO>($"{engIds.EngineerCpId}");

            double caUpdated = car.Ca, cpUpdated = car.Cp;

            // Update Ca
            if (engineerCa is not null)
            {
                var randomFactorOne = Math.Round(
                            ((2.0 * _random.NextDouble()) - 1.0),
                            3);

                if (engineerCa.CarId != car.CarId)
                    throw new ArgumentException("The aerodynamic coefficient engineer is not associated with the reported car.");

                caUpdated = Math.Clamp(car.Ca + (engineerCa.ExperienceFactor * randomFactorOne), 0, 10);
            }

            // Update Cp
            if (engineerCp is not null)
            {

                var randomFactorTwo = Math.Round(
                            ((2.0 * _random.NextDouble()) - 1.0),
                            3);

                if (engineerCp.CarId != car.CarId)
                    throw new ArgumentException("The power coefficient engineer is not associated with the specified car.");

                cpUpdated = Math.Clamp(car.Cp + (engineerCp.ExperienceFactor * randomFactorTwo), 0, 10);
            }

            //If nothing has changed, error: bad request.
            if (caUpdated == car.Ca && cpUpdated == car.Cp)
                throw new ArgumentException("No coefficients were updated.");

            var newCar = new CarResponseDTO
            {
                CarId = car.CarId,
                TeamId = car.TeamId,
                Model = car.Model,
                WeightKg = car.WeightKg,
                Speed = car.Speed,
                Ca = caUpdated,
                Cp = cpUpdated,
            };

            await httpClientCar.PutAsJsonAsync($"car/{carId}", new { ca = newCar.Ca, cp = newCar.Cp });

            return newCar;
        }

    }
}
