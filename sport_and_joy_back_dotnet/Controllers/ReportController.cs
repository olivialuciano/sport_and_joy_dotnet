using iText.Kernel.Pdf;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using iText.Layout;
using sport_and_joy_back_dotnet.Data.Repository.Interfaces;
using System.Reflection.Metadata;
using sport_and_joy_back_dotnet.Entities;
using static iText.IO.Codec.TiffWriter;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace sport_and_joy_back_dotnet.Controllers
{
    [Route("api/Reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly IReservationRepository _reservationRepository;
        public ReportsController(IUserRepository userRepository, IReservationRepository reservationRepository)
        {
            _userRepository = userRepository;
            _reservationRepository = reservationRepository;
        }



        [HttpGet("players-with-reservations")]
        public IActionResult PlayersWithReservationsReport()
        {
            var users = _userRepository.PlayersWithReservations().Result;
            using (var pdfStream = new MemoryStream())
            {
                using (var writer = new PdfWriter(pdfStream))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new iText.Layout.Document(pdf);
                        document.Add(new iText.Layout.Element.Paragraph("TECNICATURA UNIVERSITARIA EN PROGRAMACIÓN - UNIVERSIDAD AUSTRAL"));
                        document.Add(new iText.Layout.Element.Paragraph("Trabajo Práctico Integrador final - Proyecto de Laboratorio"));
                        document.Add(new iText.Layout.Element.Paragraph("Alumnos: Lucila Ansaldi, Sebastián Comparada, Olivia Luciano y Victoria Svaikauskas"));
                        document.Add(new iText.Layout.Element.Paragraph("Informe de Usuarios jugadores con reservas"));

                        var table = new iText.Layout.Element.Table(5);
                        table.AddHeaderCell("ID");
                        table.AddHeaderCell("Nombre");
                        table.AddHeaderCell("Apellido");
                        table.AddHeaderCell("Email");
                        table.AddHeaderCell("Nro. de reservas");

                        if (users != null)
                        {
                            foreach (var usr in users)
                            {
                                table.AddCell(usr.Id.ToString());
                                table.AddCell(usr.FirstName);
                                table.AddCell(usr.LastName);
                                table.AddCell(usr.Email);
                                table.AddCell(usr.ReservationsCount.ToString());

                            }
                            document.Add(table);
                        }
                    }
                }
                return File(pdfStream.ToArray(), "application/pdf", "InformeJugadoresConReservas.pdf");
            }
        }


        [HttpGet("owners-with-fields")]
        public IActionResult OwnersWithFieldsReport()
        {
            var users = _userRepository.OwnersWithFields().Result;
            using (var pdfStream = new MemoryStream())
            {
                using (var writer = new PdfWriter(pdfStream))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new iText.Layout.Document(pdf);
                        document.Add(new iText.Layout.Element.Paragraph("TECNICATURA UNIVERSITARIA EN PROGRAMACIÓN - UNIVERSIDAD AUSTRAL"));
                        document.Add(new iText.Layout.Element.Paragraph("Trabajo Práctico Integrador final - Proyecto de Laboratorio"));
                        document.Add(new iText.Layout.Element.Paragraph("Alumnos: Lucila Ansaldi, Sebastián Comparada, Olivia Luciano y Victoria Svaikauskas"));
                        document.Add(new iText.Layout.Element.Paragraph("Informe de Usuarios dueños con canchas"));

                        var table = new iText.Layout.Element.Table(5);
                        table.AddHeaderCell("ID");
                        table.AddHeaderCell("Nombre");
                        table.AddHeaderCell("Apellido");
                        table.AddHeaderCell("Email");
                        table.AddHeaderCell("Nro. de canchas");

                        if (users != null)
                        {
                            foreach (var usr in users)
                            {
                                table.AddCell(usr.Id.ToString());
                                table.AddCell(usr.FirstName);
                                table.AddCell(usr.LastName);
                                table.AddCell(usr.Email);
                                table.AddCell(usr.FieldsCount.ToString());

                            }
                            document.Add(table);
                        }
                    }
                }
                return File(pdfStream.ToArray(), "application/pdf", "InformeDueniosConCanchas.pdf");
            }
        }

            [HttpGet("reservations-in-month/{month}")]
            public IActionResult ReservationsInMonthReport(int month, int year)
            {

                var reservations = _reservationRepository.ReservationsInMonth(month, year).Result;
                using (var pdfStream = new MemoryStream())
                {
                    using (var writer = new PdfWriter(pdfStream))
                    {
                        using (var pdf = new PdfDocument(writer))
                        {
                            var document = new iText.Layout.Document(pdf);
                        document.Add(new iText.Layout.Element.Paragraph("TECNICATURA UNIVERSITARIA EN PROGRAMACIÓN - UNIVERSIDAD AUSTRAL"));
                        document.Add(new iText.Layout.Element.Paragraph("Trabajo Práctico Integrador final - Proyecto de Laboratorio"));
                        document.Add(new iText.Layout.Element.Paragraph("Alumnos: Lucila Ansaldi, Sebastián Comparada, Olivia Luciano y Victoria Svaikauskas"));
                        document.Add(new iText.Layout.Element.Paragraph($"Informe de Reservas hechas en el mes {month} del año {year}"));

                            var table = new iText.Layout.Element.Table(7);
                            table.AddHeaderCell("ID");
                            table.AddHeaderCell("Fecha");
                            table.AddHeaderCell("ID Usr");
                            table.AddHeaderCell("Mail Usr");
                            table.AddHeaderCell("ID Cancha");
                            table.AddHeaderCell("Nombre Cancha");
                            table.AddHeaderCell("Precio Cancha");

                            if (reservations != null)
                            {
                                foreach (var res in reservations)
                                {
                                    table.AddCell(res.Id.ToString());
                                    table.AddCell(res.Date.ToString());
                                    table.AddCell(res.UserId.ToString());
                                    table.AddCell(res.UserEmail.ToString());
                                    table.AddCell(res.FieldId.ToString());
                                    table.AddCell(res.FieldName.ToString());
                                    table.AddCell(res.FieldPrice.ToString());

                                }
                                document.Add(table);
                            }
                        }
                    }
                    return File(pdfStream.ToArray(), "application/pdf", $"InformeReservas{month}-{year}.pdf");

                }
            }
     }
} 