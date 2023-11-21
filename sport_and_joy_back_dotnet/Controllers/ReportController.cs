using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using sport_and_joy_back_dotnet.Data.Repository.Interfaces;

namespace sport_and_joy_back_dotnet.Controllers
{
    [Route("api/Reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IConverter _pdfConverter;
        public ReportsController(IUserRepository userRepository, IReservationRepository reservationRepository, IConverter pdfConverter)
        {
            _userRepository = userRepository;
            _reservationRepository = reservationRepository;
            _pdfConverter = pdfConverter;
        }


        [HttpGet("players-with-reservations")]
        public async Task<IActionResult> PlayersWithReservationsReport()
        {
            try
            {
                var users = await _userRepository.PlayersWithReservations();
                var htmlContent = await System.IO.File.ReadAllTextAsync("ReportsHtml/PlayersWithReservations.html");
                var pdfBytes = _pdfConverter.Convert(new HtmlToPdfDocument()
                {
                    GlobalSettings = {},
                    Objects = {
                        new ObjectSettings() {
                            HtmlContent = htmlContent,
                        }
                    }
                });
                return File(pdfBytes, "application/pdf", "players-with-reservations-report.pdf");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("owners-with-fields")]
        public async Task<IActionResult> OwnersWithFieldsReport()
        {
            try
            {
                var users = await _userRepository.OwnersWithFields();
                var htmlContent = await System.IO.File.ReadAllTextAsync("ReportsHtml/OwnersWithFields.html");
                var pdfBytes = _pdfConverter.Convert(new HtmlToPdfDocument()
                {
                    GlobalSettings = {},
                    Objects = {
                        new ObjectSettings() {
                            HtmlContent = htmlContent,
                        }
                    }
                });
                return File(pdfBytes, "application/pdf", "owners-with-fields-report.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("reservations-in-month/{month}")]
        public async Task<IActionResult> ReservationsInMonthReport(int month)
        {
            try
            {
                var reservations = await _reservationRepository.ReservationsInMonth(month);
                var htmlContent = await System.IO.File.ReadAllTextAsync("ReportsHtml/ReservationsInMonth.html");
                var pdfBytes = _pdfConverter.Convert(new HtmlToPdfDocument()
                {
                    GlobalSettings = {},
                    Objects = {
                        new ObjectSettings() {
                            HtmlContent = htmlContent,
                        }
                    }
                });
                return File(pdfBytes, "application/pdf", $"reservations-in-month-{month}.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}