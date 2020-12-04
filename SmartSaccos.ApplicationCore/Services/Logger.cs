using Newtonsoft.Json;
using SmartSaccos.ApplicationCore.Interfaces;
using SmartSaccos.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartSaccos.ApplicationCore.Services
{
    public class Logger
    {
        private readonly IRepository<AuditLog> auditRepository;
        public Logger(IRepository<AuditLog> auditLogRepository)
        {
            auditRepository = auditLogRepository;
        }

        public async Task<bool> Log(AuditLog log)
        {
            auditRepository.Add(log);
            return await auditRepository.SaveChangesAsync() > 0;
        }

        public void AddLogWithoutSaving(AuditLog log)
        {
            auditRepository.Add(log);
        }

        public IEnumerable<AuditLog> GetAuditLogs(
            int ClientId,
            DateTime dateFrom,
            DateTime dateTo)
        {
            return auditRepository.Find(e =>
                e.CompanyId == ClientId &&
                e.CreatedOn >= dateFrom &&
                e.CreatedOn <= dateTo);
        }

        public string SeliarizeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }
    }
}
