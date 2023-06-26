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

namespace Ensage.Data.Game;

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
    public const int Level = 0x4830;
    public const int TotalEXP = 0x3FA0;
    public const int LevelPoints = 0x3FF8;
    public const int ChampionName = 0x3850;
    public const int ObjectName = 0x35F0; // seems to hide behind a pointer
    public const int NetworkID = 0xC8;

    public const int UnitComponentInfo = 0x3680;
    public const int UnitComponentInfoProperties = 0x28;

    // Inside UnitComponentInfo Properties
    internal static readonly int BoundingRadius = 0x564; // if > 2000 bounding radius is 65.
    internal static readonly int RoleString = 0x800; // Example: 'marksman'

    internal static readonly int BaseAttackSpeed = 0x254;

    //base as and ratio are same for most champs, but not for all.
    internal static readonly int AttackSPeedRatio = 0x258;
    internal static readonly int IsHero = 0x84; // bool, true if its a hero
    internal static readonly int ObjectType = 0x768;
    internal static readonly int ObjectTypeDetailed = 0x20;

    #endregion

    #region ManagerTemplates

    public const int HeroList = 0x213DC68;
    public const int MinionList = 0x3998608;
    public const int TurretList = 0x51E4D20;
    public const int MissileMap = 0x51F6CD0;

    public const int ManagerTemplateListPtr = 0x8;
    public const int ManagerTemplateSize = 0x10;

    #endregion

    #region Rendering

    public const int Renderer = 0x523EBC8;
    public const int ViewMatrix = 0x5236720;

    #endregion
}