using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            }).Build();
        }
    }


}
