using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using GuestBook.Domain.Models;
using GuestBook.Domain.Services;
using GuestBook.Domain.Services.Exceptions;
using GuestBook.Domain.Services.Messages;
using GuestBook.Repository;
using Xtremecode.Infrastructure.Common;
using Xtremecode.Infrastructure.Persistence;

namespace GuestBook.Service
{
    class GuestBookService : IGuestBookService
    {
        private const int MAX_ALLOWED_IMAGE_SIZE = 5 * 1024 * 1024;
        private readonly IGuestBookEntryRepository _guestBookRepository;
        private readonly IGuestBookImageRepository _guestBookImageRepository;

        public GuestBookService(IGuestBookEntryRepository guestBookRepository, IGuestBookImageRepository guestBookImageRepository)
        {
            _guestBookRepository = guestBookRepository;
            _guestBookImageRepository = guestBookImageRepository;
        }

        public SubmitGuestBookEntryResponse SubmitGuestBookEntry(SubmitGuestBookEntryRequest request)
        {
            var response = new SubmitGuestBookEntryResponse();
            Stream imageStream = null;

            try
            {
                var newGuestBookEntry = new GuestBookEntry
                    {
                        GuestName = request.Name,
                        Comment = request.Comment,
                        PhotoUrl = request.ImageFile != null ? request.ImageFile.FileName : string.Empty
                    };
                // Validate model
                validateModel(newGuestBookEntry);

                imageStream = request.ImageFile != null ? request.ImageFile.InputStream : null;
                if (imageStream != null)
                {
                    // Resize image to <= maximum image's file size setting
                    resizeImageToMaximumSize(ref imageStream);

                    // Upload image file into cloud's file storage
                    imageStream.Seek(0, SeekOrigin.Begin);
                    // This is an IMPORTANT ONE TO DO! Or else, calling the S3's Upload Utility method would return IOException later.
                    var imageUrl = _guestBookImageRepository.UploadImageFile(request.ImageFile.ContentType, imageStream);

                    // Create new entry in cloud's table storage 
                    newGuestBookEntry.PhotoUrl = imageUrl;
                }
                _guestBookRepository.Add(newGuestBookEntry);

                // All went well
                response.IsSuccess = true;
            }
            catch (Exception exception)
            {
                // TODO: Log the exception's message
                //                ErrorItem errorItem = new ErrorItem(exception);
                var errorItem = new ErrorItem(new SubmitGuestBookEntryException(exception));
                response.Errors.Add(errorItem);
            }
            finally
            {
                if (imageStream != null)
                {
                    imageStream.Dispose();
                }
            }

            return response;
        }

        public GetCurrentGuestBookEntriesResponse GetCurrentGuestBookEntries(GetCurrentGuestBookEntriesRequest request)
        {
            var response = new GetCurrentGuestBookEntriesResponse();
            try
            {
                // Pull top 10-20 records from SimpleDB
                IEnumerable<GuestBookEntry> guestBookEntries = _guestBookRepository.FindLatestRecords(request.Top);

                // Pull the file from S3
                foreach (var guestBookEntry in guestBookEntries)
                {
                    var imageStream = !string.IsNullOrWhiteSpace(guestBookEntry.PhotoUrl) ? _guestBookImageRepository.FindByKey(guestBookEntry.PhotoUrl) : null;
                    if (imageStream != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            imageStream.CopyTo(memoryStream);
                            memoryStream.Seek(0, SeekOrigin.Begin);
                            guestBookEntry.Photo = memoryStream.ToArray();
                        }
                        imageStream.Dispose();
                    }
                    else
                    {
                        // instead of assign it with zero byte array, let's assign it with default pic 
                        using (var memoryStream = new MemoryStream())
                        {
                            Resource.anonymous_user.Save(memoryStream, ImageFormat.Jpeg);
                            memoryStream.Seek(0, SeekOrigin.Begin);
                            guestBookEntry.Photo = memoryStream.ToArray();
                        }
                    }
                }

                response.Result = guestBookEntries;
                response.IsSuccess = true;
            }
            catch (Exception exception)
            {
                // TODO: Log the exception's message 
                var errorItem = new ErrorItem(exception);
                response.Errors.Add(errorItem);
            }

            return response;
        }


        private void validateModel<TModel>(TModel model) where TModel : EntityBase
        {
            var validationContext = new ValidationContext(model);
            Validator.ValidateObject(model, validationContext);
        }

        private void resizeImageToMaximumSize(ref Stream imageStream)
        {
            var copiedImageStream = new MemoryStream();
            imageStream.CopyTo(copiedImageStream);

            while (copiedImageStream.Length > MAX_ALLOWED_IMAGE_SIZE)
            {
                var copiedImage = Bitmap.FromStream(copiedImageStream);
                var copiedImageNewSize = new Size((int)(copiedImage.Width * 0.99), (int)(copiedImage.Height * 0.99));
                var resizedCopiedImage = new Bitmap(copiedImage, copiedImageNewSize);

                copiedImageStream.Dispose();
                copiedImageStream = new MemoryStream();
                resizedCopiedImage.Save(copiedImageStream, ImageFormat.Jpeg);

                copiedImage.Dispose();
                resizedCopiedImage.Dispose();
            }

            if (copiedImageStream.Length < imageStream.Length)
            {
                //imageStream.Dispose();
                imageStream = copiedImageStream;
            }
        }
    }
}
