﻿using System.Web;
using System.Web.Optimization;

namespace SQLi.NoSql.API.MongoR
{
    public class BundleConfig
    {
        // Pour plus d'informations sur le regroupement, visitez http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilisez la version de développement de Modernizr pour le développement et l'apprentissage. Puis, une fois
            // prêt pour la production, utilisez l'outil de génération (bluid) sur http://modernizr.com pour choisir uniquement les tests dont vous avez besoin.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            //bundles.Add(new ScriptBundle("~/bundles/js/template").Include(
            //           "~/Content/admin/*js",
            //           "~/Content/global/*js"));
            //bundles.Add(new ScriptBundle("~/bundles/css/template").Include(
            //         "~/Content/admin/*css",
            //           "~/Content/global/*css"));

            bundles.Add(new ScriptBundle("~/bundles/autre").Include("~/Scripts/fastclick.js",
                                                                    "~/Scripts/nprogress.js",
                                                                    "~/Scripts/raphael.js",
                                                                    "~/Scripts/morris.js",
                                                                    "~/Scripts/custom.js"
                                                                    ));

            bundles.Add(new ScriptBundle("~/bundles/chart").Include("~/Scripts/Chart.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                         "~/Content/bootstrap.css",
                         //"~/Content/site.css",
                         "~/Content/font-awesome.min.css",
                         "~/Content/nprogress.css",
                         "~/Content/green.css",
                         "~/Content/custom.css"));
        }
    }
}
