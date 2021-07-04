using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class QuestionController : ControllerBase
    {
        private QuizDBContext _quizDBContext;
        public QuestionController(QuizDBContext quizDBContext)
        {
            _quizDBContext = quizDBContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult GetAllQuestions()
        {
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            var converted = JsonConvert.SerializeObject(_quizDBContext.Question.ToList(), jsSettings);
            return Content(converted, "application/json");
        }

        [HttpGet]
        [Route("Get")]
        public ActionResult GetQuestion(int QnID)
        {
            return Ok(_quizDBContext.Question.Where(x => x.QnID == QnID).FirstOrDefault());
        }

        [HttpPost]
        [Route("Insert")]
        public Boolean Insert(Question model)
        {
            try
            {
                _quizDBContext.Add(model);
                _quizDBContext.SaveChanges();
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }
        
        [HttpPut]
        [Route("Edit/{QnID}")]
        public ActionResult EditQuestion(int QnID, Question model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (QnID != model.QnID)
            {
                return BadRequest();
            }

            _quizDBContext.Entry(model).State = EntityState.Modified;

            try
            {
                _quizDBContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(QnID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        private bool QuestionExists(int id)
        {
            return _quizDBContext.Question.Count(e => e.QnID == id) > 0;
        }
    }
}
