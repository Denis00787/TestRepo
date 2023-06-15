using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OauthWebAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class DownloadController:ControllerBase
	{
		public DownloadController()
		{
		}

        [HttpGet(Name = "Download")]
        public async Task<IActionResult> GetFileById()
        {
            string path = "\\path\\to\\the\\file.txt";


        if (System.IO.File.Exists(path))
            {
                return File(System.IO.File.OpenRead(path), "application/octet-stream", Path.GetFileName(path));
            }
            return NotFound();
        }
    }
}

