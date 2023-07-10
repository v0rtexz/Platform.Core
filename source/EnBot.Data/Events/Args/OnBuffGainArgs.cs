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
using Ensage.Data.Game.Types;
using Ensage.Data.Game.Types.Buff;
using JetBrains.Annotations;

namespace Ensage.Data.Events.Args;

[PublicAPI]
public struct OnBuffGainArgs
{
    public BuffEntry Buff { get; set; }
    public long BuffHash { get; set; }
    public AIBaseClient Owner { get; set; }
    public long OwnerHash { get; set; }

    internal OnBuffGainArgs
    (
        BuffEntry buff,
        long buffHash,
        AIBaseClient owner,
        long ownerHash
    )
    {
        this.Buff = buff;
        this.BuffHash = buffHash;
        this.Owner = owner;
        this.OwnerHash = ownerHash;
    }
}