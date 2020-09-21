﻿using PnP.Framework.Enums;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Apps
{
    [Cmdlet(VerbsData.Publish, "PnPApp")]
    public class PublishApp : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public AppMetadataPipeBind Identity;

        [Parameter(Mandatory = false)]
        public SwitchParameter SkipFeatureDeployment;

        [Parameter(Mandatory = false)]
        public AppCatalogScope Scope = AppCatalogScope.Tenant;

        protected override void ExecuteCmdlet()
        {
            var manager = new PnP.Framework.ALM.AppManager(ClientContext);

            var app = Identity.GetAppMetadata(ClientContext, Scope);
            if (app != null)
            {
                manager.Deploy(app, SkipFeatureDeployment, Scope);
            } else
            {
                throw new Exception("Cannot find app");
            }
        }
    }
}