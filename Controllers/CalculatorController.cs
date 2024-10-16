using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using WebCalculator.Models;

namespace WebCalculator.Controllers
{
    public class CalculatorController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Calculate(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                ViewBag.Error = "Expression cannot be empty.";
                return View("Index");
            }

            try
            {
                var calculator = new Calculator(expression);
                var result = calculator.ConsoleCalculate();
                ViewBag.Result = result;
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CalculateFromFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Error = "File cannot be empty.";
                return View("Index");
            }

            var filePath = Path.GetTempFileName();

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileCalculator = new FileCalculator(filePath, "output.txt");
                var resultFilePath = fileCalculator.GetAnswerFilePath();
                ViewBag.ResultFilePath = resultFilePath;
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            finally
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            return View("Index");
        }
    }
}

