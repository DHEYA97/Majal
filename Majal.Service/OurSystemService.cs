using Azure;
using Majal.Core.Abstractions;
using Majal.Core.Abstractions.Const.Errors;
using Majal.Core.Contract;
using Majal.Core.Contract.Client;
using Majal.Core.Contract.OurSystem;
using Majal.Core.Entities;
using Majal.Core.Interfaces.Service;
using Majal.Core.Specification.EntitySpecification;
using Majal.Core.UnitOfWork;
using Mapster;
using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;

namespace Majal.Service
{
    public class OurSystemService(IUnitOfWork unitOfWork,IImageService imageService) : IOurSystemService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IImageService _imageService = imageService;

        public async Task<Result<IEnumerable<OurSystemsResponse>>> GetAllSystemAsync(OurSystemSpecification sp)
        {
            var systmes = await _unitOfWork.Repositories<OurSystem>().GetAllWithSpecificationAsync(sp);
            var response = systmes.Adapt<IEnumerable<OurSystemsResponse>>();
            return Result.Success(response);
        }
        public async Task<Result<OurSystemResponse>> GetSystemByIdAsync(OurSystemSpecification sp)
        {
            var systme = await _unitOfWork.Repositories<OurSystem>().GetByIdWithSpecificationAsync(sp);
            if (systme is null) { 
                 return Result.Failure<OurSystemResponse>(OurSystemError.OurSystemNotFound);
            }
            var response = systme.Adapt<OurSystemResponse>();
            return Result.Success(response);
        }

