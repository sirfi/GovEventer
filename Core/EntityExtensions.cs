using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using GovEventer.Models;

namespace GovEventer.Core
{
    public static class EntityExtensions
    {
        public const string EntityImageFileExtension = ".image";
        public const string EntityImageSeparator = "_";
        public const string EntityImageDirectoryPath = "~/Content/EntityImages/";

        public static void ImageSave(this BaseEntity entity, string sourceImagePath, string name, bool crop = true)
        {
            ImageSave(entity, sourceImagePath, new Size(1024, 768), name, crop);
        }

        public static void ImageSave(this BaseEntity entity, string sourceImagePath, EntityImageFormat entityImageFormat)
        {
            entityImageFormat = entityImageFormat ?? EntityImageFormat.Original;
            ImageSave(entity, sourceImagePath, entityImageFormat.Size, entityImageFormat.Name, entityImageFormat.Crop);
        }

        public static void ImageSave(this BaseEntity entity, string sourceImagePath, params EntityImageFormat[] entityImageFormats)
        {
            if (entityImageFormats.Length == 0)
            {
                entityImageFormats = EntityImageFormat.All.ToArray();
            }
            foreach (var entityImageFormat in entityImageFormats)
            {
                ImageSave(entity, sourceImagePath, entityImageFormat);
            }
        }

        public static void ImageSave(this BaseEntity entity, string sourceImagePath, int width, int height, string name, bool crop = true)
        {
            ImageSave(entity, sourceImagePath, new Size(width, height), name, crop);
        }

        public static void ImageSave(this BaseEntity entity, string sourceImagePath, int widthAndHeight, string name, bool crop = true)
        {
            ImageSave(entity, sourceImagePath, new Size(widthAndHeight, widthAndHeight), name, crop);
        }

        public static void ImageSave(this BaseEntity entity, string sourceImagePath, Size size, string name, bool crop = true)
        {
            HttpContextBase context = new HttpContextWrapper(HttpContext.Current);
            var destinationImagePath = entity.GetImagePath(name);
            var destinationImageRealPath = context.Server.MapPath(destinationImagePath);
            sourceImagePath = (sourceImagePath.StartsWith("/") || sourceImagePath.StartsWith("~")) ? sourceImagePath : ("/" + sourceImagePath);
            sourceImagePath = sourceImagePath.StartsWith("~") ? sourceImagePath : ("~" + sourceImagePath);
            var sourceImageRealPath = context.Server.MapPath(sourceImagePath);
            var sourceImage = new WebImage(sourceImageRealPath);
            sourceImage.ImageResize(size, crop).Save(destinationImageRealPath, forceCorrectExtension: false);
        }

        public static void ImageDelete(this BaseEntity entity)
        {
            var entityImageFormats = EntityImageFormat.All.ToArray();
            entity.ImageDelete(entityImageFormats);
        }

        public static void ImageDelete(this BaseEntity entity, params EntityImageFormat[] entityImageFormats)
        {
            foreach (var entityImageFormat in entityImageFormats)
            {
                entity.ImageDelete(entityImageFormat);
            }
        }

        public static void ImageDelete(this BaseEntity entity, EntityImageFormat entityImageFormat)
        {
            entity.ImageDelete(entityImageFormat.Name);
        }

        public static void ImageDelete(this BaseEntity entity, string name)
        {
            HttpContextBase context = new HttpContextWrapper(HttpContext.Current);
            var path = context.Server.MapPath(entity.GetImagePath(name));
            if (!File.Exists(path)) return;
            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public static string GetImagePath(this BaseEntity entity)
        {
            return GetImagePath(entity, EntityImageFormat.Original);
        }

        public static string GetImagePath(this BaseEntity entity, EntityImageFormat entityImageFormat)
        {
            return entity.GetImagePath(entityImageFormat.Name);
        }

        public static string GetImagePath(this BaseEntity entity, string name)
        {
            return
                EntityImageDirectoryPath +
                (entity.GetType().FullName.Contains("Dynamic") ? entity.GetType().BaseType.Name : entity.GetType().Name) +
                EntityImageSeparator +
                name +
                EntityImageSeparator +
                entity.Id +
                EntityImageFileExtension;
        }
    }
}