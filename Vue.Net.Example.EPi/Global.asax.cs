using System;
using System.Web.Mvc;
using Vue.Net.WebComponents;

namespace Vue.Net.Example.EPi
{
    public class EPiServerApplication : EPiServer.Global
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //VueDotNet.GetScriptHashes();
            //Tip: Want to call the EPiServer API on startup? Add an initialization module instead (Add -> New Item.. -> EPiServer -> Initialization Module)
        }
    }
}