using System.Runtime.CompilerServices;
using System.Threading;

namespace RealTimeShop
{
    // Managed game objects no longer have IL2CPP pointers. Weak, monotonic IDs
    // preserve visit/ownership identity without retaining customers or reusing IDs.
    internal static class ObjectIdentity
    {
        private sealed class Entry
        {
            internal readonly long Value = Interlocked.Increment(ref next);
        }
        private static long next;
        private static readonly ConditionalWeakTable<object, Entry> identities = new();
        internal static long Id(this object? value) => value == null ? 0 : identities.GetValue(value, _ => new Entry()).Value;
    }

    internal static class SaveEncoding
    {
        internal static string Hex(byte[] bytes) => BitConverter.ToString(bytes).Replace("-", "");
        internal static void Replace(string temporary, string destination)
        {
            // Same-volume atomic replacement; never delete the old sidecar first.
            if (File.Exists(destination)) File.Replace(temporary, destination, null);
            else File.Move(temporary, destination);
        }
    }
}

#if NETSTANDARD2_1
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}
#endif
