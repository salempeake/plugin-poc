CREATE TABLE JournalEntryDetail (
    Id NUMERIC IDENTITY (1,1) PRIMARY KEY,
    JournalEntryId NUMERIC NOT NULL REFERENCES JournalEntry(Id),
    AccountId NUMERIC NOT NULL REFERENCES GeneralLedgerAccounts(Id),
    Notes VARCHAR(255),
    Debit NUMERIC(18,4) NOT NULL DEFAULT 0,
    Credit NUMERIC(18,4) NOT NULL DEFAULT 0,
);