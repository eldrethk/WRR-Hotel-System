using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WRRManagement.Domain.Base;
using WRRManagement.Domain.Packages;
using WRRManagement.Domain.RoomTypes;
using WRRManagement.Infrastructure.Data;

namespace WRRManagement.Infrastructure.PackageRepository
{
    public class PackageRepository : GenericRepository<Package>, IPackage
    {
        protected new WRRContext context;
        public PackageRepository(WRRContext context) : base(context)
        {
            this.context = context;
        }

        public int Add(Package package)

        {
            int id = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "Name", ParameterValue = package.Name });
            parameters.Add(new ParameterInfo() { ParameterName = "Description", ParameterValue = package.Description });
            parameters.Add(new ParameterInfo() { ParameterName = "HotelID", ParameterValue = package.HotelID });
            parameters.Add(new ParameterInfo() { ParameterName = "Amenities", ParameterValue = false });
            parameters.Add(new ParameterInfo() { ParameterName = "ArrMon", ParameterValue = package.ArrMon });
            parameters.Add(new ParameterInfo() { ParameterName = "ArrTues", ParameterValue = package.ArrTues });
            parameters.Add(new ParameterInfo() { ParameterName = "ArrWed", ParameterValue = package.ArrWed });
            parameters.Add(new ParameterInfo() { ParameterName = "ArrThur", ParameterValue = package.ArrThurs });
            parameters.Add(new ParameterInfo() { ParameterName = "ArrFri", ParameterValue = package.ArrFri });
            parameters.Add(new ParameterInfo() { ParameterName = "ArrSat", ParameterValue = package.ArrSat });
            parameters.Add(new ParameterInfo() { ParameterName = "ArrSun", ParameterValue = package.ArrSun });
            parameters.Add(new ParameterInfo() { ParameterName = "MinDays", ParameterValue = package.MinDays });
            parameters.Add(new ParameterInfo() { ParameterName = "MaxDays", ParameterValue = package.MaxDays });
            parameters.Add(new ParameterInfo() { ParameterName = "WeekendSurcharge", ParameterValue = package.WeekendSurcharge });
            parameters.Add(new ParameterInfo() { ParameterName = "ResortFees", ParameterValue = package.ResortFees });
            parameters.Add(new ParameterInfo() { ParameterName = "ValidFrom", ParameterValue = package.ValidFrom });
            parameters.Add(new ParameterInfo() { ParameterName = "ValidTo", ParameterValue = package.ValidTo });
            parameters.Add(new ParameterInfo() { ParameterName = "EndDisplayDate", ParameterValue = package.EndDisplayDate });
            parameters.Add(new ParameterInfo() { ParameterName = "Visible", ParameterValue = package.Visible });
            parameters.Add(new ParameterInfo() { ParameterName = "SpecialPage", ParameterValue = package.SpecialPage });
            parameters.Add(new ParameterInfo() { ParameterName = "NightsFree", ParameterValue = package.NightsFree });
            parameters.Add(new ParameterInfo() { ParameterName = "NumberOfNights", ParameterValue = package.NumberOfNights });
            parameters.Add(new ParameterInfo() { ParameterName = "PercentOff", ParameterValue = package.PercentOff });
            parameters.Add(new ParameterInfo() { ParameterName = "PercentageOff", ParameterValue = package.PercentageOff });
            parameters.Add(new ParameterInfo() { ParameterName = "PricePoint", ParameterValue = package.PricePoint });
            parameters.Add(new ParameterInfo() { ParameterName = "Deposit", ParameterValue = package.Deposit });
            parameters.Add(new ParameterInfo() { ParameterName = "ExtraPersonFee", ParameterValue = package.ExtraPersonFee });
            parameters.Add(new ParameterInfo() { ParameterName = "PackageAllocation", ParameterValue = package.PackageAllocation });
            parameters.Add(new ParameterInfo() { ParameterName = "ShortDesc", ParameterValue = package.ShortDescription });

