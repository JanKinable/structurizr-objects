using System.Net;
using Structurizr;
using StructurizrObjects.Styles;

namespace StructurizrObjects
{
    public abstract class WorkspaceBase
    {
        private Workspace _workspace;
        private readonly IWorkspacePersistenceStrategy _persistenceStrategy;

        protected WorkspaceBase(long workSpaceId, string apiKey, string apiSecret)
        {
            _persistenceStrategy = new ApiWorkspacePersistenceStrategy(workSpaceId, apiKey, apiSecret);
        }

        protected WorkspaceBase(string pathToJson, string name, string description)
        {
            _persistenceStrategy = new FileWorkspacePersistenceStrategy(pathToJson, name, description);
        }

        public abstract string ContextBoundName { get; }

        public virtual DefaultStyles DefaultStyles { get; private set; }

        public Workspace GenerateAndSaveWorkspace()
        {
            _workspace = _persistenceStrategy.GetWorkspace();

            WorkspaceManager.BindWorkspace(_workspace);
            
            _workspace.Model.Enterprise = new Enterprise(ContextBoundName);

            DefaultStyles = WorkspaceManager.CreateDefaultStyles();

            WorkspaceManager.CreateModel();

            ApplyDefaultStyles(_workspace.Views.Configuration.Styles);

            _persistenceStrategy.PersistWorkspace(_workspace);

            return _workspace;
        }

        protected virtual void AdaptDefaultStyles() { }

        protected virtual void ApplyDefaultStyles(Structurizr.Styles styles)
        {
            AdaptDefaultStyles();
        }

    }
}
