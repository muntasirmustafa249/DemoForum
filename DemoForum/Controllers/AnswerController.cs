using DemoForum.Models;
using DemoForum.Services;
using DemoForum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoForum.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly AnswerService _answerService;
        private readonly AccountService _accountService;
        public AnswerController(AnswerService answerService, AccountService accountService)
        {
            _answerService = answerService;
            _accountService = accountService;
        }

        [HttpPost(Name = "PostAnswer")]
        public async Task<ActionResult> PostAnswer([FromBody] NewAnswerModel newAnswer)
        {
            var auth = Request.Headers.Authorization;

            var processedToken = await _accountService.PrepareTokenData(auth);

            if (processedToken.TokenFound)
            {
                var tokenData = await _accountService.GetTokenData(processedToken.Token);

                newAnswer.Email = tokenData.EmailAddress;
                newAnswer.UserName = tokenData.UserName;
            }
            else
            {
                if (string.IsNullOrEmpty(newAnswer.Email) || string.IsNullOrEmpty(newAnswer.UserName)) return ValidationProblem("You must provide your username and email address");
            }

            await _answerService.InsertAnswer(newAnswer);
            return Created();
        }

        [HttpPut(Name = "EditAnswer"), Authorize]
        public async Task<ActionResult> EditAnswer([FromBody] EditAnswerModel editAnswer)
        {
            var auth = Request.Headers.Authorization;

            var processedToken = await _accountService.PrepareTokenData(auth);

            var tokenData = await _accountService.GetTokenData(processedToken.Token);

            if (!tokenData.IsTokenValid) return ValidationProblem("Session expired");

            var answer = await _answerService.GetAnswer(editAnswer.Id);

            if (tokenData.UserName != answer.UserName || tokenData.EmailAddress != answer.Email) return Unauthorized("You are not allowed to edit this answer");

            await _answerService.UpdateAnswer(editAnswer);

            return Ok("Answer updated");
        }

        [HttpPut("UpvoteAnswer/{id:int}"), Authorize]
        public async Task<ActionResult> UpvoteAnswer(int id)
        {
            var auth = Request.Headers.Authorization;

            var processedToken = await _accountService.PrepareTokenData(auth);

            var tokenData = await _accountService.GetTokenData(processedToken.Token);

            if (!tokenData.IsTokenValid) return ValidationProblem("Session expired");

            var answer = await _answerService.GetAnswer(id);

            if (tokenData.UserName == answer.UserName || tokenData.EmailAddress == answer.Email) return Unauthorized("You can not upvote your own answer.");

            await _answerService.UpvoteAnswer(id);

            return Ok("Answer upvoted")
        }

        [HttpPut("DownvoteAnswer/{id:int}"), Authorize]
        public async Task<ActionResult> DownvoteAnswer(int id)
        {
            var auth = Request.Headers.Authorization;

            var processedToken = await _accountService.PrepareTokenData(auth);

            var tokenData = await _accountService.GetTokenData(processedToken.Token);

            if (!tokenData.IsTokenValid) return ValidationProblem("Session expired");

            var answer = await _answerService.GetAnswer(id);

            if (tokenData.UserName == answer.UserName || tokenData.EmailAddress == answer.Email) return Unauthorized("You can not downvote your own answer.");

            await _answerService.DownvoteAnswer(id);

            return Ok("Answer downvoted");
        }
    }
}
