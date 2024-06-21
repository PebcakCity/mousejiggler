using System.CommandLine;
using System.CommandLine.Parsing;

using Windows.Win32;

namespace PebcakCity.MouseJiggler
{
    internal static class Program
    {
        // Set these to the same as the min/max of the upDownInterval NumericUpDown
        internal static int IntervalMin = 1;
        internal static int IntervalMax = 3600;


        /// <summary>
        /// Reads the app config from a file, then parses any options that were
        /// passed on the commandline.  Commandline options override options
        /// loaded from the config.
        /// </summary>
        /// <returns>The merged config</returns>
        private static Config? GetConfig( string[] args )
        {
            Console.WriteLine("\r\nLoading config from file...");
            var config = Config.Read();

            var rootCommand = new RootCommand()
            {
                Description = "Virtually jiggles the mouse, making the computer seem not idle." +
                            " All options default to unset.  Setting any to an explicit value will override the" +
                            " corresponding setting in the config file for this run."
            };

            Option<bool?> startJiggling = new (aliases: ["--jiggle", "-j"], description: "Start with jiggling enabled (true) / disabled (false).");
            startJiggling.SetDefaultValue(null);
            rootCommand.AddOption(startJiggling);

            Option<bool?> startMin = new (aliases: ["--minimized", "-m"], description: "Start minimized (true) / unminimized (false).");
            startMin.SetDefaultValue(null);
            rootCommand.AddOption(startMin);

            Option<bool?> startZen = new (aliases: ["--zen", "-z"], description: "Use invisible jiggling method (true) or don't (false).");
            startZen.SetDefaultValue(null);
            rootCommand.AddOption(startZen);

            Option<int?> interval = new (aliases: ["--interval", "-i"], description: "Specify jiggle time interval in seconds.");
            interval.SetDefaultValue(null);
            rootCommand.AddOption(interval);

            // Merge in commandline options, replacing config settings for commandline options that are set
            rootCommand.SetHandler(( j, m, z, i ) =>
            {
                config.JiggleOnStartup = j ?? config.JiggleOnStartup;
                config.MinimizeOnStartup = m ?? config.MinimizeOnStartup;
                config.ZenJiggle = z ?? config.ZenJiggle;
                if ( i is not null )
                {
                    if ( (int)i >= IntervalMin && (int)i <= IntervalMax )
                    {
                        config.JiggleInterval = (int)i;
                    }
                    else if ( (int)i < IntervalMin )
                    {
                        Console.WriteLine($"Interval {i} < minimum, setting interval = {IntervalMin}");
                        config.JiggleInterval = IntervalMin;
                    }
                    else if ( (int)i > IntervalMax )
                    {
                        Console.WriteLine($"Interval {i} > maximum, setting interval = {IntervalMax}");
                        config.JiggleInterval = IntervalMax;
                    }
                }
            },
            startJiggling, startMin, startZen, interval);

            // Parse commandline options
            rootCommand.Invoke(args);

            // If we wanted help or version info, return null after it prints.
            if ( args.Contains("-h") || args.Contains("--help") ||
                 args.Contains("-?") || args.Contains("/?") ||
                 args.Contains("--version") )
            {
                return null;
            }

            // Otherwise, return the merged configs & get ready to start the app.
            return config;
        }


        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        public static int Main(string[] args)
        {
            // Attach to console for text output if running in console
            PInvoke.AttachConsole(dwProcessId: Helpers.AttachParentProcess);

            var config = GetConfig(args);

            // Mutex to allow only single running app instance
            var instance = new Mutex(initiallyOwned: false, name: "PebcakCity.MouseJiggler");
            try
            {
                if ( config is not null )
                {
                    if ( instance.WaitOne(0) )
                    {
                        // Start new instance
                        ApplicationConfiguration.Initialize();
                        Application.Run(new MainForm(config));
                    }
                    else
                    {
                        MessageBox.Show("An instance is already running.", "Mouse Jiggler",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong: {ex}", "Oops, my bad...",
                    MessageBoxButtons.OK, MessageBoxIcon.Error );
                return -1;
            }
            finally
            {
                instance.Close();
                Console.WriteLine("Press <ENTER> to exit.");
                PInvoke.FreeConsole();
            }
        }
    }
}
