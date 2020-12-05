using Microsoft.AspNetCore.Http;
using SmartSaccos.Domains.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SmartSaccos.MemberPortal.Helpers
{
    public static class FileHelper
    {
        public static Attachment CreateAttachment(
            this IFormFile formFile, 
            Member member, 
            string rootPath)
        {
            var fileName = WebUtility.HtmlEncode(
                Path.GetFileName(formFile.FileName));
            if (!formFile.ContentType.ToLower().Contains("image"))
            {
                throw new Exception($"The file ({fileName}) must be an  image.");
            }
            if (formFile.Length == 0)
            {
                throw new Exception($"The file ({fileName}) is empty.");
            }
            Attachment attachment = new Attachment
            {
                CompanyId = member.CompanyId,
                ContentDisposition = formFile.ContentDisposition,
                ContentType = formFile.ContentType,
                CreatedBy = member.ApplicationUserId,
                CreatedByName = $"{member.OtherNames} {member.Surname}",
                CreatedOn = DateTimeOffset.UtcNow,
                Extension = Path.GetExtension(formFile.FileName),
                FileName = formFile.FileName,
                Length = formFile.Length,
                Name = formFile.Name,
                RootPath = rootPath,
                SystemFileName = Guid.NewGuid().ToString()
            };
            return attachment;
        }

        public async static void SaveFile(this IFormFile formFile, string filePath)
        {
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await formFile.CopyToAsync(fileStream);
        }

        public static void DeleteFile(this string filePath)
        {
            File.Delete(filePath);
        }
    }
}
