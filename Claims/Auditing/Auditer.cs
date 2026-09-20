namespace Claims.Auditing
{
    /// <summary>
    /// Records audit entries by enqueuing them for asynchronous background persistence
    /// </summary>
    public class Auditer : IAuditer
    {
        private readonly IAuditQueue _auditQueue;

        public Auditer(IAuditQueue auditQueue)
        {
            _auditQueue = auditQueue;
        }

        public void AuditClaim(string id, string httpRequestType)
        {
            var claimAudit = new ClaimAudit()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                ClaimId = id
            };

            _auditQueue.Enqueue(claimAudit);
        }

        public void AuditCover(string id, string httpRequestType)
        {
            var coverAudit = new CoverAudit()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                CoverId = id
            };

            _auditQueue.Enqueue(coverAudit);
        }
    }
}