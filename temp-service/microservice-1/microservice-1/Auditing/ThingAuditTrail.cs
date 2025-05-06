using Atlas.Auditing;

namespace microservice_1.Auditing
{
    internal class ThingAuditTrail : AuditTrailLog
    {
        public string Action { get; }

        public long? ThingId { get; }

        public string ThingName { get; }

        public bool Succeeded { get; }

        public ThingAuditTrail(string action, long? thingId, string thingName, bool succeeded)
        {
            Action = action;
            ThingId = thingId;
            ThingName = thingName;
            Succeeded = succeeded;
        }
    }
}
