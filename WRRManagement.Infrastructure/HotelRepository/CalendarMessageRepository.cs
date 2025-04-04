using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WRRManagement.Domain.Base;
using WRRManagement.Domain.Hotels;
using WRRManagement.Infrastructure.Data;

namespace WRRManagement.Infrastructure.HotelRepository
{
    public class CalendarMessageRepository : GenericRepository<CalendarMessage>, ICalendarMessage
    {
        protected new WRRContext context;
        public CalendarMessageRepository(WRRContext context) : base(context)
        {
            this.context = context;
        }

        public int Add(CalendarMessage message)
        {
            int id = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "From", ParameterValue = message.DisplayFrom });
            parameters.Add(new ParameterInfo() { ParameterName="To", ParameterValue=message.DisplayTo});
            parameters.Add(new ParameterInfo() { ParameterName="Message", ParameterValue=message.Message });
            parameters.Add(new ParameterInfo() { ParameterName="HotelID", ParameterValue=message.HotelID});
            try
            {
                id = DapperHelper.ExecuteQuery("dbo.genInsCustomMessage", parameters, context);
            }
            catch (Exception ex) { }
            return id;
        }

        public void Delete(int id)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "MessageID", ParameterValue = id });
            try
            {
                DapperHelper.ExecuteQuery("dbo.genDelCustomMessage", parameters, context);
            }
            catch(Exception ex) { }
        }

        public CalendarMessage GetMessage(int id)
        {
            CalendarMessage message = new CalendarMessage();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "MessageID", ParameterValue = id });
            try
            {
                message = DapperHelper.GetRecord<CalendarMessage>("dbo.genSelCustomMessageByID", parameters, context);
            }
            catch( Exception ex) { }
            return message;            
        }

        public List<CalendarMessage> GetMessages(int hotelId)
        {
            List<CalendarMessage> messages = new List<CalendarMessage>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "HotelID", ParameterValue = hotelId });
            try
            {
                messages = DapperHelper.GetRecords<CalendarMessage>("dbo.genSelCustomMessageByHotelID", parameters, context);
            }
            catch (Exception ex) { }
            return messages;
        }

        public List<CalendarMessage> GetMessagesByDate(DateTime date, int hotelId)
        {
            List<CalendarMessage> message = new List<CalendarMessage>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "Date", ParameterValue = date});
            parameters.Add(new ParameterInfo() { ParameterName="HotelID", ParameterValue=hotelId});
            try
            {
                message = DapperHelper.GetRecords<CalendarMessage>("dbo.genSelCustomMessageByDate", parameters, context);
            }
            catch(Exception ex) { }
            return message;
        }

        public void Update(CalendarMessage message)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "From", ParameterValue = message.DisplayFrom });
            parameters.Add(new ParameterInfo() { ParameterName = "To", ParameterValue = message.DisplayTo });
            parameters.Add(new ParameterInfo() { ParameterName = "Message", ParameterValue = message.Message });
            parameters.Add(new ParameterInfo() { ParameterName = "MessageID", ParameterValue = message.MessageID});
            try
            {
                DapperHelper.ExecuteQuery("dbo.genUpdCustomMessage", parameters, context);
            }
            catch(Exception ex) { } 
        }
    }
}
