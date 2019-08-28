using System;

namespace StructurizrObjects
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ComponentAttribute : Attribute
    {
        public ComponentAttribute(string description, string technology, string type = "")
        {
            Description = description;
            Technology = technology;
            Type = type;
        }

        public string Description { get; }
        public string Technology { get; }
        public string Type { get; }
    }
}
