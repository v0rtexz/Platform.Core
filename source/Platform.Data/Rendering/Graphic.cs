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

namespace Ensage.Data.Rendering;

using GameOverlay.Drawing;
using GameOverlay.Windows;

/// <summary>
/// Class which implements functionality for drawing.
/// </summary>
public class Graphic : IDisposable
{
    private static GraphicsWindow _window;
    internal static bool _isReady;
    private static Dictionary<string, SolidBrush> _brushes;
    private static Dictionary<string, Font> _fonts;
    private static Dictionary<string, Image> _images;

    private static Geometry _gridGeometry;
    private static Rectangle _gridBounds;

    private static Graphics _instance;

    /// <summary>
    /// Drawing Instance
    /// </summary>
    public static Graphics Draw
    {
        get => _instance;
    }

    /// <summary> 
    /// Instance to add and remove brushes.
    /// </summary>
    public static Dictionary<string, SolidBrush> Brushes
    {
        get => _brushes;
    }

    /// <summary>
    /// Instance to add and remove fonts.
    /// </summary>
    public static Dictionary<string, Font> Fonts
    {
        get => _fonts;
    }

    /// <summary>
    /// Instance to add and remove images.
    /// </summary>
    public static Dictionary<string, Image> Images
    {
        get => _images;
    }

    /// <summary>
    /// Checks if graphics are ready to be drawn.
    /// </summary>
    internal static bool IsReady
    {
        get => _isReady;
    }

    /// <summary>
    /// Initializes all graphic instances.
    /// </summary>
    internal static void Init()
    {
        _brushes = new Dictionary<string, SolidBrush>();
        _fonts = new Dictionary<string, Font>();
        _images = new Dictionary<string, Image>();

        _instance = new GameOverlay.Drawing.Graphics()
        {
            MeasureFPS = true,
            PerPrimitiveAntiAliasing = false,
            TextAntiAliasing = false
        };

        _window = new GraphicsWindow(0, 0, 1920, 1080, _instance)
        {
            FPS = 300,
            IsTopmost = true,
            IsVisible = true
        };

        _window.DestroyGraphics += _window_DestroyGraphics;
        // _window.DrawGraphics += _window_DrawGraphics;
        _window.SetupGraphics += _window_SetupGraphics;

        _window.Create();
        _window.Join();

        LoadChampionIcons();
    }

    public static Image GetChampionIcon(string championName)
    {
        if (!_images.ContainsKey(championName))
            _images[championName] = Graphic.Draw.CreateImage("Resources\\Champions\\" + championName + ".png");

        return _images[championName];
    }

    private static void LoadChampionIcons()
    {
        // foreach (var hero in HeroList.Get())
        // {
        //     _images[hero.GetName()] = Graphic.Draw.CreateImage("Resources\\Champions\\" + hero.GetName() + ".png");
        //  }
    }

    private static void _window_SetupGraphics(object sender, SetupGraphicsEventArgs e)
    {
        if (e.RecreateResources)
        {
            foreach (var pair in _brushes) pair.Value.Dispose();
            foreach (var pair in _images) pair.Value.Dispose();
        }

        _brushes["black"] = _instance.CreateSolidBrush(0, 0, 0);
        _brushes["white"] = _instance.CreateSolidBrush(255, 255, 255);
        _brushes["red"] = _instance.CreateSolidBrush(255, 0, 0);
        _brushes["redglow"] = _instance.CreateSolidBrush(100, 0, 0, 40);
        _brushes["green"] = _instance.CreateSolidBrush(0, 255, 0);
        _brushes["blue"] = _instance.CreateSolidBrush(0, 0, 255);
        _brushes["yellow"] = _instance.CreateSolidBrush(255, 255, 0);
        _brushes["yellow"] = _instance.CreateSolidBrush(255, 255, 0);
        _brushes["background"] = _instance.CreateSolidBrush(0, 0, 0, 0);
        _brushes["grid"] = _instance.CreateSolidBrush(255, 255, 255, 0.2f);
        _brushes["league"] = _instance.CreateSolidBrush(191, 140, 59, 255);
        _brushes["leaguebg"] = _instance.CreateSolidBrush(27, 47, 46, 255);
        _brushes["random"] = _instance.CreateSolidBrush(0, 0, 0);

        if (e.RecreateResources) return;

        _fonts["arial"] = _instance.CreateFont("Arial", 12);
        _fonts["consolas"] = _instance.CreateFont("Consolas", 14);

        _gridBounds = new Rectangle(20, 60, _instance.Width - 20, _instance.Height - 20);
        _gridGeometry = _instance.CreateGeometry();

        for (float x = _gridBounds.Left; x <= _gridBounds.Right; x += 20)
        {
            var line = new Line(x, _gridBounds.Top, x, _gridBounds.Bottom);
            _gridGeometry.BeginFigure(line);
            _gridGeometry.EndFigure(false);
        }

        for (float y = _gridBounds.Top; y <= _gridBounds.Bottom; y += 20)
        {
            var line = new Line(_gridBounds.Left, y, _gridBounds.Right, y);
            _gridGeometry.BeginFigure(line);
            _gridGeometry.EndFigure(false);
        }

        _gridGeometry.Close();

        _isReady = true;
    }

    private static void _window_DestroyGraphics(object sender, DestroyGraphicsEventArgs e)
    {
        foreach (var pair in _brushes) pair.Value.Dispose();
        foreach (var pair in _fonts) pair.Value.Dispose();
        foreach (var pair in _images) pair.Value.Dispose();
    }

    ~Graphic()
    {
        Dispose(false);
    }

    #region IDisposable Support

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            _window.Dispose();

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion
}