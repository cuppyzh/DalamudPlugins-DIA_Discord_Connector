using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Cuppyzh.DalamudPlugins.Diamondway.Models;
using Cuppyzh.DalamudPlugins.Diamondway.Services;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Logging;
using Dalamud.Plugin;
using Newtonsoft.Json;

namespace Cuppyzh.DalamudPlugins.Diamondway
{
    public class Diamondway : IDalamudPlugin
    {
        public string Name => "Diamondway";
        public static AppConfigModel AppConfig;
        public static DiscordServices DiscordServices = new DiscordServices();

        public Diamondway(DalamudPluginInterface dalamudPluginInterface)
        {
            _LoadConfig();

            dalamudPluginInterface.Create<PluginServices>();
            PluginServices.Diamondway = this;
            PluginServices.chatGui.ChatMessage += _ChatMessage;

            DiscordServices.Startup().GetAwaiter().GetResult();
        }

        private void _LoadConfig()
        {
            try
            {
                string configFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "AppConfig.json");

                PluginLog.LogDebug($"Find config file with path: {configFile}");

                using (StreamReader streamReader = new StreamReader(configFile))
                {
                    string json = streamReader.ReadToEnd();
                    AppConfig = JsonConvert.DeserializeObject<AppConfigModel>(json);
                }
            }
            catch (Exception ex)
            {
                PluginLog.LogError(ex.Message);
                PluginLog.LogError(ex.StackTrace);
                _Dispose();
            }
        }

        private void _Dispose()
        {
            PluginLog.LogInformation("Diamondway is shuting down....");
            PluginServices.chatGui.ChatMessage -= _ChatMessage;
        }

        private void _ChatMessage(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
        {
            if (type != XivChatType.FreeCompany)
            {
                return;
            }

            DiscordServices.SendMessage(sender.TextValue, message.TextValue);
        }
    }
}
