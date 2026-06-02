using MindzenBackendBackOffice.Interfaces;
using MindzenBackendBackOffice.Models;
using MindzenBackendBackOffice.Modules.PractitionerDetails;

namespace MindzenBackendBackOffice.Services
{
    public class PractitionerService : IPractitioner
    {
        private readonly PractitionerModule _practitionerModule = new();

        public async Task<ApiResponse> UpdateFeaturedPractitionerOrder(List<FeaturedPractitionerOrderItem> practitioners)
        {
            return await _practitionerModule.UpdateFeaturedPractitionerOrder(practitioners);
        }

        public async Task<ApiResponse> UpdatePractitionerFeaturedStatus(string userId, bool isFeatured)
        {
            return await _practitionerModule.UpdatePractitionerFeaturedStatus(userId, isFeatured);
        }

        public async Task<ApiResponse> GetPractitionerFeatureDetails()
        {
            return await _practitionerModule.GetPractitionerFeatureDetails();
        }

        public async Task<ApiResponse> GetPractionerProfileDetails(string UserId)
        {
            return await _practitionerModule.GetPractionerProfileDetails(UserId);
        }
    }
}
