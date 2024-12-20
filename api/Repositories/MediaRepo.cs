using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.models.media;
using api.models.media.dtos;
using api.models.media.Mapping;
using api.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class MediaRepo(
        AppDBContext _context,
        [FromKeyedServices("createFile")] IValidator<CreateFileDto> _mediaValidator
    ) : IMedia
    {
        private readonly AppDBContext context = _context;
        private readonly IValidator<CreateFileDto> mediaValidator = _mediaValidator;
        public async Task<Media> CreateMediaFile(CreateFileDto file)
        {
            ValidationResult result = mediaValidator.Validate(file);
            if (result.IsValid)
            {
                var mediaFile = file.ToModel();
                await context.media.AddAsync(mediaFile);
                await context.SaveChangesAsync();
                return mediaFile;
            }
            else
            {
                throw new ValidationException(result.Errors);
            }
        }

        public async Task<Media?> DeleteFile(int id)
        {
            Media? file = await context.media.Where(m => m.Id == id).FirstAsync();
            if (file == null) return null;
            context.media.Remove(file);
            await context.SaveChangesAsync();
            return file;
        }

        public async Task<Media?> UpdateFile(int id, IFormFile file)
        {
            var model = await context.media.Where(f => f.Id == id).FirstAsync();
            if (model == null) return null;
            var uploadedFile = await UploadFile(file);
            if (uploadedFile == null) return null;
            model.Path = uploadedFile;
            await context.SaveChangesAsync();
            return model;
        }

        public async Task<List<string>> UploadFiles(IFormFileCollection files, string userId)
        {
            if (files == null || files.Count == 0)
            {
                throw new Exception("No files were uploaded.");
            }

            var allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".jfif", "pdf", "docx" };

            var uploadedFileNames = new List<string>();

            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    throw new Exception($"Invalid file format. Only {string.Join(", ", allowedExtensions)} files are allowed.");
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                var filePath = Path.Combine("uploads", uniqueFileName);

                try
                {
                    await using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    uploadedFileNames.Add(uniqueFileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return uploadedFileNames;
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            if (file.Length == 0 || file == null)
            {
                throw new Exception("file Not Found!");
            }
            var AllowedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".jfif", ".pdf", ".docx" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!AllowedExtensions.Contains(extension))
            {
                throw new Exception("Invalid file format. Only JPG, JPEG, and PNG files are allowed.");
            }
            string fileName = Path.GetFileName(file.FileName);
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
            string filePath = Path.Combine("uploads", uniqueFileName);

            try
            {
                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return uniqueFileName;
        }
    }
}