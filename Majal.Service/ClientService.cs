using Azure;
using MailKit;
using Majal.Core.Abstractions;
using Majal.Core.Abstractions.Const.Errors;
using Majal.Core.Contract;
using Majal.Core.Contract.Client;
using Majal.Core.Entities;
using Majal.Core.Interfaces.Service;
using Majal.Core.Specification.EntitySpecification;
using Majal.Core.UnitOfWork;
using Mapster;
using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using System.Security.Policy;

namespace Majal.Service
{
    public class ClientService(IUnitOfWork unitOfWork,IImageService imageService) : IClientService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IImageService _imageService = imageService;
        public async Task<Result<IEnumerable<ClientResponse>>> GetAllClientAsync(ClientSpecification sp)
        {
            var clients = await _unitOfWork.Repositories<Client>().GetAllWithSpecificationAsync(sp);
            var response = clients.Adapt<IEnumerable<ClientResponse>>();
            return Result.Success(response);
        }
        public async Task<Result<ClientResponse>> GetClientAsync(ClientSpecification sp)
        {
            var client = await _unitOfWork.Repositories<Client>().GetByIdWithSpecificationAsync(sp);
            if (client is null)
                return Result.Failure<ClientResponse>(ClientError.ClientNotFound);
            var response = client.Adapt<ClientResponse>();
            return Result.Success(response);
        }
      
      public async Task<Result> UpdateAsync(ClientRequest request, ClientSpecification sp)
        {
            // بدء المعاملة
            await _unitOfWork.BeginTransactionAsync();
            string url = string.Empty;
            try
            {
                var client = await _unitOfWork.Repositories<Client>().GetByIdWithSpecificationAsync(sp);
                if (client is null)
                    return Result.Failure(ClientError.ClientNotFound);

                client.Name = request.Name;

                // حفظ أو تحديث الصورة
                
                if (!string.IsNullOrWhiteSpace(request.Url))
                {
                    if (!string.IsNullOrWhiteSpace(client.Image.Url))
                    {
                        url = await _imageService.UpdateImageAsync(client.Image.Id, request.Url, nameof(client));
                    }
                    else
                    {
                        url = await _imageService.SaveImageAsync(request.Url, nameof(client));
                    }
                    client.Image.Url = url;
                }

                // تحديث الكائن
                _unitOfWork.Repositories<Client>().Update(client);
                await _unitOfWork.CompleteAsync();

                // تأكيد المعاملة
                await _unitOfWork.CommitTransactionAsync();
                return Result.Success();
            }
            catch (Exception)
            {
                // إلغاء المعاملة في حالة حدوث خطأ
                await _unitOfWork.RollbackTransactionAsync();

                // حذف الصورة في حال حدوث خطأ
                if (!string.IsNullOrWhiteSpace(url))
                {
                    await _imageService.DeleteImageAsync(url);
                }

                return Result.Failure(new Error("حصل خطأ", "حصل خطأ", 404));
            }
        }

        public async Task<Result> AddAsync(ClientRequest request, ClientSpecification sp)
        {
            // بدء المعاملة
            await _unitOfWork.BeginTransactionAsync();
            string url = string.Empty;
            try
            {
                var isClientFound = await _unitOfWork.Repositories<Client>().GetByIdWithSpecificationAsync(sp);
                if (isClientFound is not null)
                    return Result.Failure(ClientError.ClientNotFound);

                var client = new Client
                {
                    Name = request.Name,
                };

                // حفظ الصورة
                if (!string.IsNullOrWhiteSpace(request.Url))
                {
                    url = await _imageService.SaveImageAsync(request.Url, nameof(Client));
                    client.Image = new Image()
                    {
                        Url = url,
                        Type = ImageType.Client
                    };
                }

                // إضافة العميل
                await _unitOfWork.Repositories<Client>().AddAsync(client);
                await _unitOfWork.CompleteAsync();

                // تأكيد المعاملة
                await _unitOfWork.CommitTransactionAsync();
                return Result.Success();
            }
            catch (Exception)
            {
                // إلغاء المعاملة في حالة حدوث خطأ
                await _unitOfWork.RollbackTransactionAsync();

                // حذف الصورة في حال حدوث خطأ
                if (!string.IsNullOrWhiteSpace(url))
                {
                    await _imageService.DeleteImageAsync(url);
                }

                return Result.Failure(new Error("حصل خطأ", "حصل خطأ", 404));
            }
        }

        public async Task<Result> DeleteAsync(ClientSpecification sp)
        {
            var client = await _unitOfWork.Repositories<Client>().GetByIdWithSpecificationAsync(sp);
            if (client is null)
                return Result.Failure(ClientError.ClientNotFound);

            if (!string.IsNullOrWhiteSpace(client.Image.Url))
            {
                await _imageService.DeleteImageAsync(client.Image.Url);
            }
            _unitOfWork.Repositories<Client>().Delete(client);
            await _unitOfWork.CompleteAsync();
            
            return Result.Success();
        }
    }
}
