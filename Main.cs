using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.Events.Handlers;
using Exiled.API;
using Exiled.API.Interfaces;
using Exiled.API.Features;

namespace AdvancedTesla
{

    public class Main : Plugin<Config>
    {

        public override string Name { get; } = "Advanced Tesla";
        public override string Author { get; } = "Kolo";
        public override string Prefix { get; } = "Advanced Tesla";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(9, 6, 0);

        public static Main Instance;
        public EventHandler _eventHandler;
        public bool IsForced = false;
        public override void OnEnabled()
        {
            Instance = this;

            _eventHandler = new EventHandler();
            RegisterEvents();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnRegisterEvents();
            _eventHandler = null;

            Instance = null;
            base.OnDisabled();

        }
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Player.TriggeringTesla += _eventHandler.OnTriggeringTesla;
        }
        public void UnRegisterEvents()
        {
            Exiled.Events.Handlers.Player.TriggeringTesla -= _eventHandler.OnTriggeringTesla;
        }
    }
}
