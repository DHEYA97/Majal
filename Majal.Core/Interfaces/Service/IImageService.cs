using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Interfaces.Service
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(string filePath, string subFolder);
        Task<string> UpdateImageAsync(int imageId, string newFilePath, string subFolder);
        Task DeleteImageAsync(string imagePath);
    }
}
