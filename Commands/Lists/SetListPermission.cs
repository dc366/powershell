﻿using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Lists
{
    //TODO: Create Test
    [Cmdlet(VerbsCommon.Set, "PnPListPermission")]
    public class SetListPermission : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = ParameterAttribute.AllParameterSets)]
        public ListPipeBind Identity;

        [Parameter(Mandatory = true, ParameterSetName = "Group")]
        public GroupPipeBind Group;

        [Parameter(Mandatory = true, ParameterSetName = "User")]
        public string User;

        [Parameter(Mandatory = false)]
        public string AddRole = string.Empty;

        [Parameter(Mandatory = false)]
        public string RemoveRole = string.Empty;

        protected override void ExecuteCmdlet()
        {
            var list = Identity.GetList(SelectedWeb);

            if (list != null)
            {
                Principal principal = null;
                if (ParameterSetName == "Group")
                {
                    if (Group.Id != -1)
                    {
                        principal = SelectedWeb.SiteGroups.GetById(Group.Id);
                    }
                    else if (!string.IsNullOrEmpty(Group.Name))
                    {
                        principal = SelectedWeb.SiteGroups.GetByName(Group.Name);
                    }
                    else if (Group.Group != null)
                    {
                        principal = Group.Group;
                    }
                }
                else
                {
                    principal = SelectedWeb.EnsureUser(User);
                    ClientContext.ExecuteQueryRetry();
                }
                if (principal != null)
                {
                    if (!string.IsNullOrEmpty(AddRole))
                    {
                        var roleDefinition = SelectedWeb.RoleDefinitions.GetByName(AddRole);
                        var roleDefinitionBindings = new RoleDefinitionBindingCollection(ClientContext);
                        roleDefinitionBindings.Add(roleDefinition);
                        var roleAssignments = list.RoleAssignments;
                        roleAssignments.Add(principal, roleDefinitionBindings);
                        ClientContext.Load(roleAssignments);
                        ClientContext.ExecuteQueryRetry();
                    }
                    if (!string.IsNullOrEmpty(RemoveRole))
                    {
                        var roleAssignment = list.RoleAssignments.GetByPrincipal(principal);
                        var roleDefinitionBindings = roleAssignment.RoleDefinitionBindings;
                        ClientContext.Load(roleDefinitionBindings);
                        ClientContext.ExecuteQueryRetry();
                        foreach (var roleDefinition in roleDefinitionBindings.Where(roleDefinition => roleDefinition.Name == RemoveRole))
                        {
                            roleDefinitionBindings.Remove(roleDefinition);
                            roleAssignment.Update();
                            ClientContext.ExecuteQueryRetry();
                            break;
                        }
                    }
                }
                else
                {
                    WriteError(new ErrorRecord(new Exception("Principal not found"), "1", ErrorCategory.ObjectNotFound, null));
                }
            }
        }
    }
}
