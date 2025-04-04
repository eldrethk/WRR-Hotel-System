using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WRRManagement.Domain.Reservation;
using WRRManagement.Domain.RoomTypes;
using WRRManagement.Domain.Base;
using WRRManagement.Infrastructure.Data;
using WRRManagement.Infrastructure.RoomTypeRespository;
using static System.Runtime.InteropServices.JavaScript.JSType;
using WRRManagement.Domain.Hotels;
using System.Diagnostics.Tracing;
using WRRManagement.Domain.APIModels;

namespace WRRManagement.Infrastructure.ReservationRepository
{
    public class AvailablityRackRoomRepository : GenericRepository<AvailableRackRoom>, IAvailableRackRoom
    {
        protected new WRRContext context;
        protected IRackRate rackRateRepository;
        protected ITierLevel tierLevelRepository;
        protected IHotelSystem hotelSystemRepository;
        protected IAdultBase adultBaseRepository;
        protected IMaxBase maxBaseRepository;
        protected IMinStay minStayRepository;
        protected IRoomImage roomImageRepository;
        protected IRoomFeatures roomFeatureRepository;
        protected readonly IRoomAllocation roomAllocationRepository;

        public AvailablityRackRoomRepository(WRRContext context, IRackRate rackRateRep, ITierLevel tierLevelRep, IHotelSystem hotelSystem, 
            IAdultBase adultBase, IMaxBase maxBase, IMinStay minStay, IRoomImage roomImage, IRoomFeatures roomFeatureRepository, 
            IRoomAllocation roomAllocation) : base(context)
        {
            this.context = context;
            this.rackRateRepository = rackRateRep;
            this.tierLevelRepository = tierLevelRep;
            this.hotelSystemRepository = hotelSystem;
            this.adultBaseRepository = adultBase;
            this.maxBaseRepository = maxBase;
            this.minStayRepository = minStay;
            this.roomImageRepository = roomImage;
            this.roomFeatureRepository = roomFeatureRepository;
            this.roomAllocationRepository = roomAllocation;
        }

