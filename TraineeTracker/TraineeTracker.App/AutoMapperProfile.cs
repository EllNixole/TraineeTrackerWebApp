using AutoMapper;
using TraineeTracker.App.Models;
using TraineeTracker.App.Models.ViewModels;

namespace TraineeTracker.App;


public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Tracker, TrackerVM>().ReverseMap();
        CreateMap<CreateTrackerVM, Tracker>();
    }
}
