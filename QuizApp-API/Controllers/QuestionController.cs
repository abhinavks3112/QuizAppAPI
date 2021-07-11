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
        readonly private QuizDBContext _quizDBContext;
        public QuestionController(QuizDBContext quizDBContext)
        {
            _quizDBContext = quizDBContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult GetAllQuestions()
        {
            JsonSerializerSettings jsSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
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
            catch (Exception)
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

            var existingQuestion = _quizDBContext.Question.Where(q => q.QnID == QnID).FirstOrDefault();

            if(existingQuestion != null)
            {
                existingQuestion.CategoryId = model.CategoryId;
                existingQuestion.Qn = model.Qn;
                existingQuestion.ImageName = model.ImageName;
                existingQuestion.Option1 = model.Option1;
                existingQuestion.Option2 = model.Option2;
                existingQuestion.Option3 = model.Option3;
                existingQuestion.Option4 = model.Option4;
                existingQuestion.Answer = model.Answer;
                existingQuestion.Comment = model.Comment;
                
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
                        NotFound();
                    }
                }
            }
            else
            {
                NotFound();
            }

            return Ok();
        }

        private bool QuestionExists(int id)
        {
            return _quizDBContext.Question.Count(e => e.QnID == id) > 0;
        }

        [HttpPost]
        [Route("Delete")]
        public Boolean Delete([FromBody]int QnID)
        {
            try
            {
                Question model = _quizDBContext.Question.Where(q => q.QnID == QnID).FirstOrDefault();
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
