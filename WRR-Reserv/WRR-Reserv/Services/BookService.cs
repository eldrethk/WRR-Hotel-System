using Microsoft.AspNetCore.Mvc;
using WRR_Reserv.Models;
using WRRManagement.Domain.APIModels;
using WRRManagement.Domain.Hotels;
using WRRManagement.Domain.Reservation;
using WRRManagement.Domain.System;

namespace WRR_Reserv.Services
{
    public class BookService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BookService> _logger;
        public BookService(IHttpClientFactory httpClientFactory, ILogger<BookService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        private HttpClient CreateClient() => _httpClientFactory.CreateClient("WRRApiClient");
        public async Task<int> BookReservation(Reservation reservation, SearchModel search, AvailableRackRoom rackRoom, bool optIn)
        {
            try
            {
                //assign all variables to the Reservation Model
                TimeSpan ts = search.DepartureDate - search.ArrivalDate; 
                reservation.HotelID = search.HotelId;
                reservation.Adults = search.Adults;
                reservation.Children = search.Children;
                reservation.ArrivalDate = search.ArrivalDate;
                reservation.DepartureDate = search.DepartureDate;
                reservation.TotalNights = ts.Days;
                reservation.AvgDailyRate = rackRoom.avgDailyRate;
                reservation.BookedAmentity = false; //when add additional amenities
                reservation.Deposit = rackRoom.deposit;
                reservation.ExtraAdultCharge = rackRoom.extraGuestFee;
                reservation.ResortFees = rackRoom.resortFee;
                reservation.RoomTypeID = rackRoom.viewRoomType.roomType.RoomTypeID;
                reservation.SubTotal = rackRoom.subTotal;
                reservation.Taxes = rackRoom.tax;
                reservation.TotalCharge = rackRoom.total;
                reservation.TotalFees = rackRoom.allExtraFees;
                reservation.WeekendFees = rackRoom.weekEndFee;

                if (optIn)
                {
                    bool success = await AddOptins(reservation.CusEmail, reservation.HotelID, reservation.CusFirstName, reservation.CusLastName, reservation.CusState);
                    if (!success)
                    {
                        _logger.LogError("OptIn failed");                        
                    }
                }

                var client = CreateClient();
                var response = await client.PostAsJsonAsync("api/Reservation", reservation);
                if (response.IsSuccessStatusCode)
                {
                    var reservationResponse = await response.Content.ReadFromJsonAsync<Reservation>();
                    if (reservationResponse != null)
                    {
                        // Handle the returned ReservationID
                        var reservationId = reservationResponse.ReservationID;
                        return reservationId;
                    }
                }
                _logger.LogWarning("Failed to book reservation. Status Code: {StatusCode}", response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while booking the reservation.");
            }
            return 0; 
        }

        public async Task<bool> AddOptins(string email, int hotelId, string firstname, string lastname, string state)
        {
            var client = CreateClient();
            OptInEmails optIn = new OptInEmails
            {
                EmailAddress = email,
                HotelID = hotelId,
                FirstName = firstname,
                LastName = lastname,
                State = state
            };
            var response = await client.PostAsJsonAsync("api/Reservation/OptIn", optIn);
            return response.IsSuccessStatusCode;
        }
          
        public async Task<HotelInfo> GetHotelInfo(int id)
        {
            try
            {
                var client = CreateClient();
                var response = await client.GetAsync($"api/HotelInfo/{id}");
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<HotelInfo>() ?? new HotelInfo();
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, "An error occurred while fetching hotel info for ID: {Id}", id);
                return new HotelInfo();
            }

        }

        public async Task<bool> CheckAvailability(int roomId, DateTime arrivalDate, DateTime departureDate)
        {
            try
            {
                var client = CreateClient();
                var response = await client.GetAsync($"api/rooms/check/{roomId}/{arrivalDate:yyyy-MM-dd}/{departureDate:yyyy-MM-dd}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<bool>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking availability for Room ID: {RoomId}", roomId);
                return false;
            }
        }
    }
}
