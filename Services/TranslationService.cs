using AFSTranslate.Models;
using AFSTranslate.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AFSTranslate.Services
{
    public interface ITranslationService
    {
        Task<List<Response>> GetAllResponsesAsync(string UserId);
        TranslationRequestViewModel SetupTranslation();
        Task<TranslatedResponse> TranslateTextAsync(string textToTranslate, int translationTypeId, string userid);
        Response GetLatestTranslation();
        public void GetAllUserTransalation(string UserId);



    }
    public class TranslationService: ITranslationService
    {
        private readonly ApplicationDbContext _dbContext;

        public TranslationService(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<List<Response>> GetAllResponsesAsync(string UserId)
        {
            return await _dbContext.Responses.Include(x=>x.Status)
                                              .Include(x=>x.Translation).Where(x=>x.UserId == UserId).ToListAsync();
        }

        public void GetAllUserTransalation(string UserId)
        {
            var data = _dbContext.Responses.FromSqlRaw($"EXEC GetUserTranslationReponse {UserId}" ).ToList();
        }

        private SelectList GetTransalation()
        {

            return new SelectList(_dbContext.TranslationTypes
                .Select(s => new { Id = s.TranslationTypeId, Text = $"{s.Name}" }), "Id", "Text");
        }

        public TranslationRequestViewModel SetupTranslation()
        {
            return new TranslationRequestViewModel()
            {
                Translations = GetTransalation()
            };
        }
        private string GetTranslationName(int translationTypeId)
        {
            return _dbContext.TranslationTypes.Where(x => x.TranslationTypeId == translationTypeId).FirstOrDefault().Name;
        }

        public async Task<TranslatedResponse> TranslateTextAsync(string textToTranslate, int translationTypeId, string userid)
        {
            var translationType = GetTranslationName(translationTypeId);
            using (var client = new HttpClient())
            {
                var apiUrl = $"https://api.funtranslations.com/translate/{translationType.ToLower()}.json";

                var request = new 
                {
                    Text = textToTranslate
                };

                var response = await client.PostAsJsonAsync(apiUrl, request);

                if (response.IsSuccessStatusCode)
                {
                    var translationResponse = await response.Content.ReadFromJsonAsync<TTResponse>();
                    saveResponseToDb(translationResponse.Contents, 1, userid);
                    return translationResponse.Contents;
                }
                else
                {
                    saveResponseToDb(new TranslatedResponse { Text= textToTranslate, Translation = translationType, Translated="null" }, 2, userid);
                    throw new HttpRequestException("API request failed.");
                }
            }
        }

        private void saveResponseToDb(TranslatedResponse response, int status ,string userid)
        {
            var translationTypeId = _dbContext.TranslationTypes.Where(x=>x.Name.Contains(response.Translation.ToLower())).FirstOrDefault().TranslationTypeId; 
            _dbContext.Responses.Add(new Response
            {
                Text= response.Text,
                TranslatedText = response.Translated ?? "null",
                UserId= userid,
                TranslationId = translationTypeId,
                StatusId = status,
                CreatedBy = "Admin",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UpdatedBy = "Admin",
                
            });
            _dbContext.SaveChanges();
        }

        public Response GetLatestTranslation()
        {
            return _dbContext.Responses.Include(x=>x.Status).Include(x=>x.Translation).OrderByDescending(x=>x.CreatedAt).FirstOrDefault();
        }
    }
}
