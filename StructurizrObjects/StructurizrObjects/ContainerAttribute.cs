using System;

namespace StructurizrObjects
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ContainerAttribute : Attribute
    {
        public ContainerAttribute(string description, string technology)
        {
            Description = description;
            Technology = technology;
        }

        public string Description { get; }
        public string Technology { get; }
    }
}
