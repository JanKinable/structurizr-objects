using System;
using Structurizr;
using StructurizrObjects.Styles;

namespace StructurizrObjects
{
    public abstract class ElementBase
    {
        private readonly Structurizr.Styles _styleContainer;
        private readonly Action<ElementType, string> _onCreatedFromExistingElement;
        
        public StaticStructureElement Element;
        
        protected ElementBase(Structurizr.Styles styleContainer, Action<ElementType, string> onCreatedFromExistingElement)
        {
            _styleContainer = styleContainer;
            _onCreatedFromExistingElement = onCreatedFromExistingElement;
            Connectors = new Connector[0];
        }

        public virtual Connector[] Connectors { get; }

        protected Connector ConnectTo<T, TS>(string description, string technology)
            where TS: RelationshipStyleBase
        {
            if (typeof(SoftwareSystemBase).IsAssignableFrom(typeof(T)) 
                || typeof(PersonBase).IsAssignableFrom(typeof(T)) 
                || typeof(ContainerBase).IsAssignableFrom(typeof(T)) 
                || typeof(ComponentBase).IsAssignableFrom(typeof(T)))
            {
                return new Connector(typeof(T), description, technology, typeof(TS));
            }

            throw new ArgumentException($"Connection to a {typeof(T).Name} is not allowed.");
        }

        protected ElementStyleBase AddStyle<T>() where T : ElementStyleBase
        {
            var ctor = typeof(T).GetConstructor(new[] { typeof(Structurizr.Styles), typeof(Action<ElementType, string>) });
            if (ctor == null) return null;

            return ctor.Invoke(new object[] {_styleContainer, _onCreatedFromExistingElement }) as ElementStyleBase;
        }
    }
}