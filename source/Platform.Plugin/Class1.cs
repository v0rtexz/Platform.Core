using System.Numerics;
using ImGuiNET;
using Platform.Data.Events;
using Platform.Data.Game.Components;
using Platform.Data.Game.Types;
using Platform.Data.Modules.Interfaces;
using Platform.Data.Rendering;
using Serilog;

namespace Platform.Plugin
{
    public class Class1 : IScript
    {
        public string Name
        {
            get => "Test Plugin";
        }

        private ILogger logger;

        public void OnLoad()
        {
            this.logger.Debug(Name + " loaded");
        }

        public Class1()
        {
        }

        public Class1(ILogger logger)
        {
            this.logger = logger;

            this.logger.Write(Serilog.Events.LogEventLevel.Information, "Hello from first assembly");

            EventManager.RegisterCallback<EventDelegate.EvtOnUpdate>(OnTick);
        }

        public void OnTick()
        {
            AIBaseClient player = ComponentController.GetComponent<World>().GetLocalPlayer();

            Drawing.Add3DCircle(player.Pos, player.AttackRange);

            Vector2 screenPos = Vector2.Zero;
            ComponentController.GetComponent<Renderer>().WorldToScreen(player.Pos, ref screenPos);

            Drawing.AddText(player.GetName().ToUpper(),screenPos, 1.5f);

            string opAFText = "LeagueSharp (SoonTM, OP af, spinning to victory GGWP EZ)";
            Vector2 screenSize = ImGui.GetIO().DisplaySize;
            float textWidth = ImGui.CalcTextSize(opAFText).X;
            Vector2 textPos = new Vector2((screenSize.X - textWidth) / 2, 20);
            Drawing.AddText(opAFText, textPos, 1.5f);
        }
    }
}