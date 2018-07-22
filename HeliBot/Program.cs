using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;


//Dev Client Token: NDY4MjQ0NjIyMzkzNjA2MTQ0.Di2WTw.zEIvXrtNUtxtJQaYd9kP_NMkaxY
// standard Client ID: 460187958306406400
// Dev Client ID: 468244622393606144
// Token: NDYwMTg3OTU4MzA2NDA2NDAw.DhBISA.mk8YO7X14HdeCMZfaRh0IrTmySI
namespace HeliBot
{
	class Program
	{
		DiscordSocketClient _client;
		Handlers _handler;

		static void Main(string[] args)
		=> new Program().StartAsync().GetAwaiter().GetResult();


		public async Task StartAsync()
		{
			if (Config.bot.token == "" || Config.bot.token == null) return;
			_client = new DiscordSocketClient(new DiscordSocketConfig 
			{
				LogLevel = LogSeverity.Verbose
			});
			_client.Log += Log;
			await _client.LoginAsync(TokenType.Bot, Config.bot.token);
			await _client.StartAsync();
			_handler = new Handlers();
			await _handler.InitializeAsync(_client);
			await Task.Delay(-1);
		}

		private async Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.Message);
		}
	}
}
