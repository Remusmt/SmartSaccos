using SmartSaccos.Domains.Enums;

namespace SmartSaccos.Domains.Entities
{
    public class AuditLog: AppBaseEntity
    {
        /// <summary>
        /// The object name, this can be reflected if not provided
        /// </summary>
        public RecordType RecordType { get; set; }
        /// <summary>
        /// Action represented by log
        /// </summary>
        public ActionType ActionType { get; set; }
        /// <summary>
        /// The Id of the record that affected by the action
        /// </summary>
        public int RecordId { get; set; }
        /// <summary>
        /// This is a json serialized object
        /// </summary>
        public string SerializedRecord { get; set; }
    }
}
