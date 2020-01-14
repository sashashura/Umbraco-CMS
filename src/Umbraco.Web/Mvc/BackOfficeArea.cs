﻿using System.Web.Mvc;
using Umbraco.Web.Composing;
using Umbraco.Core.Configuration;
using Umbraco.Core.IO;
using Umbraco.Web.Editors;

namespace Umbraco.Web.Mvc
{
    /// <summary>
    /// An area registration for back office components
    /// </summary>
    internal class BackOfficeArea : AreaRegistration
    {
        private readonly IGlobalSettings _globalSettings;
        private readonly IIOHelper _ioHelper;

        public BackOfficeArea(IGlobalSettings globalSettings, IIOHelper ioHelper)
        {
            _globalSettings = globalSettings;
            _ioHelper = ioHelper;
        }

        /// <summary>
        /// Create the routes for the area
        /// </summary>
        /// <param name="context"></param>
        /// <remarks>
        /// By using the context to register the routes it means that the area is already applied to them all
        /// and that the namespaces searched for the controllers are ONLY the ones specified.
        /// </remarks>
        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                "Umbraco_preview",
                AreaName + "/preview/{action}/{editor}",
                new {controller = "Preview", action = "Index", editor = UrlParameter.Optional},
                new[] { "Umbraco.Web.Editors" });

            context.MapRoute(
                "Umbraco_back_office",
                AreaName + "/{action}/{id}",
                new {controller = "BackOffice", action = "Default", id = UrlParameter.Optional},
                //limit the action/id to only allow characters - this is so this route doesn't hog all other
                // routes like: /umbraco/channels/word.aspx, etc...
                new
                    {
                        action = @"[a-zA-Z]*",
                        id = @"[a-zA-Z]*"
                    },
                new[] {typeof (BackOfficeController).Namespace});
        }

        public override string AreaName => _globalSettings.GetUmbracoMvcArea(_ioHelper);
    }
}
