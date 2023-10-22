using Crawler.Core.BusinessLogics.Services;
using Crawler.Main.Extensions;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices();

var app = builder.Build();

app.Configure();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHangfireDashboard("/dashboard", new DashboardOptions()
{
    Authorization = new[] { new AllowAllConnectionsFilter() },
    IgnoreAntiforgeryToken = true
});

app.MapControllers();

RecurringJob.AddOrUpdate<GetCurranciesService>("Daily Currencies Notification", job => job.RequestCurrencyInfosAsync(), "0 0 * * *");

app.Run();