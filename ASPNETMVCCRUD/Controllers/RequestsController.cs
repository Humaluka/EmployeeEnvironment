using HelpComing.Data;
using HelpComing.Models;
using HelpComing.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HelpComing.Controllers
{
    public class RequestsController : Controller
    {
        private readonly HelpComingDbContext helpComingDbContext;

        public RequestsController(HelpComingDbContext helpComingDbContext)
        {
            this.helpComingDbContext = helpComingDbContext;
        }
        [HttpGet]

        public async Task<IActionResult> Index()
        {
            List<RequestViewModel> requestViewModels = new List<RequestViewModel>();
            List<Request> requests = await helpComingDbContext.Requests.ToListAsync();

            foreach (var request in requests)
            {
                var username = await helpComingDbContext.Users
                    .Where(u => u.UserID == request.CreateUser)
                    .Select(u => u.Username)
                    .FirstOrDefaultAsync();

                var requestViewModel = new RequestViewModel
                {
                    Request = request,
                    Username = username
                };

                requestViewModels.Add(requestViewModel);
            }

            return View(requestViewModels);
        }



        [HttpGet]
        public IActionResult Create()
        {
            CreateRequestViewModel model = new CreateRequestViewModel();

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateRequestViewModel createRequestViewModel) 
        {
            var request = new Models.Domain.Request()
            {
                RequestID = Guid.NewGuid(),
                PersonName= createRequestViewModel.PersonName,
                Photo = createRequestViewModel.Photo,
                LastSeenLocation = createRequestViewModel.LastSeenLocation,
                LastSeenDateTime = createRequestViewModel.LastSeenDateTime,
                Description = createRequestViewModel.Description,
                CreateUser = createRequestViewModel.CreateUser
            };
            await helpComingDbContext.Requests.AddAsync(request);
            await helpComingDbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
