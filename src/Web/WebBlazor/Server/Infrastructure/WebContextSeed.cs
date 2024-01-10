
using ILogger = Serilog.ILogger;

namespace WebBlazor.Server.Infrastructure;

public static class WebContextSeed
{
    public static void Seed(IApplicationBuilder applicationBuilder, IWebHostEnvironment env)
    {
        ArgumentNullException.ThrowIfNull(applicationBuilder);
        ArgumentNullException.ThrowIfNull(env);

        var log = Log.Logger;

        var settings = applicationBuilder.ApplicationServices.GetRequiredService<IOptions<AppSettings>>().Value;

        var useCustomizationData = settings.UseCustomizationData;
        var contentRootPath = env.ContentRootPath;
        var webroot = env.WebRootPath;

        if (useCustomizationData)
        {
            GetPreconfiguredImages(contentRootPath, webroot, log);
        }
    }

    static void GetPreconfiguredImages(string contentRootPath, string webroot, ILogger log)
    {
        try
        {
            var imagesZipFile = Path.Combine(contentRootPath, "Setup", "images.zip");
            if (!File.Exists(imagesZipFile))
            {
                log.Error("Zip file '{ZipFileName}' does not exists.", imagesZipFile);
                return;
            }

            var imagePath = Path.Combine(webroot, "assets", "images");
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }
            var imageFiles = Directory.GetFiles(imagePath).Select(Path.GetFileName).ToArray();

            using var zip = ZipFile.Open(imagesZipFile, ZipArchiveMode.Read);
            foreach (var entry in zip.Entries)
            {
                if (!imageFiles.Contains(entry.Name))
                {
                    var destinationFilename = Path.Combine(imagePath, entry.Name);
                    if (File.Exists(destinationFilename))
                    {
                        File.Delete(destinationFilename);
                    }
                    entry.ExtractToFile(destinationFilename);
                }
                else
                {
                    log.Warning("Skipped file '{FileName}' in zipfile '{ZipFileName}'", entry.Name, imagesZipFile);
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex, "ERROR in GetPreconfiguredImages: {Message}", ex.Message);
        }
    }
}
