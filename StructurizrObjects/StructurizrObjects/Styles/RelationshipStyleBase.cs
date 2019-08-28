using System;
using System.Linq;
using Structurizr;
using StructurizrObjects.Utils;

namespace StructurizrObjects.Styles
{
    public class RelationshipStyleBase
    {
        private readonly RelationshipStyle _default;

        protected RelationshipStyleBase(Structurizr.Styles styles, Action<ElementType, string> onCreatedFromExistingElement)
        {
            var styleName = NamedIdentity.GetNameFromType(GetType());
            var tagName = styleName.Substring(0, styleName.Length - " Line Style".Length);
            _default = styles.Relationships.FirstOrDefault(x => x.Tag == tagName);
            if (_default == null)
            {
                _default = new RelationshipStyle(tagName);
                styles.Relationships.Add(_default);
            }
            else
            {
                onCreatedFromExistingElement(ElementType.RelationStyle, tagName);
            }
        }

        protected void SetThickness(int thickness) => _default.Thickness = thickness;
        public int? Thickness
        {
            get => _default.Thickness;
            set => _default.Thickness = value;
        }

        protected void SetColor(System.Drawing.Color color) => _default.Color = color.ToHex();
        public string Color
        {
            get => _default.Color;
            set => _default.Color = value;
        }

        protected void SetFontSize(int size) => _default.FontSize = size;
        public int? FontSize
        {
            get => _default.FontSize;
            set => _default.FontSize = value;
        }

        protected void SetWidth(int width) => _default.Width = width;
        public int? Width
        {
            get => _default.Width;
            set => _default.Width = value;
        }

        protected void SetDashed(bool dashed) => _default.Dashed = dashed;
        public bool? Dashed
        {
            get => _default.Dashed;
            set => _default.Dashed = value;
        }

        protected void SetRouting(Routing routing) => _default.Routing = routing;
        public Routing? Routing
        {
            get => _default.Routing;
            set => _default.Routing = value;
        }

        protected void SetPosition(int position) => _default.Position = position;
        public int? Position
        {
            get => _default.Position;
            set => _default.Position = value;
        }

        protected void SetOpacity(int opacity) => _default.Opacity = opacity;
        public int? Opacity
        {
            get => _default.Opacity;
            set => _default.Opacity = value;
        }

        protected void SetInteractionStyle(InteractionStyle interactionStyle) => this.InteractionStyle = interactionStyle;
        public InteractionStyle InteractionStyle { get; set; } = InteractionStyle.Synchronous;

        public RelationshipStyle GetRelationshipStyle()
        {
            return _default;
        }
    }



}
