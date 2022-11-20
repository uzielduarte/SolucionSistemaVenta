using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaVenta.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.DAL.Implementacion;
using SistemaVenta.DAL.Interfaces;
using SistemaVenta.BLL.Implementacion;
using SistemaVenta.BLL.Interfaces;

namespace SistemaVenta.IOC
{
    public static class Dependencias
    {
        public static void InyectarDependencia(this IServiceCollection Service, IConfiguration Configuration)
        {
            Service.AddDbContext<DbventaContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("CadenaSQL"));
            });

            Service.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            Service.AddScoped<IVentaRepository, VentaRepository>();

            Service.AddScoped<ICorreoService, CorreoService>();

            Service.AddScoped<IFireBaseService, FireBaseService>();

        }
    }
}
