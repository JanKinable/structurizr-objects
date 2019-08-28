using System;
using System.Reflection;
using Structurizr;
using StructurizrObjects.Styles;

namespace StructurizrObjects
{

    public abstract class ContainerBase : ElementBase
    {
        public readonly Container Me;
        protected readonly SoftwareSystem SoftwareSystem;
        protected readonly Structurizr.Styles StyleContainer;
        private readonly Action<ElementType, string> _onCreatedFromExistingElement;

        protected ContainerBase(SoftwareSystem softwareSystem, Structurizr.Styles styleContainer, Action<ElementType, string> onCreatedFromExistingElement)
         : base(styleContainer, onCreatedFromExistingElement)
        {
            if(! TryGetMetadata(out string description, out string technology))
            {
                throw new System.Exception($"Missing ContainerAttribute on Container class {GetType().Name}");
            }
            
            SoftwareSystem = softwareSystem;
            StyleContainer = styleContainer;
            _onCreatedFromExistingElement = onCreatedFromExistingElement;

            Components = new ComponentBase[0];
            Styles = new ElementStyleBase[0];

            var name = NamedIdentity.GetNameFromType(GetType());
            var container = softwareSystem.GetContainerWithName(name);
            if(container == null)
            {
                container = softwareSystem.AddContainer(name, description, technology);
            }
            else
            {
                _onCreatedFromExistingElement(ElementType.Element, container.Id);
                container.Description = description;
                container.Technology = technology;
            }
            Element = Me = container;
        }

        public virtual ComponentBase[] Components { get; }
        public virtual ElementStyleBase[] Styles { get; }

        protected ComponentBase AddComponent<T>()
            where T: ComponentBase
        {
            var ctor = typeof(T).GetConstructor(new[] { typeof(Container), typeof(Structurizr.Styles), typeof(Action<ElementType, string>) });
            if (ctor == null) return null;

            return ctor.Invoke(new object[] { Me, StyleContainer, _onCreatedFromExistingElement }) as ComponentBase;
        }

        private bool TryGetMetadata(out string description, out string technology)
        {
            var attr = GetType().GetCustomAttribute<ContainerAttribute>(false);
            if (attr == null)
            {
                description = technology = string.Empty;
                return false;

            }
            description = attr.Description;
            technology = attr.Technology;
            return true;
        }
    }
}
