using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp_API.Models;

namespace QuizApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantController : ControllerBase
    {

        readonly private QuizDBContext _quizDBContext;

        public ParticipantController(QuizDBContext quizDBContext)
        {
           _quizDBContext = quizDBContext;
        }
        
        [HttpPost]
        [Route("Insert")]
        public Participant Insert(Participant model)
        {
            _quizDBContext.Add(model);
            _quizDBContext.SaveChanges();
            return model;
        }

        [HttpPost]
        [Route("Update")]
        public void UpdateOutput(Participant model)
        {
            if(ModelState.IsValid)
            {
                _quizDBContext.Entry(model).State = EntityState.Modified;
                _quizDBContext.SaveChanges();
            }
           
        }
    }
}
