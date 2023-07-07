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
using GameOverlay.Drawing;
using ImGuiNET;
using Color = System.Drawing.Color;

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
    /// Add a 3D circle to the drawing queue.
    /// </summary>
    /// <param name="pos">The world position to draw at.</param>
    /// <param name="radius">The radius of the circle.</param>
    /// <param name="useShader">True if it should be a shader circle.</param>
    /// <param name="thickness">The thickness of the circle.</param>
    public static void Add3DCircle(Vector3 pos, float radius, bool useShader = true, float thickness = 1.2f)
    {
        BackBufferDrawQueue.Enqueue(() =>
            Graphic.Draw.DrawShaderCircle(pos, radius, Color.White, Color.DeepSkyBlue, 2.6f));
    }

    public static void AddLine(Vector2 startPoint, Vector2 endPoint, float thickness = 1.2f)
    {
        BackBufferDrawQueue.Enqueue(() =>
            Graphic.Draw.DrawShaderLine(startPoint, endPoint, thickness));
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