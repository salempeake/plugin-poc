using Microsoft.Extensions.DependencyInjection;
using Peake.ERP.Accounting.Services;
using Peake.ERP.Core.Interfaces;

namespace Peake.ERP.Accounting;

public class AccountingModule : IModule
{
    public void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<GeneralLedgerService>();
    }
}