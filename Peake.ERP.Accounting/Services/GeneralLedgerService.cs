using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Peake.ERP.Accounting.Entities;

namespace Peake.ERP.Accounting.Services;

public class GeneralLedgerService
{
    private readonly IConfiguration _conf;

    public GeneralLedgerService(IConfiguration conf)
    {
        _conf = conf;
    }

    public async Task<List<dynamic>> GetGlEntries()
    {
        const string sql = """
                           
                                       WITH tb_data AS (SELECT ChartId       = c.Id
                                                         , AccountNumber = a.AccountNumber
                                                         , AccountName   = a.AccountName
                                                         , Debit         = SUM(jed.Debit)
                                                         , Credit        = SUM(jed.Credit)
                                                         , Balance       = (
                                           CASE a.RegularBalance
                                               WHEN 0 THEN SUM(jed.Debit) - SUM(jed.Credit)
                                               ELSE SUM(jed.Credit) - SUM(jed.Debit)
                                               END
                                           )
                                                    FROM ChartOfAccounts c
                                                             INNER JOIN GeneralLedgerAccounts a ON c.Id = a.ChartId
                                                             INNER JOIN JournalEntryDetail jed ON a.Id = jed.AccountId
                                                    GROUP BY c.Id, a.AccountNumber, a.AccountName, a.RegularBalance)
                                       SELECT *
                                       FROM tb_data
                                       UNION ALL
                                       SELECT NULL, NULL, NULL, SUM(Debit), SUM(Credit), SUM(Debit) - SUM(Credit)
                                       FROM tb_data
                                   
                           """;
        
        await using var cnn = new SqlConnection(_conf.GetConnectionString("DefaultConnection"));
        await cnn.OpenAsync();
        return (await cnn.QueryAsync<dynamic>(sql)).ToList();
    }
}