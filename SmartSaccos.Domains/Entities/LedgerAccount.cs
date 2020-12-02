using SmartSaccos.Domains.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartSaccos.Domains.Entities
{
    public class LedgerAccount : AppBaseEntity
    {
        public LedgerAccount()
        {
            ChildAccounts = new HashSet<LedgerAccount>();
        }
        public AccountType AccountType { get; set; }
        public DetailAccountType DetailAccountType { get; set; }
        [StringLength(50)]
        public string AccountNumber { get; set; }
        [StringLength(150)]
        public string AccountName { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Determines how deep the record is from the root parent
        /// </summary>
        public byte Height { get; set; }
        /// <summary>
        /// For Bank, Account Receivable, Accounts Payable
        /// The rest should have the home currency as currency
        /// </summary>
        public int CurrencyId { get; set; }
        /// <summary>
        /// Should be of the same type, and have same currency
        /// </summary>
        public int? ParentAccountId { get; set; }
        /// <summary>
        /// For expense accounts
        /// </summary>
        public int? TaxRateId { get; set; }
        public decimal Balance { get; set; }
        public decimal CurrencyBalance { get; set; }
        [NotMapped]
        public decimal TotalBalance { get; set; }
        [NotMapped]
        public decimal TotalCurrencyBalance { get; set; }

        public Currency Currency { get; set; }
        public LedgerAccount ParentAccount { get; set; }

        public ICollection<LedgerAccount> ChildAccounts { get; set; }

        [StringLength(50)]
        public string BankAccountNo { get; set; }
        public bool HasOverDraft { get; set; }
        public decimal OverDraftLimit { get; set; }
        public bool ShowInPettyCash { get; set; }
        public bool AddToDashboard { get; set; }

        public MainAccountType MainAccountType
        {
            get
            {
                switch (AccountType)
                {
                    case AccountType.Cash:
                    case AccountType.CurrentAsset:
                    case AccountType.OtherAsset:
                    case AccountType.AccountsReceivable:
                    case AccountType.Inventory:
                    case AccountType.FixedAsset:
                    case AccountType.AccumulatedDepreciation:
                        return MainAccountType.Asset;
                    case AccountType.CurrentLiability:
                    case AccountType.AccountsPayable:
                    case AccountType.LongTermLiability:
                        return MainAccountType.Liability;
                    case AccountType.Equity:
                        return MainAccountType.Equity;
                    case AccountType.Income:
                    case AccountType.OtherIncome:
                        return MainAccountType.Income;
                    case AccountType.CostofGoods:
                    case AccountType.Expense:
                    case AccountType.OtherExpense:
                        return MainAccountType.Expense;
                    default:
                        return MainAccountType.Expense;
                }
            }
        }

    }
}
