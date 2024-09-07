CREATE TABLE GeneralLedger (
    Id NUMERIC IDENTITY(1,1) PRIMARY KEY,
    EntityId NUMERIC NOT NULL REFERENCES GeneralLedgerEntities(Id),
    EntryDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    GlAccountId NUMERIC REFERENCES GeneralLedgerAccounts(Id),
    TransactionType VARCHAR(255) NOT NULL,
    TransactionReference VARCHAR(255) NOT NULL,
    JournalEntryId NUMERIC NOT NULL REFERENCES JournalEntry(Id),
    Description VARCHAR(255) NOT NULL,
    Debit NUMERIC(18,4) DEFAULT 0,
    Credit NUMERIC(18,4) DEFAULT 0
);