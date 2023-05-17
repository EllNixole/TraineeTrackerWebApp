using TraineeTracker.App.Models;
using TraineeTracker.App.Models.ViewModels;
using Moq;

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
                EmailConfirmed = true
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
            response.Data = Mock.Of<TrackerVM>();
            return response;
        }
    }
}
