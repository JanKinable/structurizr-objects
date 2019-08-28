using System;

namespace StructurizrObjects
{
    public class Connector
    {
        public Connector(Type type, string description, string technology, Type relationshipStyle)
        {
            ConnectTo = type;
            Description = description;
            Technology = technology;
            RelationshipStyle = relationshipStyle;
        }

        public Type ConnectTo { get; }
        public string Description { get; }
        public string Technology { get; }
        public Type RelationshipStyle { get; }
    }

}