using AFSTranslate.Models;
using Microsoft.AspNetCore.Identity;

namespace AFSTranslate
{
    public static class DatabaseSeeder
    {
        public static void SeedData(ApplicationDbContext dbContext)
        {
            SeedStatuses(dbContext);
        }

        private static void SeedStatuses(ApplicationDbContext dbContext)
        {
            if (!dbContext.Statuses.Any())
            {
                List<Status> statuses = new List<Status>
            {
                new Status { Name = "Success" },
                new Status { Name = "Error" },
            };

                dbContext.Statuses.AddRange(statuses);
                dbContext.SaveChanges();
            }

            if (!dbContext.TranslationTypes.Any())
            {
                List<TranslationType> TranslationTypes = new List<TranslationType>
                {
                    new TranslationType{ Name= "minion"},
                    new TranslationType{ Name= "ferb-latin"},
                    new TranslationType{ Name= "dothraki"},
                    new TranslationType{ Name= "valyrian"},
                    new TranslationType{ Name= "sindarin"},
                    new TranslationType{ Name= "hodor"},
                };
                dbContext.TranslationTypes.AddRange(TranslationTypes);
                dbContext.SaveChanges();
            }
        }
    }

}
