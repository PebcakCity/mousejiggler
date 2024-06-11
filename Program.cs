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
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        public static int Main(string[] args)
        {
            // Attach to console for text output if running in console
            PInvoke.AttachConsole(dwProcessId: Helpers.AttachParentProcess);

            Console.WriteLine("Loading config...");
            var config = Config.Read();
            bool? saveSettings = null;
            
            // Mutex to allow only single running app instance
            var instance = new Mutex(initiallyOwned: false, name: "Pebcak.MouseJiggler");
            try
            {
                if ( instance.WaitOne(0) )
                {
                    var rootCommand = new RootCommand()
                    {
                        Description = "Virtually jiggles the mouse, making the computer seem not idle." +
                            " All options default to unset.  Setting any to an explicit value will override the" +
                            " corresponding setting in the config file for this run.  If --save-settings is true, the" +
                            " settings specified will be updated in your config before starting.  If set to false," +
                            " this will disable saving settings completely.  If left unset, settings are saved upon" +
                            " program exit."
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

                    Option<bool?> saveConfig = new (aliases: ["--save-settings", "-s"], description: "Unset: Save program settings at exit.  True: Save these settings now.  False: Disable saving settings.");
                    saveConfig.SetDefaultValue(null);
                    rootCommand.AddOption(saveConfig);

                    // Merge in commandline options, replacing config settings for commandline options that are set
                    rootCommand.SetHandler(( j, m, z, i, s ) =>
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
                        saveSettings = s;
                    },
                    startJiggling, startMin, startZen, interval, saveConfig);

                    // Parse commandline options
                    rootCommand.Invoke(args);

                    // Just print the help message/version & exit if any of these present
                    if ( args.Contains("-h") || args.Contains("--help") ||
                         args.Contains("-?") || args.Contains("/?") ||
                         args.Contains("--version") )
                    {
                        saveSettings = false;
                        return 0;
                    }

                    /* If true, commandline options for this run are saved to
                     * config for next run.  If null, settings are saved at
                     * program exit. If false, config is left untouched. */
                    if ( saveSettings == true )
                    {
                        Console.WriteLine("Saving config...");
                        config.Write();
                    }

                    ApplicationConfiguration.Initialize();
                    Application.Run(new MainForm(config));

                    return 0;
                }
                else
                {
                    Console.WriteLine("Mouse Jiggler is already running. Aborting.");
                    saveSettings = false;
                    return 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong: {ex}", "Oops, my bad...", MessageBoxButtons.OK, MessageBoxIcon.Error );
                saveSettings = false;
                return -1;
            }
            finally
            {
                instance.Close();

                if ( saveSettings is null )
                {
                    Console.WriteLine("Saving config...");
                    config.Write();
                }
                Console.WriteLine("Press <ENTER> to exit.");
                PInvoke.FreeConsole();
            }
        }
    }
}
