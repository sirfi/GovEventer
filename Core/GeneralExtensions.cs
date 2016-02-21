using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace GovEventer.Core
{
    public static class GeneralExtensions
    {
        public static void RegisterAreaRoutes<T>(this RouteCollection routes) where T : AreaRegistration, new()
        {
            var areaRegistration = new T();
            AreaRegistrationContext context = new AreaRegistrationContext(areaRegistration.AreaName, routes);
            string str = areaRegistration.GetType().Namespace;
            if (str != null)
            {
                context.Namespaces.Add(str + ".*");
            }
            areaRegistration.RegisterArea(context);
        }

        public static string XmlPost(string address, string xmlData)
        {
            try
            {
                using (var wUpload = new WebClient())
                {
                    wUpload.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    var bPostArray = Encoding.ASCII.GetBytes(xmlData);
                    var bResponse = wUpload.UploadData(address, "POST", bPostArray);
                    var sReturnChars = Encoding.ASCII.GetChars(bResponse);
                    var sWebPage = new string(sReturnChars);
                    return sWebPage;
                }
            }
            catch
            {
                return null;
            }
        }


        public static Dictionary<T, string> GetEnumList<T>()
        {
            var type = typeof(T);
            if (!EnumHelper.IsValidForEnumHelper(type))
            {
                throw new Exception("Tip uygun değil");
            }
            var dictionary = new Dictionary<T, string>();
            var type2 = Nullable.GetUnderlyingType(type) ?? type;
            if (type2 != type)
            {
                dictionary.Add(default(T), string.Empty);
            }
            foreach (var info in type2.GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                dictionary.Add((T)info.GetRawConstantValue(), GetDisplayName(info));
            }
            return dictionary;
        }
        public static string GetEnumNameForNullable<T>(this T? value) where T : struct
        {
            if (value == null) return "";
            return value.Value.GetEnumName();
        }
        public static string GetEnumName<T>(this T value) where T : struct
        {
            var enumList = GetEnumList<T>();
            if (enumList.ContainsKey(value))
                return enumList[value];
            return value.ToString();
        }

        private static string GetDisplayName(MemberInfo field)
        {
            var customAttribute = field.GetCustomAttribute<DisplayAttribute>(false);
            if (customAttribute != null)
            {
                var name = customAttribute.GetName();
                if (!string.IsNullOrEmpty(name))
                {
                    return name;
                }
            }
            return field.Name;
        }

        public static string ReFormat(this string unformatedString, params object[] args)
        {
            try
            {
                return string.Format(unformatedString, args);
            }
            catch (Exception)
            {
                return unformatedString;
            }
        }

        public static string ToSlug(this string text)
        {
            var slugText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
            var chars = new[] { 'ı', 'ş', 'ğ', 'ö', 'ü', 'ç', 'İ', 'Ş', 'Ğ', 'Ö', 'Ü', 'Ç' };
            var newChars = new[] { 'i', 's', 'g', 'o', 'u', 'c', 'I', 'S', 'G', 'O', 'U', 'C' };
            for (var i = 0; i < chars.Length; i++)
            {
                slugText = slugText.Replace(chars[i], newChars[i]);
            }
            slugText = Regex.Replace(slugText, @"[^a-zA-Z0-9\s-]", "");
            slugText = Regex.Replace(slugText, @"\s+", " ").Trim();
            slugText = Regex.Replace(slugText, @"\s", "-");
            return slugText;
        }

        public static string ToCyrillic(this string text)
        {
            var cyrillic = Encoding.GetEncoding("Cyrillic");
            var utf8 = Encoding.UTF8;
            var utf8Bytes = utf8.GetBytes(text);
            var cyrillicBytes = Encoding.Convert(utf8, cyrillic, utf8Bytes);
            return cyrillic.GetString(cyrillicBytes);
        }
        public static string Hash(this string input)
        {
            var hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
        }

        public static bool VerifyHash(this string input, string hash)
        {
            return input.Hash().Equals(hash, StringComparison.OrdinalIgnoreCase);
        }

        public static SelectList TreeSelectList<T>(this IEnumerable<T> enumerable, string dataValueField, string dataTextField, object selectedValue = null)
        {
            var list = enumerable.ToList();
            var tType = typeof(T);
            var tParentProperty = tType.GetProperties().FirstOrDefault(y => y.PropertyType == tType);
            var tValueProperty = tType.GetProperty(dataValueField);
            var tTextProperty = tType.GetProperty(dataTextField);
            var dictionary = new Dictionary<string, string>();
            if (tParentProperty != null && tValueProperty != null && tTextProperty != null)
            {
                TreeSelectListAction(list, tParentProperty, dictionary, 0, tTextProperty, tValueProperty, null);
            }
            var selectList = new SelectList(dictionary, "Value", "Key", selectedValue);
            return selectList;
        }

        private static void TreeSelectListAction<T>(IEnumerable<T> list, PropertyInfo tParentProperty, Dictionary<string, string> dictionary, int i,
            PropertyInfo tTextProperty, PropertyInfo tValueProperty, object value)
        {
            var enumerable = list as IList<T> ?? list.ToList();
            foreach (var t in enumerable.Where(x => tParentProperty.GetValue(x) == value))
            {
                dictionary.Add(new string('█', 2 * i) + " ↳" + tTextProperty.GetValue(t),
                    tValueProperty.GetValue(t).ToString());
                TreeSelectListAction(enumerable, tParentProperty, dictionary, i + 1, tTextProperty, tValueProperty, t);
            }
        }

        public const string PageNumberKey = "p";
        public static IPagedCollection<T> Paging<T>(this IQueryable<T> queryable, int? pageNumber = null, int pageItemCount = 20, string title = "")
        {
            var context = HttpContext.Current;
            if (context != null)
            {
                int pn;
                var mvcHandler = context.CurrentHandler as MvcHandler;
                if (context.Request.Params.AllKeys.Contains(PageNumberKey) && context.Request.Params[PageNumberKey] != null)
                {
                    if (int.TryParse(context.Request.Params[PageNumberKey], out pn) && pn > 0)
                    {
                        pageNumber = pn;
                    }
                }
                else if (mvcHandler != null &&
                         (mvcHandler.RequestContext.RouteData.Values.Keys.Contains(PageNumberKey) &&
                          mvcHandler.RequestContext.RouteData.Values[PageNumberKey] != null))
                {
                    if (int.TryParse(mvcHandler.RequestContext.RouteData.Values[PageNumberKey].ToString(), out pn) && pn > 0)
                    {
                        pageNumber = pn;
                    }
                }
            }
            return new PagedCollection<T>(queryable, pageItemCount, pageNumber ?? 1, title);
        }

        public static void ImageResizeAndSave(this HttpContextBase context, string fileInputName, params ImageSavingInfo[] imageSavingInfos)
        {
            var image = WebImage.GetImageFromRequest(fileInputName);
            if (image == null) return;
            context.ImageResizeAndSave(image, imageSavingInfos);
        }

        public static void ImageResizeAndSave(this HttpContextBase context, WebImage image, params ImageSavingInfo[] imageSavingInfos)
        {
            foreach (var imageSavingInfo in imageSavingInfos)
            {
                var cloneImage = image.Clone();
                cloneImage.ImageResize(imageSavingInfo);
                var savingPath = context.Server.MapPath(imageSavingInfo.SavingVirtualPath);
                cloneImage.Save(savingPath, imageSavingInfo.SavingFormat);
            }
        }

        public static WebImage ImageResize(this WebImage webImage, ImageSavingInfo imageSavingInfo)
        {
            return webImage.ImageResize(new Size(imageSavingInfo.MaxWidth, imageSavingInfo.MaxHeight),
                imageSavingInfo.Crop);
        }


        public static WebImage ImageResize(this WebImage webImage, Size size, bool crop)
        {
            if (size.Width > webImage.Width || size.Height > webImage.Height) return webImage;
            if (webImage.Width > webImage.Height && size.Width < size.Height)
            {
                var width = (int)(webImage.Width * ((size.Height * 1.0) / webImage.Height));
                webImage.Resize(width, size.Height);
                if (crop)
                    webImage.Crop(0, ((width - size.Width) / 2), 0, ((width - size.Width) / 2));
            }
            else if (webImage.Width < webImage.Height && size.Width > size.Height)
            {
                var height = (int)(webImage.Height * ((size.Width * 1.0) / webImage.Width));
                webImage.Resize(size.Width, height);
                if (crop)
                    webImage.Crop(((height - size.Height) / 2), 0,
                        ((height - size.Height) / 2));
            }
            else if (webImage.Width > webImage.Height && size.Width > size.Height)
            {
                var height = (int)(webImage.Height * ((size.Width * 1.0) / webImage.Width));
                webImage.Resize(size.Width, height);
                if (height <= size.Height) return webImage;
                if (crop)
                    webImage.Crop((height - size.Height) / 2, 0,
                        (height - size.Height) / 2);
            }
            else
            {
                var width = (int)(webImage.Width * ((size.Height * 1.0) / webImage.Height));
                webImage.Resize(width, size.Height);
                if (width <= size.Width) return webImage;
                if (crop)
                    webImage.Crop(0, (width - size.Width) / 2, 0, (width - size.Width) / 2);
            }
            return webImage;
        }

        public static bool IsDateTimeBetween(this DateTime dateTime, DateTime? startDateTime, DateTime? endDateTime)
        {
            if (startDateTime == null) startDateTime = DateTime.MinValue;
            if (endDateTime == null) endDateTime = DateTime.MaxValue;
            if (startDateTime < endDateTime)
            {
                return startDateTime <= dateTime && dateTime <= endDateTime;
            }
            return false;
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            var i = 0;
            foreach (var t in enumerable)
            {
                action(t, i);
                i++;
            }
        }

        public static string ReplaceHtmlTags(this string s)
        {
            return Regex.Replace(s, @"<(.|\n)*?>", string.Empty);
        }

        public static void CopyItemsTo(this IDictionary<string, object> source, IDictionary<string, object> destination)
        {
            foreach (var item in source)
            {
                destination.Add(item.Key, item.Value);
            }
        }
    }

    public class ImageSavingInfo
    {
        public ImageSavingInfo(string savingVirtualPath, string savingFormat, int maxWidth, int maxHeight, bool crop = false)
        {
            SavingVirtualPath = savingVirtualPath;
            SavingFormat = savingFormat;
            MaxWidth = maxWidth;
            MaxHeight = maxHeight;
            Crop = crop;
        }

        public string SavingVirtualPath { get; set; }

        public string SavingFormat { get; set; }

        public int MaxWidth { get; set; }

        public int MaxHeight { get; set; }

        public bool Crop { get; set; }
    }
}