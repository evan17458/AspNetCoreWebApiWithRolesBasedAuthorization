using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using WebApiWithRoleAuthentication.Data;
using WebApiWithRoleAuthentication.Models;
using WebApiWithRoleAuthentication.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 配置 Serilog
builder.Host.UseSerilog((context, configuration) =>
{
    configuration

        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day); // 輸出到檔案，按天分割

});
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();//在 Service 中使用 HttpContext，取得登入者資訊、請求資料等
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();//要產生連結（像是 UrlHelper.Link()）時，取得 ActionContext
builder.Services.AddControllers(setupAction =>
{
    setupAction.ReturnHttpNotAcceptable = true;//若用戶端請求的 Accept 標頭指定了伺服器無法處理或不支援的媒體類型
    // （例如：application/xml 而專案並未加入 XML 支援），則伺服器會直接回傳 406 Not Acceptable，表示無法滿足該請求的格式需求。
})
.AddNewtonsoftJson(setupAction =>
{
    // 這邊就是設定 Newtonsoft.Json 的一些序列化參數,表示在序列化成 JSON 時，物件的屬性名稱會變成 小駝峰命名（camelCase）。
    setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    // 忽略物件序列化時的循環參考（防止 self referencing loop）
    setupAction.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
;//負責處理 Web API 的核心邏輯，即路由請求到控制器並產生回應。

//雖然新的專案預設都使用 System.Text.Json，但還是有不少情境需要或習慣使用 Newtonsoft.Json
//Newtonsoft.Json 支援更多進階功能與屬性（像是 JsonConverter、ReferenceLoopHandling、NullValueHandling 等）


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();//負責提供 API 的描述，以便於文件和測試。
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    // 加入自訂的 DocumentFilter
    c.DocumentFilter<CustomSwaggerDocumentFilter>();
});

builder.Services.AddScoped<ITouristRouteRepository, TouristRouteRepository>();

builder.Services.AddDbContext<AppDbContext>(option =>
    option.UseNpgsql(builder.Configuration.GetConnectionString("Default")!));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());  //掃描 profile 文件

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();


var app = builder.Build();


var port = Environment.GetEnvironmentVariable("PORT") ?? "10000"; // 取得環境變數中的端口，若無則預設 10000


app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
