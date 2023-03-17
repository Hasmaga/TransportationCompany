using Microsoft.AspNetCore.Mvc;
using TransportationCompany.Model;
using TransportationCompany.Model.Dto;

namespace TransportationCompany.Repositories
{
    public interface IBookingRepository
    {
        Task<ActionResult<BookingTripResDto>> BookingTripByCustomerAsync(BookingTripResDto book);
        Task<bool> CancelBookingByCustomerAsync(Guid BookingId);
        //Task<List<HistoryBookingResDto>> GetHistoryBookingByPassengerAsync();
        //Task<List<PresentBookingTicketResDto>> GetPresentBookingTicketByPassengerAsync();
    }
}
