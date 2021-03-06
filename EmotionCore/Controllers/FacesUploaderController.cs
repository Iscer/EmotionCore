﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmotionCore.Models;
using EmotionCore.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Extensions.Options;

namespace EmotionCore.Controllers
{
    public class FacesUploaderController : Controller
    {
        private readonly EmotionCoreContext _context;
        private FaceHelper faceHelper;
        private IList<FaceAttributeType> faceAttributes;
        private string UPLOAD_DIR;
        private readonly IHostingEnvironment _environment;

        public FacesUploaderController(EmotionCoreContext context, IOptions<AppSettings> settings, IHostingEnvironment environment)
        {
       
            _context = context;
            faceHelper = new FaceHelper(settings.Value.ApiKey, settings.Value.BaseUri);
            faceAttributes = new FaceAttributeType[]
            {
                FaceAttributeType.Emotion
            };
            UPLOAD_DIR = settings.Value.UploadDir;
            this._environment = environment;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            if (file?.Length > 0)
            {
                var pictureName = Guid.NewGuid().ToString();
                pictureName += Path.GetExtension(file.FileName);

                var route = Path.Combine(UPLOAD_DIR, pictureName);
                route = Path.Combine(_environment.WebRootPath, route);

                using (var stream = new FileStream(route, FileMode.Create))
                    await file.CopyToAsync(stream);

       

                using (Stream imageStream = file.OpenReadStream())
                {
                    var emoPicture = await faceHelper.DetectedFacesAndExtracFacesAsync(imageStream, faceAttributes);

                    emoPicture.Name = file.FileName;
                    emoPicture.Path = $"{UPLOAD_DIR}/{pictureName}";

                    _context.EmoPictures.Add(emoPicture);
                    await _context.SaveChangesAsync();


                    return RedirectToAction("Details", "EmoPictures", new { Id = emoPicture.Id});
                }

            }

            return View();
        }



    }
}