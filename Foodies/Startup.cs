using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Foodies.Models;

namespace Foodies
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddDbContext<ApplicationDBContext>(options =>
        //      options.UseSqlServer(Configuration.GetConnectionString("Server=.;Database=Foodies;Trusted_Connection=True;TrustServerCertificate=True;")));

        //    services.AddControllersWithViews();
        //}

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))); // Ensure you have a connection string defined in appsettings.json
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}



//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Foodies.Models;
//using System.Net.WebSockets;
//using System.Text;
//using System.Threading;

//namespace Foodies
//{
//    public class Startup
//    {
//        public Startup(IConfiguration configuration)
//        {
//            Configuration = configuration;
//        }

//        public IConfiguration Configuration { get; }

//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddDbContext<ApplicationDBContext>(options =>
//                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

//            services.AddControllersWithViews();
//        }

//        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//        {
//            if (env.IsDevelopment())
//            {
//                app.UseDeveloperExceptionPage();
//            }
//            else
//            {
//                app.UseExceptionHandler("/Home/Error");
//                app.UseHsts();
//            }

//            app.UseHttpsRedirection();
//            app.UseStaticFiles();
//            app.UseRouting();
//            app.UseAuthorization();

//            // Configure WebSocket Middleware
//            var webSocketOptions = new WebSocketOptions
//            {
//                KeepAliveInterval = TimeSpan.FromMinutes(2) // Optional: Set WebSocket keep-alive interval
//            };

//            app.UseWebSockets(webSocketOptions);

//            app.Use(async (context, next) =>
//            {
//                if (context.Request.Path == "/ws")
//                {
//                    if (context.WebSockets.IsWebSocketRequest)
//                    {
//                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
//                        await HandleWebSocketCommunication(context, webSocket);
//                    }
//                    else
//                    {
//                        context.Response.StatusCode = 400; // Bad Request
//                    }
//                }
//                else
//                {
//                    await next();
//                }
//            });

//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapControllerRoute(
//                    name: "default",
//                    pattern: "{controller=Home}/{action=Index}/{id?}");
//            });
//        }

//        private async Task HandleWebSocketCommunication(HttpContext context, WebSocket webSocket)
//        {
//            var buffer = new byte[1024 * 4];
//            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

//            while (!result.CloseStatus.HasValue)
//            {
//                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
//                var response = $"Server: {message}";

//                // Send response back to the client
//                await webSocket.SendAsync(Encoding.UTF8.GetBytes(response), result.MessageType, result.EndOfMessage, CancellationToken.None);

//                // Receive the next message
//                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
//            }

//            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
//        }
//    }
//}
