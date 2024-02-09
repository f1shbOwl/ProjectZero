﻿using Infrastructure.Contexts;
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
                services.AddDbContext<UserContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=F:\Education\ec-projects\ProjectZero\Infrastructure\Data\Users_Database.mdf;Integrated Security=True;Connect Timeout=30"), ServiceLifetime.Transient);

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

                services.AddSingleton<MainViewModel>(); 
                services.AddSingleton<MainWindow>();

                services.AddTransient<UserListViewModel>();
                services.AddTransient<UserListView>();

                services.AddTransient<UserAddViewModel>();
                services.AddTransient<UserAddView>();

                services.AddTransient<UserDetailViewModel>();
                services.AddTransient<UserDetailView>();

                services.AddTransient<EditUserViewModel>();
                services.AddTransient<EditUserView>();

                services.AddTransient<RoleListViewModel>();
                services.AddTransient<RoleListView>();

                services.AddTransient<AddressListViewModel>();
                services.AddTransient<AddressListView>();


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
