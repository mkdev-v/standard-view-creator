using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace StandardViewCreator
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Standard View Creator"),
        ExportMetadata("Description", "Create a user view with standard fields commonly used by administrators, all in one go."),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "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"),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "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"),
        ExportMetadata("BackgroundColor", "Lavender"),
        ExportMetadata("PrimaryFontColor", "Black"),
        ExportMetadata("SecondaryFontColor", "Gray")]
    public class StandardViewCreator : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new StandardViewCreatorControl();
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        public StandardViewCreator()
        {
            // If you have external assemblies that you need to load, uncomment the following to 
            // hook into the event that will fire when an Assembly fails to resolve
            // AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
        }

        /// <summary>
        /// Event fired by CLR when an assembly reference fails to load
        /// Assumes that related assemblies will be loaded from a subfolder named the same as the Plugin
        /// For example, a folder named Sample.XrmToolBox.MyPlugin 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly loadAssembly = null;
            Assembly currAssembly = Assembly.GetExecutingAssembly();

            // base name of the assembly that failed to resolve
            var argName = args.Name.Substring(0, args.Name.IndexOf(","));

            // check to see if the failing assembly is one that we reference.
            List<AssemblyName> refAssemblies = currAssembly.GetReferencedAssemblies().ToList();
            var refAssembly = refAssemblies.Where(a => a.Name == argName).FirstOrDefault();

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (refAssembly != null)
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToLower();
                string folder = Path.GetFileNameWithoutExtension(currAssembly.Location);
                dir = Path.Combine(dir, folder);

                var assmbPath = Path.Combine(dir, $"{argName}.dll");

                if (File.Exists(assmbPath))
                {
                    loadAssembly = Assembly.LoadFrom(assmbPath);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to locate dependency: {assmbPath}");
                }
            }

            return loadAssembly;
        }
    }
}