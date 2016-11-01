using HwcConsole.Cli.Utils;
using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HwcConsole
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "hwc",
                FullName = "Hostable Web Core Console",
                Description = "Commandline Utility for running IIS Hostable Web Core"
            };
            app.HelpOption("-h|--help");

            var applicationHostConfigPath = app.Option("-p|--config-path", "The path to the ApplicationHost.config file", CommandOptionType.SingleValue);

            app.OnExecute(() =>
            {
                var config = applicationHostConfigPath.Value();
                if (config == null)
                {
                    app.ShowHelp();
                    return 2;
                }
                if (!File.Exists(config))
                {
                    Reporter.Error.WriteLine("File does not exist".Red());
                    return 2;
                }

                var server = new Server(Path.GetFullPath(config));
                server.Run();
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
