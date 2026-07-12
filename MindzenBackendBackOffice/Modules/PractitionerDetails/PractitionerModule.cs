using MindzenBackendBackOffice.Models;
using MindzenBackofficeDatabaseLibrary.DataAccess.PractitionerDataAccess;
using MindzenBackofficeDatabaseLibrary.DataModels;

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
                    var top3Task = _practitionerDataAccess.GetPractitionerTop3SpecializationsByID(UserId);
                    var specializationsTask = _practitionerDataAccess.GetPractitionerSpecializationsByID(UserId);
                    var languagesTask = _practitionerDataAccess.GetLanguagesSpokenByPractitionerByID(UserId);
                    var documentsTask = _practitionerDataAccess.GetPractitionerDocumentsByID(UserId);

                    await Task.WhenAll(top3Task, specializationsTask, languagesTask, documentsTask);

                    practitionerProfile.Top3Specializations = top3Task.Result ?? [];
                    practitionerProfile.Specializations = specializationsTask.Result ?? [];
                    practitionerProfile.Languages = languagesTask.Result ?? [];
                    practitionerProfile.Documents = documentsTask.Result ?? [];

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

        #region Update Practitioner IsListed Flag
        public async Task<ApiResponse> UpdatePractitionerIsListedFlag(string userId, bool isListed)
        {
            ApiResponse apiResponse = new();
            try
            {
                await _practitionerDataAccess.UpdatePractitionerIsListedFlag(userId, isListed);

                apiResponse.Success = true;
                apiResponse.Message = isListed
                    ? "Practitioner enlisted successfully"
                    : "Practitioner delisted successfully";
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = "Error updating practitioner listed status";
                apiResponse.Data = new { errorMessage = ex.Message, errorStackTrace = ex.StackTrace };
            }
            return apiResponse;
        }
        #endregion

        #region Get All Specializations
        public async Task<ApiResponse> GetAllSpecializations()
        {
            ApiResponse apiResponse = new();
            try
            {
                var results = await _practitionerDataAccess.GetAllSpecializations();
                if (results != null && results.Count > 0)
                {
                    apiResponse.Success = true;
                    apiResponse.Message = "Specializations fetched successfully";
                    apiResponse.Data = results;
                }
                else
                {
                    apiResponse.Success = false;
                    apiResponse.Message = "No specializations found";
                }
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = "Error fetching specializations";
                apiResponse.Data = new { errorMessage = ex.Message, errorStackTrace = ex.StackTrace };
            }
            return apiResponse;
        }
        #endregion

        #region Get All Languages
        public async Task<ApiResponse> GetAllLanguages()
        {
            ApiResponse apiResponse = new();
            try
            {
                var results = await _practitionerDataAccess.GetAllLanguages();
                if (results != null && results.Count > 0)
                {
                    apiResponse.Success = true;
                    apiResponse.Message = "Languages fetched successfully";
                    apiResponse.Data = results;
                }
                else
                {
                    apiResponse.Success = false;
                    apiResponse.Message = "No languages found";
                }
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = "Error fetching languages";
                apiResponse.Data = new { errorMessage = ex.Message, errorStackTrace = ex.StackTrace };
            }
            return apiResponse;
        }
        #endregion

        #region Get All Mental Health Professions
        public async Task<ApiResponse> GetAllMentalHealthProfession()
        {
            ApiResponse apiResponse = new();
            try
            {
                var results = await _practitionerDataAccess.GetAllMentalHealthProfession();
                if (results != null && results.Count > 0)
                {
                    apiResponse.Success = true;
                    apiResponse.Message = "Mental health professions fetched successfully";
                    apiResponse.Data = results;
                }
                else
                {
                    apiResponse.Success = false;
                    apiResponse.Message = "No mental health professions found";
                }
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = "Error fetching mental health professions";
                apiResponse.Data = new { errorMessage = ex.Message, errorStackTrace = ex.StackTrace };
            }
            return apiResponse;
        }
        #endregion

        #region Save Practitioner Profile
        public async Task<ApiResponse> SavePractitionerProfile(
            PractitionerProfileRequest practitionerProfileRequest
        )
        {
            ApiResponse apiResponse = new ApiResponse();
            PractitionerProfileDbModel practitionerDbModel = new();

            try
            {
                // Set practitioner basic info
                practitionerDbModel.UserID = practitionerProfileRequest.UserID;
                practitionerDbModel.ProfessionID = practitionerProfileRequest.ProfessionID;
                practitionerDbModel.PhoneNumber = practitionerProfileRequest.PhoneNumber;
                practitionerDbModel.PracticingSince = practitionerProfileRequest.PracticingSince;
                practitionerDbModel.ProfilePicPath = practitionerProfileRequest.ProfilePicPath;
                practitionerDbModel.AboutMe = practitionerProfileRequest.AboutMe;
                practitionerDbModel.RciLicenseNumber = practitionerProfileRequest.RciLicenseNumber;

                if (
                    Double.TryParse(
                        practitionerProfileRequest.SingleSessionCharge,
                        out double singleSessionDoubleValue
                    )
                )
                {
                    practitionerDbModel.SingleSessionCharge = singleSessionDoubleValue;
                }
                else
                {
                    practitionerDbModel.SingleSessionCharge = 0;
                }

                if (
                    Double.TryParse(
                        practitionerProfileRequest.TwoSessionCharge,
                        out double twoSessionDoubleValue
                    )
                )
                {
                    practitionerDbModel.TwoSessionCharge = twoSessionDoubleValue;
                }
                else
                {
                    practitionerDbModel.TwoSessionCharge = 0;
                }

                if (
                    Double.TryParse(
                        practitionerProfileRequest.FourSessionCharge,
                        out double fourSessionDoubleValue
                    )
                )
                {
                    practitionerDbModel.FourSessionCharge = fourSessionDoubleValue;
                }
                else
                {
                    practitionerDbModel.FourSessionCharge = 0;
                }

                // Set client focus
                practitionerDbModel.AgeGroup = practitionerProfileRequest.AgeGroup;
                practitionerDbModel.IsIndividual = practitionerProfileRequest.IsIndividual;
                practitionerDbModel.IsCouple = practitionerProfileRequest.IsCouple;
                practitionerDbModel.IsFamily = practitionerProfileRequest.IsFamily;

                // Set Top 3 Specializations first
                practitionerDbModel.Top3Specializations =
                    practitionerProfileRequest.Top3Specializations ?? new List<string>();

                // Set Specializations
                var allSpecializations =
                    practitionerProfileRequest.Specializations ?? new List<string>();
                var top3Specializations =
                    practitionerProfileRequest.Top3Specializations ?? new List<string>();

                // Filter out items that are already in Top3Specializations
                practitionerDbModel.Specializations = allSpecializations
                    .Where(spec => !top3Specializations.Contains(spec))
                    .ToList();

                // Set Languages Known
                practitionerDbModel.Languages =
                    practitionerProfileRequest.Languages ?? new List<string>();

                // Set Documents
                List<PractitionerDocumentModel> documents = [];
                foreach (var doc in practitionerProfileRequest.Documents)
                {
                    documents.Add(
                        new PractitionerDocumentModel
                        {
                            DocumentType = doc.DocumentType,
                            DocumentPath = doc.DocumentPath,
                        }
                    );
                }
                practitionerDbModel.Documents = documents;

                // Save to DB
                var result = await  _practitionerDataAccess.SavePractitionerProfile(
                    practitionerDbModel
                );

                if (result > 0)
                {
                    apiResponse.Success = true;
                    apiResponse.Message = "Practitioner profile saved successfully.";
                    apiResponse.Data = new { practitionerProfileRequest.UserID };
                }
                else
                {
                    apiResponse.Success = false;
                    apiResponse.Message = "Failed to save practitioner profile.";
                }
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = "Error saving practitioner profile.";
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
