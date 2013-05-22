using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumboardSPA.Controllers
{
    using System.IO;
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// Dynamically creates a manifest file containing all files in the specified includeDirectories
    /// </summary>
    public class ManifestResult : FileResult
    {
        private readonly string version;
        private readonly HttpServerUtilityBase server;

        public ManifestResult(string version, HttpServerUtilityBase server) : base("text/cache-manifest")
        {
            this.version = version;
            this.server = server;
        }

        protected override void WriteFile(HttpResponseBase response)
        {

            
            response.Output.WriteLine("CACHE MANIFEST");

            // Increase version number if any files listed in this manifest file has changed
            response.Output.WriteLine("#v" + version);

            response.Output.WriteLine();
            response.Output.WriteLine("CACHE:");

            // Write all script and css files from bundles:
            foreach (Bundle bundle in BundleTable.Bundles)
            {
                IEnumerable<string> bundleScripts = BundleResolver.Current.GetBundleContents(bundle.Path);
                foreach (string bundleScript in bundleScripts)
                {
                    response.Output.WriteLine(bundleScript.Replace("~", string.Empty));
                }
            }

            // views:
            // they need to be explicitly defined, because these are their routes for the controller
            // which does not correspond to the file location (.cshtml ending).
            // a solution would be to keep these views as html files in a sepperate directory and not
            // delivering them via controller.
            response.Output.WriteLine("/views/NewStoryView");
            response.Output.WriteLine("/views/StoryDetailView");
            response.Output.WriteLine("/views/ScrumboardView");

            response.Output.WriteLine();
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