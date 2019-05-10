using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Vue.Net.WebComponents.VueStartup))]
namespace Vue.Net.WebComponents
{
    public class VueStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {

        }
    }
}
