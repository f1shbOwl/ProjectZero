using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.WPF.ViewModels;
using Presentation.WPF.Views;
using System.Windows;

namespace Presentation.WPF
{
    public partial class App : Application
    {
        
        private IHost builder;

        public App()
        {
            builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
            {
                services.AddDbContext<UserContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=F:\Education\ec-projects\ProjectZero\Infrastructure\Data\Users_Database.mdf;Integrated Security=True;Connect Timeout=30"));

                services.AddScoped<RoleRepo>();
                services.AddScoped<AddressRepo>();
                services.AddScoped<UserRepo>();
                services.AddScoped<AuthenticationRepo>();
                services.AddScoped<ContactInformationRepo>();

                services.AddScoped<RoleService>();
                services.AddScoped<AddressService>();
                services.AddScoped<UserService>();
                services.AddScoped<AuthenticationService>();
                services.AddScoped<ContactInformationService>();

                services.AddScoped<MainViewModel>(); 
                services.AddScoped<MainWindow>();

                services.AddScoped<UserListViewModel>();
                services.AddScoped<UserListView>();

                services.AddScoped<UserAddViewModel>();
                services.AddScoped<UserAddView>();

                services.AddScoped<UserDetailViewModel>();
                services.AddScoped<UserDetailView>();

                services.AddScoped<EditUserViewModel>();
                services.AddScoped<EditUserView>();

                services.AddScoped<RoleListViewModel>();
                services.AddScoped<RoleListView>();

                services.AddScoped<AddressListViewModel>();
                services.AddScoped<AddressListView>();


            }).Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            builder.Start();

            var mainWindow = builder.Services.GetRequiredService<MainWindow>();
            var mainViewModel = builder.Services.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = builder.Services.GetRequiredService<UserListViewModel>();
            mainWindow.Show();
        }
    }




}
