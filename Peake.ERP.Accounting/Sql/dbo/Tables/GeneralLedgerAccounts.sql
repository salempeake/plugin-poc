CREATE TABLE GeneralLedgerAccounts (
    Id NUMERIC IDENTITY(1,1) PRIMARY KEY,
    ChartId VARCHAR(50) NOT NULL REFERENCES ChartOfAccounts(Id),
    AccountNumber VARCHAR(20) NOT NULL,
    AccountName VARCHAR(255) NOT NULL,
    Description VARCHAR(2000) NOT NULL,
    AccountType INT NOT NULL, -- 1 Asset, 2 Liability, 3 Equity, 4 Expense, 5 Revenue
    RegularBalance INT NOT NULL,
    InactiveDate DATETIME2 DEFAULT NULL
)