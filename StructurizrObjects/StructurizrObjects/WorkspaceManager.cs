using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using Newtonsoft.Json;
using Structurizr;
using StructurizrObjects.Styles;
using StructurizrObjects.Utils;
using JsonWriter = Structurizr.IO.Json.JsonWriter;

namespace StructurizrObjects
{
    internal static class WorkspaceManager
    {
        private static Workspace _workspace;

        private static readonly List<SoftwareSystemBase> SoftwareSystems = new List<SoftwareSystemBase>();
        private static readonly List<ContainerBase> Containers = new List<ContainerBase>();
        private static readonly List<ComponentBase> Components = new List<ComponentBase>();
        private static readonly List<PersonBase> Persons = new List<PersonBase>();

        private static List<string> _currentIds;
        private static List<string> _currentRelationShipTags;
        private static List<string> _currentElementTags;

        internal static void BindWorkspace(Workspace workspace)
        {
            _workspace = workspace;
            RegisterCurrentObjects();
        }

        internal static void CreateModel()
        {
            var softwareSystemTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x =>
                typeof(SoftwareSystemBase).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToArray();

            SoftwareSystems.AddRange(softwareSystemTypes.Select(CreateSoftwareSystem).Where(x => x != null));

            Persons.AddRange(AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(PersonBase).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(CreatePerson)
                .Where(x => x != null));

            ApplyRelations();
        }

        internal static DefaultStyles CreateDefaultStyles()
        {
            return new DefaultStyles(_workspace.Views.Configuration.Styles, OnCreatedFromExistingElement);
        }

