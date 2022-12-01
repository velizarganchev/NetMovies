namespace NetMovies
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Data;
    using Infrastructure;
    using Services.Movies;
    using Services.Statistics;
    using NetMovies.Data.Models;
    using NetMovies.Services.Votes;

    public class Startup
    {
        readonly string nameOfSite = "NetMovies";
        public Startup(IConfiguration configuration)
            => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<NetMoviesDbContext>(options => options
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services
                .AddDatabaseDeveloperPageExceptionFilter();

            services
                .AddDefaultIdentity<AppUsers>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<NetMoviesDbContext>();

            services.AddCors(options => options.AddPolicy(name: nameOfSite,
                policy =>
                {
                    policy.WithOrigins("http://velizarg-001-site1.btempurl.com/")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                }));

            services
                .AddControllersWithViews(options =>
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddTransient<IStatisticService, StatisticService>();
            services.AddTransient<IMovieService, MovieService>();
            services.AddTransient<IVotesService, VotesService>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.PrepareDatabase();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection()
               .UseStaticFiles()
               .UseRouting()
               .UseCors(nameOfSite)
               .UseAuthentication()
               .UseAuthorization()
               .UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
