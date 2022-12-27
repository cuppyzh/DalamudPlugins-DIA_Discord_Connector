using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dalamud.Logging;
using Discord;
using Discord.WebSocket;

namespace Cuppyzh.DalamudPlugins.Diamondway.Services
{
    public class DiscordServices
    {
        private DiscordSocketClient _discordClient;

        public DiscordServices()
        {

        }

        public async Task Startup()
        {
            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };

            _discordClient = new DiscordSocketClient(config);
            await _discordClient.LoginAsync(TokenType.Bot, Diamondway.AppConfig.DiscordBotToken);
            await _discordClient.StartAsync();
            await Task.Delay(Timeout.Infinite);
        }

        public void SendMessage(string sender, string message)
        {
            PluginLog.Information($"{sender}: {message}");
        }
    }
}
