using MindzenBackendBackOffice.Models;

namespace MindzenBackendBackOffice.Interfaces
{
    public interface IPractitioner
    {
        public Task<ApiResponse> UpdateFeaturedPractitionerOrder(List<FeaturedPractitionerOrderItem> practitioners);
        public Task<ApiResponse> UpdatePractitionerFeaturedStatus(string userId, bool isFeatured);
        public Task<ApiResponse> GetPractitionerFeatureDetails();
        public Task<ApiResponse> GetPractionerProfileDetails(string UserId);
        public Task<ApiResponse> SavePractitionerProfile(PractitionerProfileRequest request);
        public Task<ApiResponse> UpdatePractitionerIsListedFlag(string userId, bool isListed);
        public Task<ApiResponse> GetAllSpecializations();
        public Task<ApiResponse> GetAllLanguages();
        public Task<ApiResponse> GetAllMentalHealthProfession();
    }
}