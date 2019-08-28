using System;
using System.Collections.Generic;
using Structurizr;
using Structurizr.Api;

namespace StructurizrObjects
{
    public class ApiWorkspacePersistenceStrategy : IWorkspacePersistenceStrategy
    {
        private readonly long _workSpaceId;
        private readonly StructurizrClient _structurizrClient;

        public ApiWorkspacePersistenceStrategy(long workSpaceId, string apiKey, string apiSecret)
        {
            _workSpaceId = workSpaceId;
            _structurizrClient = new StructurizrClient(apiKey, apiSecret);
        }

        public Workspace GetWorkspace()
        {
            try
            {
                return _structurizrClient.GetWorkspace(_workSpaceId);
            }
            catch (KeyNotFoundException ex)
            {
                throw new Exception("The Workspace is corrupt. Please delete &nd re-create.", ex);
            }
        }

        public void PersistWorkspace(Workspace workspace, bool merge = false)
        {
            if (!merge)
            {
                WorkspaceManager.CommitChanges();
            }

            _structurizrClient.MergeFromRemote = merge;
            _structurizrClient.PutWorkspace(_workSpaceId, workspace);
        }
    }
}