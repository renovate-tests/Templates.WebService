using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MorseCode.ITask;

namespace MyVendor.MyService
{
    public static class AssertionExtensions
    {
        [Pure]
        public static Func<Task> Awaiting<T>(this T subject, Func<T, ITask> action)
            => () => action(subject).AsTask();
    }
}
