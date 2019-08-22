# structurizr-objects
Use Objects to compose the C4 model. Based on [Structurizr For .Net](https://github.com/structurizr/dotnet).

## Why?
* [Structurizr For .Net](https://github.com/structurizr/dotnet) allows to adapt one workspace. From a standpoint of a Software Architect (one team) this is sufficient, but there is no way to create a separate workspace re-using the models of another workspace. With structurizr-objects you can create a project with classes. Another project can import this project and reuse those model items in their workspace. Changes to the refered project are reflected in the refering project.
* With [Structurizr For .Net](https://github.com/structurizr/dotnet) removed model items are not taken care of during the merge, only added and changed items are taken into account during merge (when this option is chosen). When you don't choose merge the entire workspace is replaced, loosing the layout of the created views (items remain but got new id's, layout is coupled by id's)
