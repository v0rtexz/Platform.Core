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
using System.Reflection;
using Ensage.Data.Game;
using Ensage.Data.Game.Components;
using Ensage.Data.Game.Types;
using GameOverlay.Drawing;
using ImGuiNET;
using Color = System.Drawing.Color;

namespace Ensage.Data.Rendering;

/// <summary>
/// Debug Console to print information about objects.
/// </summary>
internal static class DebugConsole
{
    #region Properties

    /// <summary>
    /// The object selected in the menu
    /// </summary>
    private static AIBaseClient selectedObject;

    #endregion

    #region Methods

    /// <summary>
    /// Draws the debug console.
    /// </summary>
    internal static void Draw()
    {
        bool showConsole = true;

        //  PrintLocalPlayerData();

        ImGui.Begin("Debug Console", ref showConsole);


        DrawChampionsDropdown();
        DrawMinionsDropdown();

        if (selectedObject != null)
        {
            PrintProperties(selectedObject);
        }

        ImGui.End();
    }

    /// <summary>
    /// Draws a dropdown menu to choose a champion to debug.
    /// </summary>
    private static void DrawChampionsDropdown()
    {
        // Create a list of objects you want to debug
        List<AIHeroClient> debugObjects = ComponentController.GetComponent<World>().Heroes.ToList();

        string[] objectNames = debugObjects.Select(obj => obj.ChampionName).ToArray();

        int selectedIndex = Array.IndexOf(debugObjects.ToArray(), selectedObject);
        if (ImGui.Combo("Select Object", ref selectedIndex, objectNames, objectNames.Length))
        {
            selectedObject = debugObjects[selectedIndex];
        }
    }

    /// <summary>
    /// Draws a dropdown menu to choose a minion to debug.
    /// </summary>
    private static void DrawMinionsDropdown()
    {
        // Create a list of objects you want to debug
        List<AIMinionClient> debugObjects = ComponentController.GetComponent<World>().Minions.ToList();

        string[] objectNames = debugObjects.Select(obj => obj.ObjectName).ToArray();

        int selectedIndex = Array.IndexOf(debugObjects.ToArray(), selectedObject);
        if (ImGui.Combo("Select Minion Object", ref selectedIndex, objectNames, objectNames.Length))
        {
            selectedObject = debugObjects[selectedIndex];
        }
    }

    /// <summary>
    /// Prints all object properties to console.
    /// </summary>
    /// <param name="debugObject"></param>
    private static void PrintProperties(AIBaseClient debugObject)
    {
        Type objectType = debugObject.GetType();
        PropertyInfo[] properties = objectType.GetProperties();
        MethodInfo[] methods = objectType.GetMethods();


        ImGui.Text("Property Values:");
        foreach (PropertyInfo property in properties)
        {
            object value = property.GetValue(debugObject);
            ImGui.Text($"{property.Name}: {value}");
        }

        ImGui.Separator();
        ImGui.Text("\nFunction results:");
        Vector2 pos = Vector2.Zero;

        Renderer.WorldToScreen(debugObject.Pos, ref pos);
        ImGui.Text($"WorldToScreen: {pos}");
        foreach (MethodInfo method in methods)
        {
            if (method.IsSpecialName || method.DeclaringType == typeof(object))
                continue;

            if (method.GetParameters().Length == 0)
                ImGui.Text($"{method.Name}: {method.Invoke(debugObject, null)}");
        }
    }

    /// <summary>
    /// Prints all LocalPlayer properties on the screen..
    /// </summary>
    /// <param name="debugObject"></param>
    private static void PrintLocalPlayerData()
    {
        AIHeroClient player = ComponentController.GetComponent<World>().GetLocalPlayer();

        Type objectType = player.GetType();
        PropertyInfo[] properties = objectType.GetProperties();

        Vector2 textPos = new Vector2(100, 100);
        Charm.Draw.DrawString(textPos.X, textPos.Y, "Propertys:", Color.White);
        int i = 10;
        foreach (PropertyInfo property in properties)
        {
            object value = property.GetValue(player);
            Charm.Draw.DrawString(textPos.X, textPos.Y, $"\n{property.Name}: {value}", Color.White);
            i += 30;
        }
    }

    #endregion
}