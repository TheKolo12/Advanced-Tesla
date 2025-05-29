using CommandSystem;
using System;
using Exiled.API.Features;
using BetterTesla;
using MEC;

namespace BetterTesla.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ForceEvent : ICommand
    {
        public string Command => "forceevent";
        public string[] Aliases => new[] { "fe" };
        public string Description => "Force an event from the BetterTesla plugin.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var config = Main.Instance.Config;
            var handler = Main.Instance._eventHandler;

            if (config == null || handler == null)
            {
                response = "BetterTesla is not initialized properly.";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Usage: forceevent <crazy | overcharged>";
                return false;
            }

            switch (arguments.At(0).ToLower())
            {
                case "crazy":
                    if (!config.CrazyTesla)
                    {
                        response = "CrazyTesla is disabled in the config.";
                        return false;
                    }
                    Main.Instance._eventHandler._isForced = true;
                    Timing.RunCoroutine(handler.CrazyTesla());
                    response = "CrazyTesla started successfully.";
                    return true;

                case "overcharged":
                    if (!config.OverchargedTesla)
                    {
                        response = "OverchargedTesla is disabled in the config.";
                        return false;
                    }

                    handler.ForceOverchargedTesla();
                    response = "OverchargedTesla triggered successfully.";
                    return true;

                default:
                    response = "Invalid event type. Use 'crazy' or 'overcharged'.";
                    return false;
            }
        }
    }
}
