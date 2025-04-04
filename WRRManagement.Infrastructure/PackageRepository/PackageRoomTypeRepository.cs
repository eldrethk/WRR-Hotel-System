using System;
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
    public class PackageRoomTypeRepository:GenericRepository<PackageRoomType>, IPackageRoomType
    {
        protected new WRRContext context;
        public PackageRoomTypeRepository(WRRContext context) : base(context)
        {
            this.context = context;
        }

        public void AddRoomToPackage(int packageId, List<int> roomIds)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "Packageid", ParameterValue = packageId });
            try
            {
                DapperHelper.ExecuteQuery("dbo.genDropRoomsFromPackage", parameters, context);
            }
            catch (Exception ex) { }

            foreach (int x in roomIds)
            {
                parameters = new List<ParameterInfo>();
                parameters.Add(new ParameterInfo() { ParameterName = "packageid", ParameterValue = packageId });
                parameters.Add(new ParameterInfo() { ParameterName = "RoomID", ParameterValue = x });
                try
                {
                    DapperHelper.ExecuteQuery("dbo.genInsPackageRoom", parameters, context);
                }
                catch (Exception ex) { }

            }
        }

        public List<RoomType> GetAllValidRoomBasedOnGuestCount(int packageid, int adult, int child)
        {
            List<RoomType> rooms = new List<RoomType>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            return rooms;
        }

        public List<RoomType> GetRoomTypes(int packageid)
        {
            List<RoomType> rooms = new List<RoomType>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="PackageID", ParameterValue=packageid });
            try
            {
                rooms = DapperHelper.GetRecords<RoomType>("dbo.genSelPackageRooms", parameters, context);
            }
            catch (Exception ex) { }    
            return rooms;
        }
    }
}
