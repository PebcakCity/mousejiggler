using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace PebcakCity.MouseJiggler
{
    public class Config
    {
        private string? Filename { get; set; }
        private const string DefaultConfig = "MouseJiggler.json";

        public bool JiggleOnStartup { get; set; } = false;
        public bool MinimizeOnStartup { get; set; } = false;
        public bool ZenJiggle { get; set; } = false;
        public int JiggleInterval { get; set; } = 60;

        public Config() { }
        internal static Config Read(string filename = DefaultConfig)
        {
            try
            {
                using FileStream json = new (filename, FileMode.Open);
                JsonNode? root = JsonNode.Parse(json);
                Config config = JsonSerializer.Deserialize<Config>(root)!;
                config.Filename = filename;
                return config;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading config: {ex.Message}");
                return new Config();
            }
        }

        internal void Write()
        {
            string assemblyDir = System.AppContext.BaseDirectory;
            this.Filename ??= Path.Combine(assemblyDir, DefaultConfig);
            try
            {
                using FileStream json = new (this.Filename, FileMode.Create, FileAccess.Write);
                var jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };
                JsonSerializer.Serialize(json, this, jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing config: {ex}");
            }
        }
    }
}