        public List<AvailableRackRoom> GetAvailableRackRooms(int hotelId, DateTime startDate, DateTime endDate, int adults, int children)
        {
            List<AvailableRackRoom> list = new List<AvailableRackRoom>();         

            List<RoomType> BaseCountRooms = GetRoomsBaseOnGuestCount(hotelId, adults, children);

            if (BaseCountRooms != null)
            {
                List<RoomType> ValidRooms = GetRoomsWithAllocation(BaseCountRooms, startDate, endDate);
                HotelSystem system = hotelSystemRepository.GetSystem(hotelId);

                foreach (RoomType room in ValidRooms)
                {                    
                    List<decimal> rates = new List<decimal>();
                    List<DateTime> dates = new List<DateTime>();
                    decimal subTotal = 0;
                    decimal weekEndFee = 0;
                    decimal extraGuestFee = 0;
                    decimal deposit = 0;
                    int minStay = 0;
                    int days = TimeSpan.FromDays((endDate - startDate).Days).Days;
                    for (DateTime temp = startDate; temp < endDate; temp = temp.AddDays(1))
                    {
                        //if room has min night stay requires - still display but don't need to compute the rate
                        int stay = minStayRepository.GetQuantityForDate(room.RoomTypeID, temp);
                        if (days < stay)
                        {
                            rates = null;
                            AvailableRackRoom availableRackRooms = new AvailableRackRoom()
                            {
                                minStay = stay
                            };
                            list.Add(availableRackRooms);
                        }
                        else
                        {
                            string cTier = string.Empty;
                            cTier = tierLevelRepository.GetTierForDate(hotelId, temp).ToString();

                            RackRate rate = rackRateRepository.GetRatesForReservation(room.RoomTypeID, temp);
                            if (rate == null)
                            {
                                rates = null;
                                break;
                            }
                            else
                            {
                                dates.Add(temp);
                                switch (cTier.ToLower())
                                {
                                    case "a":
                                        subTotal += rate.TierARate;
                                        rates.Add(rate.TierARate);
                                        break;
                                    case "b":
                                        subTotal += rate.TierBRate;
                                        rates.Add(rate.TierBRate);
                                        break;
                                    case "c":
                                        subTotal += rate.TierCRate;
                                        rates.Add(rate.TierCRate);
                                        break;
                                    default:
                                        subTotal += rate.TierARate;
                                        rates.Add(rate.TierARate);
                                        break;

                                }
                            }
                            //Get weeekend fees
                            if (system.WeekendFee > 0)
                            {
                                if (temp.DayOfWeek == DayOfWeek.Friday || temp.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    weekEndFee += system.WeekendFee;
                                }
                            }
                        }
                    }
                    if (rates != null)
                    {
                        if (system.AddTaxToWeekendFee)
                            weekEndFee = weekEndFee * (1 + system.TaxRate / 100);

                        if (room.MaxBase)
                        {
                            MaxBase maxBase = maxBaseRepository.GetByRoomID(room.RoomTypeID);
                            int guestToCharge = (adults + children) - maxBase.MaxBaseCount;

                            if(guestToCharge > 0)
                                extraGuestFee += (guestToCharge * system.ExtraBaseFee * days);
                        }
                        else if (room.AdultBase)
                        {
                            AdultBase adultBase = adultBaseRepository.GetByRoomID(room.RoomTypeID);
                            int adultToCharge = adults - adultBase.AdultBaseCount;
                            int childToCharge = children - adultBase.ChildBaseCount;

                            if (adultToCharge > 0)
                                extraGuestFee += (adultToCharge * system.ExtraAdultFee * days);

                            if (childToCharge > 0)
                                extraGuestFee += (childToCharge * system.ExtraChildFee * days);
                          
                        }
                        if (system.AddTaxToExtraPerson)
                            extraGuestFee = extraGuestFee * (1 + system.TaxRate / 100);

                        //cal Tax
                        decimal taxRate = system.TaxRate / 100;
                        decimal tax = subTotal * taxRate;                   

                        //cal resort fee
                        decimal resortFee = 0;
                        if (system.ResortFeeCalAs == "Flat Fee Per Day")
                            resortFee = system.ResortFee * days;
                        else if (system.ResortFeeCalAs == "Flat Fee")
                            resortFee = system.ResortFee;
                        else if (system.ResortFeeCalAs == "Flat Fee Per Person")
                            resortFee = system.ResortFee * (adults + children);
                        else
                            resortFee = system.ResortFee * days;

                        if(system.AddTaxToResortFee)
                            resortFee = resortFee * (1 + (system.TaxRate/100));

                        decimal allExtraFee = resortFee + extraGuestFee + weekEndFee;

                        //cal deposit
                        if (system.DepositRoomCalAs == "First 2 Nights Room Stay")
                        {
                            decimal twoNight = rates[0] + rates[1];
                            if (system.AddTaxToDeposit)
                                deposit = twoNight * (1 + taxRate);
                            else
                                deposit = twoNight;
                        }
                        else if (system.DepositRoomCalAs == "Flat Fee")
                        {
                            deposit = system.DepositRoomPercentage ?? 0;
                        }
                        else if (system.DepositRoomCalAs == "Total Reservation")
                        {
                            deposit = tax + allExtraFee + subTotal;
                        }                       
                        else 
                        {
                            if (system.AddTaxToDeposit)
                                deposit = rates[0] * (1 + taxRate);
                            else
                                deposit = rates[0];
                        }
                        int lowallocation = 0;
                        int allocationNumber = roomAllocationRepository.GetQuantityForDay(room.RoomTypeID, startDate);
                        int allocationLimit = hotelSystemRepository.GetSystem(hotelId).LowAllocationLimit;
                        if(allocationNumber <= allocationLimit)
                            lowallocation = allocationLimit;

                        List<RoomImage> roomImages = roomImageRepository.GetRoomImages(room.RoomTypeID);
                        foreach(var img in roomImages)
                        {
                            img.FileName = Path.Combine("img", "room-images", img.FileName);
                        }

                        ViewRoomModel roomModel = new ViewRoomModel()
                        {
                            roomType = room,
                            mainImage = roomImages.FirstOrDefault(x => x.MainImage).FileName,
                            roomImages = roomImages,
                            roomFeatures = roomFeatureRepository.GetRoomFeatures(room.RoomTypeID)
                        };
                        AvailableRackRoom availRooms = new AvailableRackRoom()
                        {
                            viewRoomType = roomModel,
                            rates = rates,
                            ratesDate = dates,
                            subTotal = subTotal,
                            weekEndFee = weekEndFee,
                            extraGuestFee = extraGuestFee,
                            resortFee = resortFee,
                            avgDailyRate = subTotal / days,
                            ViewRoomRateAs = system.DisplayRoomRatesAs,
                            total = tax + allExtraFee + subTotal,
                            tax = tax,
                            deposit = deposit,
                            allExtraFees = allExtraFee,
                            LowAllocation = lowallocation

                        };

                        list.Add(availRooms);
                    }
                 }
            }

            return list;
        }

