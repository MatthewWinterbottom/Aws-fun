using Ufpls.Api.Services;
using Ufpls.Checker;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUfplsEligibilityRule, FundValueRule>();
builder.Services.AddScoped<IUfplsEligibilityRule, DeceasedRule>();
builder.Services.AddScoped<EligibilityEvaluator>();

builder.Services.AddSingleton<UfplsCaseService>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.Run();