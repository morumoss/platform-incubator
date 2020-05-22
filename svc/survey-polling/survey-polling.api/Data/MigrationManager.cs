using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using survey_polling.api.Data;
using System;
using System.Diagnostics;

namespace survey_polling.api.sData
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var pollContext = scope.ServiceProvider.GetRequiredService<PollContext>())
                {
                    try
                    {
                        pollContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        throw;
                    }
                }
            }

            return host;
        }
    }
}
