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
    public class RequestsController : Controller
    {
        #region Constructors and Consts
        private readonly HelpComingDbContext helpComingDbContext;

        public RequestsController(HelpComingDbContext helpComingDbContext)
        {
            this.helpComingDbContext = helpComingDbContext;
        }
        #endregion
       
        #region Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RequestViewModel> requestViewModels = new List<RequestViewModel>();
            List<Request> requests = await helpComingDbContext.Requests.OrderByDescending(r => r.RequestDate).ToListAsync();

            foreach (Request request in requests)
            {
                string? username = await helpComingDbContext.Users
                    .Where(u => u.UserID == request.CreateUser)
                    .Select(u => u.Username)
                    .FirstOrDefaultAsync();

                RequestViewModel requestViewModel = new RequestViewModel()
                {
                    Request = request,
                    Username = username
                };

                requestViewModels.Add(requestViewModel);
            }
            return View(requestViewModels);
        }

        [HttpGet]
        public IActionResult Read(Guid requestID)
        {
            Request? request = helpComingDbContext.Requests.FirstOrDefault(r => r.RequestID == requestID);
            if (request != null)
            {
                User? user = helpComingDbContext.Users.FirstOrDefault(u => u.UserID == request.CreateUser);

                RequestViewModel model = new RequestViewModel();
                model.Request = request;
                model.Replies = helpComingDbContext.Replies.Where(r => r.RequestID == request.RequestID).ToList();

                if (user != null)
                {
                    model.Username = user.Username;
                }
                return View("Read", model);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            CreateRequestViewModel model = new CreateRequestViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRequestViewModel createRequestViewModel) 
        {
            Guid currentUser = GetCurrentUserID();
            Request request = new Request()
            {
                RequestID = Guid.NewGuid(),
                PersonName= createRequestViewModel.PersonName,
                LastSeenLocation = createRequestViewModel.LastSeenLocation,
                LastSeenDateTime = createRequestViewModel.LastSeenDateTime,
                Description = createRequestViewModel.Description,
                CreateUser = currentUser,
                RequestDate = DateTime.Now
            };
            if (createRequestViewModel.Photo != null)
            {
                request.Photo = createRequestViewModel.Photo;
            }
            await helpComingDbContext.Requests.AddAsync(request);
            await helpComingDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(Guid requestID)
        {
            Request? request = helpComingDbContext.Requests.FirstOrDefault(r => r.RequestID == requestID);
            if (request != null)
            {
                UpdateRequestViewModel model = new UpdateRequestViewModel
                {
                    RequestID = request.RequestID,
                    PersonName = request.PersonName,
                    Photo = request.Photo,
                    LastSeenLocation = request.LastSeenLocation,
                    LastSeenDateTime = request.LastSeenDateTime,
                    Description = request.Description,
                    CreateUser = request.CreateUser,
                    RequestDate = request.RequestDate
                };
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateRequestViewModel model)
        {
            try
            {
                Request? request = await helpComingDbContext.Requests.FirstOrDefaultAsync(r => r.RequestID.ToString() == model.RequestID.ToString());
                if (request != null)
                {
                    request.PersonName = model.PersonName;
                    request.Photo = model.Photo;
                    request.LastSeenLocation = model.LastSeenLocation;
                    request.LastSeenDateTime = model.LastSeenDateTime;
                    request.Description = model.Description;

                    await helpComingDbContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {

            }
            return RedirectToAction("Index");
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
        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(Guid requestID)
        {
            Request? request = await helpComingDbContext.Requests.FirstOrDefaultAsync(r => r.RequestID == requestID);
            if (request != null)
            {
                helpComingDbContext.Requests.Remove(request);
                await helpComingDbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Requests");
        }
        #endregion
    }
}
