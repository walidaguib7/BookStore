using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models.media;
using api.models.media.Mapping;
using api.Services;
using api.utils;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/media")]
    public class MediaController(IMedia _mediaService) : ControllerBase
    {
        private readonly IMedia mediaService = _mediaService;

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                var resource = await mediaService.UploadFile(file);
                if (resource == null) return NotFound();
                var extension = Path.GetExtension(resource);
                if (extension == ".pdf" || extension == ".docx")
                {
                    var model = await mediaService.CreateMediaFile(new models.media.dtos.CreateFileDto
                    {
                        file = resource,
                        type = "document"
                    });
                    return Ok(model);
                }
                else
                {
                    var model = await mediaService.CreateMediaFile(new models.media.dtos.CreateFileDto
                    {
                        file = resource,
                        type = "image"
                    });
                    return Ok(model);
                }


            }
            catch (ValidationException e)
            {
                return BadRequest(new ValidationErrorResponse { Errors = e.Errors.Select(e => e.ErrorMessage) });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("{userId}")]
        public async Task<IActionResult> UploadMultiFiles(IFormFileCollection formFiles, [FromRoute] string userId)
        {
            try
            {
                List<string> files = await mediaService.UploadFiles(formFiles, userId);
                ICollection<Media> models = [];
                foreach (string file in files)
                {
                    try
                    {
                        var model = await mediaService.CreateMediaFile(new models.media.dtos.CreateFileDto
                        {
                            file = file,
                            type = "image"
                        });
                        models.Add(model);
                    }
                    catch (ValidationException e)
                    {
                        return BadRequest(new ValidationErrorResponse { Errors = e.Errors.Select(e => e.ErrorMessage) });
                    }
                    catch (Exception e)
                    {
                        return BadRequest(e.Message);
                    }
                }
                var mediaModel = models.Select(m => m.ToDto());
                return Ok(mediaModel);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteFile([FromRoute] int id)
        {
            var file = await mediaService.DeleteFile(id);
            if (file == null) return NotFound();
            return Ok("File deleted!");
        }

        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateFile([FromRoute] int id, IFormFile file)
        {
            var model = await mediaService.UpdateFile(id, file);
            if (model == null) return NotFound("File Not Found!");
            return Ok("File updated!");
        }
    }
}