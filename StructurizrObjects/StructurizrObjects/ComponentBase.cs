using System;
using System.Reflection;
using Structurizr;
using StructurizrObjects.Styles;

namespace StructurizrObjects
{
    public abstract class ComponentBase: ElementBase
    {
        protected readonly Structurizr.Styles StyleContainer;
        public readonly Component Me;

        protected ComponentBase(Container container, Structurizr.Styles styleContainer, Action<ElementType, string> onCreatedFromExistingElement)
        : base(styleContainer, onCreatedFromExistingElement)
        {
            if(! TryGetMetadata(out string description, out string technology, out string type))
            {
                throw new System.Exception($"Missing ComponentAttribute on Component {GetType().Name}");
            }

            StyleContainer = styleContainer;
            Styles = new ElementStyleBase[0];

            var name = NamedIdentity.GetNameFromType(GetType());
            var component = container.GetComponentWithName(name);
            if( component == null)
            {
                if( type.Length > 0)
                {
                    try
                    {
                        component = container.AddComponent(name, description, technology, type);
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        throw new System.Exception($"The type parameter in the ComponentAttribute on {name} has an invalid value");
                    }
                }
                else
                {
                    component = container.AddComponent(name, description, technology);
                }
            }
            else
            {
                onCreatedFromExistingElement(ElementType.Element, component.Id);
                component.Description = description;
                component.Technology = technology;
                component.Type = type;
            }
            Element = Me = component;
        }

        public virtual ElementStyleBase[] Styles { get; }

        private bool TryGetMetadata(out string description, out string technology, out string type)
        {
            var attr = GetType().GetCustomAttribute<ComponentAttribute>(false);
            if (attr == null)
            {
                description = technology = type = string.Empty;
                return false;

            }
            description = attr.Description;
            technology = attr.Technology;
            type = attr.Type;
           
            return true;
        }
    }
}
