using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        [Route("Get")]
        public ActionResult GetCategory(int CategoryId)
        {
            return Ok(_quizDBContext.QuestionCategory.Where(x => x.CategoryId == CategoryId).FirstOrDefault());
        }

        [HttpPost]
        [Route("Insert")]
        public Boolean Insert(QuestionCategory model)
        {
            try
            {
                // Get the maximum id from database
                var max = _quizDBContext.QuestionCategory.DefaultIfEmpty().Max(r => r == null ? 0 : r.CategoryId);
                model.CategoryId = max + 1;
                _quizDBContext.Add(model);
                _quizDBContext.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        [HttpPut]
        [Route("Edit/{CategoryId}")]
        public ActionResult EditCategory(int CategoryId, QuestionCategory model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCategory = _quizDBContext.QuestionCategory.Where(q => q.CategoryId == CategoryId).FirstOrDefault();

            if (existingCategory != null)
            {
                existingCategory.Name = model.Name;
                existingCategory.Description = model.Description;

                try
                {
                    _quizDBContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }
            else
            {
                NotFound();
            }

            return Ok();
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
