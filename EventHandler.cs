using Exiled.Events.EventArgs.Player;
using MEC;
using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using BetterTesla;
using Feature = Exiled.API.Features;
using Exiled.API.Enums;
using Exiled.API.Features.Doors;
using System.Linq;
using UnityEngine;

namespace BetterTesla
{
    public class EventHandler
    {
        public static EventHandler EventHandlers;

        public Dictionary<Feature.TeslaGate, int> TeslaActivations = new();
        private Dictionary<Feature.TeslaGate, float> _lastTeslaTriggerTime = new();


        private CoroutineHandle _crazyTeslaHandle;
        private bool _crazyTeslaRunning = false;
        public bool _isForced = false;

        public void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            var Config = Main.Instance.Config;

            // Advance
            if (Config.AdvanceTesla && ev.Player.CurrentItem != null && Config.RequiredItems.Contains(ev.Player.CurrentItem.Type))
            {
                ev.IsAllowed = false;
                string hint = Config.Hint.Replace("%Role%", "[REDACTED]");
                ev.Player.ShowHint(hint, 3);
                Log.Debug($"[BetterTesla] Tesla disabled for {ev.Player.Nickname}");
                return;
            }

            // Crazy 
            if (Config.CrazyTesla && !_crazyTeslaRunning)
            {
                _crazyTeslaRunning = true;
                _crazyTeslaHandle = Timing.RunCoroutine(CrazyTesla());
                Log.Debug("[CrazyTesla] Started CrazyTesla coroutine.");

/*                Timing.CallDelayed(Config.CrazyTeslaDuration, () =>
                {
                    if (_crazyTeslaRunning)
                    {
                        Timing.KillCoroutines(_crazyTeslaHandle);
                        _crazyTeslaRunning = false;
                        Log.Debug("[CrazyTesla] Coroutine stopped after duration.");
                    }
                });*/
            }
            // Overcharge
            if (Config.OverchargedTesla)
            {
                if (!_lastTeslaTriggerTime.ContainsKey(ev.Tesla))
                    _lastTeslaTriggerTime[ev.Tesla] = 0f;

                float currentTime = Time.time;

                if (currentTime - _lastTeslaTriggerTime[ev.Tesla] >= 15f)
                {
                    _lastTeslaTriggerTime[ev.Tesla] = currentTime;

                    if (!TeslaActivations.ContainsKey(ev.Tesla))
                        TeslaActivations[ev.Tesla] = 0;

                    TeslaActivations[ev.Tesla]++;
                    Log.Debug($"[BetterTesla] Tesla {ev.Tesla} activations: {TeslaActivations[ev.Tesla]}");

                    if (TeslaActivations.Values.Sum() > Config.MinOverchargeActivations)
                    {
                        List<ZoneType> zones = new()
                        {
                            ZoneType.Surface,
                            ZoneType.HeavyContainment,
                            ZoneType.LightContainment,
                            ZoneType.Entrance
                        };
                        Map.TurnOffAllLights(Config.BlackoutDuration, zones);
                        Cassie.MessageTranslated(Config.BlackoutCassieContent, Config.BlackoutCassieMessage);

                        foreach (var doors in Door.List)
                        {
                            doors.Lock(DoorLockType.Lockdown079);
                            doors.IsOpen = false;
                        }

                        Timing.CallDelayed(Config.BlackoutDuration, UnlockAllDoorsAndClear);
                    }
                }
            }               
        }
        public void UnlockAllDoorsAndClear()
        {
            foreach (var door in Door.List)
            {
                door.Unlock();
            }
            TeslaActivations.Clear();
        }

        // Start Coroutine
        public IEnumerator<float> CrazyTesla()
        {
            var Config = Main.Instance.Config;

            // "Normal Activation"
            if (!_isForced)
            {
                yield return Timing.WaitForSeconds(Config.CrazyTeslaInterval);

                Cassie.MessageTranslated(Config.CassieContent, Config.CassieMessage);
                for (int i = 0; i < 4; i++)
                {
                    Log.Debug("[Crazy Tesla] i added 1, now is " + i);
                    foreach (var tesla in Feature.TeslaGate.List)
                    {
                        tesla.Trigger();
                    }
                    yield return 5f;  
                }
                Log.Debug($"[Crazy Tesla] CrazyTesla has been activated , Normal Activation ");
            }
            // Activation By Command
            else
            {
                yield return Timing.WaitForSeconds(3);
                Cassie.MessageTranslated(Config.CassieContent, Config.CassieMessage);
                for (int i = 0; i < 4; i++)
                {
                    Log.Debug("[Crazy Tesla] i added 1, now is " + i);
                    foreach (var tesla in Feature.TeslaGate.List)
                    {
                        tesla.Trigger();
                    }
                    yield return Timing.WaitForSeconds(7);  
                }
                Log.Debug($"[Crazy Tesla] CrazyTesla has been activated by command ");
                _isForced = false;
            }
            _crazyTeslaRunning = false;
            yield break;
        }



        public void OnRoundEnd(RoundEndedEventArgs ev)
        {
            if (_crazyTeslaRunning)
            {
                Timing.KillCoroutines(_crazyTeslaHandle);
                _crazyTeslaRunning = false;
                Log.Debug("[CrazyTesla] Coroutine stopped on round end.");
            }
            TeslaActivations.Clear();
        }

        public bool IsCrazyTeslaRunning => _crazyTeslaRunning;
   
        public void ForceOverchargedTesla()
        {
            var Config = Main.Instance.Config;

            List<ZoneType> zones = new()
            {
                ZoneType.Surface,
                ZoneType.HeavyContainment,
                ZoneType.LightContainment,
                ZoneType.Entrance
            };

            Map.TurnOffAllLights(Config.BlackoutDuration, zones);
            Cassie.MessageTranslated(Config.BlackoutCassieContent, Config.BlackoutCassieMessage);

            foreach (var door in Door.List)
            {
                door.Lock(DoorLockType.Lockdown079);
                door.IsOpen = false;
            }

            Timing.CallDelayed(Config.BlackoutDuration, UnlockAllDoorsAndClear);
        }
    }
}
