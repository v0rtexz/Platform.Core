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

using Ensage.Data.Game.Types;
using Ensage.Data.Memory;

namespace Ensage.Data.Game.Components.ManagerTemplates;

using System.Collections;
using ProcessMemoryUtilities.Managed;

/// <summary>
/// Responsible for accessing the <see cref="AIMinionClient"/> instance of <see cref="ManagerTemplate"/>.
/// </summary>
public class MinionManager : ManagerTemplate, IEnumerable<AIMinionClient>
{
    /// <summary>
    /// Enumerates all Heroes.
    /// </summary>
    /// <returns></returns>
    public IEnumerator<AIMinionClient> GetEnumerator()
    {
        for (int i = 0; i < GetListSize(ManagerTemplateType.Minions); i++)
        {
            long minionPtr = 0;
            NativeWrapper.ReadProcessMemory(MemoryAccessor.Handle, GetListPtr(ManagerTemplateType.Minions) + i * 0x8,
                ref minionPtr);

            AIMinionClient minion = new AIMinionClient(minionPtr);

            yield return minion;
        }
    }

    public MinionManager() : base()
    {
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}