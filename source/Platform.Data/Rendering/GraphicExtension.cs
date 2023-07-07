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
using Ensage.Data.Game.Components;
using GameOverlay.Drawing;

namespace Ensage.Data.Rendering;

using System.Drawing;

public static class GraphicExtension
{
    private static Dictionary<Color, IBrush> solidBrushCache = new Dictionary<Color, IBrush>();

    private static IBrush GetSolidBrush(this GameOverlay.Drawing.Graphics gr, Color color)
    {
        if (solidBrushCache.ContainsKey(color))
        {
            // Return the cached solid brush
            return solidBrushCache[color];
        }
        else
        {
            // Create a new solid brush and cache it
            IBrush solidBrush = gr.CreateSolidBrush(color.R, color.G, color.B, color.A);
            solidBrushCache.Add(color, solidBrush);
            return solidBrush;
        }
    }

    private static Color LerpColor(Color startColor, Color endColor, float t)
    {
        int r = (int)(startColor.R + (endColor.R - startColor.R) * t);
        int g = (int)(startColor.G + (endColor.G - startColor.G) * t);
        int b = (int)(startColor.B + (endColor.B - startColor.B) * t);
        int a = (int)(startColor.A + (endColor.A - startColor.A) * t);

        r = Math.Clamp(r, 0, 255);
        g = Math.Clamp(g, 0, 255);
        b = Math.Clamp(b, 0, 255);
        a = Math.Clamp(a, 0, 255);

        return Color.FromArgb(a, r, g, b);
    }

    private static float LerpColor(float startValue, float endValue, float t)
    {
        return startValue + (endValue - startValue) * t;
    }

    private static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    private static float animationTime = 0f; // Current animation time
    static Stopwatch stopwatch = new Stopwatch();
    private static float pulseSpeed = 2f; // Speed of the pulsing effect

    private static Dictionary<GameOverlay.Drawing.Graphics, float> animationTimes =
        new Dictionary<GameOverlay.Drawing.Graphics, float>();


    private static float Clamp01(float value)
    {
        return MathF.Max(0f, MathF.Min(1f, value));
    }

    public static void Animate(this GameOverlay.Drawing.Graphics gr, Vector3 position, float startRadius,
        float endRadius, Color c)
    {
        // Check if animation time dictionary contains the current graphics object
        if (!animationTimes.ContainsKey(gr))
        {
            // If not, add the graphics object with an initial animation time of 0
            animationTimes.Add(gr, 0f);
        }

        float animationDuration = 0.6f;

        // Get the animation time for the current graphics object
        float animationTime = animationTimes[gr];

        // Update the animation time
        animationTime += (float)stopwatch.Elapsed.TotalSeconds;

        // Ensure animation time stays within the animation duration
        animationTime %= animationDuration;

        // Store the updated animation time back to the dictionary
        animationTimes[gr] = animationTime;

        // Calculate the animation progress
        float progress = animationTime / animationDuration;

        // Calculate the fade progress
        float fadeProgress = Clamp01((animationDuration - animationTime) / 0.2f);

        // Apply fading effect to the animation progress
        progress = Lerp(0f, progress, fadeProgress);

        float pulseFactor = MathF.Sin(animationTime * pulseSpeed);

        // Calculate the thickness and glow width using the progress and pulse factor
        float radius = Lerp(startRadius, endRadius, progress) + pulseFactor * 20f;


        for (int i = 0; i < 3; i++)
        {
            // Call your DrawShaderCircle method with the updated thickness and glowWidth
            Graphic.Draw.DrawShaderCircle(position, radius + i * 50, c, Color.PaleVioletRed, c);
        }

        // Reset the stopwatch for the next frame
        stopwatch.Restart();
    }


