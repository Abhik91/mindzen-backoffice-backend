using System.Data;
using Dapper;
using MindzenBackofficeDatabaseLibrary.DataModels;
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

    }
}