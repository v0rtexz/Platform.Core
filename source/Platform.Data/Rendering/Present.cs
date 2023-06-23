#region Copyright (c) Ensage GmbH

// ////////////////////////////////////////////////////////////////////////////////
//
//        Ensage GmbH Source Code
//        Copyright (c) 2020-2023 Ensage GmbH
//        ALL RIGHTS RESERVED.
//
//    The entire contents of this file is protected by German and
//    International Copyright Laws. Unauthorized reproduction,
//    reverse-engineering, and distribution of all or any portion of
//    the code contained in this file is strictly prohibited and may
//    result in severe civil and criminal penalties and will be
//    prosecuted to the maximum extent possible under the law.
//
//    RESTRICTIONS
//
//    THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES
//    ARE CONFIDENTIAL AND PROPRIETARY TRADE SECRETS OF
//    Ensage GMBH.
//
//    THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED
//    FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE
//    COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE
//    AVAILABLE TO OTHER INDIVIDUALS WITHOUT WRITTEN CONSENT
//    AND PERMISSION FROM Ensage GMBH.
// 
// ////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Diagnostics;
using System.Numerics;
using ImGuiScene;
using Platform.Data.Events;
using Platform.Data.Game.Components;
using Platform.Data.Game.Types;

namespace Platform.Data.Rendering;

using Platform.Data.Game.Components;
using Platform.Data.Events;
using ImGuiNET;

/// <summary>
/// Implements the <see cref="Overlay"/> class and creates a overlay window.
/// </summary>
internal class Present
{
    private float lastGameTime; // The last synchronized game time

    public Present()
    {
        Render();
    }

    void Test()
    {
        // Create a Stopwatch to measure the rendering time
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();


        float gameTime = ComponentController.GetComponent<ClockFacade>().GetGameTime();
        float targetFrameRate = 2500f; // TODO: Add the game's FPS count
        float targetFrameTime = 1f / targetFrameRate;

        float deltaTime = gameTime - lastGameTime;


        lastGameTime = gameTime;

        ComponentController.GetComponent<Renderer>().UpdateRenderer();
        EventManager.InvokeCallback<EventDelegate.EvtOnUpdate>();

        AIBaseClient player = ComponentController.GetComponent<World>().GetLocalPlayer();

        Drawing.Add3DCircle(player.Pos, player.AttackRange, true, 2f);

        Vector2 screenPos = Vector2.Zero;
        ComponentController.GetComponent<Renderer>().WorldToScreen(player.Pos, ref screenPos);

        Drawing.AddText(player.GetName().ToUpper(), screenPos, 1.5f);

        string opAFText = "LeagueSharp (SoonTM, OP af, spinning to victory GGWP EZ)";
        Vector2 screenSize = ImGui.GetIO().DisplaySize;
        float textWidth = ImGui.CalcTextSize(opAFText.ToUpper()).X;
        Vector2 textPos = new Vector2((screenSize.X - textWidth) / 2, 20);
        Drawing.AddText(opAFText.ToUpper(), textPos, 1.5f);

        Console.WriteLine(Drawing.DrawQueue.Count);
        if (Drawing.DrawQueue.Count > 0)
        {
            Action drawing = Drawing.DrawQueue.Dequeue();
            drawing.Invoke();
        }


        DebugConsole.Draw();
        RenderMainMenu();

        // Stop the Stopwatch and output the rendering time
        stopwatch.Stop();
        TimeSpan elapsed = stopwatch.Elapsed;
        Console.WriteLine("Rendering time: " + elapsed.TotalMilliseconds + " ms");
    }

    private void Render()
    {
        using (var scene = SimpleImGuiScene.CreateOverlay(RendererFactory.RendererBackend.DirectX11))
        {
            scene.OnBuildUI += Test;

            scene.Run();
        }
    }

    private void RenderMainMenu()
    {
        bool open = true;

        if (!ImGui.BeginMainMenuBar())
            return;


        if (ImGui.BeginMenu("Orbwalker"))
        {
            if (ImGui.BeginMenu("Side Menu Item"))
            {
                if (ImGui.MenuItem("Side Item 1"))
                {
                    // Handle "Side Item 1" menu item selection
                }

                if (ImGui.MenuItem("Side Item 2"))
                {
                    // Handle "Side Item 2" menu item selection
                }

                ImGui.EndMenu();
            }

            ImGui.EndMenu();
        }

        if (ImGui.BeginMenu("Evade"))
        {
            if (ImGui.BeginMenu("Side Menu Item"))
            {
                if (ImGui.MenuItem("Side Item 1"))
                {
                    // Handle "Side Item 1" menu item selection
                }

                if (ImGui.MenuItem("Side Item 2"))
                {
                    // Handle "Side Item 2" menu item selection
                }

                ImGui.EndMenu();
            }

            ImGui.EndMenu();
        }

        if (ImGui.BeginMenu("Awareness"))
        {
            if (ImGui.BeginMenu("Side Menu Item"))
            {
                if (ImGui.MenuItem("Side Item 1"))
                {
                    // Handle "Side Item 1" menu item selection
                }

                if (ImGui.MenuItem("Side Item 2"))
                {
                    // Handle "Side Item 2" menu item selection
                }

                ImGui.EndMenu();
            }

            ImGui.EndMenu();
        }
    }
}