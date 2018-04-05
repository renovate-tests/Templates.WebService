using System;

namespace Axoom.MyService.Contacts
{
    /// <summary>
    /// Used to report service-specific metrics.
    /// </summary>
    public interface IContactMetrics
    {
        /// <summary>
        /// Counts occurances of an event.
        /// </summary>
        void Poke();

        /// <summary>
        /// Starts a timer for the duration of a job. Place inside a using statement.
        /// </summary>
        IDisposable TimerWrite();
    }
}