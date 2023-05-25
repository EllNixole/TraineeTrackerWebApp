﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NuGet.Protocol;
using TraineeTracker.App.Controllers;
using TraineeTracker.App.Data;
using TraineeTracker.App.Models;
using TraineeTracker.App.Models.ViewModels;
using TraineeTracker.App.Services;

namespace TraineeTrackerTests;

public class TrackerServiceShould
{
    private TrackerService? _sut;
    private Mock<IMapper> _mapper;
    private TraineeTrackerContext _context;
    private Mock<UserManager<Spartan>> _userManager; 


    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<TraineeTrackerContext>().UseInMemoryDatabase(databaseName: "fakeDatabase").Options;
        _context = new TraineeTrackerContext(options);
        _context.RemoveRange(_context.Spartans);
        _context.RemoveRange(_context.TrackerItems);
        _context.TrackerItems.AddRange(new List<Tracker>(){
            
        });
        _context.Spartans.AddRange(new List<Spartan>()
        {
            
        });
        _mapper = new Mock<IMapper>();
        _userManager = Helper.MockUserManager<Spartan>(new List<Spartan>());
        _sut = new TrackerService(_context, _mapper.Object, _userManager.Object);
    }

    [Test]
    [Category("Instantiation")]
    public void BeAbleToBeInstantiated()
    {
        Assert.That(_sut, Is.InstanceOf<TrackerService>());
    }

    [Test]
    [Category("CreateTrackers")]
    [Category("Happy Path")]
    public void CreateTrackerEntriesAsync_GivenValidData_ReturnsSuccessfulResponse()
    {
        var fakeTracker = Helper.CreateFakeTracker("Bob");
        _mapper.Setup(m => m.Map<Tracker>(It.IsAny<CreateTrackerVM>())).Returns(fakeTracker);
        var result = _sut.CreateTrackerEntriesAsync(fakeTracker.Spartan, It.IsAny<CreateTrackerVM>());
        Assert.That(result.Result, Is.InstanceOf<ServiceResponse<CreateTrackerVM>>());
        Assert.That(result.Result.Success, Is.EqualTo(true));
    }

    [Test]
    [Category("CreateTrackers")]
    [Category("Sad Path")]
    public void CreateTrackerEntriesAsync_GivenNoSpartan_ReturnsFailedResponse()
    {
        var fakeTracker = Helper.CreateFakeTracker("Bob");
        var result = _sut.CreateTrackerEntriesAsync(null, It.IsAny<CreateTrackerVM>());
        Assert.That(result.Result, Is.InstanceOf<ServiceResponse<CreateTrackerVM>>());
        Assert.That(result.Result.Success, Is.EqualTo(false));
        Assert.That(result.Result.Message, Does.Contain("No user found"));
    }

    [Test]
    [Category("CreateTrackers")]
    [Category("Sad Path")]
    public void CreateTrackerEntriesAsync_GivenFailedSaveChanges_ReturnsFailedResponse()
    {
        var fakeSpartan = Helper.CreateFakeSpartan("A");
        fakeSpartan.Id = null;
        var fakeTracker = Helper.CreateFakeTracker("Bob");
        _mapper.Setup(m => m.Map<Tracker>(It.IsAny<CreateTrackerVM>())).Returns(fakeTracker);
        var result = _sut.CreateTrackerEntriesAsync(fakeSpartan, It.IsAny<CreateTrackerVM>());
        Assert.That(result.Result, Is.InstanceOf<ServiceResponse<CreateTrackerVM>>());
        Assert.That(result.Result.Success, Is.EqualTo(false));
        Assert.That(result.Result.Message, Does.Contain("Database could not be updated"));
    }

    [Test]
    [Category("DeleteTrackers")]
    [Category("Happy Path")]
    public void DeleteTrackerEntriesAsync_GivenCorrectData_ReturnsSuccessfulResponse()
    {
        var fakeSpartan = Helper.CreateFakeSpartan("Bob");
        var fakeTracker = Helper.CreateFakeTracker("Bob");
        _mapper.Setup(m => m.Map<TrackerVM>(It.IsAny<Tracker>())).Returns(new TrackerVM());
        _mapper.Setup(m => m.Map<SpartanDTO>(It.IsAny<Spartan>())).Returns(new SpartanDTO());
        var result = _sut.DeleteTrackerEntriesAsync(fakeSpartan,fakeTracker.Id);
        Assert.That(result.Result, Is.InstanceOf<ServiceResponse<TrackerVM>>());
        Assert.That(result.Result.Success, Is.EqualTo(true));
    }

    [Test]
    [Category("DeleteTrackers")]
    [Category("Happy Path")]
    public void DeleteTrackerEntriesAsync_GivenRoleAdmin_ReturnsSuccessfulResponse()
    {
        var fakeSpartan = Helper.CreateFakeSpartan("Paul");
        fakeSpartan.Role = "Admin";
        var fakeTracker = Helper.CreateFakeTracker("Bob");
        _context.Add(fakeTracker);
        _mapper.Setup(m => m.Map<TrackerVM>(It.IsAny<Tracker>())).Returns(new TrackerVM());
        _mapper.Setup(m => m.Map<SpartanDTO>(It.IsAny<Spartan>())).Returns(new SpartanDTO());
        var result = _sut.DeleteTrackerEntriesAsync(fakeSpartan, fakeTracker.Id);
        Assert.That(result.Result, Is.InstanceOf<ServiceResponse<TrackerVM>>());
        Assert.That(result.Result.Success, Is.EqualTo(true));
    }

    [Test]
    [Category("DeleteTrackers")]
    [Category("Sad Path")]
    public void DeleteTrackerEntriesAsync_GivenNoSpartan_ReturnsFailedResponse()
    {
        var fakeTracker = Helper.CreateFakeTracker("Bob");
        var result = _sut.DeleteTrackerEntriesAsync(null, fakeTracker.Id);
        Assert.That(result.Result, Is.InstanceOf<ServiceResponse<TrackerVM>>());
        Assert.That(result.Result.Success, Is.EqualTo(false));
        Assert.That(result.Result.Message, Does.Contain("No user found"));
    }

    [Test]
    [Category("DeleteTrackers")]
    [Category("Sad Path")]
    public void DeleteTrackerEntriesAsync_GivenNoTrackers_ReturnsFailedResponse()
    {
        var fakeSpartan = Helper.CreateFakeSpartan("Bob");
        var fakeTracker = Helper.CreateFakeTracker("Bob");
        var result = _sut.DeleteTrackerEntriesAsync(fakeSpartan, (fakeTracker.Id)+1);
        Assert.That(result.Result, Is.InstanceOf<ServiceResponse<TrackerVM>>());
        Assert.That(result.Result.Success, Is.EqualTo(false));
        Assert.That(result.Result.Message, Does.Contain("There are no tracker entries to do!"));
    }

    [Test]
    [Category("DeleteTrackers")]
    [Category("Sad Path")]
    public void DeleteTrackerEntriesAsync_GivenFailedSaveChanges_ReturnsFailedResponse()
    {
        var fakeSpartan = Helper.CreateFakeSpartan("Bob");
        var fakeTracker = Helper.CreateFakeTracker("Bob");
        fakeTracker.Id = 0;
        _mapper.Setup(m => m.Map<TrackerVM>(It.IsAny<Tracker>())).Returns(new TrackerVM());
        _mapper.Setup(m => m.Map<SpartanDTO>(It.IsAny<Spartan>())).Returns(new SpartanDTO());
        var result = _sut.DeleteTrackerEntriesAsync(fakeSpartan, fakeTracker.Id);
        Assert.That(result.Result.Success, Is.EqualTo(false));
        Assert.That(result.Result.Message, Does.Contain("There are no tracker entries to do!"));
    }

    [Test]
    [Category("DeleteTrackers")]
    [Category("Sad Path")]
    public void DeleteTrackerEntriesAsync_GivenInvalidRole_ReturnsFailedResponse()
    {
        var fakeSpartan = Helper.CreateFakeSpartan("Bob");
        fakeSpartan.Id = "Paul";
        var fakeTracker = Helper.CreateFakeTracker("Bob");
        var result = _sut.DeleteTrackerEntriesAsync(fakeSpartan, fakeTracker.Id);
        Assert.That(result.Result.Data, Is.EqualTo(null));
    }

    [Test]
    [Category("EditTrackers")]
    [Category("Happy Path")]
    public void EditTrackerEntriesAsync_GivenCorrectData_ReturnsCorrectResponse()
    {
        var fakeTracker = Helper.CreateFakeTracker("Bob");
        _context.TrackerItems.Add(fakeTracker);
        _context.SaveChanges();
        var fakeTrackerVM = new EditTrackerVM() { Id = 1};
        fakeTrackerVM.ContinueDoingText = "Something else";
        _mapper.Setup(m => m.Map<Tracker>(It.IsAny<EditTrackerVM>())).Returns(fakeTracker);
        var result = _sut.EditTrackerEntriesAsync(fakeTracker.Spartan, 1, fakeTrackerVM);
        Assert.That(result.Result, Is.TypeOf<ServiceResponse<EditTrackerVM>>());
        Assert.That(result.Result.Success, Is.EqualTo(true));

    }

    [Test]
    [Category("EditTrackers")]
    [Category("Sad Path")]
    public void EditTrackerEntriesAsync_GivenNoSpartans_ReturnsFailedResponse()
    {
        var fakeTracker = Helper.CreateFakeTracker("Bob");
        _context.TrackerItems.Add(fakeTracker);
        _context.SaveChanges();
        var fakeTrackerVM = new EditTrackerVM() { Id = 1 };
        fakeTrackerVM.ContinueDoingText = "Something else";
        _mapper.Setup(m => m.Map<Tracker>(It.IsAny<EditTrackerVM>())).Returns(fakeTracker);
        var result = _sut.EditTrackerEntriesAsync(null, 1, fakeTrackerVM);
        Assert.That(result.Result, Is.TypeOf<ServiceResponse<EditTrackerVM>>());
        Assert.That(result.Result.Success, Is.EqualTo(false));
        Assert.That(result.Result.Message, Does.Contain("No user found"));
    }

    [Test]
    [Category("EditTrackers")]
    [Category("Sad Path")]
    public void EditTrackerEntriesAsync_GivenIdMismatch_ReturnsFailedResponse()
    {
        var fakeTracker = Helper.CreateFakeTracker("Bob");
        _context.TrackerItems.Add(fakeTracker);
        _context.SaveChanges();
        var fakeTrackerVM = new EditTrackerVM() { Id = 2 };
        fakeTrackerVM.ContinueDoingText = "Something else";
        _mapper.Setup(m => m.Map<Tracker>(It.IsAny<EditTrackerVM>())).Returns(fakeTracker);
        var result = _sut.EditTrackerEntriesAsync(fakeTracker.Spartan, 1, fakeTrackerVM);
        Assert.That(result.Result, Is.TypeOf<ServiceResponse<EditTrackerVM>>());
        Assert.That(result.Result.Success, Is.EqualTo(false));
        Assert.That(result.Result.Message, Does.Contain("Error updating"));
    }

    [Test]
    [Category("GetDetails")]
    [Category("Happy Path")]
    public void GetDetailsAsync_GivenCorrectDataTrainee_ReturnsCorrectResponse()
    {
        var fakeTracker = Helper.CreateFakeTracker("Bob");
        _context.TrackerItems.Add(fakeTracker);
        _context.SaveChanges();
        _mapper.Setup(m => m.Map<DetailsTrackerVM>(It.IsAny<Tracker>)).Returns(It.IsAny<DetailsTrackerVM>());
        var result = _sut.GetDetailsAsync(fakeTracker.Spartan, fakeTracker.Id, "Trainee");
        Assert.That(result.Result, Is.TypeOf<ServiceResponse<DetailsTrackerVM>>());
        Assert.That(result.Result.Success, Is.EqualTo(true));
    }

    [Test]
    [Category("GetDetails")]
    [Category("Sad Path")]
    public void GetDetailsAsync_GivenNullId_ReturnsFailedResponse()
    {
        var fakeTracker = Helper.CreateFakeTracker("Bob");
        var result = _sut.GetDetailsAsync(fakeTracker.Spartan, null, "Trainee");
        Assert.That(result.Result, Is.TypeOf<ServiceResponse<DetailsTrackerVM>>());
        Assert.That(result.Result.Success, Is.EqualTo(false));
    }

    [Test]
    [Category("GetDetails")]
    [Category("Sad Path")]
    public void GetDetailsAsync_GivenNoSpartan_ReturnsFailedResponse()
    {
        var fakeTracker = Helper.CreateFakeTracker("Bob");
        var result = _sut.GetDetailsAsync(null, fakeTracker.Id, "Trainee");
        Assert.That(result.Result, Is.TypeOf<ServiceResponse<DetailsTrackerVM>>());
        Assert.That(result.Result.Success, Is.EqualTo(false));
        Assert.That(result.Result.Message, Does.Contain("No user found"));
    }


}
