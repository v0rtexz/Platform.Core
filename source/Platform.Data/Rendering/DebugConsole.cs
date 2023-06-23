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

using Platform.Data.Game.Components;
using Platform.Data.Game.Types;

namespace Platform.Data.Rendering;

using System.Numerics;

using Platform.Data.Game.Components;
using Platform.Data.Game.Types;
using System.Reflection;
using ImGuiNET;

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

        ImGui.Begin("Debug Console", ref showConsole);


        DrawObjectDropdown();

        if (selectedObject != null)
        {
            PrintProperties(selectedObject);
        }

        ImGui.End();
    }

    /// <summary>
    /// Draws a dropdown menu to choose the object to debug.
    /// </summary>
    private static void DrawObjectDropdown()
    {
        // Create a list of objects you want to debug
        List<AIBaseClient> debugObjects = new List<AIBaseClient>();

        AIHeroClient player = ComponentController.GetComponent<World>().GetLocalPlayer();
        debugObjects.Add(player);

        string[] objectNames = debugObjects.Select(obj => obj.GetName()).ToArray();

        int selectedIndex = Array.IndexOf(debugObjects.ToArray(), selectedObject);
        if (ImGui.Combo("Select Object", ref selectedIndex, objectNames, objectNames.Length))
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

        ImGui.Text("Property Values:");
        foreach (PropertyInfo property in properties)
        {
            object value = property.GetValue(debugObject);
            ImGui.Text($"{property.Name}: {value}");
        }

        ImGui.Separator();
        ImGui.Text("\nFunction results:");
        Vector2 pos = Vector2.Zero;
        if (ComponentController.GetComponent<Renderer>().WorldToScreen(debugObject.Pos, ref pos))
            ImGui.Text($"WorldToScreen: {pos}");
    }

    #endregion
}