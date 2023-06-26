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
using Ensage.Data.Events;
using Ensage.Data.Game;
using Ensage.Data.Game.Components;
using ImGuiNET;
using ImGuiScene;
using JetBrains.Annotations;

namespace Ensage.Data.Rendering;

/// <summary>
/// Implements the <see cref="Overlay"/> class and creates a overlay window.
/// </summary>
internal class Present
{
    private float lastGameTime; // The last synchronized game time
    private Engine engine;
    private World world;

    /// <summary>
    /// Initializes a new instance of the <see cref="Present"/> class.
    /// </summary>
    /// <param name="engine">Engine instance.</param>
    /// <param name="world">World instance.</param>
    public Present([NotNull] Engine engine, [NotNull] World world)
    {
        this.world = world ?? throw new ArgumentNullException(nameof(world));
        this.engine = engine ?? throw new ArgumentNullException(nameof(engine));

        Task.Run(() => { Render(); });
    }

    /// <summary>
    /// Setup ImGui styles.
    /// </summary>
    public void Setup()
    {
        var style = ImGui.GetStyle();

        style.Colors[(int)ImGuiCol.WindowBg] = new Vector4(0.5f, 0.5f, 0.5f, 0.1f);

        // Adjust rounding and padding
        style.WindowRounding = 2.0f;
        style.FrameRounding = 2.0f;
        style.ScrollbarRounding = 2.0f;
        style.GrabRounding = 2.0f;
        style.DisplaySafeAreaPadding = new Vector2(4.0f, 4.0f);
    }


    void OnPresent()
    {
        // Create a Stopwatch to measure the rendering time
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        foreach (var player in world.Missiles.GetMap())
        {
            if (player.Value.Pos.Length() <= 0)
                continue;

            Drawing.Add3DCircle(player.Value.Pos, 50f, false, 1.5f);

            Vector2 screenPos = Vector2.Zero;
            Renderer.WorldToScreen(player.Value.Pos, ref screenPos);

            string opAFText = "LeagueSharp (SoonTM, OP af, spinning to victory GGWP EZ)";
            Vector2 screenSize = ImGui.GetIO().DisplaySize;
            float textWidth = ImGui.CalcTextSize(opAFText).X;
            Vector2 textPos = new Vector2((screenSize.X - textWidth) / 2, 20);
            Drawing.AddShaderText(opAFText, textPos, 1.5f);
        }

        foreach (var player in world.Heroes)
        {
            Drawing.Add3DCircle(player.Pos, player.AttackRange + 65f);
        }

        engine.Renderer.Update();
        EventManager.InvokeCallback<EventDelegate.EvtOnUpdate>();

        // Swap the buffers
        var tempBuffer = Drawing.FrontBufferDrawQueue;
        Drawing.FrontBufferDrawQueue = Drawing.BackBufferDrawQueue;
        Drawing.BackBufferDrawQueue = tempBuffer;
        Drawing.BackBufferDrawQueue.Clear();

        // Render the content from the front buffer
        while (Drawing.FrontBufferDrawQueue.Count > 0)
        {
            var drawing = Drawing.FrontBufferDrawQueue.Dequeue();
            drawing.Invoke();
        }

        DebugConsole.Draw();
        RenderMainMenu();

        // Stop the Stopwatch and output the rendering time
        stopwatch.Stop();
        TimeSpan elapsed = stopwatch.Elapsed;

        // Console.WriteLine("Rendering time: " + elapsed.TotalMilliseconds + " ms");
    }

    private void Render()
    {
        using (var scene = SimpleImGuiScene.CreateOverlay(RendererFactory.RendererBackend.DirectX11))
        {
            Setup();
            scene.OnBuildUI += OnPresent;
            scene.FramerateLimit = new FramerateLimit(FramerateLimit.LimitType.Unbounded, 300);

            scene.Run();
        }
    }

    public unsafe void RenderMainMenu()
    {
    }
}