﻿using System.Web;
using System.Web.Optimization;

namespace ScrumboardSPA
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/bootstrap-responsive.css")
                .Include("~/Content/site.css"));
            
            bundles.Add(new ScriptBundle("~/bundles/3rdParty")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/underscore.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularJs")
                .Include("~/Scripts/angular.js"));

            bundles.Add(new ScriptBundle("~/bundles/app")
                .Include("~/App/appModule.js")
                .IncludeDirectory("~/App/viewmodels", "*.js")
                .IncludeDirectory("~/App/services", "*.js"));
        }
    }
}