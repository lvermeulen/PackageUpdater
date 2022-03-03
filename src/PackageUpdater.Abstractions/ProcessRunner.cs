using System.Diagnostics;
using System.Threading.Tasks;

namespace PackageUpdater.Abstractions
{
    public class ProcessRunner
    {
        public async Task<RunResult> RunProcessAsync(string processName, string arguments, string workingDirectory)
        {
            var startInfo = new ProcessStartInfo(processName, arguments)
            {
                UseShellExecute = false,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using var process = Process.Start(startInfo);
            if (process == null)
            {
                return null;
            }

            string result = await process.StandardOutput.ReadToEndAsync();
            result += await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            return new RunResult
            {
                ExitCode = process.ExitCode,
                Output = result
            };
        }
    }

    public class RunResult
    {
        public int ExitCode { get; set; }
        public string Output { get; set; }
    }
}