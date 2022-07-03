using CURDOperationWithImageUploadCore5_Demo.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CURDOperationWithImageUploadCore5_Demo.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.EnsureCreated();

                if (!context.Speakers.Any())
                {
                    context.Speakers.AddRange(new List<Speaker>()
                    {
                        new Speaker()
                        {
                          SpeakerName="Jack Christiansen",
                          Experience=5,
                          Qualification="MSc Computer Science",
                          SpeakingDate=DateTime.Now.AddDays(2) ,
                          SpeakingTime=DateTime.Now.AddDays(2).AddHours(18).AddMinutes(00),
                          ProfilePicture="/avatar.png",
                          Venue="Bangalore"
                        },
                        new Speaker()
                        {
                          SpeakerName="Brenden Legros",
                          Experience=7,
                          Qualification="MBA",
                          SpeakingDate=DateTime.Now.AddDays(2) ,
                          SpeakingTime=DateTime.Now.AddDays(2).AddHours(20).AddMinutes(00),
                          ProfilePicture="/avatar.png",
                          Venue="Hyderabad"
                        },
                        new Speaker()
                        {
                          SpeakerName="Julia Adward",
                          Experience=5,
                          Qualification="Digital Marketing",
                          SpeakingDate=DateTime.Now.AddDays(2) ,
                          SpeakingTime=DateTime.Now.AddDays(2).AddHours(20).AddMinutes(00),
                          ProfilePicture="/avatar.png",
                          Venue="Chennai"
                        }
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
