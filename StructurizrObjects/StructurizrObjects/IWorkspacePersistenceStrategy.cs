using Structurizr;

namespace StructurizrObjects
{
    public interface IWorkspacePersistenceStrategy
    {
        Workspace GetWorkspace();

        void PersistWorkspace(Workspace workspace);
    }
}