using System.IO;
using SkiaSharp;

namespace api.Infrastructure
{
    public static class ImageExtensions
    {
        public static byte[] ResizeImage(this string path, int size)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            using var input = File.OpenRead(path);
            using var inputStream = new SKManagedStream(input);
            using var original = SKBitmap.Decode(inputStream);
            int width, height;

            if (original.Width > original.Height)
            {
                width = size;
                height = original.Height * size / original.Width;
            }
            else
            {
                width = original.Width * size / original.Height;
                height = size;
            }

            using var resized =
                original.Resize(new SKImageInfo(width, height), SKFilterQuality.High);

            if (resized == null) return null;

            using var image = SKImage.FromBitmap(resized);


            return image.Encode(SKEncodedImageFormat.Jpeg, 75).ToArray();
        }
    }
}