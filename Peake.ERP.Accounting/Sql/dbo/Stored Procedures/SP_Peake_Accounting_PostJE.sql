CREATE OR ALTER PROCEDURE SP_Peake_Accounting_PostJE (
    @JournalEntryId NUMERIC
)
AS
    BEGIN
       INSERT INTO GeneralLedger(EntityId, EntryDate, GlAccountId, TransactionType, TransactionReference, JournalEntryId, Description, Debit, Credit)
        SELECT
            EntityId = je.EntityId
            , EntryDate = GETDATE()
            , GlAccountId = jed.AccountId
            , TransactionType = 'Peake.Accounting.Entities.JournalEntry'
            , TransactionReference = je.Reference
            , JournalEntryId = je.Id
            , Description = je.Description
            , Debit = jed.Debit
            , Credit = jed.Credit
        FROM JournalEntry je
            INNER JOIN dbo.JournalEntryDetail jed on je.Id = jed.JournalEntryId
        WHERE 1=1
        /* Debit and Credit are Equal */
            AND (SELECT SUM(Debit) FROM JournalEntryDetail WHERE JournalEntryId = @JournalEntryId) = (SELECT SUM(Credit) FROM JournalEntryDetail WHERE JournalEntryId = @JournalEntryId)
        /* No Other GL records have the same JournalEntryId */
            AND NOT EXISTS (SELECT 1 FROM GeneralLedger WHERE JournalEntryId = @JournalEntryId)
        /* JE Header Id = @JournalEntryId */
            AND je.Id = @JournalEntryId
    END