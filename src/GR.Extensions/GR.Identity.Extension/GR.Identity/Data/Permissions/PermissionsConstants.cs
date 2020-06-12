using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GR.Identity.Data.Permissions
{
    public static class PermissionsConstants
    {
        public static class CorePermissions
        {
            #region Roles

            public const string BpmCreateRole = "Core_CreateRole";
            public const string BpmEditRole = "Core_EditRole";
            public const string BpmReadRole = "Core_ReadRole";
            public const string BpmDeleteRole = "Core_DeleteRole";

            #endregion Roles

            #region Groups

            public const string BpmGroupCreate = "Core_GroupCreate";
            public const string BpmGroupRead = "Core_GroupRead";
            public const string BpmGroupUpdate = "Core_GroupUpdate";
            public const string BpmGroupDelete = "Core_GroupDelete";

            #endregion Groups

            #region Users

            public const string BpmUserCreate = "Core_UserCreate";
            public const string BpmUserRead = "Core_UserRead";
            public const string BpmUserUpdate = "Core_UserUpdate";
            public const string BpmUserDelete = "Core_UserDelete";

            #endregion Users

            #region Profiles

            public const string BpmProfileCreate = "Core_ProfileCreate";
            public const string BpmProfileRead = "Core_ProfileRead";
            public const string BpmProfileUpdate = "Core_ProfileUpdate";
            public const string BpmProfileDelete = "Core_ProfileDelete";

            #endregion Profiles

            #region Processes

            public const string BpmProcessCreate = "Core_ProcessCreate";
            public const string BpmProcessUpdate = "Core_ProcessUpdate";
            public const string BpmProcessRead = "Core_ProcessRead";
            public const string BpmProcessDelete = "Core_ProcessDelete";

            #endregion Processes

            #region Entity

            public const string BpmEntityCreate = "Core_EntityCreate";
            public const string BpmEntityUpdate = "Core_EntityUpdate";
            public const string BpmEntityRead = "Core_EntityRead";
            public const string BpmEntityDelete = "Core_EntityDelete";

            #endregion Entity

            #region Forms

            public const string BpmFormCreate = "Core_FormCreate";
            public const string BpmFormUpdate = "Core_FormUpdate";
            public const string BpmFormRead = "Core_FormRead";
            public const string BpmFormDelete = "Core_FormDelete";

            #endregion Forms

            #region Tables

            public const string BpmTableCreate = "Core_TableCreate";
            public const string BpmTableUpdate = "Core_TableUpdate";
            public const string BpmTableRead = "Core_TableRead";
            public const string BpmTableDelete = "Core_TableDelete";

            #endregion Tables
            
            #region Lead

            public const string BpmLeadCreate = "Core_LeadCreate";
            public const string BpmLeadUpdate = "Core_LeadUpdate";
            public const string BpmLeadRead = "Core_LeadRead";
            public const string BpmLeadDelete = "Core_LeadDelete";

            #endregion

            #region Client

            public const string BpmClientCreate = "Core_ClientCreate";
            public const string BpmClientUpdate = "Core_ClientUpdate";
            public const string BpmClientRead = "Core_ClientRead";
            public const string BpmClientDelete = "Core_ClientDelete";

            #endregion

            #region PipeLine

            public const string BpmPipeLineCreate = "Core_PipeLineCreate";
            public const string BpmPipeLineUpdate = "Core_PipeLineUpdate";
            public const string BpmPipeLineRead = "Core_PipeLineRead";
            public const string BpmPipeLineDelete = "Core_PipeLineDelete";

            #endregion

            #region PipeLineStage

            public const string BpmPipeLineStageCreate = "Core_PipeLineStageCreate";
            public const string BpmPipeLineStageUpdate = "Core_PipeLineStageUpdate";
            public const string BpmPipeLineStageRead = "Core_PipeLineStageRead";
            public const string BpmPipeLineStageDelete = "Core_PipeLineStageDelete";

            #endregion

            #region LeadState

            public const string BpmLeadStateCreate = "Core_LeadStateCreate";
            public const string BpmLeadStateUpdate = "Core_LeadStateUpdate";
            public const string BpmLeadStateRead = "Core_LeadStateRead";
            public const string BpmLeadStateDelete = "Core_LeadStateDelete";

            #endregion

            #region DocumentTemplate

            public const string BpmDocumentTemplateCreate = "Core_DocumentTemplateCreate";
            public const string BpmDocumentTemplateUpdate = "Core_DocumentTemplateUpdate";
            public const string BpmDocumentTemplateRead = "Core_DocumentTemplateRead";
            public const string BpmDocumentTemplateDelete = "Core_DocumentTemplateDelete";

            #endregion

            #region Agreement

            public const string BpmAgreementCreate = "Core_AgreementCreate";
            public const string BpmAgreementUpdate = "Core_AgreementUpdate";
            public const string BpmAgreementRead = "Core_AgreementRead";
            public const string BpmAgreementDelete = "Core_AgreementDelete";

            #endregion

        }

        public static IEnumerable<string> PermissionsList(ClientName client = ClientName.All)
        {
            var permissions = new List<string>();

            switch (client)
            {
                case ClientName.Core:// specific core
                    {
                        var corePermissions = typeof(CorePermissions).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                        permissions.AddRange(corePermissions.Select(_ => _.GetValue(null).ToString()).ToList());
                    }
                    break;

                case ClientName.All:
                    {
                        var corePermissions = typeof(CorePermissions).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                        permissions.AddRange(corePermissions.Select(_ => _.GetValue(null).ToString()).ToList());
                    }
                    break;
            }

            return permissions;
        }

        public enum ClientName
        {
            Core,
            All
        }
    }
}