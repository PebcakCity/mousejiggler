using System.CommandLine;
using System.CommandLine.Parsing;
using System.Diagnostics;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace PebcakCity.MouseJiggler
{
    internal static class Program
    {
        // Set these to the same as the min/max of the upDownInterval NumericUpDown
        internal static int IntervalMin = 1;
        internal static int IntervalMax = 3600;

        private static Config? GetConfig( string[] args )
        {
            Console.WriteLine("Loading config from file...");
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
                        Console.WriteLine("An instance is already running.");

                        // Try to show the current running instance
                        Process thisInstance = Process.GetCurrentProcess();
                        var otherInstance = Process.GetProcessesByName(thisInstance.ProcessName).Where(p => p.Id != thisInstance.Id).FirstOrDefault();
                        if ( otherInstance != null )
                        {
                            // Unfortunately, this only works if the window is actually visible (ie. not minimized to tray)
                            // Otherwise, it returns 0 and neither ShowWindow nor SetForegroundWindow do anything, nor
                            // is there anything .NET can do to get the Form/MainForm from the handle and make it visible.
                            // As a workaround, maybe try these?:
                            // https://stackoverflow.com/questions/67058960/getting-hwnd-of-a-hidden-window
                            // https://stackoverflow.com/questions/16185217/c-sharp-process-mainwindowhandle-always-returns-intptr-zero

                            var handle = otherInstance.MainWindowHandle;
                            if ( handle != 0 )
                            {
                                HWND hWnd = new(handle);
                                //PInvoke.ShowWindow(hWnd, SHOW_WINDOW_CMD.SW_NORMAL);
                                PInvoke.SetForegroundWindow(hWnd);
                            }
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong: {ex}", "Oops, my bad...", MessageBoxButtons.OK, MessageBoxIcon.Error );
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