    public static void Animate(this GameOverlay.Drawing.Graphics gr, Vector3 position, float startRadius,
        float endRadius, Color mainColor, Color secondColor, Color glowColor)
    {
        // Check if animation time dictionary contains the current graphics object
        if (!animationTimes.ContainsKey(gr))
        {
            // If not, add the graphics object with an initial animation time of 0
            animationTimes.Add(gr, 0f);
        }

        float animationDuration = 0.6f;

        // Get the animation time for the current graphics object
        float animationTime = animationTimes[gr];

        // Update the animation time
        animationTime += (float)stopwatch.Elapsed.TotalSeconds;

        // Ensure animation time stays within the animation duration
        animationTime %= animationDuration;

        // Store the updated animation time back to the dictionary
        animationTimes[gr] = animationTime;

        // Calculate the animation progress
        float progress = animationTime / animationDuration;

        // Calculate the fade progress
        float fadeProgress = Clamp01((animationDuration - animationTime) / 0.2f);

        // Apply fading effect to the animation progress
        progress = Lerp(0f, progress, fadeProgress);

        float pulseFactor = MathF.Sin(animationTime * pulseSpeed);

        // Calculate the thickness and glow width using the progress and pulse factor
        float radius = Lerp(startRadius, endRadius, progress) + pulseFactor * 20f;


        for (int i = 0; i < 3; i++)
        {
            // Call your DrawShaderCircle method with the updated thickness and glowWidth
            Graphic.Draw.DrawShaderCircle(position, radius + i * 50, mainColor, secondColor, glowColor);
        }

        // Reset the stopwatch for the next frame
        stopwatch.Restart();
    }


    public static void DrawTransparentRectangleWithText(this GameOverlay.Drawing.Graphics gr, string text,
        Image icon = null)
    {
        // gr.ClearScene();
        float screenWidth = gr.Width;
        float screenHeight = gr.Height;
        float rectWidth = screenWidth * 0.15f;
        float rectHeight = screenHeight * 0.07f;
        float rectLeft = (screenWidth - rectWidth) / 2;
        float rectTop = screenHeight * 0.1f;
        float rectRight = rectLeft + rectWidth;
        float rectBottom = rectTop + rectHeight;


        IBrush brush = gr.CreateSolidBrush(211, 213, 211, 55); // Almost transparent white

        // Draw the text in the middle of the rectangle
        float fontSize = Math.Min(rectHeight * 0.2f, rectWidth);
        float textHeight = Graphic.Fonts["consolas"].FontSize;
        float textWidth = fontSize * 20; // Calculate the text width based on font size and number of characters
        float textX = rectLeft + (rectWidth - textWidth) / 2;
        float textY = rectTop + (rectHeight - textHeight) / 2;

        //adjust rectangle if text size is large
        if (text.Length > 20)
        {
            gr.FillRoundedRectangle(brush, rectLeft, rectTop, rectRight + (text.Length - 20) * 15, rectBottom, 10f);
        }
        else
            gr.FillRoundedRectangle(brush, rectLeft, rectTop, rectRight, rectBottom, 10f);

        gr.DrawText(Graphic.Fonts["consolas"], 20f, Graphic.Brushes["white"], textX + 86, textY - 5,
            text); // White text

        if (icon != null)
        {
            Graphic.Draw.DrawImage(icon, rectLeft + 75, rectTop + 75, rectLeft + 10, rectTop + 5);
        }
    }


    public static void DrawCircle3D(this GameOverlay.Drawing.Graphics gr, Vector3 pos, float radius,
        IBrush brush = null, float thickness = 1.2f)
    {
        if (brush == null)
            brush = Graphic.Brushes["league"];

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
                Graphic.Draw.DrawLine(brush, prevScreenPos.X, prevScreenPos.Y, screenPos.X, screenPos.Y, 2);

            prevScreenPos = screenPos;
        }
    }

    public static void DrawShaderCircle(this GameOverlay.Drawing.Graphics gr, Vector3 pos, float radius,
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

        IBrush lineBrush = gr.CreateSolidBrush(startColor.R, startColor.G, startColor.B, startColor.A);
        IBrush glowBrush = gr.CreateSolidBrush(adjustedGlowColor.R, adjustedGlowColor.G, adjustedGlowColor.B,
            adjustedGlowColor.A);


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
            lineBrush = gr.CreateSolidBrush(intermediateColor.R, intermediateColor.G, intermediateColor.B,
                intermediateColor.A);

            //   gr.DrawLine(Graphic.Brushes["black"], prevScreenPos.X + 1, prevScreenPos.Y + 1, screenPos.X + 1, screenPos.Y + 1, thickness);
            Graphic.Draw.DrawLine(glowBrush, prevScreenPos.X, prevScreenPos.Y, screenPos.X, screenPos.Y, glowWidth);
            Graphic.Draw.DrawLine(lineBrush, prevScreenPos.X, prevScreenPos.Y, screenPos.X, screenPos.Y, thickness);

            prevScreenPos = screenPos;
        }

        lineBrush = gr.CreateSolidBrush(endColor.R, endColor.G, endColor.B, endColor.A);

        Graphic.Draw.DrawLine(glowBrush, prevScreenPos.X, prevScreenPos.Y, screenPositions[0].X, screenPositions[0].Y,
            glowWidth);
        Graphic.Draw.DrawLine(lineBrush, prevScreenPos.X, prevScreenPos.Y, screenPositions[0].X, screenPositions[0].Y,
            thickness);
    }

    private static bool preparedShader = false;
    private static float[] CosValues;
    private static float[] SinValues;

