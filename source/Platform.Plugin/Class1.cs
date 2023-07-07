using System.Numerics;
using Ensage.Data.Events;
using Ensage.Data.Game;
using Ensage.Data.Game.Components;
using Ensage.Data.Game.Types;
using Ensage.Data.Modules.Interfaces;
using Ensage.Data.Rendering;
using ImGuiNET;
using Serilog;

namespace Ensage.Plugin
{
    public class Class1 : IScript
    {
        public string Name
        {
            get => "Test Plugin";
        }

        private ILogger logger;
        private World world;
        private Engine engine;

        public void OnLoad()
        {
            this.logger.Debug(Name + " loaded");
        }

        public Class1()
        {
        }

        public Class1(ILogger logger, World world, Engine engine)
        {
            this.logger = logger;
            this.world = world;

            this.logger.Write(Serilog.Events.LogEventLevel.Information, "Hello from first assembly");

            EventManager.RegisterCallback<EventDelegate.EvtOnUpdate>(OnTick);
        }

        public void OnTick()
        {
            ImGui.Begin("Jiingz Jinx");

            ImGui.End();

            AIBaseClient player = world.GetLocalPlayer();

            Drawing.Add3DCircle(player.Pos, player.AttackRange);

            Vector2 screenPos = Vector2.Zero;
            Renderer.WorldToScreen(player.Pos, ref screenPos);

           // Drawing.AddShaderText(player.ObjectName.ToUpper(), screenPos, 1.5f);

            string opAFText = "LeagueSharp (SoonTM, OP af, spinning to victory GGWP EZ)";
            Vector2 screenSize = ImGui.GetIO().DisplaySize;
            float textWidth = ImGui.CalcTextSize(opAFText).X;
            Vector2 textPos = new Vector2((screenSize.X - textWidth) / 2, 20);
           // Drawing.AddShaderText(opAFText, textPos, 1.5f);
        }
    }
}