using MindzenBackofficeDatabaseLibrary.Helper;
using MindzenBackofficeDatabaseLibrary.Internal;
using MindzenDatabaseLibrary.DataModels;

namespace MindzenDatabaseLibrary.DataAccess.User
{
    public class UserDataAccess
    {
        private readonly SqlDataAccess _sqlDataAccess = new();

        #region Get User Details
        public async Task<List<UserDetails>> GetUserDetails(
            string? UserId = null,
            string? Email = null,
            bool IncludePassword = false
        )
        {
            try
            {
                var parameter = new { UserID = UserId, Username = Email, IncludePassword };

                var result = await _sqlDataAccess.LoadData<UserDetails, dynamic>(
                    "dbo.GetUserDetails",
                    parameter,
                    Global.MindzenConnectionString!
                );
                return result;
            }
            catch
            {
                throw;
            }
        }
        #endregion

    }
}