using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ExoticHistoricSites.Server.Utilities;

public static class ImageConverter
{
    public static async Task<string> ConvertToBase64(IFormFile file, int maxWidth = 300)
    {
        using var image = await Image.LoadAsync(file.OpenReadStream());

        if (image.Width > maxWidth)
        {
            var ratio = (double)maxWidth / image.Width;
            var newHeight = (int)(image.Height * ratio);

            image.Mutate(x => x.Resize(maxWidth, newHeight));
        }

        using var ms = new MemoryStream();
        await image.SaveAsJpegAsync(ms);
        return Convert.ToBase64String(ms.ToArray());
    }

    public static bool IsValidImage(IFormFile file)
    {
        try
        {
            using var image = Image.Load(file.OpenReadStream());
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static async Task<string> ResizeAndConvertToBase64(
        string base64String,
        int maxWidth = 300
    )
    {
        try
        {
            var bytes = Convert.FromBase64String(base64String);
            using var ms = new MemoryStream(bytes);
            using var image = await Image.LoadAsync(ms);

            if (image.Width > maxWidth)
            {
                var ratio = (double)maxWidth / image.Width;
                var newHeight = (int)(image.Height * ratio);

                image.Mutate(x => x.Resize(maxWidth, newHeight));
            }

            using var outputMs = new MemoryStream();
            await image.SaveAsJpegAsync(outputMs);
            return Convert.ToBase64String(outputMs.ToArray());
        }
        catch
        {
            return base64String; // Return original if processing fails
        }
    }
}
