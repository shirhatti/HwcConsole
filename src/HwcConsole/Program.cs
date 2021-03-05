using HwcConsole.Cli.Utils;
using HwcConsole.Properties;
using Microsoft.Extensions.CommandLineUtils;
using System;
using System.IO;
using System.Threading;
using System.Xml;

namespace HwcConsole
{
    public class Program
    {
        private static readonly string appHostConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "applicationHost.config");
        public static void GenerateAppHostConfig(string sitePath)
        {
            var appHostconfig = Resources.applicationHostConfig;

            var document = new XmlDocument();
            document.LoadXml(appHostconfig);

            var vDirNode = document.SelectSingleNode("configuration/system.applicationHost/sites/site[@id='1']/application/virtualDirectory[@path='/']");
            vDirNode.Attributes["physicalPath"].Value = sitePath;

            document.Save(appHostConfigPath);
        }
        public static int Main(string[] args)
        {
            var exitEvent = new ManualResetEvent(false);
            Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e)
            {
                e.Cancel = true;
                exitEvent.Set();
            };
            var app = new CommandLineApplication
            {
                Name = "hwc",
                FullName = "Hostable Web Core Console",
                Description = "Commandline Utility for running IIS Hostable Web Core"
            };
            app.HelpOption("-h|--help");

            var sitePath = app.Option("-s|--site-path", "The path to your website", CommandOptionType.SingleValue);

            app.OnExecute(() =>
            {
                var sitePathValue = sitePath.Value();
                if (sitePathValue == null)
                {
                    app.ShowHelp();
                    return 2;
                }
                if (!Directory.Exists(sitePathValue))
                {
                    Reporter.Error.WriteLine("Directory does not exist".Red());
                    return 2;
                }

                GenerateAppHostConfig(sitePathValue);
                var server = new Server(appHostConfigPath);
                server.Start();
                Reporter.Output.WriteLine("Listening. Press Ctrl + C to stop listening...");
                exitEvent.WaitOne();
                Reporter.Output.WriteLine("Exiting");
                server.Stop();
                return 0;
            });

            try
            {
                return app.Execute(args);
            }
            catch (Exception e)
            {
                Reporter.Error.WriteLine(e.Message.Red());
                Reporter.Output.WriteLine(e.ToString().Yellow());
            }

            return 1;
        }
    }
}
