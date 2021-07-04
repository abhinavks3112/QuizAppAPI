using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuizApp_API.Models;

namespace QuizApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private QuizDBContext _quizDBContext;
        public CategoryController(QuizDBContext quizDBContext)
        {
            _quizDBContext = quizDBContext;
        }

        [HttpGet]
        [Route("GetCategories")]
        public ActionResult GetCategories()
        {
            List<DropdownModel> lstCategories= _quizDBContext.QuestionCategory.Select(x => new DropdownModel { 
             Id = x.CategoryId,
             Name = x.Name
            }).ToList();

            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            var converted = JsonConvert.SerializeObject(lstCategories, jsSettings);
            return Content(converted, "application/json");
        }
    }
}
