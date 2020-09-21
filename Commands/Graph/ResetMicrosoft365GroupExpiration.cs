﻿#if !PNPPSCORE
using PnP.Framework.Graph;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Reset, "PnPMicrosoft365GroupExpiration")]
    [Alias("Reset-PnPUnifiedGroupExpiration")]
    [CmdletHelp("Renews the Microsoft 365 Group by extending its expiration with the number of days defined in the group expiration policy set on the Azure Active Directory",
        DetailedDescription = "Renews the Microsoft 365 Group by extending its expiration with the number of days defined in the group expiration policy set on the Azure Active Directory",
        Category = CmdletHelpCategory.Graph)]
    
    [CmdletRelatedLink(Text = "Documentation", Url = "https://docs.microsoft.com/graph/api/group-renew")]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Directory_ReadWrite_All | MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class ResetMicrosoft365GroupExpiration : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public Microsoft365GroupPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var group = Identity.GetGroup(AccessToken);
            UnifiedGroupsUtility.RenewUnifiedGroup(group.GroupId, AccessToken);
        }
    }
}
#endif