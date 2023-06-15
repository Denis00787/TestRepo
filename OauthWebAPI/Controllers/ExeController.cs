using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OauthWebAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ExeController: ControllerBase
	{
		public ExeController()
		{
		}

		[HttpGet(Name = "Execute")]
		public ActionResult<string> ExecuteCommand()
		{
            using (var process = new Process())
            {
                process.StartInfo.FileName = @"..\HelloWorld\bin\Debug\helloworld.exe"; // relative path. absolute path works too.
                //process.StartInfo.Arguments = $"{id}"; // put custom params if you have them
                //process.StartInfo.FileName = @"cmd.exe";
                //process.StartInfo.Arguments = @"/c dir";      // print the current working directory information
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                process.OutputDataReceived += (sender, data) => Console.WriteLine(data.Data);
                process.ErrorDataReceived += (sender, data) => Console.WriteLine(data.Data);
                Console.WriteLine("starting");
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                var exited = process.WaitForExit(1000 * 10);     // (optional) wait up to 10 seconds
                Console.WriteLine($"exit {exited}");
            }
            return "value";

        }
    }
}