        internal static void CommitChanges()
        {
            var dependencyIds = new List<string>();

            if (_currentIds.Any())
            {
                Console.WriteLine($"{_currentIds.Count} deleted object(s) found. Cleaning the model:");
            }
            else
            {
                return;
            }

            foreach (var currentId in _currentIds)
            {
                var element = _workspace.Model.GetElement(currentId);
                //check on dependencies (they are not loaded by the scanner)
                if (NamedIdentity.TryGetTypeFromExternalAssemblyByElementName(element.Name, out var dependency))
                {
                    dependencyIds.Add(currentId);
                    continue;
                }

                switch (element)
                {
                    case SoftwareSystem softwareSystem:
                        Console.WriteLine($"\tRemoving SoftwareSystem {softwareSystem.Name}");
                        _workspace.Model.SoftwareSystems.Remove(softwareSystem);
                        break;
                    case Person person:
                        Console.WriteLine($"\tRemoving Person {person.Name}");
                        _workspace.Model.People.Remove(person);
                        break;
                    case Container container:
                        Console.WriteLine($"\tRemoving Container {container.Name}");
                        ((SoftwareSystem)container.Parent).Containers.Remove(container);
                        break;
                    case Component component:
                        Console.WriteLine($"\tRemoving Component {component.Name}");
                        ((Container)component.Parent).Components.Remove(component);
                        break;
                }
            }

            var styles = _workspace.Views.Configuration.Styles;
            foreach (var currentRelationShipTag in _currentRelationShipTags)
            {
                Console.WriteLine($"\tRemoving RelationshipStyle {currentRelationShipTag}");
                var relStyle = styles.Relationships.Find(x => x.Tag == currentRelationShipTag);
                styles.Relationships.Remove(relStyle);
            }

            foreach (var currentElementTag in _currentElementTags.Where(x => x != "Element"))
            {
                Console.WriteLine($"\tRemoving ElementStyle {currentElementTag}");
                var elStyle = styles.Elements.Find(x => x.Tag == currentElementTag);
                styles.Elements.Remove(elStyle);
            }

            //remove all the dependencies
            foreach (var currentId in _currentIds)
            {
                if (dependencyIds.Contains(currentId)) continue;
                //
                // Cleanup views
                foreach (var landscapeViewItem in _workspace.Views.SystemLandscapeViews)
                {
                    var foundLandscapeView = landscapeViewItem.Elements.FirstOrDefault(x => x.Id == currentId);
                    if (foundLandscapeView != null)
                    {
                        Console.WriteLine($"\tRemoving SystemLandscapeView Element {foundLandscapeView.Element.Name}");
                        landscapeViewItem.Elements.Remove(foundLandscapeView);
                    }
                }

                var contextView = _workspace.Views.SystemContextViews.FirstOrDefault(x => x.SoftwareSystemId == currentId);
                if (contextView != null)
                {
                    Console.WriteLine($"\tRemoving SystemContextView {contextView.Name}");
                    _workspace.Views.SystemContextViews.Remove(contextView);
                }
                foreach (var contextViewItem in _workspace.Views.SystemContextViews)
                {
                    var foundContextView = contextViewItem.Elements.FirstOrDefault(x => x.Id == currentId);
                    if (foundContextView != null)
                    {
                        Console.WriteLine($"\tRemoving SystemContextView Element {foundContextView.Element.Name}");
                        contextViewItem.Elements.Remove(foundContextView);
                    }
                }

                var containerView = _workspace.Views.ContainerViews.FirstOrDefault(x => x.SoftwareSystemId == currentId);
                if (containerView != null)
                {
                    Console.WriteLine($"\tRemoving ContainerView {containerView.Name}");
                    _workspace.Views.ContainerViews.Remove(containerView);
                }
                foreach (var containerViewItem in _workspace.Views.ContainerViews)
                {
                    var foundContainerView = containerViewItem.Elements.FirstOrDefault(x => x.Id == currentId);
                    if (foundContainerView != null)
                    {
                        Console.WriteLine($"\tRemoving ContainerView Element {foundContainerView.Element.Name}");
                        containerViewItem.Elements.RemoveWhere((e) => e.Id == currentId);
                    }
                }

                var componentView = _workspace.Views.ComponentViews.FirstOrDefault(x => x.ContainerId == currentId);
                if (componentView != null)
                {
                    Console.WriteLine($"\tRemoving ComponentView {componentView.Name}");
                    _workspace.Views.ComponentViews.Remove(componentView);
                }
                foreach (var componentViewItem in _workspace.Views.ComponentViews)
                {
                    var foundComponentView = componentViewItem.Elements.FirstOrDefault(x => x.Id == currentId);
                    if (foundComponentView != null)
                    {
                        Console.WriteLine($"\tRemoving ComponentView Element {foundComponentView.Element.Name}");
                        componentViewItem.Elements.RemoveWhere((e) => e.Id == currentId);
                    }
                }

                //
                //cleanup relations
                var relations = _workspace.Model.Relationships.Where(x => x.DestinationId == currentId);
                foreach (var relationship in relations)
                {
                    Console.WriteLine($"\tRemoving Relationship {relationship}");
                    foreach (var softwareSystem in _workspace.Model.SoftwareSystems)
                    {
                        if (softwareSystem.Relationships.Contains(relationship))
                        {
                            softwareSystem.Relationships.Remove(relationship);
                        }

                        foreach (var container in softwareSystem.Containers)
                        {
                            if (container.Relationships.Contains(relationship))
                            {
                                container.Relationships.Remove(relationship);
                            }

                            foreach (var component in container.Components)
                            {
                                if (component.Relationships.Contains(relationship))
                                {
                                    container.Relationships.Remove(relationship);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void OnCreatedFromExistingElement(ElementType elementType, string id)
        {
            int idx;
            switch (elementType)
            {
                case ElementType.Element:
                    idx = _currentIds.IndexOf(id);
                    if (idx > -1)
                    {
                        _currentIds.RemoveAt(idx);
                    }
                    break;
                case ElementType.ElementStyle:
                    idx = _currentElementTags.IndexOf(id);
                    if (idx > -1)
                    {
                        _currentElementTags.RemoveAt(idx);
                    }
                    break;
                case ElementType.RelationStyle:
                    idx = _currentRelationShipTags.IndexOf(id);
                    if (idx > -1)
                    {
                        _currentRelationShipTags.RemoveAt(idx);
                    }
                    break;
            }
        }


        private static void RegisterCurrentObjects()
        {
            var sb = new StringBuilder();
            using (var wr = new StringWriter(sb))
            {
                new JsonWriter(true).Write(_workspace, wr);
            }

            var xDoc = JsonConvert.DeserializeXNode(sb.ToString(), "workspace");
            //exclude relationships!
            _currentIds = xDoc.XPathSelectElements("//model/.//id").Where(x => x.Parent != null && x.Parent.Name != "relationships").Select(x => x.Value).ToList();

            _currentRelationShipTags = xDoc.XPathSelectElements("//views/configuration/styles/relationships/tag").Select(x => x.Value).ToList();
            _currentElementTags = xDoc.XPathSelectElements("//views/configuration/styles/elements/tag").Select(x => x.Value).ToList();
        }

        private static SoftwareSystemBase CreateSoftwareSystem(Type type)
        {
            var ctor = type.GetConstructor(new[] { typeof(Workspace), typeof(Action<ElementType, string>) });
            if (ctor == null) return null;

            Action<ElementType, string> callback = OnCreatedFromExistingElement;
            var softwareSystem = ctor.Invoke(new object[] { _workspace, callback }) as SoftwareSystemBase;
            if (softwareSystem == null) return null;

            softwareSystem.Me.AddUniqueTags(softwareSystem.Styles.Select(x => x?.GetElementStyle()?.Tag).Where(x => x != null).ToArray());
            foreach (var container in softwareSystem.Containers)
            {
                container.Me.AddUniqueTags(container.Styles.Select(x => x?.GetElementStyle()?.Tag).Where(x => x != null).ToArray());
                Containers.Add(container);
                foreach (var component in container.Components)
                {
                    component.Me.AddUniqueTags(component.Styles.Select(x => x?.GetElementStyle()?.Tag).Where(x => x != null).ToArray());
                    Components.Add(component);
                }
            }
            return softwareSystem;
        }

        private static SoftwareSystemBase CreateExternalSoftwareSystem(Type type)
        {
            var ctor = type.GetConstructor(new[] { typeof(Workspace), typeof(Action<ElementType, string>) });
            if (ctor == null) return null;

            Action<ElementType, string> callback = OnCreatedFromExistingElement;
            var softwareSystem = ctor.Invoke(new object[] { _workspace, callback }) as SoftwareSystemBase;
            if (softwareSystem == null) return null;

            //change to external
            softwareSystem.Me.Location = Location.External;

            var tags = softwareSystem.Styles.Select(x => x?.GetElementStyle()?.Tag).Where(x => x != null).ToList();
            tags.Add("Dependency");
            softwareSystem.Me.AddUniqueTags(tags);
            foreach (var container in softwareSystem.Containers)
            {
                var containerTags = container.Styles.Select(x => x?.GetElementStyle()?.Tag).Where(x => x != null).ToList();
                containerTags.Add("Dependency");
                container.Me.AddUniqueTags(containerTags);
                Containers.Add(container);
                foreach (var component in container.Components)
                {
                    var componentTags = component.Styles.Select(x => x?.GetElementStyle()?.Tag).Where(x => x != null).ToList();
                    componentTags.Add("Dependency");
                    component.Me.AddUniqueTags(componentTags);
                    Components.Add(component);
                }
            }
            return softwareSystem;
        }

        private static PersonBase CreatePerson(Type type)
        {
            var ctor = type.GetConstructor(new[] { typeof(Workspace), typeof(Action<ElementType, string>) });
            if (ctor == null) return null;

            Action<ElementType, string> callback = OnCreatedFromExistingElement;

            var person = ctor.Invoke(new object[] { _workspace, callback }) as PersonBase;
            if (person == null) return null;
            person.Me.AddUniqueTags(person.Styles.Select(x => x.GetElementStyle().Tag).ToArray());

            return person;
        }

        private static void ApplyRelations()
        {
            ApplyConnections(Persons);
            ApplyConnections(SoftwareSystems);
            ApplyConnections(Containers);
            ApplyConnections(Components);
        }

        private static void ApplyConnections(IEnumerable<ElementBase> elements)
        {
            var softwareSystemDependencies = new List<SoftwareSystemBase>();
            
            foreach (var component in elements)
            {
                foreach (var connector in component.Connectors)
                {
                    if (typeof(SoftwareSystemBase).IsAssignableFrom(connector.ConnectTo))
                    {
                        softwareSystemDependencies.AddRange(ConnectToSoftwareSystem(connector, component.Element));
                    }
                    if (typeof(ContainerBase).IsAssignableFrom(connector.ConnectTo))
                    {
                        ConnectToContainer(connector, component.Element);
                    }
                    if (typeof(ComponentBase).IsAssignableFrom(connector.ConnectTo))
                    {
                        ConnectToComponent(connector, component.Element);
                    }
                }
            }

            if(softwareSystemDependencies.Any())
                SoftwareSystems.AddRange(softwareSystemDependencies);
        }

        private static List<SoftwareSystemBase> ConnectToSoftwareSystem(Connector connector, StaticStructureElement fromElement)
        {
            var softwareSystemDependencies = new List<SoftwareSystemBase>();
            var connectedSystem =
                _workspace.Model.GetSoftwareSystemWithName(NamedIdentity.GetNameFromType(connector.ConnectTo));
            if (connectedSystem == null)
            {
                //possibly from another library: for now only allow SoftwareSystem (loose containers and components are not allowed)
                if (!typeof(SoftwareSystemBase).IsAssignableFrom(connector.ConnectTo)) return softwareSystemDependencies;

                var softwareSystem = CreateExternalSoftwareSystem(connector.ConnectTo);
                softwareSystemDependencies.Add(softwareSystem);
                connectedSystem = softwareSystem.Me;
            }

            var ctor = connector.RelationshipStyle.GetConstructor(new[] { typeof(Structurizr.Styles), typeof(Action<ElementType, string>) });
            if (ctor == null) return softwareSystemDependencies;
            Action<ElementType, string> callback = OnCreatedFromExistingElement;
            var connectorStyle = (RelationshipStyleBase)ctor.Invoke(new object[] { _workspace.Views.Configuration.Styles, callback });

            var relationship = fromElement.Relationships.FirstOrDefault(x => x.DestinationId == connectedSystem.Id)
                            ?? fromElement.Uses(connectedSystem, connector.Description, connector.Technology, connectorStyle.InteractionStyle);
            relationship.AddUniqueTag(connectorStyle.GetRelationshipStyle().Tag);
            return softwareSystemDependencies;
        }

        private static void ConnectToContainer(Connector connector, StaticStructureElement fromElement)
        {
            var connectedContainer = _workspace.Model.SoftwareSystems
                .SelectMany(x => x.Containers)
                .FirstOrDefault(x => x.Name == NamedIdentity.GetNameFromType(connector.ConnectTo));
            if (connectedContainer == null) return;

            var ctor = connector.RelationshipStyle.GetConstructor(new[] { typeof(Structurizr.Styles), typeof(Action<ElementType, string>) });
            if (ctor == null) return;
            Action<ElementType, string> callback = OnCreatedFromExistingElement;
            var connectorStyle = (RelationshipStyleBase)ctor.Invoke(new object[] { _workspace.Views.Configuration.Styles, callback });

            var relationship = fromElement.Relationships.FirstOrDefault(x => x.DestinationId == connectedContainer.Id)
                            ?? fromElement.Uses(connectedContainer, connector.Description, connector.Technology, connectorStyle.InteractionStyle);
            relationship.AddUniqueTag(connectorStyle.GetRelationshipStyle().Tag);
        }

        private static void ConnectToComponent(Connector connector, StaticStructureElement fromElement)
        {
            var connectedSystem = _workspace.Model.SoftwareSystems.SelectMany(x => x.Containers)
                .SelectMany(x => x.Components)
                .FirstOrDefault(x => x.Name == NamedIdentity.GetNameFromType(connector.ConnectTo));
            if (connectedSystem == null) return;

            var ctor = connector.RelationshipStyle.GetConstructor(new[] { typeof(Structurizr.Styles), typeof(Action<ElementType, string>) });
            if (ctor == null) return;
            Action<ElementType, string> callback = OnCreatedFromExistingElement;
            var connectorStyle = (RelationshipStyleBase)ctor.Invoke(new object[] { _workspace.Views.Configuration.Styles, callback });

            var relationship = fromElement.Relationships.FirstOrDefault(x => x.DestinationId == connectedSystem.Id)
                               ?? fromElement.Uses(connectedSystem, connector.Description, connector.Technology, connectorStyle.InteractionStyle);
            relationship.AddUniqueTag(connectorStyle.GetRelationshipStyle().Tag);
        }
    }

}