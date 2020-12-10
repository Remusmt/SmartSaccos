namespace SmartSaccos.Domains.Enums
{
    public enum UserType
    {
        Member,
        Admin
    }

    public enum ActionType
    {
        Create,
        Update,
        Delete
    }

    public enum ApprovalAction
    {
        Approved,
        OnHold,
        Rejected
    }

    public enum RecordType
    {
        CostCenter,
        CurrencyConversion,
        LedgerAccount,
        Member
    }
    public enum Gender
    {
        Female,
        Male,
        Other
    }

    public enum MaritalStatus
    {
        Single,
        Married,
        Divorced,
        widowed
    }

    public enum MemberStatus
    {
        Entered,
        KycPersonal,
        KycDocs,
        kycPassport,
        PaidMembership,
        Active,
        OnHold,
        Rejected,
        Left
    }

    public enum AttachmentType
    {
        IdFront,
        IdBack,
        Avator,
        PassportCopy,
        Signature,
        Other
    }

    public enum OccupationType
    {
        Employed,
        SelfEmployed,
        EmployedInBussiness
    }

    public enum LearntAboutUs
    {
        Member,
        Friend,
        Website,
        Advertisement
    }

    public enum MainAccountType
    {
        Asset,
        Liability,
        Equity,
        Income,
        Expense
    }
    public enum AccountType
    {
        ALL = 0,
        /// <summary>
        /// Balance sheet current asset sub category in the asset category
        /// Under cash and cash equivalents
        /// </summary>
        Cash,
        /// <summary>
        /// Balance sheet current asset sub category in the asset category
        /// </summary>
        CurrentAsset,
        /// <summary>
        /// Balance sheet current asset sub category in the asset category
        /// </summary>
        OtherAsset,
        /// <summary>
        /// Balance sheet current asset sub category in the asset category
        /// </summary>
        AccountsReceivable,
        /// <summary>
        /// Balance sheet current asset sub category in the asset category
        /// </summary>
        Inventory,
        /// <summary>
        /// Balance sheet long term asset sub category in the asset category
        /// </summary>
        FixedAsset,
        /// <summary>
        /// Balance sheet long term asset sub category in the asset category
        /// It's shown as a negative value to reduce the value of depreciated asset,
        /// so as to reflect the current value of the long term assets.
        /// </summary>
        AccumulatedDepreciation,
        /// <summary>
        /// Balance sheet sub category of current liabilities in the liabilities sections
        /// These are liabilities that are to be paid in the next 12 months.
        /// </summary>
        CurrentLiability,
        /// <summary>
        /// Balance sheet sub category of accounts payable in the liabilities sections
        /// These are liabilities that are to be paid in the next 12 months.
        /// </summary>
        AccountsPayable,
        /// <summary>
        /// Balance sheet sub category of long term liabilities in the liabilities sections
        /// </summary>
        LongTermLiability,
        /// <summary>
        /// Balance sheet's equity category.
        /// This is the networth of the company
        /// </summary>
        Equity,
        /// <summary>
        /// Used for accounts that are used to record companies primary income source
        /// </summary>
        Income,
        /// <summary>
        /// Used for secondary income
        /// it is added to operating profit to get pretax income on the income statement
        /// </summary>
        OtherIncome,
        /// <summary>
        /// Accounts used to record cost of goods
        /// </summary>
        CostofGoods,
        /// Used for all expenses that are subtracted to gross profit to get
        /// opearating profit on income statement
        /// </summary>
        Expense,
        /// <summary>
        /// Use for accounts used only for non operating expenses
        /// These are subtracted from operating profit to get pretax income
        /// </summary>
        OtherExpense
    }

    public enum DetailAccountType
    {
        Bank,
        PettyCash,
        MobileMoney
    }

    public enum SubLedger
    {
        /// <summary>
        /// Records all the credit sales transactions and 
        /// payments received from a customer against credit sales.
        /// </summary>
        AccountsReceivable,
        /// <summary>
        /// Records all the credit purchases and payments to creditors.
        /// </summary>
        AccountsPayable,
        /// <summary>
        /// It records all transaction data for individual fixed assets
        /// </summary>
        FixedAssets,
        /// <summary>
        /// contain transaction about the receipt of raw material, 
        /// Movement of stock, conversion into finished stock, scrap, or absolute inventory.
        /// </summary>
        InventoryLedger,
        /// <summary>
        /// Records all types of purchases, whether it was paid or to be paid.
        /// </summary>
        PurchaseLedger,
        /// <summary>
        /// Records all types of sales, whether it is cash sales or credit sales
        /// </summary>
        SalesLedger,
        /// <summary>
        /// Cash sales, cash purchase, and expenses paid in cash
        /// </summary>
        CashLedger
    }

}
