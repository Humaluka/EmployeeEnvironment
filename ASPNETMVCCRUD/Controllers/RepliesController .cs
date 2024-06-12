using HelpComing.Data;
using HelpComing.Models;
using HelpComing.Models.Domain;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace HelpComing.Controllers
{
    public class RepliesController : Controller
    {
        #region Constructors and Consts
        private readonly HelpComingDbContext helpComingDbContext;

        public RepliesController(HelpComingDbContext helpComingDbContext)
        {
            this.helpComingDbContext = helpComingDbContext;
        }
        #endregion

        #region Reply
        [HttpGet]
        public IActionResult Comment(Guid requestID)
        {
            CreateReplyViewModel model = new CreateReplyViewModel();
            model.RequestID = requestID;

            return View("Comment", model);
        }

        [HttpPost]
        public async Task<IActionResult> ReplyToRequest(CreateReplyViewModel model)
        {
            Request? request = await helpComingDbContext.Requests.FirstOrDefaultAsync(r => r.RequestID == model.RequestID);
            if (request != null)
            {
                Guid currentUser = GetCurrentUserID();
                Reply reply = new Reply()
                {
                    ReplyID = Guid.NewGuid(),
                    RequestID = request.RequestID,
                    PostUser = currentUser,
                    ReplyMessage = model.ReplyMessage,
                    ReplyDateTime = DateTime.Now
                };
                await helpComingDbContext.Replies.AddAsync(reply);
                await helpComingDbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Requests");
        }
        #endregion

        #region Methods
        public Guid GetCurrentUserID()
        {
            if (User.Identity.IsAuthenticated)
            {
                string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (Guid.TryParse(userIdString, out Guid userId))
                {
                    return userId;
                }
            }
            return Guid.Empty;
        }

        public static string? GetUsernameByUserId(Guid? userId, HelpComingDbContext helpComingDbContext)
        {
            using (helpComingDbContext)
            {
                User user = helpComingDbContext.Users.FirstOrDefault(u => u.UserID == userId);
                if (user != null)
                {
                    return user.Username;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion
    }
}
