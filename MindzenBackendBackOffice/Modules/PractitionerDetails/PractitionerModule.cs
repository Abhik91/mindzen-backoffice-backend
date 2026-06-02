using MindzenBackendBackOffice.Models;
using MindzenBackofficeDatabaseLibrary.DataAccess.PractitionerDataAccess;

namespace MindzenBackendBackOffice.Modules.PractitionerDetails
{
    public class PractitionerModule
    {
        private readonly PractitionerDataAccess _practitionerDataAccess;

        public PractitionerModule()
        {
            _practitionerDataAccess = new PractitionerDataAccess();
        }

        #region Update Featured Practitioner Order
        public async Task<ApiResponse> UpdateFeaturedPractitionerOrder(List<FeaturedPractitionerOrderItem> practitioners)
        {
            ApiResponse apiResponse = new();
            try
            {
                var mapped = practitioners
                    .Select(p => (p.UserID ?? string.Empty, p.FeaturedOrder))
                    .ToList();

                string updatedBy = "admin";
                await _practitionerDataAccess.UpdateFeaturedPractitionerOrder(mapped, updatedBy);

                apiResponse.Success = true;
                apiResponse.Message = "Featured practitioner order updated successfully";
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = "Error updating featured practitioner order";
                apiResponse.Data = new
                {
                    errorMessage = ex.Message,
                    errorStackTrace = ex.StackTrace,
                };
            }
            return apiResponse;
        }
        #endregion

        #region Update Practitioner Featured Status
        public async Task<ApiResponse> UpdatePractitionerFeaturedStatus(string userId, bool isFeatured)
        {
            ApiResponse apiResponse = new();
            try
            {
                string updatedBy = "admin";
                await _practitionerDataAccess.UpdatePractitionerFeaturedStatus(userId, isFeatured, updatedBy);

                apiResponse.Success = true;
                apiResponse.Message = isFeatured
                    ? "Practitioner marked as featured successfully"
                    : "Practitioner removed from featured successfully";
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = "Error updating practitioner featured status";
                apiResponse.Data = new
                {
                    errorMessage = ex.Message,
                    errorStackTrace = ex.StackTrace,
                };
            }
            return apiResponse;
        }
        #endregion

        #region Get Practitioner Feature Details
        public async Task<ApiResponse> GetPractitionerFeatureDetails()
        {
            ApiResponse apiResponse = new();
            try
            {
                var results = await _practitionerDataAccess.GetPractitionerFeatureDetails();

                if (results != null && results.Count > 0)
                {
                    apiResponse.Success = true;
                    apiResponse.Message = "Practitioner feature details fetched successfully";
                    apiResponse.Data = results;
                }
                else
                {
                    apiResponse.Success = false;
                    apiResponse.Message = "No practitioners found";
                    apiResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = "Error fetching practitioner feature details";
                apiResponse.Data = new
                {
                    errorMessage = ex.Message,
                    errorStackTrace = ex.StackTrace,
                };
            }
            return apiResponse;
        }
        #endregion

        #region Get Practitioner Profile Details
        public async Task<ApiResponse> GetPractionerProfileDetails(string UserId)
        {
            ApiResponse apiResponse = new();
            try
            {
                var results = await _practitionerDataAccess.GetPractitionerProfileDetails(UserId);
                var practitionerProfile = results?.FirstOrDefault();

                if (practitionerProfile != null)
                {
                    apiResponse.Success = true;
                    apiResponse.Message = "Practitioner profile details fetched successfully";
                    apiResponse.Data = practitionerProfile;
                }
                else
                {
                    apiResponse.Success = false;
                    apiResponse.Message = "Practitioner profile details not found";
                    apiResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = "Error fetching practitioner profile details";
                apiResponse.Data = new
                {
                    errorMessage = ex.Message,
                    errorStackTrace = ex.StackTrace,
                };
            }
            return apiResponse;
        }
        #endregion
    }
}
