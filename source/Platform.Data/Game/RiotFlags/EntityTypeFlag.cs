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

namespace Ensage.Data.Game.RiotFlags;

[Flags]
public enum EntityTypeFlag : long
{
    Champion = 7957694998179309635,
    Special = 30506402751803475,
    Ward = 5989923982968774999,
    Minion_Lane = 8944270284747073869,
    Minion_Lane_Siege = 7306920423476651374,
    Minion_Lane_Ranged = 7306930284704785774,
    Minion_Lane_Melee = 7306365152824092014,
    Minion_Lane_Super = 8243118342183806318,
    Monster = 2338042707385937741,
    Monster_Epic = 2340781521963538015,
    Monster_Dragon = 2336927746459059295,
    Special_Void = 2340781521963538015,
    Structure_Turret = 4294967297,
    UNKNOWN = 5980780305148018688,
}