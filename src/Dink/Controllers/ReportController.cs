using System.Net.Http;
using System.Threading.Tasks;
using DinkToPdf;
using Microsoft.AspNetCore.Mvc;

namespace Dink.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ReportController : Controller
    {
        [HttpGet]
        public IActionResult Summary()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Generate()
        {
            var url = "http://localhost:80/report/summary";
            var client = new HttpClient();
            var html = await client.GetStringAsync(url);

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    DPI = 96,
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = html,
                        WebSettings = {
                            DefaultEncoding = "utf-8",
                            MinimumFontSize = 60
                         },
                        HeaderSettings = { FontSize = 10, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                    }
                }
            };

            var converter = new SynchronizedConverter(new PdfTools());
            var result = converter.Convert(doc);
            return File(result, "application/pdf", $"qa-1.pdf");
        }
    }
}