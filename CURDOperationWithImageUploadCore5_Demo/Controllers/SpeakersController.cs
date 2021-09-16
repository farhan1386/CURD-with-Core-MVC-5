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
        
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SpeakersController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _webHostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Speakers.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speaker = await _context.Speakers
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

                _context.Add(speaker);
                await _context.SaveChangesAsync();
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

            var speaker = await _context.Speakers.FindAsync(id);
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
                var speaker = await _context.Speakers.FindAsync(model.Id);
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
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, FileLocation.FileUploadFolder, model.ExistingImage);
                        System.IO.File.Delete(filePath);
                    }

                    speaker.ProfilePicture = ProcessUploadedFile(model);
                }
                _context.Update(speaker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speaker = await _context.Speakers
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
            var speaker = await _context.Speakers.FindAsync(id);
            var CurrentImage = Path.Combine(Directory.GetCurrentDirectory(),FileLocation.DeleteFileFromFolder,speaker.ProfilePicture);
            _context.Speakers.Remove(speaker);
            if (System.IO.File.Exists(CurrentImage))
            {
                System.IO.File.Delete(CurrentImage);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpeakerExists(int id)
        {
            return _context.Speakers.Any(e => e.Id == id);
        }

        private string ProcessUploadedFile(SpeakerViewModel model)
        {
            string uniqueFileName = null;

            if (model.SpeakerPicture != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath,FileLocation.FileUploadFolder);
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
