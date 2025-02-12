﻿using System;

using DiscordRPC;
using DiscordRPC.Logging;
using DiscordRPC.Message;
using Dalamud.Logging;

namespace Dalamud.RichPresence.Managers
{
    internal class DiscordPresenceManager : IDisposable
    {
        private const string DISCORD_CLIENT_ID = "478143453536976896";
        private DiscordRpcClient RpcClient;

        internal DiscordPresenceManager()
        {
            this.CreateClient();
        }

        private void CreateClient()
        {
            if (RpcClient is null || RpcClient.IsDisposed)
            {
                // Create new RPC client
                RpcClient = new DiscordRpcClient(DISCORD_CLIENT_ID);

                PluginLog.LogInformation("Creating RPC client.");

                // Skip identical presences
                RpcClient.SkipIdenticalPresence = true;

                // Set logger
                RpcClient.Logger = new DiscordPresenceLogger();

                RpcClient.ShutdownOnly = true;
                RpcClient.OnRpcMessage += (sender, e) => { PluginLog.LogVerbose("RPC message: " + e.Type + " at " + e.TimeCreated); };
                RpcClient.OnConnectionEstablished += (sender, e) => { PluginLog.LogVerbose("RPC connection established. Waiting for ready signal."); };
                RpcClient.OnError += (sender, e) => { PluginLog.LogError("RPC transmission error: " + e.Message); };
                RpcClient.OnConnectionFailed += (sender, e) => { PluginLog.LogError("RPC connection failed on pipe " + e.FailedPipe + " - " + e.Type.ToString()); };
                RpcClient.OnReady += (sender, e) => { PluginLog.LogInformation("RPC connected and ready to transmit."); };
                // Subscribe to events
                RpcClient.OnPresenceUpdate += (sender, e) => { PluginLog.LogVerbose("Recieved RPC update: " + e.Name); };
            }

            if (!RpcClient.IsInitialized)
            {
                // Connect to the RPC
                if (!RpcClient.Initialize())
                {
                    PluginLog.Error("Failed to initialize Discord RPC client.");
                }
                else PluginLog.LogInformation("Initialised RPC client.");
            }
        }

        public void SetPresence(DiscordRPC.RichPresence newPresence)
        {
            this.CreateClient();
            RpcClient.SetPresence(newPresence);
        }

        public void ClearPresence()
        {
            this.CreateClient();
            RpcClient.ClearPresence();
        }

        public void UpdatePresenceDetails(string details)
        {
            this.CreateClient();
            RpcClient.UpdateDetails(details);
        }

        public void UpdatePresenceStartTime(DateTime newStartTime)
        {
            this.CreateClient();
            RpcClient.UpdateStartTime(newStartTime);
        }

        public void Dispose()
        {
            RpcClient?.Dispose();
        }
    }
}