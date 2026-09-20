namespace Claims.Auditing
{
    /// <summary>
    /// An in-memory queue of audit entries waiting to be persisted.
    /// </summary>
    public interface IAuditQueue
    {
        /// <summary>Adds an audit entry to the queue</summary>
        void Enqueue(object auditEntry);

        /// <summary>Waits for and removes the next audit entry from the queue</summary>
        ValueTask<object> DequeueAsync(CancellationToken cancellationToken);
    }
}