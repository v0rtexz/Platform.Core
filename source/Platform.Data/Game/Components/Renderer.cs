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

using System.Runtime.InteropServices;
using Ensage.Data.Memory;
using Ensage.Data.Utils;
using JetBrains.Annotations;
using ProcessMemoryUtilities.Managed;
using SharpDX;

namespace Ensage.Data.Game.Components;

using Vector2 = System.Numerics.Vector2;
using Vector3 = System.Numerics.Vector3;

/// <summary>
/// Class to update the renderer information.
/// </summary>
public class Renderer : IGameComponent
{
    #region Properties

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
    private struct DisplayInfo
    {
        [FieldOffset(0xC)] internal readonly int width;
        [FieldOffset(0x10)] internal readonly int height;
    }

    /// <summary>
    /// Instance of the renderer. Remains unchanged throughout the game.
    /// </summary>
    private long rendererInstance;

    /// <summary>
    /// Dynamic Viewmatrix. Updated on tick.
    /// </summary>
    private static Matrix viewMatrix;

    /// <summary>
    /// Dynamic View Projection Matrix. Updated on tick.
    /// </summary>
    private static Matrix viewProjectionMatrix;

    /// <summary>
    /// Displayinfo which contains the width and height of the display.
    /// </summary>
    private static DisplayInfo displayInfo;

    #endregion

    #region Constructors and Destructors

    public Renderer()
    {
        NativeWrapper.ReadProcessMemory<Matrix>(MemoryAccessor.Handle,
            (IntPtr)(MemoryAccessor.BaseAddress + Offsets.ViewMatrix), ref viewMatrix);

        NativeWrapper.ReadProcessMemory<Matrix>(MemoryAccessor.Handle,
            (IntPtr)(MemoryAccessor.BaseAddress + Offsets.ViewMatrix + 0x40), ref viewProjectionMatrix);
        //Get Renderer Instance
        NativeWrapper.ReadProcessMemory<long>(MemoryAccessor.Handle,
            MemoryAccessor.BaseAddress + Offsets.Renderer, ref rendererInstance);

        //This is basically a typecast ( (RendererStruct*)*DWORD* )
        NativeWrapper.ReadProcessMemory<DisplayInfo>(MemoryAccessor.Handle, (IntPtr)rendererInstance,
            ref displayInfo);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Update the renderer values
    /// </summary>
    internal void Update()
    {
        NativeWrapper.ReadProcessMemory<Matrix>(MemoryAccessor.Handle,
            (IntPtr)(MemoryAccessor.BaseAddress + Offsets.ViewMatrix), ref viewMatrix);

        NativeWrapper.ReadProcessMemory<Matrix>(MemoryAccessor.Handle,
            (IntPtr)(MemoryAccessor.BaseAddress + Offsets.ViewMatrix + 0x40), ref viewProjectionMatrix);
    }

    /// <summary>
    /// Transforms world coordinates to screen coordinates
    /// </summary>
    /// <param name="pos">The 3D world position.</param>
    /// <param name="screenPos">The buffer for the result.</param>
    /// <returns>True if the transformation was successful.</returns>
    [PublicAPI]
    public static bool WorldToScreen(Vector3 pos, ref Vector2 screenPos)
    {
        Matrix matrix = Matrix.Multiply(viewMatrix, viewProjectionMatrix);

        Vector4 clipCoords;
        clipCoords.X = pos.X * matrix[0] + pos.Y * matrix[4] + pos.Z * matrix[8] + matrix[12];
        clipCoords.Y = pos.X * matrix[1] + pos.Y * matrix[5] + pos.Z * matrix[9] + matrix[13];
        clipCoords.Z = pos.X * matrix[2] + pos.Y * matrix[6] + pos.Z * matrix[10] + matrix[14];
        clipCoords.W = pos.X * matrix[3] + pos.Y * matrix[7] + pos.Z * matrix[11] + matrix[15];

        if (clipCoords.W <= 0f)
        {
            // Position is behind the camera.
            return false;
        }


        System.Numerics.Vector3 M;
        M.X = clipCoords.X / clipCoords.W;
        M.Y = clipCoords.Y / clipCoords.W;
        M.Z = clipCoords.Z / clipCoords.W;

        screenPos.X = (displayInfo.width / 2f) * (1f + M.X);
        screenPos.Y = displayInfo.height - ((displayInfo.height / 2f) * (1f + M.Y));

        return true;
    }

    public OperationResult Construct()
    {
        return OperationResult.SUCCESS;
    }

    #endregion
}