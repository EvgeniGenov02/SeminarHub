using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SeminarHub.Data;
using SeminarHub.Data.Models;
using SeminarHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SeminarHub.Controllers
{
    [Authorize]
    public class SeminarController : Controller
    {

        private readonly SeminarHubDbContext data;

        public SeminarController(SeminarHubDbContext context)
        {
            data = context;
        }
        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        //All
        public async Task<IActionResult> All()
        {
            var entity = await data.Seminars.Select(s => new SeminarViewModel(
              s.Id,
              s.Topic,
              s.Lecturer,
              s.Details,
              s.OrganizerId,
              s.Organizer,
              s.DateAndTime.ToString(DataConstants.DataFormat),
              s.Duration,
              s.CategoryId,
              s.Category
              )).ToListAsync();

            return View(entity);
        }

        //Add
        public async Task<IActionResult> Add()
        {
            var sategorys = await data.Categorys.AsNoTracking().ToListAsync();
            var model = new SeminarFormModel();
            model.Categories = sategorys;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(SeminarFormModel model)
        {
            
            if (ModelState.IsValid)
            {
                return View(model); 
            }
            var category = await data.Categorys.FirstOrDefaultAsync(c => c == model.Category);

            if (category == null)
            {
                ModelState.AddModelError(nameof(SeminarFormModel.Category), "Category does not exist.");
            }

            var organizer = await data.Users.FindAsync(GetUserId()) 
             ?? throw new Exception("Organizer does not exist.");

            var seminar = new Seminar();

            seminar.Topic = model.Topic;
            seminar.Lecturer = model.Lecturer;
            seminar.Details = model.Details;
            seminar.OrganizerId = GetUserId();
            seminar.Organizer = organizer;
            seminar.DateAndTime = model.DateAndTime;
            seminar.Duration = model.Duration;
            seminar.CategoryId = model.CategoryId;
            seminar.Category = model.Category;
                
            await data.Seminars.AddAsync(seminar);

            await data.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        //Joined
        public async Task<IActionResult> Joined()
        {
            var seminars = await data.Seminars.ToListAsync();
            var userId = GetUserId();

            var seminarsParticipantsID = await data.SeminarsParticipants
                .Where(sp => sp.ParticipantId == userId)
                .Select(sp => sp.SeminarId)
                .ToListAsync();


            List<SeminarViewModel> viewData = new List<SeminarViewModel>();


            foreach (var item in seminars)
            {
                var organizer = await data.Users.FirstOrDefaultAsync(u => u.Id == item.OrganizerId)
                ?? throw new Exception("Organizer does not exist.");

                if (seminarsParticipantsID.Contains(item.Id))
                {
                    SeminarViewModel newSeminar = new SeminarViewModel(
                    item.Id,
                    item.Topic,
                    item.Lecturer,
                    item.Details,
                    item.OrganizerId,
                    organizer,
                    item.DateAndTime.ToString(DataConstants.DataFormat),
                    item.Duration,
                    item.CategoryId,
                    item.Category
                        );

                    var organiserUser = await data.Users.FindAsync(item.OrganizerId);

                    if (organiserUser == null)
                    {
                        return BadRequest();
                    }

                    viewData.Add(newSeminar);
                }

            }

            return View(viewData);
        }

        //Edit
        public async Task<IActionResult> Edit(int id)
        {
            var seminarForView = await data.Seminars.FirstOrDefaultAsync(e => e.Id == id);

            if (seminarForView == null)
            {
                return BadRequest();
            }

            var categorys = await data.Categorys.AsNoTracking().ToListAsync();

            SeminarFormModel seminarFormModel = new();

            seminarFormModel.Topic  = seminarForView.Topic ;
            seminarFormModel.Lecturer = seminarForView.Lecturer;
            seminarFormModel.Details = seminarForView.Details;
            seminarFormModel.DateAndTime = seminarForView.DateAndTime;
            seminarFormModel.Duration = seminarForView.Duration;
            seminarFormModel.CategoryId = seminarForView.CategoryId;
            seminarFormModel.Category = seminarForView.Category;
            seminarFormModel.Categories = categorys;


            return View(seminarFormModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SeminarFormModel seminarFormViewModel)
        {
            var seminar = await data.Seminars.FindAsync(seminarFormViewModel.Id);

            if (seminar == null)
            {
                return NotFound();
            }

            var organizer = await data.Users.FindAsync(GetUserId());

            if (organizer == null)
            {
                return NotFound();
            }

       
            seminar.Topic = seminarFormViewModel.Topic;
            seminar.Lecturer = seminarFormViewModel.Lecturer;
            seminar.Details = seminarFormViewModel.Details;
            seminar.DateAndTime = seminarFormViewModel.DateAndTime;
            seminar.Duration = seminarFormViewModel.Duration;
            seminar.CategoryId = seminarFormViewModel.CategoryId;

            
            seminar.Organizer = organizer;

            await data.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        //Join
        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            var seminars = await data.Seminars.ToListAsync();
            var userId = GetUserId();

            SeminarParticipant? seminarParticipant = await data.SeminarsParticipants
           .FirstOrDefaultAsync(s => s.SeminarId == id && s.ParticipantId == GetUserId());

            if (seminarParticipant != null)
            {
                return RedirectToAction(nameof(Joined));
            }


            if (!seminars.Any(s => s.Id == id))
            {
                return BadRequest();
            }

            await data.SeminarsParticipants.AddAsync(new SeminarParticipant
            {
                SeminarId = id,
                ParticipantId = userId
            });


            await data.SaveChangesAsync();

            return RedirectToAction(nameof(Joined));
        }

        //Leave
        public async Task<IActionResult> Leave(int id)
        {
            var userId = GetUserId();

            var participant = await data.SeminarsParticipants
                .FirstOrDefaultAsync(p => p.SeminarId == id && p.ParticipantId == userId);

            if (participant == null)
            {
                return NotFound();
            }

            data.SeminarsParticipants.Remove(participant);

            await data.SaveChangesAsync();

            return RedirectToAction(nameof(Joined));
        }

        //Details
        public async Task<IActionResult> Details(int id)
        {
            
            var seminar = await data.Seminars.FirstOrDefaultAsync(s => s.Id == id);
             
             

                  
                if (seminar == null)
                {
                    return NotFound();
                }

            var category = await data.Categorys.FirstOrDefaultAsync(c => c.Id == seminar.CategoryId)
                ?? throw new Exception("Category does not exist."); 
            
            var organizer = await data.Users
               .FirstOrDefaultAsync(u => u.Id == seminar.OrganizerId) 
                ?? throw new Exception("Organizer does not exist."); 

            var viewModel = new SeminarViewModel(
                seminar.Id,
                seminar.Topic,
                seminar.Lecturer,
                seminar.Details,
                organizer.Id,
                organizer,
                seminar.DateAndTime.ToString(),
                seminar.Duration,
                category.Id,
                category
                );
                

                return View(viewModel);
            
        }


        //Delete
        public async Task<IActionResult> Delete(int id)
        {
            var seminar = await data.Seminars.FindAsync(id);


            if (seminar == null)
            {
                return NotFound();
            }


            return View(seminar);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seminar = await data.Seminars.FindAsync(id);


            if (seminar == null)
            {
                return NotFound();
            }

            if (seminar == null)
            {
                return BadRequest();
            }

            var organizer = await data.Users
              .FirstOrDefaultAsync(u => u.Id == seminar.OrganizerId)
              ?? throw new Exception("Organizer does not exist.");

            var participantsToRemove = await data.SeminarsParticipants
              .Where(s => s.SeminarId == seminar.Id)
              .ToListAsync();

            foreach (var participant in participantsToRemove)
            {
                data.SeminarsParticipants.Remove(participant);
            }

            data.Seminars.Remove(seminar);

            await data.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }
    }
}
