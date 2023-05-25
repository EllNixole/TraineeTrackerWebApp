using Moq;
using TraineeTracker.App.Models;
using TraineeTracker.App.Models.ViewModels;

namespace TraineeTrackerTests
{
    public static class Helper
    {
        public static ServiceResponse<Spartan> GetSpartanServiceResponse()
        {
            var response = new ServiceResponse<Spartan>();
            response.Data = new Spartan
            {
                Id = "Id",
                Email = "Talal@spartaglobal.com",
                EmailConfirmed = true,
                Role = "Trainee"
            };

            return response;
        }
        public static ServiceResponse<T> GetFailedServiceResponse<T>(string message = "")
        {
            var response = new ServiceResponse<T>();
            response.Success = false;
            response.Message = message;
            return response;
        }

        public static ServiceResponse<IEnumerable<TrackerVM>> GetTrackerListServiceResponse()
        {
            var response = new ServiceResponse<IEnumerable<TrackerVM>>();
            response.Data = new List<TrackerVM>
            {
                Mock.Of<TrackerVM>(),
                Mock.Of<TrackerVM>()
            };
            return response;
        }

        public static ServiceResponse<TrackerVM> GetTrackerServiceResponse()
        {
            var response = new ServiceResponse<TrackerVM>();
            response.Data = new TrackerVM() { Title = "temp", Spartan = new SpartanDTO() { UserName = "Talal" } };
            return response;
        }
        public static ServiceResponse<EditTrackerVM> GetEditTrackerServiceResponse()
        {
            var response = new ServiceResponse<EditTrackerVM>();
            response.Data = Mock.Of<EditTrackerVM>();
            return response;
        }
        public static ServiceResponse<DetailsTrackerVM> GetDetailsTrackerServiceResponse()
        {
            var response = new ServiceResponse<DetailsTrackerVM>();
            response.Data = Mock.Of<DetailsTrackerVM>();
            return response;
        }
        public static ServiceResponse<CreateTrackerVM> GetCreateTrackerServiceResponse()
        {
            var response = new ServiceResponse<CreateTrackerVM>();
            response.Data = Mock.Of<CreateTrackerVM>();
            return response;
        }
    }
}
