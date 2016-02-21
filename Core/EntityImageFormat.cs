using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace GovEventer.Core
{
    public class EntityImageFormat
    {
        public static readonly EntityImageFormat Original = new EntityImageFormat("OriginalImage", new Size(1024, 1024), false);
        public static readonly EntityImageFormat Cover = new EntityImageFormat("CoverImage", new Size(1024, 512), true);
        public static readonly EntityImageFormat Icon = new EntityImageFormat("IconImage", new Size(128, 128), true);
        public static readonly IReadOnlyCollection<EntityImageFormat> All = new ReadOnlyCollection<EntityImageFormat>(new[] { Original, Cover, Icon });
        public EntityImageFormat()
        {

        }
        public EntityImageFormat(string name, Size size, bool crop)
            : this()
        {
            Name = name;
            Size = size;
            Crop = crop;
        }
        public string Name { get; set; }
        public Size Size { get; set; }
        public bool Crop { get; set; }
    }
}