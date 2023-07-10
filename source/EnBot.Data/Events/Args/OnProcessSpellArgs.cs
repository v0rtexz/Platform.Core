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
using JetBrains.Annotations;

namespace Ensage.Data.Events.Args;

[PublicAPI]
public struct OnProcessSpellArgs
{
    public string Name { get; set; }
    public string CasterName { get; set; }
    public AIBaseClient Caster { get; set; }
    public long CasterHash { get; set; }
    public long NetworkID { get; set; }
    public float StartTime { get; set; }
    public bool IsAutoAttack { get; set; }
    public bool IsSpell { get; set; }
    public Vector3 StartPosition { get; set; }
    public Vector3 EndPosition { get; set; }
    
    
    internal OnProcessSpellArgs
    (
        string name,
        string casterName,
        long casterHash,
        long networkId,
        Vector3 startPosition,
        Vector3 endPosition,
        float startTime,
        bool isAutoAttack,
        bool isSpell
    )
    {
        this.Name = name;
        this.CasterName = casterName;
        this.CasterHash = casterHash;
        this.NetworkID = networkId;
        this.StartPosition = startPosition;
        this.EndPosition = endPosition;
        this.StartTime = startTime;
        this.IsAutoAttack = isAutoAttack;
        this.IsSpell = isSpell;
    }
}