using F1Simulator.EngineeringService.Services.Interfaces;

namespace F1Simulator.EngineeringService.Services
{
    public class EngineeringService :  IEngineeringService
    {
		private readonly IHttpClientFactory _hhtpClientFactory;
        private readonly Random _random;

        public EngineeringService(IHttpClientFactory hhtpClientFactory)
        {
            _hhtpClientFactory = hhtpClientFactory;
            _random = new Random();
        }


        // EngineersRequestDTO > os dois Ids (ca e cp)
        // CarResultDTO > Id do car, Ca e Cp
        public async Task<CarResultDTO> PutCarCoefficientsAsync(EngineersRequestDTO eng, string carId)
        {
			try
			{
                var httpClientCar = _hhtpClientFactory.CreateClient("Car");

                var car = await httpClientCar.GetFromJsonAsync<CarResponseDTO>($"Car/{carId}");

                if (car is null)
                    throw new KeyNotFoundException("Car not found.");

                var httpClientEngineer = _hhtpClientFactory.CreateClient("Engineer");

                var engineerCa = await httpClientEngineer.GetFromJsonAsync<EngResponseDTO>($"Engineer/{eng.EngineerCaId}");
                var engineerCp = await httpClientEngineer.GetFromJsonAsync<EngResponseDTO>($"Engineer/{eng.EngineerCpId}");


                if (engineerCa is null && engineerCp is null)
                    throw new KeyNotFoundException("Engineers not found.");

                var randomFactor = Math.Round(
                        ((2.0 * _random.NextDouble()) - 1.0),
                        3);
                
                double caUpdated = car.Ca == 0 ? 0 : car.Ca + (engineerCa.ExperienceFactor * randomFactor);

                double cpUpdated = car.Cp == 0 ? 0 : car.Cp + (engineerCp.ExperienceFactor * randomFactor);

                return new CarResultDTO = new Car
                {
                    CarId = car.Id,
                    Ca = caUpdated,
                    Cp = cpUpdated
                };

            }
			catch (Exception ex)
			{
				throw;
			}
        }
    }
}