        private List<RoomType> GetRoomsBaseOnGuestCount(int hotelId, int adults, int children)
        {
            List<RoomType> validlist = new List<RoomType>();

            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "HotelID", ParameterValue = hotelId });

            int totalGuests = adults + children;
            try
            {
                var (adultBases, rooms) = DapperHelper.GetMultipleHorizontalRecords<AdultBase, RoomType>("dbo.genSelAdultBase", parameters, context, map: (adultBase, room) =>
                {
                    adultBase.RoomTypeID = room.RoomTypeID;
                    return adultBase;
                },
                splitOn: "RoomTypeID"
                );

                foreach (AdultBase adultBase in adultBases)
                {
                    if (totalGuests <= adultBase.MaxRoomTotal)
                    {
                        if (adults <= adultBase.MaxAdult && children <= adultBase.MaxChild)
                        {
                            validlist.Add(rooms.FirstOrDefault(x => x.RoomTypeID == adultBase.RoomTypeID));
                        }
                    }
                }
            }
            catch (Exception ex) { }

            try
            {
                var (maxBases, rooms) = DapperHelper.GetMultipleHorizontalRecords<MaxBase, RoomType>("dbo.genSelMaxBase", parameters, context, map: (maxBase, room) =>
                {
                    maxBase.RoomTypeID = room.RoomTypeID;
                    return maxBase;
                },
                splitOn: "RoomTypeID"
                );           

                foreach (MaxBase maxBase in maxBases)
                {
                    if (totalGuests <= maxBase.MaxBaseCount)
                    {
                        validlist.Add(rooms.FirstOrDefault(x => x.RoomTypeID == maxBase.RoomTypeID));
                    }
                }
            }
            catch (Exception ex) { }

            return validlist;
        }

        private List<RoomType> GetRoomsWithAllocation(List<RoomType> rooms, DateTime arrival, DateTime departure)
        {
            List<RoomType> ValidList = new List<RoomType>();

            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "Start", ParameterValue = arrival });
            parameters.Add(new ParameterInfo() { ParameterName = "End", ParameterValue = departure });
            if (rooms.Count > 0)
            {
                foreach (RoomType room in rooms)
                {
                    bool valid = false;
                    parameters.Add(new ParameterInfo() { ParameterName = "RoomID", ParameterValue = room.RoomTypeID });
                    try
                    {
                        valid = DapperHelper.ExecuteQueryBool("dbo.genSelRoomsWithAllocation", parameters, context, "@Valid");
                    }
                    catch (Exception ex) { }
                    if (valid)
                    {
                        ValidList.Add(room);
                    }
                }
            }

            return ValidList;
        }
    }
}