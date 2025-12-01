using System.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Npgsql;
using test.Infrastructure.JWT;
using test.Infrastructure.Swagger;
using test.Repositories;
using test.Repositories.Interfaces;
using test.Services;
using test.Services.Interfaces;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment()) {
    Env.Load();

    var connectionString = builder.Configuration.GetConnectionString("Default");
    var password = Env.GetString("DEVELOPMENT_DATABASE_PASSWORD");

    builder.Configuration.GetSection("ConnectionStrings")["Default"] = connectionString + password;
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(options => {
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
	options.TokenValidationParameters = TokenService.GetValidationParameters(builder.Configuration);
});

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(c => {
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		Scheme = "bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header
	});

	// Endpoints filter ([Authorize])
	c.OperationFilter<AuthorizeCheckOperationFilter>();

	// Auto-append controller name to tags
	c.OperationFilter<AutoTagOperationFilter>();
	
	// Enable tags (Public/Private)
	c.EnableAnnotations();
});

builder.Services.AddScoped<IDbConnection>(sp =>
	new NpgsqlConnection(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
