using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuanLyCaThi.Data;
using QuanLyCaThi.Models.Process;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<CheckSecurityKey>();
builder.Services.AddTransient<UpdateValue>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// Truy cập IdentityOptions
// services.Configure<IdentityOptions> (options => {
//     // Thiết lập về Password
//     options.Password.RequireDigit = false; // Không bắt phải có số
//     options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
//     options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
//     options.Password.RequireUppercase = false; // Không bắt buộc chữ in
//     options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
//     options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

//     // Cấu hình Lockout - khóa user
//     options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5); // Khóa 5 phút
//     options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
//     options.Lockout.AllowedForNewUsers = true;

//     // Cấu hình về User.
//     options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
//         "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
//     options.User.RequireUniqueEmail = true;  // Email là duy nhất

//     // Cấu hình đăng nhập.
//     options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
//     options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại

// });

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ExamRegistration}/{action=Index}/{id?}");

app.Run();
