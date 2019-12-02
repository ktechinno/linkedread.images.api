using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using api.images.Models;


namespace api.images.Controllers
{
    [Route("v1/profile-uploads")]
    public class ProfileUploadsController : Controller
    {
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            try
            {
                string subFolder =  Request.Host.Host.ToString() + ".profiles";
                string folderName = Path.Combine("../../resources", subFolder);
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if ( ! Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                if (file.Length > 0){
                    string storeName = Guid.NewGuid().ToString("N");
                    string fullPath = Path.Combine(uploadPath, storeName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var profileImage = new ProfileImage(){
                        storeName = storeName,
                        contentType = file.ContentType
                    };

                    return Ok(profileImage);
                } else {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}