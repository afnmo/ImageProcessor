using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageProcessor.Services;

public class ImageProcessingService
{
    private const int Increment = 5;

    public void ApplyIncremental(string imagePath)
    {
        using Image<Rgba32> image = Image.Load<Rgba32>(imagePath);

        image.ProcessPixelRows(accessor =>
        {
            for (int y = 0; y < accessor.Height; y++)
            {
                Span<Rgba32> row = accessor.GetRowSpan(y);

                for (int x = 0; x < row.Length; x++)
                {
                    ref Rgba32 pixel = ref row[x];

                    pixel.R = Clamp(pixel.R + Increment);
                    pixel.G = Clamp(pixel.G + Increment);
                    pixel.B = Clamp(pixel.B + Increment);
                }
            }
        });

        image.Save(imagePath);
    }

    private static byte Clamp(int value)
        => (byte)Math.Min(255, value);
}
