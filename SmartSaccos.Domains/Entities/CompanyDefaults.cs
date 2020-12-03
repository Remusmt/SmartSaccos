
namespace SmartSaccos.Domains.Entities
{
    public class CompanyDefaults: AppBaseEntity
    {
        public int DefaultCurrency { get; set; }
        /// <summary>
        /// To allow for users not to enforce account numbers
        /// </summary>
        public bool UseAccountNumbers { get; set; }
        /// <summary>
        /// When true select all from ledgeraccounts table (charts of accounts)
        /// else select accounts not used in parentid column
        /// </summary>
        public bool AllowPostingToParentAccount { get; set; }
        /// <summary>
        /// Gives user a mechanism to prevent transactions falling outside a period
        /// And a way to close books
        /// </summary>
        public bool UseFinancialYear { get; set; }
        public int CurrentFinancialYear { get; set; }
    }
}
