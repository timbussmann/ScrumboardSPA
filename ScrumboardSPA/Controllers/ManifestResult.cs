using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumboardSPA.Controllers
{
    using System.IO;
    using System.Web.Mvc;

    /// <summary>
    /// Dynamically creates a manifest file containing all files in the specified includeDirectories
    /// </summary>
    public class ManifestResult : FileResult
    {
        private readonly string version;
        private readonly HttpServerUtilityBase server;
        private readonly IEnumerable<string> includeDirectories;

        public ManifestResult(string version, HttpServerUtilityBase server, IEnumerable<string> includeDirectories) : base("text/cache-manifest")
        {
            this.version = version;
            this.server = server;
            this.includeDirectories = includeDirectories;
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            response.Output.WriteLine("CACHE MANIFEST");

            // Increase version number if any files listed in this manifest file has changed
            response.Output.WriteLine("#v" + version);

            response.Output.WriteLine("CACHE:");

            // Not optimal since all js files are listed for caching, even if they are not used due to bundling configuration
            IEnumerable<string> cachableFiles = this.includeDirectories.SelectMany(this.GetFilesIn);
            foreach (string cachableFile in cachableFiles)
            {
                response.Output.WriteLine(cachableFile);
            }

            // views:
            // they need to be explicitly defined, because these are their routes for the controller
            // which does not correspond to the file location (.cshtml ending).
            // a solution would be to keep these views as html files in a sepperate directory and not
            // delivering them via controller.
            response.Output.WriteLine("/views/NewStoryView");
            response.Output.WriteLine("/views/StoryDetailView");
            response.Output.WriteLine("/views/ScrumboardView");

            response.Output.WriteLine("NETWORK:");
            response.Output.WriteLine("*");
        }

        private IEnumerable<string> GetFilesIn(string directory)
        {
            string physicalPath = server.MapPath(directory);
            var files = Directory.EnumerateFiles(physicalPath, "*", SearchOption.AllDirectories);
            return files.Select(file => file.Replace(physicalPath, directory).Replace("\\", "/"));
        }
    }
}