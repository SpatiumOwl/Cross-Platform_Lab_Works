using Lab_05_Prog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab_05_Prog.Controllers
{
    [Authorize]
    public class LabsController : Controller
    {
        public IActionResult Lab01()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Lab01(ProcessorModel model)
        {
            try
            {
                string inputData = new StreamReader(model.Data.OpenReadStream()).ReadToEnd().Trim();
                model.Response = Lab_05_Lib.Lab01.Calculate(inputData);
                model.Calculated = true;
            }
            catch (Exception ex)
            {
                model.ErrorValue = ex.Message;
            }
            return View(model);
        }
        public IActionResult Lab02()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Lab02(ProcessorModel model)
        {
            try
            {
                string inputData = new StreamReader(model.Data.OpenReadStream()).ReadToEnd().Trim();
                model.Response = Lab_05_Lib.Lab02.Calculate(inputData);
                model.Calculated = true;
            }
            catch (Exception ex)
            {
                model.ErrorValue = ex.Message;
            }
            return View(model);
        }
        public IActionResult Lab03()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Lab03(ProcessorModel model)
        {
            try
            {
                string inputData = new StreamReader(model.Data.OpenReadStream()).ReadToEnd().Trim();
                model.Response = Lab_05_Lib.Lab03.Calculate(inputData);
                model.Calculated = true;
            }
            catch (Exception ex)
            {
                model.ErrorValue = ex.Message;
            }
            return View(model);
        }
    }
}
