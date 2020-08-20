using System.IO;
using SkiaSharp;

namespace GameTrove.Application.Infrastructure
{
    public static class ImageExtensions
    {
        public static byte[] AsByteArray(this Stream stream)
        {
            using MemoryStream output = new MemoryStream();

            stream.CopyTo(output);

            return output.ToArray();
        }
        public static Stream ResizeImage(this Stream imageStream, int size)
        {
            imageStream.Seek(0, SeekOrigin.Begin);

            using MemoryStream memoryStream = new MemoryStream();

            imageStream.CopyTo(memoryStream);

            return new MemoryStream(memoryStream.ToArray().ResizeImage(size));
        }

        public static byte[] ResizeImage(this byte[] imageBytes, int size)
        {
            if (imageBytes.Length == 0)
            {
                return null;
            }

            using var input = new MemoryStream(imageBytes);

            return input.ResizeImageStream(size);
        }

        public static byte[] ResizeImage(this string path, int size)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            using var input = File.OpenRead(path);

            return input.ResizeImageStream(size);
        }

        private static byte[] ResizeImageStream(this Stream imageStream, int size)
        {
            using var inputStream = new SKManagedStream(imageStream);
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