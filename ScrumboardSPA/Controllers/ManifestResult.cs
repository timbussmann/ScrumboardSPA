﻿namespace ScrumboardSPA.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// Dynamically creates a manifest file containing all files in the specified includeDirectories
    /// </summary>
    public class ManifestResult : FileResult
    {
        private readonly HttpServerUtilityBase server;

#if DEBUG
        private static readonly string Version = Guid.NewGuid().ToString();
#else
        private static readonly string Version = "1.0";
#endif

        public ManifestResult(HttpServerUtilityBase server) : base("text/cache-manifest")
        {
            this.server = server;
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            response.Output.WriteLine("CACHE MANIFEST");

            // Increase version number if any files listed in this manifest file has changed
            response.Output.WriteLine("#v" + Version);
            
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
            response.Output.WriteLine("/views/StoryDetailView");
            response.Output.WriteLine("/views/ScrumboardView");
            response.Output.WriteLine("/views/ResolveConflictView");

            response.Output.WriteLine();
            response.Output.WriteLine("NETWORK:");
            response.Output.WriteLine("*");

            response.Output.WriteLine();
            response.Output.WriteLine("FALLBACK:");
            response.Output.WriteLine("/views/NewStoryView /views/OfflineView");
        }

        private IEnumerable<string> GetFilesIn(string directory)
        {
            string physicalPath = server.MapPath(directory);
            var files = Directory.EnumerateFiles(physicalPath, "*", SearchOption.AllDirectories);
            return files.Select(file => file.Replace(physicalPath, directory).Replace("\\", "/"));
        }
    }
}