// Pre-calculate cos and sin values for the entire range of segments
    static void PrepareShader()
    {
        int segments = 100;
        float step = (2.0f * MathF.PI) / segments;
        CosValues = new float[segments + 1];
        SinValues = new float[segments + 1];
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * step;
            CosValues[i] = MathF.Cos(angle);
            SinValues[i] = MathF.Sin(angle);
        }
    }

    public static void DrawShaderCircle(this GameOverlay.Drawing.Graphics gr, Vector3 pos, float radius,
        Color startColor, Color endColor, float thickness = 1.2f)
    {
        if (!preparedShader)
            PrepareShader();

        if (radius <= 0)
            return;

        Vector3 worldPos = pos;

        int segments = 100;
        Vector2[] screenPositions = new Vector2[segments + 1];

        IBrush lineBrush = gr.GetSolidBrush(startColor);
        IBrush glowBrush = gr.GetSolidBrush(endColor);

        float[] cosValues = CosValues;
        float[] sinValues = SinValues;

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
            lineBrush = gr.GetSolidBrush(intermediateColor);

            Graphic.Draw.DrawLine(glowBrush, prevScreenPos.X, prevScreenPos.Y, screenPos.X, screenPos.Y, 2.5f);
            Graphic.Draw.DrawLine(lineBrush, prevScreenPos.X, prevScreenPos.Y, screenPos.X, screenPos.Y, thickness);

            prevScreenPos = screenPos;
        }
    }

    public static void DrawShaderLine(this GameOverlay.Drawing.Graphics gr, Vector2 startPoint, Vector2 endPoint,
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

        Graphic.Draw.DrawLine(Graphic.Brushes["redglow"], startPoint1.X, startPoint1.Y, endPoint1.X, endPoint1.Y,
            gapWidth);


        Vector2 topLeft = startPoint1 + perpendicular * halfThickness;
        Vector2 topRight = endPoint1 + perpendicular * halfThickness;
        Vector2 bottomLeft = startPoint2 + perpendicular * halfThickness;
        Vector2 bottomRight = endPoint2 + perpendicular * halfThickness;


        Graphic.Draw.DrawLine(Graphic.Brushes["redglow"], startPoint2.X, startPoint2.Y, endPoint2.X, endPoint2.Y,
            gapWidth);
    }


    public static void DrawArrowToEnemy(this GameOverlay.Drawing.Graphics gr, Vector3 playerPosition,
        Vector3 enemyPosition, IBrush brush, float lineWidth, float arrowSize)
    {
        Vector2 playerScreenPos = Vector2.Zero;
        if (!Renderer.WorldToScreen(playerPosition, ref playerScreenPos))
            return;

        Vector2 enemyScreenPos = Vector2.Zero;
        if (!Renderer.WorldToScreen(enemyPosition, ref enemyScreenPos))
            return;

        playerScreenPos.Y += 30f;

        // Calculate the direction from player to enemy
        Vector2 direction = enemyScreenPos - playerScreenPos;
        direction = Vector2.Normalize(direction);

        // Calculate the perpendicular direction for arrowheads
        Vector2 perpendicular = new Vector2(-direction.Y, direction.X);

        // Calculate the arrowhead coordinates based on player's position
        Vector2 arrowhead1 = playerScreenPos - (direction * arrowSize) + (perpendicular * arrowSize);
        Vector2 arrowhead2 = playerScreenPos - (direction * arrowSize) - (perpendicular * arrowSize);

        // Draw the arrow line
        gr.DrawLine(brush, playerScreenPos.X, playerScreenPos.Y, playerScreenPos.X, playerScreenPos.Y, lineWidth);
        gr.DrawLine(brush, playerScreenPos.X, playerScreenPos.Y, arrowhead1.X, arrowhead1.Y, lineWidth);
        gr.DrawLine(brush, playerScreenPos.X, playerScreenPos.Y, arrowhead2.X, arrowhead2.Y, lineWidth);
    }
}