using MindzenBackendBackOffice.Models;

namespace MindzenBackendBackOffice.Modules.FileProcessing
{
    public class FileModule(HttpClient httpClient, string mainBackendBaseUrl)
    {
        public async Task<ApiResponse> GetPractitionerProfilePic(string filePath)
        {
            ApiResponse apiResponse = new();
            try
            {
                var response = await httpClient.GetAsync(
                    $"{mainBackendBaseUrl}/api/file/unsigned-download?filePath={Uri.EscapeDataString(filePath)}");

                if (!response.IsSuccessStatusCode)
                {
                    apiResponse.Success = false;
                    apiResponse.Message = "File not found on main backend";
                    return apiResponse;
                }

                var contentType = response.Content.Headers.ContentType?.MediaType
                                  ?? "application/octet-stream";
                var fileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('"')
                               ?? Path.GetFileName(filePath);

                apiResponse.Success = true;
                apiResponse.Message = "File fetched successfully";
                apiResponse.Data = new CreatedFileContent
                {
                    Stream = await response.Content.ReadAsStreamAsync(),
                    ContentType = contentType,
                    FileName = fileName
                };
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = "Error fetching file";
                apiResponse.Data = new { errorMessage = ex.Message };
            }
            return apiResponse;
        }

        public async Task<ApiResponse> UploadFile(IFormFile formFile, string fileName, string directoryPath)
        {
            ApiResponse apiResponse = new();
            try
            {
                using var formData = new MultipartFormDataContent();

                var fileContent = new StreamContent(formFile.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(
                    formFile.ContentType ?? "application/octet-stream");

                formData.Add(fileContent, "UploadFile", formFile.FileName);
                formData.Add(new StringContent(fileName), "FileName");
                formData.Add(new StringContent(directoryPath), "DirectoryPath");

                var response = await httpClient.PostAsync(
                    $"{mainBackendBaseUrl}/api/file/upload", formData);

                var body = await response.Content.ReadFromJsonAsync<ApiResponse>();

                if (body == null || !body.Success)
                {
                    apiResponse.Success = false;
                    apiResponse.Message = "File upload failed on main backend";
                    return apiResponse;
                }

                return body;
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = "File failed to upload";
                apiResponse.Data = new { errorMessage = ex.Message };
            }
            return apiResponse;
        }
    }
}
