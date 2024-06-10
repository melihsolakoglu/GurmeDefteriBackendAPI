using GurmeDefteriWebUI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;

namespace GurmeDefteriWebUI.Controllers
{
    public class InformationController : Controller
    {
        private readonly InformationService _informationService;
        public InformationController()
        {
            _informationService ??= new();
        }

        public IActionResult Index()
        {
            return View();
        }        

            public async Task<IActionResult> DownloadLogsAsync(string date)
        {
            string filneName = $"{DateTime.Now}GurmeDefteriLogs.txt";
            string content = await  _informationService.GetLogsWithDate(ConvertDateFormat(date)) ;
            byte[] byteArray = Encoding.UTF8.GetBytes(content);
            MemoryStream stream = new MemoryStream(byteArray);
            return File(stream, "text/plain", filneName);
        }

        static string ConvertDateFormat(string dateString)
        {
            // Girdi string'ini DateTime olarak parse et
            DateTime date = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            // DateTime'ı istenen formatta string'e dönüştür
            return date.ToString("dd-MM-yyyy");
        }
    }
}
