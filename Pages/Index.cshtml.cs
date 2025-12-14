using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ImageProcessor.Services;

namespace ImageProcessor.Pages;

public class IndexModel : PageModel
{
    private readonly ImageProcessingService _imageService;

    public IndexModel(ImageProcessingService imageService)
    {
        _imageService = imageService;
    }

    public IActionResult OnPostRestore()
    {
        Restore("roger1.jpg");
        Restore("luffy1.jpg");
        return new OkResult();
    }

    public JsonResult OnPostStep()
    {
        string path = GetImagePath("roger1.jpg");
        _imageService.ApplyIncremental(path);
        return ImageResult("roger1.jpg");
    }

    public JsonResult OnPostAuto()
    {
        string path = GetImagePath("luffy1.jpg");
        _imageService.ApplyIncremental(path);
        return ImageResult("luffy1.jpg");
    }


    private void Restore(string file)
    {
        string root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        System.IO.File.Copy(
            Path.Combine(root, "images/originals", file),
            Path.Combine(root, "images", file),
            overwrite: true
        );
    }

    private string GetImagePath(string file)
    {
        return Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot/images",
            file
        );
    }

    private JsonResult ImageResult(string file)
    {
        return new JsonResult(new
        {
            imageUrl = "/images/" + file +
                       "?v=" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        });
    }
}
