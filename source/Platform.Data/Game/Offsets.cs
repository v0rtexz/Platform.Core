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

namespace Platform.Data.Game;

/// <summary>
/// Class holding all memory offsets.
/// </summary>
public class Offsets
{
    #region General

    public const int GameTime = 0x51E8C70;
    public const int LocalPlayer = 0x51F52D0;

    #endregion

    #region Game Objects

    public const int Health = 0x1068;
    public const int MaxHealth = 0x1080;
    public const int Mana = 0x340;
    public const int MaxMana = 0x358;
    public const int Armor = 0x16A4;
    public const int BonusArmor = 0x1680;
    public const int MagicResist = 0x1684;
    public const int BonusMagicResist = 0x1688;
    public const int MovementSpeed = 0x16BC;
    public const int Team = 0x3C;
    public const int Pos = 0x220;
    public const int AttackRange = 0x16C4;
    public const int BaseAttack = 0x1BC8;
    public const int BonusAttack = 0x1AA8;
    public const int BonusAtkSpeed = 0x164C;
    public const int AtkSpeedMulti = 0x1678;
    public const int Direction = 0x3EB9;
    public const int SizeMulti = 0x1694;
    public const int AbilityPower = 0x15F8;
    public const int Lethality = 0x1DD8;
    public const int ArmorPenPerc = 0x1DF0;
    public const int AbilityPenFlat = 0x11C0;
    public const int AbilityPenPerc = 0x11C8;
    public const int AbilityHaste = 0x1A00;
    public const int Level = 0x4038;
    public const int Name = 0x3850;

    #endregion

    #region Rendering

    public const int Renderer = 0x523EBC8;
    public const int ViewMatrix = 0x5236720;

    #endregion
}