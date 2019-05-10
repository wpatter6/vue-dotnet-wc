![Vue dot net](https://raw.githubusercontent.com/wpatter6/vue-dotnet/master/vue-dotnet-icon.png)
#Vue.Net.WebComponents

Simple integration of Vue.js web components and .NET standard MVC

- `IVueComponent` interface is used for building EPiServer blocks with Vue web components
- `IVueComponent.RenderBlock` extension method will render the HTML for the Vue web component when a class implements the `IVueComponent` interface.
- Configuration transform and `VueConfig` utility class are used to easily define where Vue script files live and what components are available in configuration files.
	- Web.Config section `<vueConfig>` for .NET Framework 4.6.2+
	- vuesettings.config for dotnet core projects.
- `HtmlHelper.RenderVueScripts` extension method can be used to easily define where Vue scripts should be rendered
- See example projects for basic configuration and implementation examples.
