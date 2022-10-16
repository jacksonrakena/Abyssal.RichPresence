using DiscordRPC.Logging;
using Dalamud.Logging;
public class DiscordPresenceLogger : ILogger
{
    public LogLevel Level { get { return LogLevel.Trace; } set { } }

    public void Error(string message, params object[] args) => PluginLog.LogError(message, args);

    public void Info(string message, params object[] args) => PluginLog.LogInformation(message, args);

    public void Trace(string message, params object[] args) => PluginLog.LogVerbose(message, args);

    public void Warning(string message, params object[] args) => PluginLog.LogWarning(message, args);
}