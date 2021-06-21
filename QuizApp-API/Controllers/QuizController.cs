using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp_API.Models;

namespace QuizApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private QuizDBContext _quizDBContext = new QuizDBContext();
        public class QuestionData{
          public int[] qIDs { get; set; }
          public int categoryId { get; set; }
        }
        public QuizController(QuizDBContext quizDBContext)
        {
            _quizDBContext = quizDBContext;
        }

        [HttpGet]
        [Route("Questions")]
        public ActionResult GetQuestions(int CategoryId, int numOfQuestions)
        {
            var Qns = _quizDBContext.Question.Where(x => x.CategoryId == CategoryId)
                .Select(x => new{ x.QnID, x.Qn, x.ImageName, x.Option1, x.Option2, x.Option3, x.Option4, x.Answer, x.Comment})
                .OrderBy(y => Guid.NewGuid())
                .Take(numOfQuestions)
                .ToArray();
            var formatQnsOptions = Qns.AsEnumerable()
                .Select(x => new{x.QnID, x.Qn, x.ImageName,Options = new String[]{x.Option1, x.Option2, x.Option3, x.Option4}, x.Answer, x.Comment })
                .ToList();
            return Ok(formatQnsOptions);
        }
    }
}
