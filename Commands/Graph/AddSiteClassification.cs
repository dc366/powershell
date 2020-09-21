﻿using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Add, "PnPSiteClassification")]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Directory_ReadWrite_All)]
    public class AddSiteClassification : PnPGraphCmdlet
    {

        [Parameter(Mandatory = true)]
        public List<string> Classifications;

        protected override void ExecuteCmdlet()
        {
            try
            {
                var settings = PnP.Framework.Graph.SiteClassificationsUtility.GetSiteClassificationsSettings(AccessToken);
                foreach (var classification in Classifications)
                {
                    if (!settings.Classifications.Contains(classification))
                    {
                        settings.Classifications.Add(classification);
                    }
                }
                PnP.Framework.Graph.SiteClassificationsUtility.UpdateSiteClassificationsSettings(AccessToken, settings);
            }
            catch (ApplicationException ex)
            {
                if (ex.Message == @"Missing DirectorySettingTemplate for ""Group.Unified""")
                {
                    WriteError(new ErrorRecord(new InvalidOperationException("Site Classification is not enabled for this tenant"), "SITECLASSIFICATION_NOT_ENABLED", ErrorCategory.ResourceUnavailable, null));
                } else
                {
                    throw;
                }
            }
        }
    }
}