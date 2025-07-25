using System.Collections;
using System.Resources;

namespace Genie4Lib.Utility;

/// <summary>
/// Warning- this routine will impact the resources of the application.
/// App will have to be rebuilt  after it is done
/// </summary>
public static class BatchLoadResources
{
    public static void LoadImages()
    {
        return;
        throw new Exception("Not supported");
        string currentAppDirectory = Directory.GetCurrentDirectory();
        int binStart = currentAppDirectory.IndexOf("\\bin", StringComparison.OrdinalIgnoreCase);
        string sourceDirectory = currentAppDirectory.Substring(0, binStart);
        int binEnd = sourceDirectory.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase);
        string propertiesDirectory = Path.Combine(sourceDirectory.Substring(0,binEnd), @"Genie4Lib\Properties");
        string imagesDirectory = Path.Combine(sourceDirectory, @"Genie4Lib\Resources"); // Change to your images folder
        string resxFile = Path.Combine(propertiesDirectory, "Resources.resx"); // Change to your .resx file
        AddNewResources(imagesDirectory, resxFile, "*.png");

    }
    internal static void AddNewResources(string sourceFolder, string targetResourceFile, string filePattern)
    {
        // Read existing resources
        var existingResources = new Dictionary<string, object>();
        using (ResXResourceReader reader = new ResXResourceReader(targetResourceFile))
        {
            foreach (DictionaryEntry entry in reader)
            {
                existingResources[entry.Key.ToString()] = entry.Value;
            }
        }

        // Add new images (or overwrite if key exists)
        foreach (string file in Directory.GetFiles(sourceFolder, "*.*", SearchOption.TopDirectoryOnly))
        {
            string ext = Path.GetExtension(file).ToLowerInvariant();
            if (ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".bmp" || ext == ".gif" || ext == ".ico")
            {
                string resourceName = Path.GetFileNameWithoutExtension(file).Replace('-', '_');
                using (Bitmap img = new Bitmap(file))
                {
                    byte[] imgAsBytes = ConvertBitmapToBytes(img);
                    existingResources[resourceName] = imgAsBytes;
                }
            }
        }

        // Write all resources back to the .resx file
        using (ResXResourceWriter writer = new ResXResourceWriter(targetResourceFile))
        {
            foreach (var kvp in existingResources)
            {
                writer.AddResource(kvp.Key, kvp.Value);
            }
        }

        Console.WriteLine("Resources merged and added successfully.");
    }
    internal static byte[] ConvertBitmapToBytes(Image image)
    {
        using (var stream = new MemoryStream())
        {
            image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return stream.ToArray();
        }
    }
}
