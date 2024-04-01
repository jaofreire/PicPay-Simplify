using Amazon.SimpleEmail;
using Microsoft.EntityFrameworkCore;
using PicPaySimplify.Data;
using PicPaySimplify.Repositories;
using PicPaySimplify.Repositories.Interface;
using PicPaySimplify.Services.AWSServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEntityFrameworkSqlServer().AddDbContext<PicPayDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DataBaseSql"));
});

builder.Services.AddAWSService<IAmazonSimpleEmailService>().AddTransient<SESWrapper>();
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
