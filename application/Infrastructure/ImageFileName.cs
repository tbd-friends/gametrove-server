using System;

namespace GameTrove.Application.Infrastructure
{
    public class ImageFileName
    {
        public static string GetGameImageFile(Guid imageId, Guid gameId, ImageSize size)
        {
            string name = $"{gameId}_{imageId}";

            switch (size)
            {
                case ImageSize.Thumb:
                    name = $"{name}_tm";
                    break;
                case ImageSize.Small:
                    name = $"{name}_sm";
                    break;
                case ImageSize.Medium:
                    name = $"{name}_md";
                    break;
                case ImageSize.Large:
                    name = $"{name}";
                    break;
            }

            return $"{name}.jpg".ToLower();
        }
    }
}