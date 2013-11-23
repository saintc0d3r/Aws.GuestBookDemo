using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using GuestBook.Domain.Services;
using GuestBook.Repository;
using GuestBook.Web.Controllers;
using GuestBook.Web.Controllers.PostResponses.Home;
using GuestBook.Web.Controllers.PresentationModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Xtremecode.Infrastructure.Di;

namespace GuestBook.IntegrationTest.WebControllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private readonly IGuestBookService _guestBookService = DependencyInjector.Get<IGuestBookService>();
        private static MemoryStream _imageStream;
        private const string TEST_GUEST_NAME = "Eva Huang";
        private const string TEST_GUEST_COMMENT = "Hello, my name is Eva";
        private static GuestBookEntryModel _guestBookEntryModel;

        [TestMethod]
        public void Should_Be_Able_To_Submit_Guest_Book_Entry_And_List_Current_Entries()
        {
            // Arrange
            var homeController = new HomeController(_guestBookService);
            var imageUpload = prepareUploadedImage();

            // act & assert
            testSubmitGuestBookEntry(homeController, imageUpload);

            // Act & assert - Show index
            testCallingIndexAction(homeController);
        }

        [TestMethod]
        public void Should_Be_Able_To_Submit_Guest_Book_Entry_With_No_Picture_And_List_Current_Entries()
        {
            // Arrange
            var homeController = new HomeController(_guestBookService);

            // act & assert
            testSubmitGuestBookEntry(homeController, null);

            // Act & assert - Show index
            testCallingIndexAction(homeController);
        }

        [ClassCleanup]
        public static void Teardown()
        {
            if (_imageStream != null)
            {
                _imageStream.Dispose();
                _imageStream = null;
            }

            var guestBookEntryRepository = DependencyInjector.Get<IGuestBookEntryRepository>();
            var testRecordToDelete = guestBookEntryRepository.FindByName(_guestBookEntryModel.Name);
            if (testRecordToDelete != null)
            {
                guestBookEntryRepository.Remove(testRecordToDelete);
                var guestBookImageRepository = DependencyInjector.Get<IGuestBookImageRepository>();
                guestBookImageRepository.Remove(testRecordToDelete.PhotoUrl);
            }
        }

        private void testCallingIndexAction(HomeController homeController)
        {
            // Act - Show index
            var result = homeController.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ViewBag.GuestBookEntries);
            var guestBookEntries = result.ViewBag.GuestBookEntries as IEnumerable<GuestBookItemModel>;
            Assert.IsNotNull(guestBookEntries);
            Assert.IsTrue(guestBookEntries.Any(guestBookEntry => guestBookEntry.Name.Equals(TEST_GUEST_NAME) && guestBookEntry.Comment.Equals(TEST_GUEST_COMMENT)));
        }

        private void testSubmitGuestBookEntry(HomeController homeController, HttpPostedFileBase imageUpload)
        {
            // Arrange            
            _guestBookEntryModel = new GuestBookEntryModel { Name = TEST_GUEST_NAME, Comment = TEST_GUEST_COMMENT };

            // Act            
            var submitActionResult = homeController.Submit(_guestBookEntryModel, imageUpload);
            var redirectToRouteResult = submitActionResult as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(redirectToRouteResult);
            Assert.IsNotNull(redirectToRouteResult.RouteValues["action"]);
            Assert.AreEqual("Index", redirectToRouteResult.RouteValues["action"]);
            var submitResponse = homeController.TempData["SubmitResponse"] as SubmitResponse;
            Assert.IsNotNull(submitResponse);
            Assert.AreEqual(true, submitResponse.IsSuccess);
            Assert.AreEqual("Data submission is success.", submitResponse.Message);

        }

        private HttpPostedFileBase prepareUploadedImage()
        {
            var mockRepository = new MockRepository();
            var uploadedImage = mockRepository.Stub<HttpPostedFileBase>();
            using (mockRepository.Record())
            {
                SetupResult
                    .For(uploadedImage.ContentType)
                    .Return("img/jpeg");

                SetupResult.For(uploadedImage.FileName)
                           .Return("eva_huang.jpg");

                _imageStream = new MemoryStream();
                Resource.Eva_Huang.Save(_imageStream, ImageFormat.Jpeg);

                SetupResult.For(uploadedImage.InputStream)
                           .Return(_imageStream);
            }

            return uploadedImage;
        }
    }
}
