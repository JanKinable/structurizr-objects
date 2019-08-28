using System;
using System.IO;
using Structurizr;
using Structurizr.IO.Json;

namespace StructurizrObjects
{
    public class FileWorkspacePersistenceStrategy : IWorkspacePersistenceStrategy
    {
        private readonly string _pathToJson;
        private readonly string _name;
        private readonly string _description;

        public FileWorkspacePersistenceStrategy(string pathToJson, string name, string description)
        {
            _pathToJson = pathToJson;
            _name = name;
            _description = description;
        }

        public Workspace GetWorkspace()
        {
            if (!File.Exists(_pathToJson)) return new Workspace(_name, _description);

            try
            {
                using (var sr = new StreamReader(_pathToJson))
                using (var rdr = new StringReader(sr.ReadToEnd()))
                {
                    return (new JsonReader()).Read(rdr);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void PersistWorkspace(Workspace workspace)
        {
            using (var sw = new StreamWriter(_pathToJson))
            {
                new JsonWriter(true).Write(workspace, sw);
            }
        }
    }
}