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
    public class CommentController : ControllerBase
    {
        private readonly CommentService _commentService;
        private readonly AccountService _accountService;
        public CommentController(CommentService commentService, AccountService accountService)
        {
            _commentService = commentService;
            _accountService = accountService;
        }

        [HttpPost(Name = "PostComment"), Authorize]
        public async Task<ActionResult> PostComment([FromBody] NewCommentModel newComment)
        {
            var auth = Request.Headers.Authorization;

            var processedToken = await _accountService.PrepareTokenData(auth);

            var tokenData = await _accountService.GetTokenData(processedToken.Token);

            newComment.Email = tokenData.EmailAddress;
            newComment.UserName = tokenData.UserName;

            await _commentService.InsertComment(newComment);
            return Created();
        }

        [HttpPut(Name = "EditComment"), Authorize]
        public async Task<ActionResult> EditComment([FromBody] EditCommentModel editComment)
        {
            var auth = Request.Headers.Authorization;

            var processedToken = await _accountService.PrepareTokenData(auth);

            var tokenData = await _accountService.GetTokenData(processedToken.Token);

            if (!tokenData.IsTokenValid) return ValidationProblem("Session expired");

            var comment = await _commentService.GetComment(editComment.Id);

            if (tokenData.UserName != comment.UserName || tokenData.EmailAddress != comment.Email) return Unauthorized("You are not allowed to edit this comment");

            await _commentService.UpdateComment(editComment);

            return Ok("Comment updated");
        }

        [HttpPut("UpvoteComment/{id:int}"), Authorize]
        public async Task<ActionResult> UpvoteComment(int id)
        {
            var auth = Request.Headers.Authorization;

            var processedToken = await _accountService.PrepareTokenData(auth);

            var tokenData = await _accountService.GetTokenData(processedToken.Token);

            if (!tokenData.IsTokenValid) return ValidationProblem("Session expired");

            var comment = await _commentService.GetComment(id);

            if (tokenData.UserName == comment.UserName || tokenData.EmailAddress == comment.Email) return Unauthorized("You can not upvote your own comment.");

            await _commentService.UpvoteComment(id);

            return Ok("Comment upvoted");
        }
    }
}
