using CustomerLibCore.Api.Dtos;
using CustomerLibCore.Api.Filters;
using CustomerLibCore.Data.Repositories;
using CustomerLibCore.Data.Repositories.EF;
using CustomerLibCore.ServiceLayer.Services;
using CustomerLibCore.ServiceLayer.Services.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CustomerLibCore.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// SqlServer db context
			services.AddDbContext<CustomerLibDataContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("CustomerLibDb")));

			services.AddTransient<INoteRepository, NoteRepository>();
			services.AddTransient<IAddressRepository, AddressRepository>();
			services.AddTransient<ICustomerRepository, CustomerRepository>();

			// Services
			services.AddTransient<INoteService, NoteService>();
			services.AddTransient<IAddressService, AddressService>();
			services.AddTransient<ICustomerService, CustomerService>();

			// AutoMapper
			services.AddAutoMapper(typeof(AutoMapperApiProfile));

			// Controllers
			services.AddControllers((options) => options.Filters.Add(new ExceptionFilter()))
				.AddControllersAsServices();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1",
					new OpenApiInfo { Title = "CustomerLibCore.Api", Version = "v1" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c =>
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomerLibCore.Api v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
