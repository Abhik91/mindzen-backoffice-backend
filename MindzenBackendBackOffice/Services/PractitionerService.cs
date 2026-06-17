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

        public async Task<ApiResponse> SavePractitionerProfile(PractitionerProfileRequest request)
        {
            return await _practitionerModule.SavePractitionerProfile(request);
        }

        public async Task<ApiResponse> UpdatePractitionerIsListedFlag(string userId, bool isListed)
        {
            return await _practitionerModule.UpdatePractitionerIsListedFlag(userId, isListed);
        }

        public async Task<ApiResponse> GetAllSpecializations()
        {
            return await _practitionerModule.GetAllSpecializations();
        }

        public async Task<ApiResponse> GetAllLanguages()
        {
            return await _practitionerModule.GetAllLanguages();
        }

        public async Task<ApiResponse> GetAllMentalHealthProfession()
        {
            return await _practitionerModule.GetAllMentalHealthProfession();
        }
    }
}
