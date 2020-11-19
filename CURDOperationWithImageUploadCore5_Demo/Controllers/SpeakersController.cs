using CURDOperationWithImageUploadCore5_Demo.Data;
using CURDOperationWithImageUploadCore5_Demo.Models;
using CURDOperationWithImageUploadCore5_Demo.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CURDOperationWithImageUploadCore5_Demo.Controllers
{
    public class SpeakersController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        public SpeakersController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            db = context;
            webHostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await db.Speakers.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speaker = await db.Speakers
                .FirstOrDefaultAsync(m => m.Id == id);

            var speakerViewModel = new SpeakerViewModel()
            {
                Id = speaker.Id,
                SpeakerName = speaker.SpeakerName,
                Qualification = speaker.Qualification,
                Experience = speaker.Experience,
                SpeakingDate = speaker.SpeakingDate,
                SpeakingTime = speaker.SpeakingTime,
                Venue = speaker.Venue,
                ExistingImage = speaker.ProfilePicture
            };

            if (speaker == null)
            {
                return NotFound();
            }

            return View(speaker);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpeakerViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                Speaker speaker = new Speaker
                {
                    SpeakerName = model.SpeakerName,
                    Qualification = model.Qualification,
                    Experience = model.Experience,
                    SpeakingDate = model.SpeakingDate,
                    SpeakingTime = model.SpeakingTime,
                    Venue = model.Venue,
                    ProfilePicture = uniqueFileName
                };

                db.Add(speaker);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speaker = await db.Speakers.FindAsync(id);
            var speakerViewModel = new SpeakerViewModel()
            {
                Id = speaker.Id,
                SpeakerName = speaker.SpeakerName,
                Qualification = speaker.Qualification,
                Experience = speaker.Experience,
                SpeakingDate = speaker.SpeakingDate,
                SpeakingTime = speaker.SpeakingTime,
                Venue = speaker.Venue,
                ExistingImage = speaker.ProfilePicture
            };

            if (speaker == null)
            {
                return NotFound();
            }
            return View(speakerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SpeakerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var speaker = await db.Speakers.FindAsync(model.Id);
                speaker.SpeakerName = model.SpeakerName;
                speaker.Qualification = model.Qualification;
                speaker.Experience = model.Experience;
                speaker.SpeakingDate = model.SpeakingDate;
                speaker.SpeakingTime = model.SpeakingTime;
                speaker.Venue = model.Venue;

                if (model.SpeakerPicture != null)
                {
                    if (model.ExistingImage != null)
                    {
                        string filePath = Path.Combine(webHostEnvironment.WebRootPath, "Uploads", model.ExistingImage);
                        System.IO.File.Delete(filePath);
                    }

                    speaker.ProfilePicture = ProcessUploadedFile(model);
                }
                db.Update(speaker);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();

            //if (id != model.Id)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(speaker);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!SpeakerExists(speaker.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(speaker);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speaker = await db.Speakers
                .FirstOrDefaultAsync(m => m.Id == id);

            var speakerViewModel = new SpeakerViewModel()
            {
                Id = speaker.Id,
                SpeakerName = speaker.SpeakerName,
                Qualification = speaker.Qualification,
                Experience = speaker.Experience,
                SpeakingDate = speaker.SpeakingDate,
                SpeakingTime = speaker.SpeakingTime,
                Venue = speaker.Venue,
                ExistingImage = speaker.ProfilePicture
            };
            if (speaker == null)
            {
                return NotFound();
            }

            return View(speakerViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var speaker = await db.Speakers.FindAsync(id);
            var CurrentImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", speaker.ProfilePicture);
            db.Speakers.Remove(speaker);
            if (await db.SaveChangesAsync() > 0)
            {
                if (System.IO.File.Exists(CurrentImage))
                {
                    System.IO.File.Delete(CurrentImage);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SpeakerExists(int id)
        {
            return db.Speakers.Any(e => e.Id == id);
        }

        private string ProcessUploadedFile(SpeakerViewModel model)
        {
            string uniqueFileName = null;

            if (model.SpeakerPicture != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.SpeakerPicture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.SpeakerPicture.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}
