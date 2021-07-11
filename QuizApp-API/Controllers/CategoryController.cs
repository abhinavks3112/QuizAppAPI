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
            List<QuestionCategory> lstCategories = new List<QuestionCategory>();
            lstCategories = _quizDBContext.QuestionCategory.ToList();

            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            var converted = JsonConvert.SerializeObject(lstCategories, jsSettings);
            return Content(converted, "application/json");
        }

        [HttpGet]
        [Route("LoadCategories")]
        public ActionResult LoadCategories()
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

        [HttpPost]
        [Route("Delete")]
        public Boolean Delete([FromBody] int Id)
        {
            try
            {
                QuestionCategory model = _quizDBContext.QuestionCategory.Where(q => q.CategoryId == Id).FirstOrDefault();
                if (model != null)
                {
                    _quizDBContext.Remove(model);
                    _quizDBContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