            try
            {
                id = DapperHelper.ExecuteQuery("dbo.genInsPackage", parameters, context);
            }
            catch (Exception ex) { }
            return id;


        }

        public int AddRoomToPackage(int packageId, List<int> roomIds)
        {
            int id = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "PackageID", ParameterValue = packageId });
            try
            {
                DapperHelper.ExecuteQuery("dbo.genDropRoomsFromPackage", parameters, context);
                foreach (int roomId in roomIds)
                {
                    parameters = new List<ParameterInfo>();
                    parameters.Add(new ParameterInfo() { ParameterName = "PackageID", ParameterValue = packageId });
                    parameters.Add(new ParameterInfo() { ParameterName = "RoomTypeID", ParameterValue = roomId });
                    id = DapperHelper.ExecuteQuery("dbo.genInsPackageRoom", parameters, context);
                }
            }
            catch (Exception ex) { }
            return id;
        }

        public void Delete(int packageId)
        {

            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "Packageid", ParameterValue = packageId });
            try
            {
                DapperHelper.ExecuteQuery("dbo.genInvisiblePackage", parameters, context);
            }
            catch (Exception ex) { }
        }

        public Package GetPackage(int packageId)
        {
            Package package = null;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "PackageID", ParameterValue = packageId });
            try
            {
                package = DapperHelper.GetRecord<Package>("dbo.genSelPackageByID", parameters, context);
            }
            catch (Exception e) { }
            return package;
        }

        public List<Package> GetPackages(int hotelId)
        {
            List<Package> packages = new List<Package>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "HotelID", ParameterValue = hotelId });

            try
            {
                packages = DapperHelper.GetRecords<Package>("dbo.genSelPackagesByHotelID", parameters, context);
            }
            catch (Exception e) { }
            return packages;
        }

        public List<Package> GetPackagesByRoom(int roomId, DateTime start, DateTime end)
        {
            List<Package> packages = new List<Package>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "RoomTypeID", ParameterValue = roomId });
            parameters.Add(new ParameterInfo() { ParameterName = "Start", ParameterValue = start });
            parameters.Add(new ParameterInfo() { ParameterName = "End", ParameterValue = end });
            try
            {
                packages = DapperHelper.GetRecords<Package>("dbo.genSelPackagesAssociatedWithRoom", parameters, context);
            }
            catch (Exception e) { }
            return packages;
        }

        public List<Package> GetPackagesForSpecialPage(int hotelId)
        {
            List<Package> packages = new List<Package>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "HotelID", ParameterValue = hotelId });

            try
            {
                packages = DapperHelper.GetRecords<Package>("dbo.genSelPackageForSpecialPage", parameters, context);
            }
            catch (Exception e) { }
            return packages;
        }

        public List<RoomType> GetRoomTypes(int packageid)
        {
            List<RoomType> roomTypes = new List<RoomType>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "PackageID", ParameterValue = packageid });
            try
            {
                roomTypes = DapperHelper.GetRecords<RoomType>("dbo.genSelPackageRooms", parameters, context);
            }
            catch (Exception e) { }
            return roomTypes;

        }

        public List<Package> GetPackagesWithRackRate(int hotelId)
        {
            List<Package> packages = new List<Package>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "HotelID", ParameterValue = hotelId });

            try
            {
                packages = DapperHelper.GetRecords<Package>("dbo.genSelPackagesWithRates", parameters, context);
            }
            catch (Exception e) { }
            return packages;
        }

        public void Update(Package package)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "Name", ParameterValue = package.Name });
            parameters.Add(new ParameterInfo() { ParameterName = "Description", ParameterValue = package.Description });
            parameters.Add(new ParameterInfo() { ParameterName = "PackageID", ParameterValue = package.PackageID });
            parameters.Add(new ParameterInfo() { ParameterName = "Amenities", ParameterValue = false });
            parameters.Add(new ParameterInfo() { ParameterName = "ArrMon", ParameterValue = package.ArrMon });
            parameters.Add(new ParameterInfo() { ParameterName = "ArrTues", ParameterValue = package.ArrTues });
            parameters.Add(new ParameterInfo() { ParameterName = "ArrWed", ParameterValue = package.ArrWed });
            parameters.Add(new ParameterInfo() { ParameterName = "ArrThur", ParameterValue = package.ArrThurs });
            parameters.Add(new ParameterInfo() { ParameterName = "ArrFri", ParameterValue = package.ArrFri });
            parameters.Add(new ParameterInfo() { ParameterName = "ArrSat", ParameterValue = package.ArrSat });
            parameters.Add(new ParameterInfo() { ParameterName = "ArrSun", ParameterValue = package.ArrSun });
            parameters.Add(new ParameterInfo() { ParameterName = "MinDays", ParameterValue = package.MinDays });
            parameters.Add(new ParameterInfo() { ParameterName = "MaxDays", ParameterValue = package.MaxDays });
            parameters.Add(new ParameterInfo() { ParameterName = "WeekendSurcharge", ParameterValue = package.WeekendSurcharge });
            parameters.Add(new ParameterInfo() { ParameterName = "ResortFees", ParameterValue = package.ResortFees });
            parameters.Add(new ParameterInfo() { ParameterName = "ValidFrom", ParameterValue = package.ValidFrom });
            parameters.Add(new ParameterInfo() { ParameterName = "ValidTo", ParameterValue = package.ValidTo });
            parameters.Add(new ParameterInfo() { ParameterName = "EndDisplayDate", ParameterValue = package.EndDisplayDate });
            parameters.Add(new ParameterInfo() { ParameterName = "Visible", ParameterValue = package.Visible });
            parameters.Add(new ParameterInfo() { ParameterName = "SpecialPage", ParameterValue = package.SpecialPage });
            parameters.Add(new ParameterInfo() { ParameterName = "NightsFree", ParameterValue = package.NightsFree });
            parameters.Add(new ParameterInfo() { ParameterName = "NumberOfNights", ParameterValue = package.NumberOfNights });
            parameters.Add(new ParameterInfo() { ParameterName = "PercentOff", ParameterValue = package.PercentOff });
            parameters.Add(new ParameterInfo() { ParameterName = "PercentageOff", ParameterValue = package.PercentageOff });
            parameters.Add(new ParameterInfo() { ParameterName = "PricePoint", ParameterValue = package.PricePoint });
            parameters.Add(new ParameterInfo() { ParameterName = "Deposit", ParameterValue = package.Deposit });
            parameters.Add(new ParameterInfo() { ParameterName = "ExtraPersonFee", ParameterValue = package.ExtraPersonFee });
            parameters.Add(new ParameterInfo() { ParameterName = "PackageAllocation", ParameterValue = package.PackageAllocation });
            parameters.Add(new ParameterInfo() { ParameterName = "SmImage", ParameterValue = package.SmImage });
            parameters.Add(new ParameterInfo() { ParameterName = "ShortDesc", ParameterValue = package.ShortDescription });
            try
            {
                DapperHelper.ExecuteQuery("dbo.genUpdPackage", parameters, context);
            }
            catch (Exception ex) { }

        }        

        public void UpdateImageToPackage(int packageId, string image)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "PackageID", ParameterValue = packageId });
            parameters.Add(new ParameterInfo() { ParameterName = "SmImage", ParameterValue = image });
            try
            {
                DapperHelper.ExecuteQuery("dbo.genUpdPackageImage", parameters, context);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
