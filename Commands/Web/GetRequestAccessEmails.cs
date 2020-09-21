﻿using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPRequestAccessEmails")]
    public class GetRequestAccessEmails : PnPWebCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            var emails = SelectedWeb.GetRequestAccessEmails();
            WriteObject(emails, true);
        }
    }
}