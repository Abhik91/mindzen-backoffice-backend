using MindzenBackendBackOffice.Models;

namespace MindzenBackendBackOffice.Interfaces
{
    public interface IPractitioner
    {
        public Task<ApiResponse> UpdateFeaturedPractitionerOrder(List<FeaturedPractitionerOrderItem> practitioners);
        public Task<ApiResponse> UpdatePractitionerFeaturedStatus(string userId, bool isFeatured);
        public Task<ApiResponse> GetPractitionerFeatureDetails();
public Task<ApiResponse> GetPractionerProfileDetails(string UserId);
    }
}