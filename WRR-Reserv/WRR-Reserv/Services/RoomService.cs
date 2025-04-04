using System.Net.Http;
using WRRManagement.Domain.Reservation;
using WRRManagement.Domain.RoomTypes;
using WRRManagement.Domain.APIModels;

namespace WRR_Reserv.Services
{
    public class RoomService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<RoomService> _logger;  

        public RoomService(IHttpClientFactory httpClientFactory, ILogger<RoomService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }


        private HttpClient CreateClient() => _httpClientFactory.CreateClient("WRRApiClient");

        public async Task<List<ViewRoomModel>> GetAllRoomsAsync(int hotelId)
        {          
            try
            {

                var client = CreateClient();
                var response = await client.GetAsync($"api/rooms/list/{hotelId}");

                if(response == null)
                {
                    _logger.LogError("Error fetching all rooms for hotel: {hotelId}", hotelId);
                    return new List<ViewRoomModel>();
                }
               
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<List<ViewRoomModel>>() ?? new List<ViewRoomModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all rooms for hotel: {hotelId}", hotelId);
                return new List<ViewRoomModel>();
            }
        }

        public async Task<List<AvailableRackRoom>> GetAvailableRoomsAsync(int hotelId, DateTime arrivalDate, DateTime departureDate, int adults, int children)
        {
            try
            {
                var client = CreateClient();

                var response = await client.GetAsync($"api/rooms/Availability/{hotelId}/{arrivalDate:yyyy-MM-dd}/{departureDate:yyyy-MM-dd}/{adults}/{children}");
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<List<AvailableRackRoom>>() ?? new List<AvailableRackRoom>();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error fetching available rooms for hotel: {hotelId}", hotelId);
                return new List<AvailableRackRoom>();
            }
        }


    }
}
