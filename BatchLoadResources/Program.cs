using System.Collections;
using System;
using System.IO;
using System.Resources;
using System.Drawing;
using System.Drawing.Common;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BatchLoadResources
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            string currentDirectory = Directory.GetCurrentDirectory();
            string propertiesDirectory = Path.Combine(currentDirectory, @"..\..\Properties");
            string imagesDirectory = propertiesDirectory + @"\Resources"; // Change to your images folder
            string resxFile =Path.Combine(propertiesDirectory,"Resources.resx"); // Change to your .resx file
            AddNewResources(imagesDirectory, resxFile, "*.png");

        }
        internal  static void AddNewResources(string sourceFolder, string targetResourceFile, string filePattern)
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
                        existingResources[resourceName] = img;
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
    }
}
