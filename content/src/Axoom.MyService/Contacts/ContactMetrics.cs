using System;
using JetBrains.Annotations;
using Nexogen.Libraries.Metrics;
using Nexogen.Libraries.Metrics.Extensions;

namespace Axoom.MyService.Contacts
{
    /// <summary>
    /// Used to report service-specific metrics.
    /// </summary>
    [UsedImplicitly]
    public class ContactMetrics : IContactMetrics
    {
        public ContactMetrics(IMetrics metrics)
        {
            _pokes = metrics.Counter()
                .Name("myservice_contact_pokes")
                .Help("Number of times contacts were poked")
                .Register();
            _writeDurations = metrics.Histogram()
                .Name("myservice_contact_write_time")
                .Help("Average duration of persistence write operations for contacts")
                .Register();
        }
        
        private readonly ICounter _pokes;

        /// <inheritdoc/>
        public IDisposable TimerWrite() => _writeDurations.Timer();
        
        private readonly IHistogram _writeDurations;

        /// <inheritdoc/>
        public void Poke() => _pokes.Increment();
    }
}
