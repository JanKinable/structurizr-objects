using System;
using System.Linq;
using Structurizr;
using StructurizrObjects.Utils;

namespace StructurizrObjects.Styles
{
    public class ElementStyleBase
    {
        private readonly ElementStyle _default;

        protected ElementStyleBase(Structurizr.Styles styles, Action<ElementType,string> onCreatedFromExistingElement)
        {
            var styleName = NamedIdentity.GetNameFromType(GetType());
            var tagName = styleName.Substring(0, styleName.Length - " Style".Length);
            _default = styles.Elements.FirstOrDefault(x => x.Tag == tagName);
            if (_default == null)
            {
                _default = new ElementStyle(tagName);
                styles.Elements.Add(_default);
            }
            else
            {
                onCreatedFromExistingElement(ElementType.ElementStyle, tagName);
            }
                
        }

        protected void SetBackgroundColor(System.Drawing.Color color) => _default.Background = color.ToHex();
        public virtual string BackgroundColor
        {
            get => _default.Background;
            set => _default.Background = value;
        }

        protected void SetColor(System.Drawing.Color color) => _default.Color = color.ToHex();
        public virtual string Color
        {
            get => _default.Color;
            set => _default.Color = value;
        }

        protected void SetFontSize(int size) => _default.FontSize = size;
        public virtual int? FontSize
        {
            get => _default.FontSize;
            set => _default.FontSize = value;
        }

        protected void SetShape(Shape shape) => _default.Shape = shape;
        public virtual Shape Shape
        {
            get => _default.Shape;
            set => _default.Shape = value;
        }

        protected void SetBorder(Border border) => _default.Border = border;
        public virtual Border Border
        {
            get => _default.Border;
            set => _default.Border = value;
        }

        protected void SetWidth(int width) => _default.Width = width;
        public virtual int? Width
        {
            get => _default.Width;
            set => _default.Width = value;
        }

        protected void SetHeight(int height) => _default.Height = height;
        public virtual int? Height
        {
            get => _default.Height;
            set => _default.Height = value;
        }

        protected void SetOpacity(int opacity) => _default.Opacity = opacity;
        public virtual int? Opacity
        {
            get => _default.Opacity;
            set => _default.Opacity = value;
        }

        protected void SetShowMetadata(bool show) => _default.Metadata = show;
        public virtual bool? ShowMetadata
        {
            get => _default.Metadata;
            set => _default.Metadata = value;
        }

        protected void SetShowDescription(bool show) => _default.Description = show;
        public virtual bool? ShowDescription
        {
            get => _default.Description;
            set => _default.Description = value;
        }

        public ElementStyle GetElementStyle()
        {
            return _default;
        }
    }



}
