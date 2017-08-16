using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Zu.Chrome
{
    //https://stackoverflow.com/questions/2316596/system-diaganostics-process-id-isnt-the-same-process-id-shown-in-task-manager?noredirect=1&lq=1
    public class ProcessWithJobObject
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateJobObject(IntPtr lpJobAttributes, string lpName);

        [DllImport("kernel32.dll")]
        public static extern bool AssignProcessToJobObject(IntPtr hJob, IntPtr hProcess);

        [DllImport("kernel32.dll")]
        public static extern bool TerminateJobObject(IntPtr hJob, uint uExitCode);

        IntPtr job;

        public Process StartProc(string path, string commandLine = null)
        {
            if (job == IntPtr.Zero)
                job = CreateJobObject(IntPtr.Zero, null);
            ProcessStartInfo si = new ProcessStartInfo(path);
            if (!string.IsNullOrWhiteSpace(commandLine)) si.Arguments = /*"/c " +*/ commandLine;
            //si.CreateNoWindow = false;
            si.UseShellExecute = false;
            Process proc = Process.Start(si);
            AssignProcessToJobObject(job, proc.Handle);
            return proc;
        }

        public void TerminateProc()
        {
            // terminate the Job object, which kills all processes within it
            if (job != null)
                TerminateJobObject(job, 0);
            job = IntPtr.Zero;
        }
    }
}
