using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale.Multiplayer.Server
{
    public class ServerCommandLineArgs
    {
        public ushort Port { get; private set; } = 7777;
        public string ListenIp { get; private set; } = "0.0.0.0";
        public ushort MaxPlayers { get; private set; } = 8;
        public bool IsBatchMode { get; private set; }

        public ServerCommandLineArgs(string[] args)
        {
            ParseArguments(args);
        }

        public static ServerCommandLineArgs ParseFromSystem()
        {
            return new ServerCommandLineArgs(Environment.GetCommandLineArgs());
        }

        private void ParseArguments(string[] args)
        {
            if (args == null) return;

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i].ToLower();

                if (arg == "-batchmode")
                {
                    IsBatchMode = true;
                }
                else if (arg == "-port" && i + 1 < args.Length)
                {
                    if (ushort.TryParse(args[i + 1], out ushort parsedPort)) Port = parsedPort;
                }
                else if (arg == "-ip" && i + 1 < args.Length)
                {
                    ListenIp = args[i + 1];
                }
                else if (arg == "-maxplayers" && i + 1 < args.Length)
                {
                    if (ushort.TryParse(args[i + 1], out ushort parsedMax)) MaxPlayers = parsedMax;
                }
            }
        }
    }
}