        public async Task<Result> AddAsync(OurSystemRequest request, OurSystemSpecification sp)
        {
            await _unitOfWork.BeginTransactionAsync();
            List<string> savedImages = new List<string>();
            try
            {

                var isOurSystemFound = await _unitOfWork.Repositories<OurSystem>().GetByIdWithSpecificationAsync(sp);
                if (isOurSystemFound is not null)
                    return Result.Failure(OurSystemError.OurSystemFound);

                if (string.IsNullOrEmpty(request.MainImage))
                    return Result.Failure(OurSystemError.OurSystemNotFound);

                var url = await _imageService.SaveImageAsync(request.MainImage, nameof(OurSystem));
                savedImages.Add(url);
                var ourSystem = new OurSystem
                {
                    Name = request.Name,
                    MainContentMedia = request.MainContentMedia,
                    Content = request.Content,
                    DemoUrl = request.DemoUrl,
                    Image = new Image()
                    {
                        Url = url,
                        Type = ImageType.System
                    },
                    HasDemo = !string.IsNullOrEmpty(request.DemoUrl),
                    Features = new List<Feature>(),
                    SystemImages = new List<SystemImage>()
                };

                if (request.Features?.Count() > 0)
                {
                    request.Features.ForEach(f =>
                            ourSystem.Features.Add(new Feature { Content = f }));
                }
                if (request.SystemImages?.Count > 0)
                {
                    await Task.WhenAll(request.SystemImages.Select(async (img) =>
                    {
                        var imageUrl = await _imageService.SaveImageAsync(img, nameof(OurSystem));
                        savedImages.Add(imageUrl);
                        ourSystem.SystemImages.Add(new SystemImage
                        {
                            Image = new Image
                            {
                                Url = imageUrl
                            }
                        });
                    }));
                }
                await _unitOfWork.Repositories<OurSystem>().AddAsync(ourSystem);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return Result.Success();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync(); // إلغاء المعاملة عند حدوث خطأ
                foreach (var imagePath in savedImages)
                {
                    await _imageService.DeleteImageAsync(imagePath); // حذف الصور المحفوظة
                }
                return Result.Failure(new Error("حصل خطاء", "حصل خطاء",404));
            }
        }
        public async Task<Result> UpdateAsync(OurSystemRequest request, OurSystemSpecification sp)
        {
            await _unitOfWork.BeginTransactionAsync();
            List<string> savedImages = new List<string>();
            List<string> DeletedImages = new List<string>();
            try
            {

                var OurSystem = await _unitOfWork.Repositories<OurSystem>().GetByIdWithSpecificationAsync(sp);
                if (OurSystem is null)
                    return Result.Failure(OurSystemError.OurSystemNotFound);

                OurSystem = request.Adapt(OurSystem);

                if (!string.IsNullOrEmpty(request.MainImage))
                {
                    var url = await _imageService.SaveImageAsync(request.MainImage, nameof(OurSystem));
                    savedImages.Add(url);
                    var oldImage = OurSystem.Image;
                    OurSystem.Image = new Image()
                    {
                        Url = url,
                        Type = ImageType.System
                    };

                    DeletedImages.Add(oldImage.Url);
                    await _imageService.DeleteImageAsync(oldImage.Url);
                    _unitOfWork.Repositories<Image>().Delete(oldImage);
                }

                if (request.Features?.Count() > 0)
                {
                    var currentFeatures = OurSystem.Features.Select(x=>x.Content).ToList();
                    var newFeatures = request.Features.ToList();

                    currentFeatures.ForEach(f =>
                        _unitOfWork.Repositories<Feature>().Delete(OurSystem.Features.First(x=>x.Content == f))
                    );

                    newFeatures.ForEach(f =>
                            OurSystem.Features.Add(new Feature { Content = f })
                    );
                }
                if (request.SystemImages?.Count > 0)
                {
                    var currentSystemImages = await _unitOfWork.Repositories<SystemImage>()
                                                    .GetAllWithSpecificationAsync(new SystemImageSpecification(OurSystem.Id));

                    var currentImages = currentSystemImages.Select(si => si.Image).ToList();


                    await Task.WhenAll(request.SystemImages.Select(async (img) =>
                    {
                        var imageUrl = await _imageService.SaveImageAsync(img, nameof(OurSystem));
                        savedImages.Add(imageUrl);
                        OurSystem.SystemImages.Add(new SystemImage
                        {
                            Image = new Image
                            {
                                Url = imageUrl
                            }
                        });
                    }));

                    foreach(var sysImage in currentSystemImages)
                    {
                        _unitOfWork.Repositories<SystemImage>().Delete(sysImage);
                    }

                    foreach (var image in currentImages)
                    {
                        DeletedImages.Add(image.Url);
                        await _imageService.DeleteImageAsync(image.Url);
                        _unitOfWork.Repositories<Image>().Delete(image);
                    }
                }
                
                OurSystem.HasDemo = !string.IsNullOrEmpty(OurSystem.DemoUrl);
                _unitOfWork.Repositories<OurSystem>().Update(OurSystem);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return Result.Success();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync(); // إلغاء المعاملة عند حدوث خطأ
                
                foreach (var imagePath in DeletedImages)
                {
                    await _imageService.SaveImageAsync(imagePath,nameof(OurSystem)); // حذف الصور المحفوظة
                }

                foreach (var imagePath in savedImages)
                {
                    await _imageService.DeleteImageAsync(imagePath); // حذف الصور المحفوظة
                }
                
                return Result.Failure(new Error("حصل خطاء", "حصل خطاء", 404));
            }
        }
        public async Task<Result> DeleteAsync(OurSystemSpecification sp)
        {
            await _unitOfWork.BeginTransactionAsync();
            List<string> DeletedImages = new List<string>();
            try
            {

                var OurSystem = await _unitOfWork.Repositories<OurSystem>().GetByIdWithSpecificationAsync(sp);
                if (OurSystem is null)
                    return Result.Failure(OurSystemError.OurSystemNotFound);

                DeletedImages.Add(OurSystem.Image.Url);
                await _imageService.DeleteImageAsync(OurSystem.Image.Url);

                var currentSystemImages = await _unitOfWork.Repositories<SystemImage>()
                                                    .GetAllWithSpecificationAsync(new SystemImageSpecification(OurSystem.Id));

                var currentImages = currentSystemImages.Select(si => si.Image).ToList();

                foreach (var sysImage in currentSystemImages)
                {
                    _unitOfWork.Repositories<SystemImage>().Delete(sysImage);
                }

                foreach (var image in currentImages)
                {
                    DeletedImages.Add(image.Url);
                    await _imageService.DeleteImageAsync(image.Url);
                    _unitOfWork.Repositories<Image>().Delete(image);
                }

                var currentFeatures = OurSystem.Features.Select(x => x.Content).ToList();
                currentFeatures.ForEach(f =>
                    _unitOfWork.Repositories<Feature>().Delete(OurSystem.Features.First(x => x.Content == f))
                );

                _unitOfWork.Repositories<OurSystem>().Delete(OurSystem);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return Result.Success();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync(); // إلغاء المعاملة عند حدوث خطأ

                foreach (var imagePath in DeletedImages)
                {
                    await _imageService.SaveImageAsync(imagePath, nameof(OurSystem)); // حذف الصور المحفوظة
                }

                return Result.Failure(new Error("حصل خطاء", "حصل خطاء", 404));
            }
        }
    }
}
