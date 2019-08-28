using System;
using System.Reflection;
using Structurizr;
using StructurizrObjects.Styles;

namespace StructurizrObjects
{

    public abstract class SoftwareSystemBase : ElementBase
    {
        public readonly SoftwareSystem Me;
        protected readonly Workspace Workspace;
        private readonly Action<ElementType,string> _onCreatedFromExistingElement;
        protected readonly Structurizr.Styles StyleContainer;

        protected SoftwareSystemBase(Workspace workspace, Action<ElementType,string> onCreatedFromExistingElement)
         : base(workspace.Views.Configuration.Styles, onCreatedFromExistingElement)
        {
            Workspace = workspace;
            _onCreatedFromExistingElement = onCreatedFromExistingElement;
            
            Styles = new ElementStyleBase[0];
            Containers = new ContainerBase[0];

            if(! TryGetMetadata(out string description, out Location location))
            {
                throw new System.Exception($"Missing SoftwareSystemAttribute on SoftwareSystem {GetType().Name}");
            }
            StyleContainer = workspace.Views.Configuration.Styles;
            var name = NamedIdentity.GetNameFromType(GetType());
            var softwareSystem = workspace.Model.GetSoftwareSystemWithName(name);
            if (softwareSystem == null)
            {
                softwareSystem = workspace.Model.AddSoftwareSystem(location, name, description);
            }
            else
            {
                onCreatedFromExistingElement(ElementType.Element, softwareSystem.Id);
                softwareSystem.Location = location;
                softwareSystem.Description = description;
            }
            Element = Me = softwareSystem;
        }

        public virtual ContainerBase[] Containers { get; }

        public virtual ElementStyleBase[] Styles { get; }

        protected ContainerBase AddContainer<T>()
            where T: ContainerBase
        {
            var ctor = typeof(T).GetConstructor(new[] { typeof(SoftwareSystem), typeof(Structurizr.Styles), typeof(Action<ElementType, string>) });
            if (ctor == null) return null;

            return ctor.Invoke(new object[] { Me, StyleContainer, _onCreatedFromExistingElement }) as ContainerBase;
        }

        private bool TryGetMetadata(out string description, out Location location)
        {
            var attr = GetType().GetCustomAttribute<SoftwareSystemAttribute>(false);
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
