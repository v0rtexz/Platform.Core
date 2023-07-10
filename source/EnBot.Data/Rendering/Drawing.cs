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
            DrawCircle3D(pos, radius, thickness));
    }

    public static void AddShaderLine(Vector2 startPoint, Vector2 endPoint,
        float thickness = 1.2f)
    {
        BackBufferDrawQueue.Enqueue(() =>
            DrawShaderLine(startPoint, endPoint, thickness));
    }

    private static void DrawShaderLine(Vector2 startPoint, Vector2 endPoint,
        float thickness = 1.2f)
    {
        //IBrush lineBrush = gr.CreateSolidBrush(StartColor.R, StartColor.G, StartColor.B, StartColor.A);

        Vector2 direction = endPoint - startPoint;
        Vector2 perpendicular = new Vector2(-direction.Y, direction.X);
        perpendicular = Vector2.Normalize(perpendicular);

        float halfThickness = thickness / 2f;
        float gapWidth = thickness * 20f; // Adjust the multiplier to control the size of the gap

        Vector2 gapOffset = perpendicular * (gapWidth / 2f);

        Vector2 startPoint1 = startPoint + gapOffset;
        Vector2 endPoint1 = endPoint + gapOffset;
        Vector2 startPoint2 = startPoint - gapOffset;
        Vector2 endPoint2 = endPoint - gapOffset;

        AddLine(startPoint1.X, startPoint1.Y, endPoint1.X, endPoint1.Y, Color.FromArgb(20, 100, 0, 0), gapWidth);


        Vector2 topLeft = startPoint1 + perpendicular * halfThickness;
        Vector2 topRight = endPoint1 + perpendicular * halfThickness;
        Vector2 bottomLeft = startPoint2 + perpendicular * halfThickness;
        Vector2 bottomRight = endPoint2 + perpendicular * halfThickness;

        AddLine(startPoint2.X, startPoint2.Y, endPoint2.X, endPoint2.Y, Color.FromArgb(20, 100, 0, 0), gapWidth);
    }

    public static void AddShaderCircle(Vector3 pos, float radius,
        Color firstColor, Color secondColor, Color glowCol, float thickness = 1.2f)
    {
        BackBufferDrawQueue.Enqueue(() =>
            DrawShaderCircle(pos, radius, firstColor, secondColor, glowCol, thickness));
    }

    private static void DrawCircle3D(Vector3 pos, float radius, float thickness = 1.2f)
    {
        Vector3 worldPos = pos;


        int segments = 100;
        float step = (2.0f * (float)Math.PI) / segments;
        float[] cosValues = new float[segments + 1];
        float[] sinValues = new float[segments + 1];

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * step;
            cosValues[i] = (float)Math.Cos(angle);
            sinValues[i] = (float)Math.Sin(angle);
        }

        Vector2 prevScreenPos = Vector2.Zero;
        for (int i = 0; i <= segments; i++)
        {
            Vector3 point = worldPos;
            point.X += cosValues[i] * radius;
            point.Z += sinValues[i] * radius;

            Vector2 screenPos = Vector2.Zero;
            if (!Renderer.WorldToScreen(point, ref screenPos))
                continue;

            if (prevScreenPos != Vector2.Zero)
                Charm.Draw.DrawLine(prevScreenPos.X, prevScreenPos.Y, screenPos.X, screenPos.Y, 2, Color.White);
            // Graphic.Draw.DrawLine(brush, prevScreenPos.X, prevScreenPos.Y, screenPos.X, screenPos.Y, 2);

            prevScreenPos = screenPos;
        }
    }

    private static Color LerpColor(Color startColor, Color endColor, float t)
    {
        int r = (int)(startColor.R + (endColor.R - startColor.R) * t);
        int g = (int)(startColor.G + (endColor.G - startColor.G) * t);
        int b = (int)(startColor.B + (endColor.B - startColor.B) * t);
        int a = (int)(startColor.A + (endColor.A - startColor.A) * t);

        return Color.FromArgb(a, r, g, b);
    }

    private static float LerpColor(float startValue, float endValue, float t)
    {
        return startValue + (endValue - startValue) * t;
    }

    public static void DrawShaderCircle(Vector3 pos, float radius,
        Color firstColor, Color secondColor, Color glowCol, float thickness = 1.2f)
    {
        Vector3 worldPos = pos;


        int segments = 100;
        float step = (2.0f * MathF.PI) / segments;
        Vector2[] screenPositions = new Vector2[segments + 1];

        float[] cosValues = new float[segments + 1];
        float[] sinValues = new float[segments + 1];
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * step;
            cosValues[i] = MathF.Cos(angle);
            sinValues[i] = MathF.Sin(angle);
        }

        Color startColor = firstColor;
        Color endColor = secondColor;
        float glowWidth = 6.5f;
        Color glowColor = glowCol; //Color.FromArgb(147, 112, 219);
        Color adjustedGlowColor = Color.FromArgb(100, glowColor.R, glowColor.G, glowColor.B);

        for (int i = 0; i <= segments; i++)
        {
            float cosValue = cosValues[i];
            float sinValue = sinValues[i];

            Vector3 point = worldPos + new Vector3(cosValue * radius, 0, sinValue * radius);

            Vector2 screenPos = Vector2.Zero;
            if (!Renderer.WorldToScreen(point, ref screenPos))
                continue;

            screenPositions[i] = screenPos;
        }

        Vector2 prevScreenPos = screenPositions[segments];
        for (int i = 0; i <= segments; i++)
        {
            Vector2 screenPos = screenPositions[i];

            float progress = (float)i / segments;
            if (screenPos.Y < worldPos.Y)
                progress = MathF.Min(progress * LerpColor(0.8f, 1f, screenPos.Y / worldPos.Y), 1f);

            Color intermediateColor = LerpColor(startColor, endColor, progress);

            //   gr.DrawLine(Graphic.Brushes["black"], prevScreenPos.X + 1, prevScreenPos.Y + 1, screenPos.X + 1, screenPos.Y + 1, thickness);
            Drawing.AddLine(prevScreenPos.X, prevScreenPos.Y, screenPos.X, screenPos.Y, adjustedGlowColor, thickness);
            Drawing.AddLine(prevScreenPos.X, prevScreenPos.Y, screenPos.X, screenPos.Y, intermediateColor, thickness);

            prevScreenPos = screenPos;
        }

        Drawing.AddLine(prevScreenPos.X, prevScreenPos.Y, screenPositions[0].X,
            screenPositions[0].Y, adjustedGlowColor, glowWidth);

        Drawing.AddLine(prevScreenPos.X, prevScreenPos.Y, screenPositions[0].X,
            screenPositions[0].Y, endColor, thickness);
    }

    public static void AddLine(float x1, float y1, float x2, float y2, System.Drawing.Color color,
        float thickness = 1.2f)
    {
        BackBufferDrawQueue.Enqueue(() =>
            Charm.Draw.DrawLine(x1, y1, x2, y2, thickness, color));
    }

    public static void AddRectangleWithEmptySpace(Vector2 startPos, Vector2 endPos, float gapSize,
        System.Drawing.Color color, float thickness = 1.2f)
    {
        BackBufferDrawQueue.Enqueue(() =>
            DrawRectangleWithEmptySpace(startPos, endPos, gapSize, color, thickness));
    }

    public static void DrawRectangleWithEmptySpace(Vector2 startPos, Vector2 endPos, float gapSize,
        System.Drawing.Color color, float thickness = 1.2f)
    {
        // Calculate the size of the rectangle
        float width = endPos.X - startPos.X;
        float height = endPos.Y - startPos.Y;

        // Top side
        AddLine(startPos.X, startPos.Y, endPos.X, startPos.Y, color, thickness);

        // Right side (excluding the gap area)
        AddLine(endPos.X, startPos.Y, endPos.X, startPos.Y + gapSize, color, thickness);
        AddLine(endPos.X, startPos.Y + gapSize + gapSize, endPos.X, endPos.Y, color, thickness);

        // Bottom side
        AddLine(endPos.X, endPos.Y, startPos.X, endPos.Y, color, thickness);

        // Left side (excluding the gap area)
        AddLine(startPos.X, endPos.Y, startPos.X, endPos.Y - gapSize, color, thickness);
        AddLine(startPos.X, endPos.Y - gapSize - gapSize, startPos.X, startPos.Y, color, thickness);
    }

    /// <summary>
    /// Draws text on a given position.
    /// </summary>
    /// <param name="text">The text to draw.</param>
    /// <param name="pos">The position to draw at.</param>
    /// <param name="color">The color of the text.</param>
    public static void AddText(string text, Vector2 pos, Color color, float fontSize = 24f)
    {
        BackBufferDrawQueue.Enqueue(() => Charm.Draw.DrawString(pos.X, pos.Y, text, color, fontSize));
    }

    public static void AddText(string text, float x, float y, Color color, float fontSize = 24f)
    {
        BackBufferDrawQueue.Enqueue(() => Charm.Draw.DrawString(x, y, text, color, fontSize));
    }

    #endregion
}