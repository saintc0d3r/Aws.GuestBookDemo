using System.Web.Mvc;

using GuestBook.Domain.Models;
using GuestBook.Domain.Services;
using GuestBook.Domain.Services.Messages;
using GuestBook.Web.Controllers;
using GuestBook.Web.Controllers.PostResponses.Home;
using GuestBook.Web.Controllers.PresentationModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;

namespace GuestBook.Spec.WebControllers
{
    [TestClass]
    public class HomeControllerSpec
    {
        private readonly MockRepository _mockRepository = new MockRepository();
        private const string TEST_GUEST_NAME = "Eva Huang";
        private const string TEST_GUEST_COMMENT = "Hello, my name is Eva";

        [TestMethod]
        public void Can_Submit_Guest_Book_Entry()
        {
            // Arrange
            SubmitGuestBookEntryRequest submitGuestBookEntryRequest = new SubmitGuestBookEntryRequest
                {
                    Name = TEST_GUEST_NAME,
                    Comment = TEST_GUEST_COMMENT,
                    ImageFile = null
                };
            SubmitGuestBookEntryResponse expectedSubmitGuestBookEntryResponse = new SubmitGuestBookEntryResponse { IsSuccess = true };

            IGuestBookService guestBookService = _mockRepository.Stub<IGuestBookService>();
            using (_mockRepository.Record())
            {
                SetupResult
                    .For(guestBookService.SubmitGuestBookEntry(submitGuestBookEntryRequest))
                    .IgnoreArguments()
                    .Return(expectedSubmitGuestBookEntryResponse);
            }

            HomeController homeController = new HomeController(guestBookService);
            GuestBookEntryModel guestBookEntryModel = new GuestBookEntryModel { Name = TEST_GUEST_NAME, Comment = TEST_GUEST_COMMENT };

            // Act
            var actionResult = homeController.Submit(guestBookEntryModel, null);
            RedirectToRouteResult redirectToRouteResult = actionResult as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(redirectToRouteResult);
            Assert.IsNotNull(redirectToRouteResult.RouteValues["action"]);
            Assert.AreEqual("Index", redirectToRouteResult.RouteValues["action"]);
            SubmitResponse submitResponse = homeController.TempData["SubmitResponse"] as SubmitResponse;
            Assert.IsNotNull(submitResponse);
            Assert.AreEqual(true, submitResponse.IsSuccess);
            Assert.AreEqual("Data submission is success.", submitResponse.Message);
        }

        [TestMethod]
        public void Can_Display_List_Of_Guest_Book_Items()
        {
            // Arrange
            IGuestBookService guestBookService = _mockRepository.Stub<IGuestBookService>();
            var getCurrentGuestBookEntriesRequest = new GetCurrentGuestBookEntriesRequest { Top = 10 };
            var expectedGetCurrentGuestBookEntriesResponse = 
                new GetCurrentGuestBookEntriesResponse {
                    Result = new[] {
                    new GuestBookEntry {GuestName = TEST_GUEST_NAME, Comment = TEST_GUEST_COMMENT, PhotoUrl = null}                            
                },
                    IsSuccess = true
                };

            using (_mockRepository.Record())
            {
                SetupResult
                    .For(guestBookService.GetCurrentGuestBookEntries(getCurrentGuestBookEntriesRequest))
                    .IgnoreArguments()
                    .Return(expectedGetCurrentGuestBookEntriesResponse);
            }

            HomeController homeController = new HomeController(guestBookService);

            // Act
            var actionResult = homeController.Index();

            // Assert
            actionResult.AssertViewRendered().ForView("Index");
        }
    }
}
