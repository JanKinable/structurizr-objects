using System;
using System.Linq;
using Structurizr;
using StructurizrObjects.Utils;
using Color = System.Drawing.Color;

namespace StructurizrObjects.Styles
{
    public class DefaultStyles
    {
        private readonly ElementStyle _elementStyle;

        public DefaultStyles(Structurizr.Styles styles, Action<ElementType, string> onCreatedFromExistingElement)
        {
            _elementStyle = styles.Elements.FirstOrDefault(x => x.Tag == "Element");
            if (_elementStyle == null)
            {
                _elementStyle = new ElementStyle("Element");
                styles.Elements.Add(_elementStyle);
            }

            Person = new PersonStyle(styles, onCreatedFromExistingElement);
            SoftwareSystem = new SoftwareSystemStyle(styles, onCreatedFromExistingElement);
            Container = new ContainerStyle(styles, onCreatedFromExistingElement);
            Component = new ComponentStyle(styles, onCreatedFromExistingElement);
            AsyncLineStyle = new AsynchronousLineStyle(styles, onCreatedFromExistingElement);
            SyncLineStyle = new SynchronousLineStyle(styles, onCreatedFromExistingElement);
            DependencyStyle = new DependencyStyle(styles, onCreatedFromExistingElement);
        }

        public Color PrimaryBackgroundColor
        {
            get => Color.FromArgb(int.Parse(_elementStyle.Background.Replace("#", ""), System.Globalization.NumberStyles.AllowHexSpecifier));
            set => _elementStyle.Background = value.ToHex();
        }

        public Color PrimaryColor
        {
            get => Color.FromArgb(int.Parse(_elementStyle.Color.Replace("#", ""), System.Globalization.NumberStyles.AllowHexSpecifier));
            set => _elementStyle.Color = value.ToHex();
        }

        public PersonStyle Person { get; }
        public SoftwareSystemStyle SoftwareSystem { get; }
        public ContainerStyle Container { get; }
        public ComponentStyle Component { get; }
        public AsynchronousLineStyle AsyncLineStyle { get; }
        public SynchronousLineStyle SyncLineStyle { get; }
        public DependencyStyle DependencyStyle { get; }
    }

    public class PersonStyle : ElementStyleBase
    {
        public PersonStyle(Structurizr.Styles styles, Action<ElementType, string> onCreatedFromExistingElement)
            : base(styles, onCreatedFromExistingElement)
        {
            SetShape(Shape.Person);
        }
    }
    public class SoftwareSystemStyle : ElementStyleBase
    {
        public SoftwareSystemStyle(Structurizr.Styles styles, Action<ElementType, string> onCreatedFromExistingElement)
            : base(styles, onCreatedFromExistingElement)
        {
        }
    }
    public class DependencyStyle : ElementStyleBase
    {
        public DependencyStyle(Structurizr.Styles styles, Action<ElementType, string> onCreatedFromExistingElement)
            : base(styles, onCreatedFromExistingElement)
        {
            SetBorder(Border.Dashed);
            SetBackgroundColor(System.Drawing.Color.DarkGray);
            SetColor(System.Drawing.Color.White);
            SetOpacity(50);
        }
    }

    public class ContainerStyle : ElementStyleBase
    {
        public ContainerStyle(Structurizr.Styles styles, Action<ElementType, string> onCreatedFromExistingElement)
            : base(styles, onCreatedFromExistingElement)
        {
        }
    }

    public class ComponentStyle : ElementStyleBase
    {
        public ComponentStyle(Structurizr.Styles styles, Action<ElementType, string> onCreatedFromExistingElement)
            : base(styles, onCreatedFromExistingElement)
        {
        }
    }

    public class SynchronousLineStyle : RelationshipStyleBase
    {
        public SynchronousLineStyle(Structurizr.Styles styles, Action<ElementType, string> onCreatedFromExistingElement)
            : base(styles, onCreatedFromExistingElement)
        {
            SetDashed(false);
        }
    }

    public class AsynchronousLineStyle : RelationshipStyleBase
    {
        public AsynchronousLineStyle(Structurizr.Styles styles, Action<ElementType, string> onCreatedFromExistingElement)
            : base(styles, onCreatedFromExistingElement)
        {
            SetDashed(true);
        }
    }
}
