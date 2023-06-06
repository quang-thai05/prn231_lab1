using Lab1.Formatter;
using Lab1.Mapper;
using Lab1.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(
    options =>
    {
        options.OutputFormatters.Add(new CsvOutputFormatter());
        options.InputFormatters.Add(new CsvInputFormatter());
    }
);
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Prn231DbDemoContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("ConStr"))
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(MapperProfile));

var app = builder.Build();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();