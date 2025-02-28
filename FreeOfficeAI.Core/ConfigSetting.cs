using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeOfficeAI.Core
{
    public class ConfigSetting
    {
        private const string fileName = "ConfigSetting.json";

        private static ConfigSetting Instance { get; set; }

        public bool IsLocal { get; set; }

        public LLMFrame Frame { get; set; }

        public string ApiUrl { get; set; }

        public string ModelName { get; set; }

        public static ConfigSetting GetSetting()
        {
            if (Instance == null)
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                string filename = Path.Combine(path, fileName);
                if (File.Exists(filename))
                {
                    string json = File.ReadAllText(filename);
                    ConfigSetting setting = JsonSerializer.Deserialize<ConfigSetting>(json);

                    Instance = setting;

                    return setting;
                }
                else
                {
                    return new ConfigSetting()
                    {
                        IsLocal = true,
                        Frame = LLMFrame.Ollama,
                        ApiUrl = "http://10.6.100.112:11434",  // "http://localhost:11434",
                        ModelName = "deepseek-r1:14b",  //"codellama:34b",  // "deepseek-r1:1.5b",
                        //ModelName_Excel = "qwen2.5-coder:32b",
                    };
                }
            }
            else
                return Instance;
        }

        public static bool SaveSetting(ConfigSetting setting, out string errorMsg)
        {
            errorMsg = "";
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                string filename = Path.Combine(path, fileName);
                string json = JsonSerializer.Serialize(setting);
                File.WriteAllText(filename, json);

                Instance = setting;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }

            return true;
        }
    }

    public enum LLMFrame
    {
        Ollama,
    }
}
