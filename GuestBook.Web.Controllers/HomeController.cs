using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using GuestBook.Domain.Models;
using GuestBook.Domain.Services;
using GuestBook.Domain.Services.Messages;
using GuestBook.Web.Controllers.PostResponses.Home;
using GuestBook.Web.Controllers.PresentationModels;
using Xtremecode.Infrastructure.WebMvc;

namespace GuestBook.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGuestBookService _guestBookService;
        private const int INITIAL_NUMBER_OF_DISPLAYED_ITEMS = 6;

        public HomeController(IGuestBookService guestBookService)
        {
            _guestBookService = guestBookService;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";
            return View();
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public ActionResult Index()
        {
            ViewBag.SubmitResponse = TempData["SubmitResponse"];
            //ViewBag.UploadedImage = TempData["UploadedImage"];
            ViewBag.GuestBookEntries = retrieveGuestBookEntries();
            return View("Index");
        }

        [HttpPost]
        [SetTempDataModelState, HandleRecaptchaChallenge]
        public ActionResult Submit(GuestBookEntryModel submittedGuestBookEntry, HttpPostedFileBase imageFileUpload)
        {
            if (!ModelState.IsValid)
            {
                //retainUploadedImage(imageFileUpload);
                return RedirectToAction("Index");
            }

            SubmitGuestBookEntryResponse response =
                _guestBookService.SubmitGuestBookEntry(new SubmitGuestBookEntryRequest { Name = submittedGuestBookEntry.Name, Comment = submittedGuestBookEntry.Comment, ImageFile = imageFileUpload });

            if (response.IsSuccess)
            {
                ModelState.Clear();
                TempData["SubmitResponse"] = new SubmitResponse
                    {
                        Message = "Data submission is success.",
                        IsSuccess = true
                    };
            }
            else
            {
                ModelState.AddModelError(string.Empty, response.Errors.ToString());
                TempData["SubmitResponse"] = new SubmitResponse { Message = "Data submission is Failed." };
            }

            return RedirectToAction("Index");
        }

        //private void retainUploadedImage(HttpPostedFileBase imageFileUpload)
        //{
        //    if ((imageFileUpload != null) && (imageFileUpload.InputStream != null))
        //    {
        //        using (var copiedStream = new MemoryStream())
        //        {h
        //            imageFileUpload.InputStream.Seek(0, SeekOrigin.Begin);
        //            imageFileUpload.InputStream.CopyTo(copiedStream);
        //            TempData["UploadedImage"] = copiedStream.ToArray();
        //        }
        //    }
        //}

        private IEnumerable<GuestBookItemModel> retrieveGuestBookEntries()
        {
            var getCurrentGuestBookEntriesResponse = _guestBookService.GetCurrentGuestBookEntries(new GetCurrentGuestBookEntriesRequest{Top = INITIAL_NUMBER_OF_DISPLAYED_ITEMS});
            var result = getCurrentGuestBookEntriesResponse.IsSuccess ? getCurrentGuestBookEntriesResponse.Result.ToMvcModels<GuestBookEntry, GuestBookItemModel>() : new GuestBookItemModel[0];
            return result.ToArray();
        }

        //#endregion
    }
}
