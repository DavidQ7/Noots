﻿using Common.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WriteAPI.ConstraintsUploadFiles;

namespace WriteAPI.ControllerConfig
{
    public static class ControllerExtensions
    {
        public static string GetUserEmail(this ControllerBase controller)
        {
            var email = controller.User.Claims.FirstOrDefault(x => x.Type.Contains("emailaddress"))?.Value;
            return email;
        }

        public static OperationResult<T> ValidateFile<T>(
            this ControllerBase controller, 
            IFormFile file, 
            List<string> contentTypes,
            long? maximumAllowableFileSize = null)
        {
            var size = maximumAllowableFileSize.HasValue ? maximumAllowableFileSize.Value : FileSizeConstraints.MaxRequestFileSize;
            if (file.Length > size)
            {
                return new OperationResult<T>().SetFileSizeTooLarge();
            }

            if (!contentTypes.Contains(file.ContentType))
            {
                return new OperationResult<T>().SetNoSupportExtension();
            }

            return new OperationResult<T>(success: true, default(T));
        }

    }
}