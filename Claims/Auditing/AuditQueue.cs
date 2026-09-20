using System.Threading.Channels;

namespace Claims.Auditing
{
    /// <summary>
    /// Channel-based in-memory implementation of <see cref="IAuditQueue"/>.
    /// </summary>
    public class AuditQueue : IAuditQueue
    {
        private readonly Channel<object> _channel = Channel.CreateUnbounded<object>();

        public void Enqueue(object auditEntry)
        {
            _channel.Writer.TryWrite(auditEntry);
        }

        public async ValueTask<object> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _channel.Reader.ReadAsync(cancellationToken);
        }
    }
}