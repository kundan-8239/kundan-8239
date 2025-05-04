using System.Threading.Tasks;
using Atlas.Auditing;

namespace microservice_1.Auditing
{
    /// <summary>
    /// Extension helper class to generate audit events.
    /// </summary>
    internal static class AuditExtensions
    {
        internal const string ActionCreated = "Created";
        internal const string ActionUpdated = "Updated";
        internal const string ActionDeleted = "Deleted";
        
        public static async Task ThingCreated<T>(this IAuditor<T> auditor, long? thingId, string thingName, bool actionSucceeded) where T : class
        {
            await auditor.LogAsync(new ThingAuditTrail(ActionCreated, thingId, thingName, actionSucceeded));
        }

        public static async Task ThingUpdated<T>(this IAuditor<T> auditor, long thingId, string thingName, bool actionSucceeded) where T : class
        {
            await auditor.LogAsync(new ThingAuditTrail(ActionUpdated, thingId, thingName, actionSucceeded));
        }

        public static async Task ThingRenamed<T>(this IAuditor<T> auditor, long thingId, string oldName, string newName, bool actionSucceeded) where T : class
        {
            await auditor.LogAsync(new ThingRenamedAuditTrail(thingId, newName, oldName, actionSucceeded));
        }

        public static async Task ThingDeleted<T>(this IAuditor<T> auditor, long thingId, string thingName, bool actionSucceeded) where T : class
        {
            await auditor.LogAsync(new ThingAuditTrail(ActionDeleted, thingId, thingName, actionSucceeded));
        }
    }
}
