using Majal.Core.Entities;
using Majal.Core.Interfaces.Service;
using Majal.Core.Specification.EntitySpecification;
using Majal.Core.UnitOfWork;
using Majal.Repository.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Service
{
    public class ImageService(IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork) : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private const long MaxFileSize = 2 * 1024 * 1024; // 5 MB

        public async Task<string> SaveImageAsync(string filePath, string subFolder)
        {
            var fileExtension = Path.GetExtension(filePath).ToLower();
            if (!_allowedExtensions.Contains(fileExtension))
            {
                throw new InvalidOperationException("امتداد الملف غير مسموح.");
            }

            // التحقق من حجم الملف
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Length > MaxFileSize)
            {
                throw new InvalidOperationException("حجم الملف يتجاوز الحد المسموح به (2 ميجابايت).");
            }

            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, subFolder);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // توليد GUID واستخدام الامتداد الأصلي للملف
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var destinationPath = Path.Combine(folderPath, fileName);

            // نسخ الملف إلى المسار الجديد بشكل غير متزامن
            using (var sourceStream = new FileStream(filePath, FileMode.Open))
            using (var destinationStream = new FileStream(destinationPath, FileMode.Create))
            {
                await sourceStream.CopyToAsync(destinationStream);
            }

            return Path.Combine(subFolder, fileName); // إرجاع مسار الملف الجديد

        }

        // حذف الصورة
        public async Task DeleteImageAsync(string imagePath)
        {
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath);
            if (File.Exists(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath));  // حذف الصورة بشكل غير متزامن
            }
        }

        // تحديث الصورة
        public async Task<string> UpdateImageAsync(int imageId, string newFilePath, string subFolder)
        {
            var image = await _unitOfWork.Repositories<Image>().GetByIdWithSpecificationAsync(new ImageSpecification(imageId));
            if (image is null)
            {
                throw new FileNotFoundException("Image not found.");
            }

            // حذف الصورة القديمة
            await DeleteImageAsync(image.Url);

            // حفظ الصورة الجديدة
           return await SaveImageAsync(newFilePath, subFolder);
        }

    }

}
