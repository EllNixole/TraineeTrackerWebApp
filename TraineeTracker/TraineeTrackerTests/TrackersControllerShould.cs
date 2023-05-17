using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NuGet.Protocol;
using NUnit.Framework;
using TraineeTracker.App.Controllers;
using TraineeTracker.App.Models;
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
            mockService.Setup(s => s.GetTrackersAsync(fakeSpartanServiceResponse.Data, It.IsAny<string>(), null).Result).Returns(Helper.GetTrackerListServiceResponse());

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
            mockService.Setup(s => s.GetTrackersAsync(fakeSpartanServiceResponse.Data, It.IsAny<string>(), null)).ReturnsAsync(Helper.GetFailedServiceResponse<IEnumerable<TrackerVM>>("Problem!"));

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

        [Test]
        [Category("Deletion")]

		public void Delete_WithSuccessfulServiceResponse_ReturnsRedirectedAction()
		{
			var mockService = new Mock<ITrackerService>();
			var fakeSpartanServiceResponse = Helper.GetSpartanServiceResponse();
			mockService.Setup(s => s.GetUserAsync(It.IsAny<HttpContext>()).Result).Returns(fakeSpartanServiceResponse);
			mockService.Setup(s => s.DeleteTrackerAsync(fakeSpartanServiceResponse.Data, It.IsAny<int>()).Result).Returns(Helper.GetTrackerServiceResponse());

			_sut = new TrackersController(mockService.Object);

			var result = _sut.Delete(It.IsAny<int>()).Result;

			Assert.That(result, Is.InstanceOf<RedirectToActionResult>());

			var raResult = result as RedirectToActionResult;

			Assert.That(raResult!.ActionName, Is.EqualTo("Index"));
		}

		[Test]
		[Category("Deletion")]

		public void Delete_WithUnSuccessfulServiceResponse_ReturnsProblem()
		{
			var mockService = new Mock<ITrackerService>();
			var fakeSpartanServiceResponse = Helper.GetFailedServiceResponse<Spartan>();
			mockService.Setup(s => s.GetUserAsync(It.IsAny<HttpContext>()).Result).Returns(fakeSpartanServiceResponse);
			mockService.Setup(s => s.DeleteTrackerAsync(fakeSpartanServiceResponse.Data, It.IsAny<int>()).Result).Returns(Helper.GetFailedServiceResponse<TrackerVM>("Problem"));

			_sut = new TrackersController(mockService.Object);

			var result = _sut.Delete(It.IsAny<int>()).Result;

			Assert.That(result, Is.InstanceOf<ObjectResult>());

			var objResult = result as ObjectResult;

			Assert.That(objResult!.ToJson(), Does.Contain("Problem"));
		}
        
        [Test]
		[Category("EditGet")]

		public void EditGet_WithSuccessfulServiceResponse_ReturnsViewResult()
		{
			var mockService = new Mock<ITrackerService>();
            var fakeSpartanServiceResponse = Helper.GetSpartanServiceResponse();
			mockService.Setup(s => s.GetUserAsync(It.IsAny<HttpContext>()).Result).Returns(fakeSpartanServiceResponse);
			mockService.Setup(s => s.GetDetailsAsync(fakeSpartanServiceResponse.Data, It.IsAny<int>(), It.IsAny<string>()).Result).Returns(Helper.GetTrackerServiceResponse());

			_sut = new TrackersController(mockService.Object);

			var result = _sut.Edit(It.IsAny<int>()).Result;

			Assert.That(result, Is.InstanceOf<ViewResult>());

			var viewResult = result as ViewResult;

			Assert.That(viewResult!.Model, Is.InstanceOf<TrackerVM>());
		}
        
        [Test]
		[Category("EditGet")]

		public void EditGet_WithUnSuccessfulServiceResponse_ReturnsProblem()
		{
			var mockService = new Mock<ITrackerService>();
            var fakeSpartanServiceResponse = Helper.GetFailedServiceResponse<Spartan>();
			mockService.Setup(s => s.GetUserAsync(It.IsAny<HttpContext>()).Result).Returns(fakeSpartanServiceResponse);
			mockService.Setup(s => s.GetDetailsAsync(fakeSpartanServiceResponse.Data, It.IsAny<int>(), It.IsAny<string>()).Result).Returns(Helper.GetFailedServiceResponse<TrackerVM>("Problem"));

			_sut = new TrackersController(mockService.Object);

			var result = _sut.Edit(It.IsAny<int>()).Result;

			Assert.That(result, Is.InstanceOf<ObjectResult>());

			var objResult = result as ObjectResult;

			Assert.That(objResult!.ToJson(), Does.Contain("Problem"));
		}

		[Test]
		[Category("EditPost")]

		public void EditPost_WithSuccessfulServiceResponse_ReturnsViewResult()
		{
			var mockService = new Mock<ITrackerService>();
			var fakeSpartanServiceResponse = Helper.GetSpartanServiceResponse();
			mockService.Setup(s => s.GetUserAsync(It.IsAny<HttpContext>()).Result).Returns(fakeSpartanServiceResponse);
			mockService.Setup(s => s.EditTrackerAsync(fakeSpartanServiceResponse.Data, It.IsAny<int>(), It.IsAny<TrackerVM>()).Result).Returns(Helper.GetTrackerServiceResponse());

			_sut = new TrackersController(mockService.Object);

			var result = _sut.Edit(It.IsAny<int>(), It.IsAny<TrackerVM>()).Result;

			Assert.That(result, Is.InstanceOf<RedirectToActionResult>());

			var raResult = result as RedirectToActionResult;

            Assert.That(raResult!.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        [Category("Creation")]
        public void Create_WithUnSuccessfulServiceResponse_ReturnsProblems()
        {
            var mockService = new Mock<ITrackerService>();
            var fakeSpartanServiceResponse = Helper.GetSpartanServiceResponse();
            mockService.Setup(s => s.GetUserAsync(It.IsAny<HttpContext>()).Result).Returns(fakeSpartanServiceResponse);
            mockService.Setup(s => s.CreateTrackerAsync(fakeSpartanServiceResponse.Data, It.IsAny<CreateTrackerVM>()).Result).Returns(Helper.GetFailedServiceResponse<TrackerVM>("Problem!"));

            _sut = new TrackersController(mockService.Object);

            var result = _sut.Create(It.IsAny<CreateTrackerVM>()).Result;

            Assert.That(result, Is.InstanceOf<ViewResult>());

            var viewResult = result as ViewResult;

            Assert.That(viewResult.Model, Is.Null);
        }
        //Add in failed spartan clause;
        [Test]
        [Category("Details")]
        public void Details_WithSuccessfulServiceResponse_ReturnsView()
        {
            var mockService = new Mock<ITrackerService>();
            var fakeSpartanServiceResponse = Helper.GetSpartanServiceResponse();
            mockService.Setup(s => s.GetUserAsync(It.IsAny<HttpContext>()).Result).Returns(fakeSpartanServiceResponse);
            mockService.Setup(s => s.GetDetailsAsync(fakeSpartanServiceResponse.Data, It.IsAny<int>(), It.IsAny<string>()).Result).Returns(Helper.GetTrackerServiceResponse());

            _sut = new TrackersController(mockService.Object);

            var result = _sut.Details(It.IsAny<int>()).Result;

            Assert.That(result, Is.InstanceOf<ViewResult>());

            var viewResult = result as ViewResult;

            Assert.That(viewResult.Model, Is.InstanceOf<TrackerVM>());
        }

        [Test]
        [Category("Details")]
        public void Details_WithUnSuccessfulServiceResponse_ReturnsProblem()
        {
            var mockService = new Mock<ITrackerService>();
            var fakeSpartanServiceResponse = Helper.GetSpartanServiceResponse();
            mockService.Setup(s => s.GetUserAsync(It.IsAny<HttpContext>()).Result).Returns(fakeSpartanServiceResponse);
            mockService.Setup(s => s.GetDetailsAsync(fakeSpartanServiceResponse.Data, It.IsAny<int>(), It.IsAny<string>()).Result).Returns(Helper.GetFailedServiceResponse<TrackerVM>("Problem!"));

            _sut = new TrackersController(mockService.Object);

            var result = _sut.Details(It.IsAny<int>()).Result;

            Assert.That(result, Is.InstanceOf<ObjectResult>());

            var objResult = result as ObjectResult;

            Assert.That(objResult.ToJson(),Does.Contain("Problem!"));
            Assert.That(objResult.StatusCode, Is.EqualTo(500));
        }
        //Add in failed spartan response clause;
        [Test]
        [Category("Update")]
        public void Update_WithSuccessfulServiceResponse_ReturnsRedirectedAction()
        {
            var mockService = new Mock<ITrackerService>();
            var fakeSpartanServiceResponse = Helper.GetSpartanServiceResponse();
            mockService.Setup(s => s.GetUserAsync(It.IsAny<HttpContext>()).Result).Returns(fakeSpartanServiceResponse);
            mockService.Setup(s => s.UpdateTrackerReviewedAsync(fakeSpartanServiceResponse.Data,It.IsAny<int>(),It.IsAny<MarkReviewedVM>()).Result).Returns(Helper.GetTrackerServiceResponse());

            _sut = new TrackersController(mockService.Object);

            var result = _sut.UpdateTrackerReviewed(It.IsAny<int>(), It.IsAny<MarkReviewedVM>()).Result;

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());

            var raResult = result as RedirectToActionResult;

            Assert.That(raResult!.ActionName,Is.EqualTo("Index"));
        }
        [Test]
        [Category("Update")]
        public void Update_WithUnSuccessfulServiceResponse_ReturnsProblem()
        {
            var mockService = new Mock<ITrackerService>();
            var fakeSpartanServiceResponse = Helper.GetSpartanServiceResponse();
            mockService.Setup(s => s.GetUserAsync(It.IsAny<HttpContext>()).Result).Returns(fakeSpartanServiceResponse);
            mockService.Setup(s => s.UpdateTrackerReviewedAsync(fakeSpartanServiceResponse.Data, It.IsAny<int>(), It.IsAny<MarkReviewedVM>()).Result).Returns(Helper.GetFailedServiceResponse<TrackerVM>("Problem!"));

            _sut = new TrackersController(mockService.Object);

            var result = _sut.UpdateTrackerReviewed(It.IsAny<int>(),It.IsAny<MarkReviewedVM>()).Result;

            Assert.That(result, Is.InstanceOf<ObjectResult>());

            var objResult = result as ObjectResult;

            Assert.That(objResult.ToJson(), Does.Contain("Problem!"));
            Assert.That(objResult.StatusCode, Is.EqualTo(500));
        }

		[Test]
		[Category("EditPost")]
		public void EditPost_WithUnSuccessfulServiceResponse_ReturnsNotFound()
		{
			var mockService = new Mock<ITrackerService>();
			var fakeSpartanServiceResponse = Helper.GetFailedServiceResponse<Spartan>();
			mockService.Setup(s => s.GetUserAsync(It.IsAny<HttpContext>()).Result).Returns(fakeSpartanServiceResponse);
			mockService.Setup(s => s.EditTrackerAsync(fakeSpartanServiceResponse.Data, It.IsAny<int>(), It.IsAny<TrackerVM>()).Result).Returns(Helper.GetFailedServiceResponse<TrackerVM>("Problem"));

			_sut = new TrackersController(mockService.Object);

			var result = _sut.Edit(It.IsAny<int>(), It.IsAny<TrackerVM>()).Result;

			Assert.That(result, Is.InstanceOf<NotFoundResult>());

			var nfResult = result as NotFoundResult;

			Assert.That(nfResult!.StatusCode, Is.EqualTo(404));
		}
	}
}