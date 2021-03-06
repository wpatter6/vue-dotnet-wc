![Vue dot net](https://raw.githubusercontent.com/wpatter6/vue-dotnet-wc/master/vue-dotnet-icon.png)

# Vue.Net.WebComponents

![Nuget Status](https://buildstats.info/nuget/Vue.Net.WebComponents)

Simple integration of Vue.js web components and .NET standard MVC

- `IVueComponent` interface is used for building C# objects that can be rendered into Vue web components.
- `IVueComponent.RenderComponent` extension method will render the HTML for the Vue web component when a class implements the `IVueComponent` interface.
- Various parts of the `IVueComponent` interface have been isolated and have their own extension methods now, in cases where only partial rendering of the component is needed.
- Configuration transform and `VueConfig` utility class are used to easily define where Vue script files live and what components are available in configuration files. - Web.Config section `<vueConfig>` for .NET Framework 4.6.2+ - vuesettings.config for dotnet core projects.
- `VueDotNet.RenderVueScripts` method can be used to easily define where Vue scripts should be rendered
- See example projects for basic configuration and implementation examples.

## Getting Started

- Clone this repository
- Make sure node.js is installed, and dotnet core 2.2
- Open command line to vue-dotnet-wc-example-ts
- Run `npm install` followed by `npm run build` and verify no errors in console. Read that project's readme.md for more options on the vue side.
- Open the solution file in Visual Studio
- Restore, build and run the Vue.Net.Examples.Core. Read that project's readme.md for more options on the dotnet side.
- If you see the Vue.js logo and the default hello world, everything is working!
