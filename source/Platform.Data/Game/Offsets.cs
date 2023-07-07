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

    public const int GameTime = 0x521EE00; // F3 0F 5C 35 ?? ?? ?? ?? 0F 28 F8
    public const int LocalPlayer = 0x522B768; // 48 8B 3D ?? ?? ?? ?? 48 3B CF

    #endregion

    #region Game Objects

    public const int Health = 0x1068;
    public const int MaxHealth = 0x1080;
    public const int Mana = 0x340;
    public const int MaxMana = 0x358;
    public const int Armor = 0x16A4;
    public const int BonusArmor = 0x16A8;
    public const int MagicResist = 0x16AC;
    public const int BonusMagicResist = 0x16B0;
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
    public const int TotalEXP = 0x3F98;
    public const int LevelPoints = 0x3FF0;
    public const int ChampionName = 0x3848;
    public const int ObjectName = 0x35E8; // seems to hide behind a pointer
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

    public const int HeroList = 0x2173340; // 48 8B 05 ? ? ? ? 45 33 E4 0F 57 C0
    public const int MinionList = 0x39CDCE0; // 48 8B 0D ? ? ? ? E8 ? ? ? ? EB 07
    public const int TurretList = 0x521A3F0; // 48 89 0D ? ? ? ? 33 C9
    public const int MissileMap = 0x522B828; // 48 8B 0D ? ? ? ? 48 8D 54 24 ? E8 ? ? ? ? 48 8B 7C 24 ?

    public const int ManagerTemplateListPtr = 0x8;
    public const int ManagerTemplateSize = 0x10;

    #endregion

    #region Missiles

    public const int MissileName = 0x60;

    public const int MissileSpellInfo = 0x2E8;

    public const int MissileStartPosition = 0x38c;
    public const int MissileEndPosition = 0x398;

    #endregion

    #region SpellBook

    public const int SpellBook = 0x30C8;
    public const int SpellSlotLevel = 0x28;
    public const int SpellSlotTime = 0x30;
    public const int SpellSlotCharges = 0x58;
    public const int SpellSlotTimeCharge = 0x78;
    public const int SpellSlotSpellInfo = 0x130;

    #endregion

    #region ActiveSpell

    public const int SpellCast = 0x2A30;
    public const int SpellCastSpellInfo = 0x8;

    public const int ActiveSpellStartTime = 0x180;
    public const int ActiveSpellEndTime = 0x168;

    public const int ActiveSpellStartPos = 0xac;
    public const int ActiveSpellEndPos = 0xb8;
    public const int ActiveSpellIsAA = 0x110;
    public const int ActiveSpellIsSpell = 0x114;

    public const int SourceIndex = 0x8c;
    public const int TargetIndex = 0xe0;

    #endregion

    #region SpellInfo

    public const int SpellName = 0x28;

    public const int
        DetailedMissileInfo = 0x238; // Missile -> 0x238 | contains caster name and stuff like Particle strings

    public const int CasterName = 0x10;

    #endregion

    #region SpellData

    public const int SpellData = 0x60;
    public const int SpellDataMissileName = 0x80;

    #endregion

    #region BuffManager

    public const int BuffManagerInstance = 0x27c0;
    public const int BuffStartTime = 0x18;
    public const int BuffEndTime = 0x1C;
    public const int BuffCount = 0x8C;
    public const int BuffCountAlt = 0x3C;
    public const int BuffCountAlt2 = 0x38;
    public const int BuffType = 0x8;
    public const int BuffInfoName = 0x8;

    #endregion

    #region Rendering

    public const int Renderer = 0x5274650; // 4C 39 2D ? ? ? ? 74 5C
    public const int ViewMatrix = 0x526C1B0; // 48 8D 0D ? ? ? ? 0F 10 00

    #endregion
}