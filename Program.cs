using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System.Text;

namespace AspDependencyInjectionApp
{
    /*
    interface ILogger
    {
        void Log(string message);
    }
    class ConsoleLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine(message);
    }
    class Message
    {
        ILogger logger;
        public string Text { set; get; }
        public Message(ILogger logger)
        {
            this.logger = logger;
        }
        public void SendLog() => logger.Log(Text);
    }
    */

    public interface IDateTimeService
    {
        string GetDate();
    }

    public class ShortDateTimeService : IDateTimeService
    {
        public string GetDate() => DateTime.Now.ToShortDateString();
    }

    public class LongDateTimeService : IDateTimeService
    {
        public string GetDate() => DateTime.Now.ToLongDateString();
    }

    class DateService
    {
        public string GetDate() => DateTime.Now.ToLongDateString();
    }

    public static class DateServiceExtensions
    {
        public static void AddDateService(this IServiceCollection services)
        {
            services.AddTransient<DateService>();
        }
    }

    

    

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            var services = builder.Services;
            services.AddTransient<IDateTimeService, ShortDateTimeService>();
            //services.AddTransient<DateService>();
            services.AddDateService();


            var app = builder.Build();

            app.Run(async context =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                var date = app.Services.GetService<DateService>();
                await context.Response.WriteAsync($"Short date: {date?.GetDate()}");
            });

            /*
            app.Run(async context =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";

                var body = new StringBuilder();
                body.Append("<h1>All services:</h1>");
                body.Append("<table>");
                body.Append("<tr><td>Type</td><td>Lifetime</td><td>Implementation</td></tr>");
                foreach(var service in services)
                {
                    body.Append("<tr>");
                    body.Append($"<td>{service.ServiceType.FullName}</td>");
                    body.Append($"<td>{service.Lifetime}</td>");
                    body.Append($"<td>{service.ImplementationType?.FullName}</td>");
                    body.Append("</tr>");
                }
                body.Append("</table>");

                await context.Response.WriteAsync(body.ToString());
            });
            */
            app.Run();
        }
    }
}