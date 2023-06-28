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

using System.Numerics;
using Ensage.Data.Game.Components;
using ImGuiNET;

namespace Ensage.Data.Rendering;

/// <summary>
/// Class responsible for drawings.
/// </summary>
public static class Drawing
{
    #region Properties

    /// <summary>
    /// Queue which stores all drawing functions that should be called in Present.
    /// </summary>
    internal static Queue<Action> BackBufferDrawQueue = new Queue<Action>();

    internal static Queue<Action> FrontBufferDrawQueue = new Queue<Action>();

    #endregion

    #region Methods

    /// <summary>
    /// Draw a 2D Circle.
    /// </summary>
    /// <param name="vec">The position to draw at.</param>
    /// <param name="radius">The radius of the circle.</param>
    /// <param name="color">The color to draw the circle.</param>
    public static void DrawCircle(Vector2 vec, float radius, uint color)
    {
        BackBufferDrawQueue.Enqueue(() => ImGui.GetForegroundDrawList().AddCircle(vec, radius, color));
    }

    /// <summary>
    /// Add a 3D circle to the drawing queue.
    /// </summary>
    /// <param name="pos">The world position to draw at.</param>
    /// <param name="radius">The radius of the circle.</param>
    /// <param name="useShader">True if it should be a shader circle.</param>
    /// <param name="thickness">The thickness of the circle.</param>
    public static void Add3DCircle(Vector3 pos, float radius, bool useShader = true, float thickness = 1.2f)
    {
        BackBufferDrawQueue.Enqueue(() => Draw3DCircle(pos, radius, useShader, thickness));
    }

    /// <summary>
    /// Draw a 3D world circle.
    /// </summary>
    /// <param name="pos">Center position of the circle in world coordinates.</param>
    /// <param name="radius">Radius of the circle.</param>
    /// <param name="useShader">True if it should be a shader circle.</param>
    /// <param name="thickness">Thickness of the circle's outline.</param>
    public static void Draw3DCircle(Vector3 pos, float radius, bool useShader = true, float thickness = 1.2f)
    {
        Vector3 worldPos = pos;

        var drawList = ImGui.GetForegroundDrawList();

        // Start drawing a new path for the circle
        drawList.PathClear();
        Vector2 screenPos = Vector2.Zero;
        for (var i = 0; i <= 100; i++)
        {
            var angle = (float)i * Math.PI * 1.98f / 98.0f;
            worldPos.X = (float)(pos.X + Math.Cos(angle) * radius);
            worldPos.Z = (float)(pos.Z + Math.Sin(angle) * radius);

            Renderer.WorldToScreen(worldPos, ref screenPos);
            drawList.PathLineTo(screenPos);
        }

        // End the path and stroke it with the specified color and thickness
        drawList.PathStroke(ImGui.ColorConvertFloat4ToU32(new Vector4(0, 255f, 0f, 150f)), ImDrawFlags.None, thickness);

        if (!useShader)
            return;

        // Access the vertex buffer
        var vtxBuffer = drawList.VtxBuffer;

        // Calculate the total number of vertices in the buffer
        var numVertices = (int)vtxBuffer.Size;

        // Define animation properties
        var time = DateTime.Now.TimeOfDay.TotalSeconds;
        var frequency = 1.6f; // Adjust this value to control the frequency of the waving animation
        var amplitude = 0.45f; // Adjust this value to control the amplitude of the waving animation

        // Calculate the color based on the gradient
        var lightPurple = new Vector4(0.278f, 0.498f, 0.839f, 0.8f);
        var darkRed = new Vector4(1f, 0.1f, 0.32f, 0.8f);

        // Loop through the vertices and apply the shader effect
        for (var i = 0; i < numVertices; i++)
        {
            // Get a reference to the current vertex
            var vertex = vtxBuffer[i];

            // Calculate the position relative to the circle's center
            var position = vertex.pos - screenPos;

            // Calculate the distance from the circle's center
            var distance = position.Length();

            // Calculate gradient
            var gradient = Math.Clamp(distance / radius, 0f, 1f);

            // Calculate the color based on the gradient
            var shaderColor = Vector4.Lerp(lightPurple, darkRed, gradient);

            // Apply the waving animation effect to the shader color
            var animationOffset = (float)Math.Sin(time * frequency + gradient * Math.PI * 2) * amplitude;
            shaderColor += new Vector4(animationOffset, animationOffset, animationOffset, 0f);

            // Clamp the color channels to ensure they stay within the defined range
            shaderColor = Vector4.Clamp(shaderColor, lightPurple, darkRed);

            // Update the modified color back to the vertex
            vertex.col = ImGui.ColorConvertFloat4ToU32(shaderColor);

            // Update the modified vertex back to the buffer
            vtxBuffer[i].col = vertex.col;
        }
    }

    /// <summary>
    /// Draws text on a given position.
    /// </summary>
    /// <param name="text">The text to draw.</param>
    /// <param name="pos">The position to draw at.</param>
    /// <param name="color">The color of the text.</param>
    public static void AddShaderText(string text, Vector2 pos, float scale = 1.0f)
    {
        var drawList = ImGui.GetForegroundDrawList();

        // Create a shader effect for the text
        var gradient = Math.Clamp(Math.Sin(DateTime.Now.TimeOfDay.TotalSeconds * 2.0) * 0.5 + 0.5, 0.0, 1.0);
        var shaderColor = new Vector4(1.0f, (float)gradient, 0.0f, 1.0f);
        var shaderColorU32 = ImGui.ColorConvertFloat4ToU32(shaderColor);


        drawList.AddText(pos, shaderColorU32, text);
    }

    /// <summary>
    /// Draws text on a given position.
    /// </summary>
    /// <param name="text">The text to draw.</param>
    /// <param name="pos">The position to draw at.</param>
    /// <param name="color">The color of the text.</param>
    public static void AddText(string text, Vector2 pos, Vector4 color, float scale = 1.0f)
    {
        var drawList = ImGui.GetForegroundDrawList();

        // Create a shader effect for the text
        var u32Color = ImGui.ColorConvertFloat4ToU32(color);

        drawList.AddText(pos, u32Color, text);
    }

    #endregion
}