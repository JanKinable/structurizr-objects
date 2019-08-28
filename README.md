# structurizr-objects
Use Objects to compose the C4 model. Based on [Structurizr For .Net](https://github.com/structurizr/dotnet).

## Why?
* [Structurizr For .Net](https://github.com/structurizr/dotnet) allows to adapt one workspace. From a standpoint of a Software Architect (one team) this is sufficient, but there is no way to create a separate workspace re-using the models of another workspace. With structurizr-objects you can create a project with classes. Another project can import this project and reuse those model items in their workspace. Changes to the refered project are reflected in the refering project.
* With [Structurizr For .Net](https://github.com/structurizr/dotnet) removed model items are not taken care of during the merge, only added and changed items are taken into account during merge (when this option is chosen). When you don't choose merge the entire workspace is replaced, loosing the layout of the created views (items remain but got new id's, layout is coupled by id's)
* Having a typed system allows to validate the model during compilation

> The level of `Code` is not included.

## How?

Next basic classes for modeling are available: `WorkspaceBase`, `SoftwareSystemBase`, `PersonBase`, `ContainerBase`, `ComponentBase`. 

For styling we have 2 classes: `ElementStyleBase` and `RelationshipStyleBase`.

### Setting up the project

Create a C# class library and add the Nuget package:
```
Install-Package StructurizrObjects
```

Think on how you wil organize your objects: represent the structure the same way as the hiërachy, group per object type,...

It is assumed you already have a Workspace created in Structurizr. If not, do this first by visiting [Structurizr](https://structurizr.com).

Get the workspace id, apiKey and apiSecret. Go to your [dashboard](https://structurizr.com/dashboard) and click on the `Show more...` button to reveal the information.

> You can take a view at the examples directory for some coding examples.

#### Workspace

Add a new class eg. `MyWorkspace.cs` and inherit from `WorkspaceBase`. Pass the workspace id, apiKey and apiSecret in the constructor. It is a best practise to seal the class.

```csharp
public sealed class MyWorkspace : WorkspaceBase
{
    public MyWorkspace()
            : base([workspaceId], [apikey], [apiSecret])
    {
    }
}
```

In addition you can give an ContextBoundName to the workspace. This will draw a dotted line around all _internal_ objects.

```csharp
public sealed class MyWorkspace : WorkspaceBase
{
    public MyWorkspace()
            : base([workspaceId], [apikey], [apiSecret])
    {
    }

    public override string ContextBoundName => "MyContext";

}
```


#### Person

Add a new class eg. `MyPerson.cs` and inherit from `PersonBase`. Create the constructor passing the workspace (class from Structurizr for .Net) and an Action. This action is to register if this class is already contained in the persisted model. This is used in the base class.

Add a `PersonAttribute` to the class and add some description and state the location (either internal or external).

```csharp
[Person("Primary User of the systems", Location.Internal)]
public sealed class MyPerson : PersonBase
{
    public MyPerson(Workspace workspace, Action<ElementType, string> onCreatedFromExistingElement)
            : base(workspace, onCreatedFromExistingElement)
    {
    }
}
```
#### SoftwareSystem

Add a new class eg. `MySoftwareSystem.cs` and inherit from `SoftwareSystemBase`. Create the constructor passing the workspace (class from Structurizr for .Net) and an Action. This action is to register if this class is already contained in the persisted model.

Add a `SoftwareSystemAttribute` to the class and add some description and state the location (either internal or external).
```csharp
[SoftwareSystem("Main SoftwareSystem", Location.Internal)]
public sealed class MySoftwareSystem : SoftwareSystemBase
{
    public MySoftwareSystem(Workspace workspace, Action<ElementType, string> onCreatedFromExistingElement)
            : base(workspace, onCreatedFromExistingElement)
    {
    }
}
```

#### Container

Add a new class eg. `MyContainer.cs` and inherit from `ContainerBase`. Create the constructor passing the SoftwareSystem (class from Structurizr for .Net), styleContainer and the Action (as describe before). The styleContainer is also necessary to register styles in the model. Because they only exist on the _Worspace/Views_ level (outside of the _Workspace/model_) we need a reference here (in the above classes we have the workspace available so not needed there).

> a container can only exist within the context of a software system.

Add a `ContainerAttribute` to the class and add some description and state the technology (leave blank if optional).

```csharp
[Container(description: "My Container rocks", technology:"")]
public sealed class MyContainer : ContainerBase
{
    public MyContainer(SoftwareSystem softwaresystem, Structurizr.Styles styleContainer, Action<ElementType, string> onCreatedFromExistingElement)
            : base(softwaresystem, styleContainer, onCreatedFromExistingElement)
    {
    }
}
```

#### Component

Add a new class eg. `MyComponent.cs` and inherit from `ComponentBase`. Create the constructor passing the Container (class from Structurizr for .Net), styleContainer and the Action (as describe before). 

> a component can only exist within the context of a container.

Add a `ComponentAttribute` to the class and add some description, state the technology (leave blank if optional) and optional the type of component (see Structurizr documentation).

```csharp
[Component("My Component does it all", technology: "", type:"")]
public sealed class MyComponent : ContainerBase
{
    public MyComponent(Container container, Structurizr.Styles styleContainer, Action<ElementType, string> onCreatedFromExistingElement)
            : base(container, styleContainer, onCreatedFromExistingElement)
    {
    }
}
```

### Connecting the elements

To connect elements (or as used in Stucturizr: setup a relationship), override the `Connectors` property of your element class and call the `Connect<T,TS>(string description, string technology)` method. The first generic parameter `T` is the element to connect to, the second `TS` is the connector style (see furthur).

```csharp
public sealed class MyComponent : ContainerBase
{
    ...
    public override Connector[] Connectors => new Connector[]
    {
        ConnectTo<AnotherComponent, MyLineStyle>("uses", ""),
    };
}
```

### Styles

Styles are _cascading_, meaning the order of setting the styles is applied. The 'Default Styles' are always applied first. Custom Styles are set per class, here the order is also determining.

#### Default styles

The workspace contains default styles for each basic component (Person, SoftwareSystem, Container and Component). These styles can be adapted in the workspace you created by overriding the `AdaptDefaultStyles` and change the `DefaultStyles` property:
```csharp
public class MyWorkspace : WorkspaceBase
{
    ...
    protected override void AdaptDefaultStyles()
    {
        DefaultStyles.PrimaryColor = Color.White;
        DefaultStyles.PrimaryBackgroundColor = Color.Green;
        DefaultStyles.SoftwareSystem.Shape = Shape.Hexagon;
    }
}
```

#### Custom styles

You can create a custom style for either an element or a relation.

> You can inherit from one of the default styles or your own custom style to build up your own cascading styling.

##### Custom Element Style

Add a new class eg. `MyElementStyle.cs` and inherit from `ElementStyleBase`. Add a constructor as shown here. 

You can adapt the style by using the _Set..._ methods.

```csharp
public class MyElementStyle: ElementStyleBase
{
    public MyElementStyle(Structurizr.Styles styles, Action<ElementType, string> onCreatedFromExistingElement) 
        : base(styles, onCreatedFromExistingElement)
    {
        SetBackgroundColor(System.Drawing.Color.DodgerBlue);
        SetColor(System.Drawing.Color.GhostWhite);
        SetOpacity(10);
        SetShowDescription(false);
        SetShowMetadata(true);
    }
}
```

This style can be assigned in any _Element_ class by overriding the `Styles` property and calling the generic method `AddStyle<>()`:
```csharp
public class MyPerson : PersonBase
{
    ...

    public override ElementStyleBase[] Styles => new[]
    {
        AddStyle<MyElementStyle>()
    };
}

```

##### Custom Relationship Style

Add a new class eg. `MyElementStyle.cs` and inherit from `ElementStyleBase`. Add a constructor as shown here. 

You can adapt the style by using the _Set..._ methods.

```csharp
public class MyRelationshipStyle: RelationshipStyleBase
{
    public MyRelationshipStyle(Structurizr.Styles styles, Action<ElementType, string> onCreatedFromExistingElement) 
        : base(styles, onCreatedFromExistingElement)
    {
        SetBackgroundColor(System.Drawing.Color.DodgerBlue);
        SetColor(System.Drawing.Color.GhostWhite);
        SetOpacity(10);
        SetShowDescription(false);
        SetShowMetadata(true);
    }
}
```

This style can be passed when setting up a connection in the second generic parameter:
```csharp
public class MyPerson : PersonBase
{
    ...

    public override Connector[] Connectors => new Connector[]
    {
        ConnectTo<MySoftwareSystem, MyRelationshipStyle>("Just for fun", "no tech involved"),
    };
}

```

### Pushing/Syncing changes to Structurizr

Create a console project and reference the project you just created. Instantiate the workspace and call the `GenerateAndSaveWorkspace()`. 

```csharp
class Program
{
    static void Main(string[] args)
    {
        var myWorkspace = new MyWorkspace();
        myWorkspace.GenerateAndSaveWorkspace();
    }
```

After you run this, your **model** is created in the Stucturizr WebUI. 

You can now start drawing your diagrams!

### Pushing/Syncing changes to a json file

StructurizrObjects allows also to work around a json file to upload it manualy via the Structurizr WebUI.

If you want to use this option, create an overload constructor on your workspace, passing the path of the file:

```csharp
public sealed class MyWorkspace : WorkspaceBase
{
    public MyWorkspace()
            : base([workspaceId], [apikey], [apiSecret])
    {
    }

    public MyWorkspace(string pathToJson)
            : base(pathToJson)
    {
    }

    public override string ContextBoundName => "MyContext";

}
```

In the console program use this constructor to save the workspace as a json file.
```csharp
class Program
{
    static void Main(string[] args)
    {
        var myWorkspace = new LabWorkspace([pathToJson]);
        myWorkspace.GenerateAndSaveWorkspace();
    }
```
**TODO**
- allow adding tags for filtered views
