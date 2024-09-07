CREATE TABLE JournalEntry (
    Id NUMERIC IDENTITY (1,1) PRIMARY KEY,
    EntityId NUMERIC REFERENCES GeneralLedgerEntities(Id),
    JournalDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    ReverseDate DATETIME2,
    Reference VARCHAR(255),
    Description VARCHAR(2000),
    
);