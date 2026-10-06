using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSandbox
{
    public class InstancceExistenceMonitor
    {
        static List<Func<bool>> monitors = new List<Func<bool>>();
        static InstancceExistenceMonitor()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    monitors.RemoveAll(x => !x());
                }
            });
            var monitor = new InstancceExistenceMonitor();
        }

        public void BeginMonitor<T>(T obj) where T : class
        {
            var weakRef = new WeakReference<T>(obj);
            var hashCode = obj.GetHashCode();
            Console.WriteLine($"Instance of {typeof(T).FullName} is start monitoring. {hashCode} - {obj}\n {GetCallStack()}");
            monitors.Add(() =>
            {
                T target;
                var isLeave = weakRef.TryGetTarget(out target);
                if (!isLeave)
                {
                    Console.WriteLine($"Instance of {typeof(T).FullName} has been garbage collected. {hashCode}");
                }
                return isLeave;
            });
        }

        private string GetCallStack()
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            var frames = stackTrace.GetFrames();
            if (frames == null)
            {
                return string.Empty;
            }

            var callStack = string.Empty;
            foreach (var frame in frames)
            {
                var method = frame.GetMethod();
                callStack += $"   at {method.DeclaringType.FullName}.{method.Name}";
                callStack += Environment.NewLine;
            }
            return callStack;
        }
    }
}
