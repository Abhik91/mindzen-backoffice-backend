using MindzenBackendBackOffice.Models;

namespace MindzenBackendBackOffice.Modules.FileProcessing
{
    public class FileModule(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        private readonly string _mainBackendUrl = configuration["MainBackend:BaseUrl"] ?? string.Empty;

        public async Task<ApiResponse> GetPractitionerProfilePic(string filePath)
        {
            ApiResponse apiResponse = new();
            try
            {
                var client = httpClientFactory.CreateClient();
                var url = $"{_mainBackendUrl}/api/file/unsigned-download?filePath={filePath}";

                var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

                if (!response.IsSuccessStatusCode)
                {
                    apiResponse.Success = false;
                    apiResponse.Message = "File not found";
                    return apiResponse;
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var contentType = response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";
                var fileName = Path.GetFileName(filePath);

                apiResponse.Success = true;
                apiResponse.Message = "File found and ready to download";
                apiResponse.Data = new CreatedFileContent
                {
                    Stream = stream,
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
    }
}
