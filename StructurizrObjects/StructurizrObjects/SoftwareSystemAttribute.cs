using System;
using Structurizr;

namespace StructurizrObjects
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SoftwareSystemAttribute : Attribute
    {
        public SoftwareSystemAttribute(string description, Location location)
        {
            Description = description;
            Location = location;
        }

        public string Description { get; }
        public Location Location { get; }
    }
}
