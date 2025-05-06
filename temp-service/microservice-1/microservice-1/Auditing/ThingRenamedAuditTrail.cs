namespace microservice_1.Auditing
{
    internal class ThingRenamedAuditTrail : ThingAuditTrail
    {
        public string PreviousName { get; set; }

        public ThingRenamedAuditTrail(long? thingId, string newName, string previousName, bool succeeded):base(AuditExtensions.ActionUpdated, thingId, newName, succeeded)
        {
            PreviousName = previousName;
        }
    }
}