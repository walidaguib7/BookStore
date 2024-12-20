using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models.media;
using api.models.media.dtos;

namespace api.Services
{
    public interface IMedia
    {
        public Task<Media> CreateMediaFile(CreateFileDto file);
        public Task<string> UploadFile(IFormFile file);
        public Task<List<string>> UploadFiles(IFormFileCollection files, string userId);
        public Task<Media?> DeleteFile(int id);
        public Task<Media?> UpdateFile(int id, IFormFile file);
    }
}