using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Usables;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API;
using Exiled.API.Features;

namespace AdvancedTesla
{
    public class EventHandler
    {
        public static EventHandler EventHandlers;
        public Dictionary<Exiled.API.Features.TeslaGate, CoroutineHandle> CrazyTesla = new Dictionary<Exiled.API.Features.TeslaGate, CoroutineHandle>();
        public void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            // Advanced Tesla
            if (Main.Instance.Config.AdvanceTesla)
            {
                if (ev.Player.CurrentItem != null && Main.Instance.Config.RequiredItems.Contains(ev.Player.CurrentItem.Type))
                {
                    ev.IsAllowed = false;
                    string Hint = Main.Instance.Config.Hint.Replace("%Role%", "[REDACTED]");
                    ev.Player.ShowHint(Hint, 3);
                }
                if (Main.Instance.Config.CrazyTesla)
                {
                    if (!CrazyTesla.ContainsKey(ev.Tesla))
                    {
                        var handle = Timing.RunCoroutine(AdvancedTeslaCoroutine(ev.Tesla));
                        CrazyTesla.Add(ev.Tesla, handle);
                    }
                }

            }
        }
        // This needs to be fixed
        public System.Collections.Generic.IEnumerator<float> AdvancedTeslaCoroutine(Exiled.API.Features.TeslaGate tesla)
        {
            // Advanded Tesla
            if (Main.Instance.Config.CrazyTesla)
            {
                if (Main.Instance.IsForced)
                {
                    // Maybe this is not needed
                    /*if (tesla.IsShocking)
                    {
                        if (CrazyTesla.ContainsKey(tesla))
                        {
                            CrazyTesla.Remove(tesla);
                        }
                        yield break;
                    }*/
                    Log.Debug($"[CrazyTesla] Forced activation triggered for Tesla at {tesla.Position}.");
                    Exiled.API.Features.Cassie.MessageTranslated(Main.Instance.Config.CassieContent, Main.Instance.Config.CassieMessage, false, true, true);

                    yield return Timing.WaitForSeconds(7);

                    tesla.Trigger(true);
                    tesla.Trigger(true);
                    Main.Instance.IsForced = false; 

                }
                while (true)
                {
                    yield return Timing.WaitForSeconds(Main.Instance.Config.CrazyTeslaInterval);

                    // Maybe this is not needed
                    /* (tesla.IsShocking)
                    {
                        if (CrazyTesla.ContainsKey(tesla))
                        {
                            CrazyTesla.Remove(tesla);
                        }
                        yield break;
                    }*/
                    Log.Debug($"[CrazyTesla] Normal activation triggered for Tesla at {tesla.Position}.");
                    Exiled.API.Features.Cassie.MessageTranslated(Main.Instance.Config.CassieContent, Main.Instance.Config.CassieMessage, false, true, true);

                    yield return Timing.WaitForSeconds(7);

                    tesla.Trigger(true);
                    tesla.Trigger(true);
                }

            }
        }
        
    }
}
