using System;
using System.Reflection;
using Structurizr;
using StructurizrObjects.Styles;

namespace StructurizrObjects
{
    public abstract class PersonBase : ElementBase
    {

        public readonly Person Me;

        protected readonly Workspace Workspace;
        protected readonly Structurizr.Styles StyleContainer;


        protected PersonBase(Workspace workspace, Action<ElementType, string> onCreatedFromExistingElement)
            : base(workspace.Views.Configuration.Styles, onCreatedFromExistingElement)
        {
            Workspace = workspace;
            if (!TryGetMetadata(out string description, out Location location))
            {
                throw new System.Exception($"Missing PersonAttribute on Person {GetType().Name}");
            }

            StyleContainer = workspace.Views.Configuration.Styles;
            Styles = new ElementStyleBase[0];

            var name = NamedIdentity.GetNameFromType(GetType());
            var person = workspace.Model.GetPersonWithName(name);
            if (person == null)
            {
                person = workspace.Model.AddPerson(location, name, description);
            }
            else
            {
                onCreatedFromExistingElement(ElementType.Element, person.Id);
                person.Location = location;
                person.Description = description;
            }
            Element = Me = person;
        }

        public virtual ElementStyleBase[] Styles { get; }

        private bool TryGetMetadata(out string description, out Location location)
        {
            var attr = GetType().GetCustomAttribute<PersonAttribute>(false);
            if (attr == null)
            {
                description = string.Empty;
                location = Location.Unspecified;
                return false;

            }
            description = attr.Description;
            location = attr.Location;
            return true;
        }
    }
}
