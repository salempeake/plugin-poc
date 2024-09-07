using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Peake.ERP.Accounting.Services;

namespace Peake.ERP.Accounting.Areas.Accounting.Pages;

public class Index : PageModel
{
    private readonly GeneralLedgerService _glService;

    public Index(GeneralLedgerService glService)
    {
        _glService = glService;
    }
    
    [BindProperty]
    public List<dynamic>? TbData { get; set; }

    public async Task OnGet()
    {
        TbData = await _glService.GetGlEntries();
    }
}