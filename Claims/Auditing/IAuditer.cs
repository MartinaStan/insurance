namespace Claims.Auditing
{
    /// <summary>
    /// Records audit entries for claim and cover operations.
    /// </summary>
    public interface IAuditer
    {
        /// <summary>
        /// Records an audit entry for an HTTP operation performed on a claim.
        /// </summary>
        void AuditClaim(string id, string httpRequestType);

        /// <summary>
        /// Records an audit entry for an HTTP operation performed on a cover.
        /// </summary>
        void AuditCover(string id, string httpRequestType);
    }
}