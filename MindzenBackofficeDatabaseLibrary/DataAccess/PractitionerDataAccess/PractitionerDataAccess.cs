using System.Data;
using Dapper;
using MindzenBackofficeDatabaseLibrary.DataModels;
using MindzenDatabaseLibrary.DataModels;
using MindzenBackofficeDatabaseLibrary.Helper;
using MindzenBackofficeDatabaseLibrary.Internal;

namespace MindzenBackofficeDatabaseLibrary.DataAccess.PractitionerDataAccess
{
    public class PractitionerDataAccess
    {
        private readonly SqlDataAccess _sqlDataAccess;

        public PractitionerDataAccess()
        {
            _sqlDataAccess = new SqlDataAccess();
        }

        #region Get Practitioner Profile Details
        public async Task<List<PractitionerProfileDbModel>> GetPractitionerProfileDetails(
            string UserId
        )
        {
            try
            {
                var parameter = new { UserID = UserId };

                var result = await _sqlDataAccess.LoadData<PractitionerProfileDbModel, dynamic>(
                    "dbo.GetPractitionerProfileDetails",
                    parameter,
                    Global.MindzenConnectionString ?? string.Empty
                );
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Featured Practitioner Order
        public async Task<int> UpdateFeaturedPractitionerOrder(
            List<(string UserID, int FeaturedOrder)> practitioners, string updatedBy
        )
        {
            try
            {
                var table = new DataTable();
                table.Columns.Add("UserID", typeof(string));
                table.Columns.Add("FeaturedOrder", typeof(int));
                foreach (var (userId, order) in practitioners)
                    table.Rows.Add(userId, order);

                var parameters = new DynamicParameters();
                parameters.Add("@FeaturedPractitioners", table.AsTableValuedParameter("dbo.FeaturedPractitionerOrderType"));
                parameters.Add("@UpdatedBy", updatedBy);

                return await _sqlDataAccess.SaveData(
                    "dbo.UpdateFeaturedPractitionerOrder",
                    parameters,
                    Global.MindzenConnectionString ?? string.Empty
                );
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Practitioner Featured Status
        public async Task<int> UpdatePractitionerFeaturedStatus(
            string userId, bool isFeatured, string updatedBy
        )
        {
            try
            {
                var parameter = new
                {
                    UserID = userId,
                    IsFeatured = isFeatured,
                    UpdatedBy = updatedBy
                };

                return await _sqlDataAccess.SaveData(
                    "dbo.UpdatePractitionerFeaturedStatus",
                    parameter,
                    Global.MindzenConnectionString ?? string.Empty
                );
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Practitioner Feature Details
        public async Task<List<PractitionerFeatureDetailsDbModel>> GetPractitionerFeatureDetails()
        {
            try
            {
                var result = await _sqlDataAccess.LoadData<PractitionerFeatureDetailsDbModel, dynamic>(
                    "dbo.GetPractitionerFeatureDetails",
                    new { },
                    Global.MindzenConnectionString ?? string.Empty
                );
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region SavePractitionerProfile
        public async Task<int> SavePractitionerProfile(
            PractitionerProfileDbModel practitionerProfileDbModel
        )
        {
            try
            {
                // Convert string lists to model lists for DataTable conversion
                var specializationModels =
                    practitionerProfileDbModel
                        .Specializations?.Select(s => new PractitionerSpecializationsModel
                        {
                            SpecializationID = s,
                        })
                        .ToList() ?? new List<PractitionerSpecializationsModel>();
                var top3SpecializationModels =
                    practitionerProfileDbModel
                        .Top3Specializations?.Select(s => new PractitionerSpecializationsModel
                        {
                            SpecializationID = s,
                        })
                        .ToList() ?? new List<PractitionerSpecializationsModel>();
                var languageModels =
                    practitionerProfileDbModel
                        .Languages?.Select(l => new PractitionerSpeakingLanguageModel
                        {
                            LanguageID = l,
                        })
                        .ToList() ?? new List<PractitionerSpeakingLanguageModel>();

                DataTable specializationDataTable = Global.ConvertToDataTable(
                    specializationModels,
                    typeof(PractitionerSpecializationsModel)
                );
                DataTable top3SpecializationDataTable = Global.ConvertToDataTable(
                    top3SpecializationModels,
                    typeof(PractitionerSpecializationsModel)
                );
                DataTable languagesDataTable = Global.ConvertToDataTable(
                    languageModels,
                    typeof(PractitionerSpeakingLanguageModel)
                );
                DataTable documentsDataTable = Global.ConvertToDataTable(
                    practitionerProfileDbModel.Documents ?? new List<PractitionerDocumentModel>(),
                    typeof(PractitionerDocumentModel)
                );

                var parameters = new DynamicParameters();
                parameters.Add("UserID", practitionerProfileDbModel.UserID);
                parameters.Add("ProfessionID", practitionerProfileDbModel.ProfessionID);
                parameters.Add("PhoneNumber", practitionerProfileDbModel.PhoneNumber);
                parameters.Add("PracticingSince", practitionerProfileDbModel.PracticingSince);
                parameters.Add("ProfilePicPath", practitionerProfileDbModel.ProfilePicPath);
                parameters.Add("AboutMe", practitionerProfileDbModel.AboutMe);
                parameters.Add("RciLicenseNumber", practitionerProfileDbModel.RciLicenseNumber);
                parameters.Add("SingleSessionCharge", practitionerProfileDbModel.SingleSessionCharge);
                parameters.Add("TwoSessionCharge", practitionerProfileDbModel.TwoSessionCharge);
                parameters.Add("FourSessionCharge", practitionerProfileDbModel.FourSessionCharge);
                parameters.Add("AgeGroup", practitionerProfileDbModel.AgeGroup);
                parameters.Add("IsIndividual", practitionerProfileDbModel.IsIndividual);
                parameters.Add("IsCouple", practitionerProfileDbModel.IsCouple);
                parameters.Add("IsFamily", practitionerProfileDbModel.IsFamily);
                parameters.Add(
                    "Specializations",
                    specializationDataTable.AsTableValuedParameter(
                        "dbo.TherapistLifeCoachSpecializationType"
                    )
                );
                parameters.Add(
                    "Top3Specializations",
                    top3SpecializationDataTable.AsTableValuedParameter(
                        "dbo.TherapistLifeCoachSpecializationType"
                    )
                );
                parameters.Add(
                    "Languages",
                    languagesDataTable.AsTableValuedParameter(
                        "dbo.TherapistLifeCoachSpeakingLanguageType"
                    )
                );
                parameters.Add(
                    "Documents",
                    documentsDataTable.AsTableValuedParameter(
                        "dbo.TherapistLifeCoachDocumentsType"
                    )
                );
                parameters.Add(
                    "RowsAffected",
                    dbType: DbType.Int32,
                    direction: ParameterDirection.Output
                ); // OUTPUT parameter

                await _sqlDataAccess.SaveScalarData<dynamic>(
                    "dbo.AddPractitionerProfileDetails",
                    parameters,
                    Global.MindzenConnectionString ?? string.Empty
                );
                var result = parameters.Get<int>("RowsAffected");
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Practitioner Top 3 Specializations By ID
        public async Task<List<string>> GetPractitionerTop3SpecializationsByID(string UserId)
        {
            try
            {
                var parameter = new { UserID = UserId };

                var result = await _sqlDataAccess.LoadData<string, dynamic>(
                    "dbo.GetPractitionerTop3SpecializationsByID",
                    parameter,
                    Global.MindzenConnectionString ?? string.Empty
                );
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Practitioner Documents By ID
        public async Task<List<PractitionerDocumentModel>> GetPractitionerDocumentsByID(string UserId)
        {
            try
            {
                var parameter = new { UserID = UserId };

                var result = await _sqlDataAccess.LoadData<PractitionerDocumentModel, dynamic>(
                    "dbo.GetPractitionerDocumentsByID",
                    parameter,
                    Global.MindzenConnectionString ?? string.Empty
                );
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Practitioner Specializations By ID
        public async Task<List<string>> GetPractitionerSpecializationsByID(string UserId)
        {
            try
            {
                var parameter = new { UserID = UserId };

                var result = await _sqlDataAccess.LoadData<string, dynamic>(
                    "dbo.GetPractitionerSpecializationsByID",
                    parameter,
                    Global.MindzenConnectionString ?? string.Empty
                );
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Languages Spoken By Practitioner By ID
        public async Task<List<string>> GetLanguagesSpokenByPractitionerByID(string UserId)
        {
            try
            {
                var parameter = new { UserID = UserId };

                var result = await _sqlDataAccess.LoadData<string, dynamic>(
                    "dbo.GetLanguagesSpokenByPractitionerByID",
                    parameter,
                    Global.MindzenConnectionString ?? string.Empty
                );
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Practitioner IsListed Flag
        public async Task<int> UpdatePractitionerIsListedFlag(string practitionerId, bool isListed)
        {
            try
            {
                var parameter = new { PractitionerId = practitionerId, IsListed = isListed };

                return await _sqlDataAccess.SaveData(
                    "dbo.UpdatePractitionerIsListedFlag",
                    parameter,
                    Global.MindzenConnectionString ?? string.Empty
                );
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetAllSpecializations
        public async Task<List<SpecializationDbModel>> GetAllSpecializations()
        {
            try
            {
                var result = await _sqlDataAccess.LoadData<SpecializationDbModel, dynamic>(
                    "dbo.GetAllSpecializations",
                    new { },
                    Global.MindzenConnectionString ?? string.Empty
                );
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetAllLanguages
        public async Task<List<LanguageDbModel>> GetAllLanguages()
        {
            try
            {
                var result = await _sqlDataAccess.LoadData<LanguageDbModel, dynamic>(
                    "dbo.GetAllLanguages",
                    new { },
                    Global.MindzenConnectionString ?? string.Empty
                );
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetAllMentalHealthProfession
        public async Task<List<MentalHealthProfessionDbModel>> GetAllMentalHealthProfession()
        {
            try
            {
                var result = await _sqlDataAccess.LoadData<MentalHealthProfessionDbModel, dynamic>(
                    "dbo.GetAllMentalHeathProfession",
                    new { },
                    Global.MindzenConnectionString ?? string.Empty
                );
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


    }
}