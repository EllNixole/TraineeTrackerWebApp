using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NuGet.Protocol;
using NUnit.Framework;
using TraineeTracker.App.Controllers;
using TraineeTracker.App.Models.ViewModels;
using TraineeTracker.App.Services;

namespace TraineeTrackerTests
{
    public class Tests
    {
        private TrackersController? _sut;

        [Test]
        [Category("Instantiation")]
        public void BeAbleToBeInstantiated()
        {
            var mockService = new Mock<ITrackerService>();
            _sut = new TrackersController(mockService.Object);
            Assert.That(_sut, Is.InstanceOf<TrackersController>());
        }

        [Test]
        [Category("Index")]
        public void Index_WithSuccessfulServiceResponse_ReturnsTracker()
        {
            var mockService = new Mock<ITrackerService>();
            var fakeSpartanServiceResponse = Helper.GetSpartanServiceResponse();
            mockService.Setup(s => s.GetUserAsync(It.IsAny<HttpContext>()).Result).Returns(fakeSpartanServiceResponse);
            mockService.Setup(s => s.GetTrackersAsync(fakeSpartanServiceResponse.Data, null)).ReturnsAsync(Helper.GetTrackerListServiceResponse());

            _sut = new TrackersController(mockService.Object);

            var result = _sut.Index().Result;

            Assert.That(result, Is.InstanceOf<ViewResult>());

            var viewResult = result as ViewResult;

            Assert.That(viewResult.Model, Is.InstanceOf<IEnumerable<TrackerVM>>());
        }
        [Test]
        [Category("Index")]
        public void Index_WithUnSuccessfulServiceResponse_ReturnsProblem()
        {
            var mockService = new Mock<ITrackerService>();
            var fakeSpartanServiceResponse = Helper.GetSpartanServiceResponse();
            mockService.Setup(s => s.GetUserAsync(It.IsAny<HttpContext>()).Result).Returns(fakeSpartanServiceResponse);
            mockService.Setup(s => s.GetTrackersAsync(fakeSpartanServiceResponse.Data, null)).ReturnsAsync(Helper.GetFailedServiceResponse<IEnumerable<TrackerVM>>("Problem!"));

            _sut = new TrackersController(mockService.Object);

            var result = _sut.Index().Result;

            Assert.That(result, Is.InstanceOf<ObjectResult>());

            var objResult = result as ObjectResult;

            Assert.That(objResult.ToJson(), Does.Contain("Problem!"));
            Assert.That((int)objResult.StatusCode, Is.EqualTo(500));
        }

        //Test with null spartan too. 

        [Test]
        [Category("Creation")]
        public void Create_WithSuccessfulServiceResponse_ReturnsRedirectedAction()
        {
            var mockService = new Mock<ITrackerService>();
            var fakeSpartanServiceResponse = Helper.GetSpartanServiceResponse();
            mockService.Setup(s => s.GetUserAsync(It.IsAny<HttpContext>()).Result).Returns(fakeSpartanServiceResponse);
            mockService.Setup(s => s.CreateTrackerAsync(fakeSpartanServiceResponse.Data,It.IsAny<CreateTrackerVM>()).Result).Returns(Helper.GetTrackerServiceResponse());

            _sut = new TrackersController(mockService.Object);

            var result = _sut.Create(It.IsAny<CreateTrackerVM>()).Result;

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());

            var raResult = result as RedirectToActionResult;

            Assert.That(raResult!.ActionName, Is.EqualTo("Index"));
        }
    }
}