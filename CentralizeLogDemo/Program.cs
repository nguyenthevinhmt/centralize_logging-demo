using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.Kafka;
namespace CentralizeLogDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Đọc cấu hình từ appsettings.json
            //Log.Logger  = new LoggerConfiguration()
            //    .ReadFrom.Configuration(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build())
            //    .WriteTo.Debug()
            //    .CreateLogger();
            //builder.Host.UseSerilog();

            //builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
            //var brokers = args[0];
            //var topic = args[1];
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration().WriteTo.Console(new JsonFormatter())
                .WriteTo.Kafka(
                        bootstrapServers: "localhost:9092",
                        topic: "logs")
                .CreateLogger();
            //LoggerFactory loggerFactory = new LoggerFactory();
            builder.Host.UseSerilog();

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            //app.UseSerilogRequestLogging();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
            Log.CloseAndFlush();
        }
    }
}