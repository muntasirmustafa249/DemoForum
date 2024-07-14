using DemoForum.Services;
using DemoForum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoForum.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly QuestionService _questionService;
        private readonly AccountService _accountService;
        public QuestionController(QuestionService questionService, AccountService accountService)
        {
            _questionService = questionService;
            _accountService = accountService;
        }

        [HttpGet(Name = "GetAllQuestions")]
        public async Task<ActionResult> GetAllQuestions()
        {
            var questions = await _questionService.GetQuestions();
            return Ok(questions);
        }

        [HttpGet("GetQuestionDetail/{id:int}")]
        public async Task<ActionResult> GetQuestionDetail(int id)
        {
            var questionDetail = await _questionService.GetQuestionDetails(id);
            return Ok(questionDetail);
        }

        [HttpPost(Name = "PostQuestion")]
        public async Task<ActionResult> PostQuestion([FromBody] NewQuestionModel newQuestion)
        {
            var auth = Request.Headers.Authorization;

            var processedToken = await _accountService.PrepareTokenData(auth);

            if (processedToken.TokenFound)
            {
                var tokenData = await _accountService.GetTokenData(processedToken.Token);

                newQuestion.Email = tokenData.EmailAddress;
                newQuestion.UserName = tokenData.UserName;
            }
            else
            {
                if (string.IsNullOrEmpty(newQuestion.Email) || string.IsNullOrEmpty(newQuestion.UserName)) return ValidationProblem("You must provide your username and email address");
            }

            await _questionService.InsertQuestion(newQuestion);
            return Created();
        }

        [HttpPut(Name = "EditQuestion"), Authorize]
        public async Task<ActionResult> EditQuestion([FromBody] EditQuestionModel editQuestion)
        {
            var auth = Request.Headers.Authorization;

            var processedToken = await _accountService.PrepareTokenData(auth);

            var tokenData = await _accountService.GetTokenData(processedToken.Token);

            if (!tokenData.IsTokenValid) return ValidationProblem("Session expired");

            var question = await _questionService.GetQuestion(editQuestion.Id);

            if (tokenData.UserName != question.UserName || tokenData.EmailAddress != question.Email) return Unauthorized("You are not allowed to edit this question");

            await _questionService.UpdateQuestion(editQuestion);

            return Ok("Question updated");
        }

        [HttpPut("UpvoteQuestion/{id:int}"), Authorize]
        public async Task<ActionResult> UpvoteQuestion(int id)
        {
            var auth = Request.Headers.Authorization;

            var processedToken = await _accountService.PrepareTokenData(auth);

            var tokenData = await _accountService.GetTokenData(processedToken.Token);

            if (!tokenData.IsTokenValid) return ValidationProblem("Session expired");

            var question = await _questionService.GetQuestion(id);

            if (tokenData.UserName == question.UserName || tokenData.EmailAddress == question.Email) return Unauthorized("You can not upvote your own question.");

            await _questionService.UpvoteQuestion(id);

            return Ok("Question upvoted");
        }

        [HttpPut("DownvoteQuestion/{id:int}"), Authorize]
        public async Task<ActionResult> DownvoteQuestion(int id)
        {
            var auth = Request.Headers.Authorization;

            var processedToken = await _accountService.PrepareTokenData(auth);

            var tokenData = await _accountService.GetTokenData(processedToken.Token);

            if (!tokenData.IsTokenValid) return ValidationProblem("Session expired");

            var question = await _questionService.GetQuestion(id);

            if (tokenData.UserName == question.UserName || tokenData.EmailAddress == question.Email) return Unauthorized("You can not downvote your own question.");

            await _questionService.DownvoteQuestion(id);

            return Ok("Question downvoted");
        }
    }
}
