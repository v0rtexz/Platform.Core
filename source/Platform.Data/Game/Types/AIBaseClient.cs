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
using Ensage.Data.Game.Components;
using Ensage.Data.Game.RiotFlags;
using Ensage.Data.Memory;
using Ensage.Data.Utils;
using JetBrains.Annotations;
using ProcessMemoryUtilities.Managed;

namespace Ensage.Data.Game.Types;

/// <summary>
/// Represents all GameObjects.
/// </summary>
public abstract class AIBaseClient
{
    #region Properties

    public long address;

    [PublicAPI] public float Health => GetProperty<float>(Offsets.Health);

    [PublicAPI] public float MaxHealth => GetProperty<float>(Offsets.MaxHealth);

    [PublicAPI] public float Mana => GetProperty<float>(Offsets.Mana);

    [PublicAPI] public float MaxMana => GetProperty<float>(Offsets.MaxMana);

    [PublicAPI] public float Armor => GetProperty<float>(Offsets.Armor);

    [PublicAPI] public float BonusArmor => GetProperty<float>(Offsets.BonusArmor);

    [PublicAPI] public float MagicResist => GetProperty<float>(Offsets.MagicResist);

    [PublicAPI] public float BonusMagicResist => GetProperty<float>(Offsets.BonusMagicResist);

    [PublicAPI] public float MovementSpeed => GetProperty<float>(Offsets.MovementSpeed);

    [PublicAPI] public int Team => GetProperty<int>(Offsets.Team);

    [PublicAPI] public Vector3 Pos => GetProperty<Vector3>(Offsets.Pos);

    [PublicAPI] public float AttackRange => GetProperty<float>(Offsets.AttackRange);

    [PublicAPI] public float BaseAttack => GetProperty<float>(Offsets.BaseAttack);

    [PublicAPI] public float BonusAttack => GetProperty<float>(Offsets.BonusAttack);

    [PublicAPI] public float BonusAtkSpeed => GetProperty<float>(Offsets.BonusAtkSpeed);

    [PublicAPI] public float AtkSpeedMulti => GetProperty<float>(Offsets.AtkSpeedMulti);
    [PublicAPI] public float Direction => GetProperty<float>(Offsets.Direction);
    [PublicAPI] public float SizeMulti => GetProperty<float>(Offsets.SizeMulti);
    [PublicAPI] public float AbilityPower => GetProperty<float>(Offsets.AbilityPower);
    [PublicAPI] public float Lethality => GetProperty<float>(Offsets.Lethality);
    [PublicAPI] public float ArmorPenPerc => GetProperty<float>(Offsets.ArmorPenPerc);
    [PublicAPI] public float AbilityPenFlat => GetProperty<float>(Offsets.AbilityPenFlat);
    [PublicAPI] public float AbilityPenPerc => GetProperty<float>(Offsets.AbilityPenPerc);
    [PublicAPI] public float AbilityHaste => GetProperty<float>(Offsets.AbilityHaste);
    [PublicAPI] public float BoundingRadius => GetBoundingRadius();
    [PublicAPI] public int Level => GetProperty<int>(Offsets.Level);
    [PublicAPI] public EntityTypeFlag ObjectFlag => GetObjectTypeFlag();
    [PublicAPI] public string ObjectName => GetObjectName();
    [PublicAPI] public long HashedName => GetNameHash();
    [PublicAPI] public long NetworkID => GetProperty<long>(Offsets.NetworkID);
    [PublicAPI] public string TypeName => GetObjType();

    [PublicAPI] public Vector2 ScreenPosition => GetScreenPos();

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AIBaseClient"/> class.
    /// </summary>
    /// <param name="address">The memory address of the unit.</param>
    public AIBaseClient(long address)
    {
        this.address = address;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Read a property out of the objects memory space.
    /// </summary>
    /// <param name="offset">The offset to read from.</param>
    /// <typeparam name="TType">The type of the property.</typeparam>
    /// <returns>The property value.</returns>
    protected TType GetProperty<TType>(int offset)
        where TType : unmanaged
    {
        TType buffer = default(TType);
        NativeWrapper.ReadProcessMemory<TType>(MemoryAccessor.Handle, (IntPtr)(this.address + offset), ref buffer);

        return buffer;
    }

    /// <summary>
    /// Get the position transformed into the screen position.
    /// </summary>
    /// <returns>The screen position as<see cref="Vector2"/>.</returns>
    private Vector2 GetScreenPos()
    {
        Vector2 screenPos = Vector2.Zero;
        Renderer.WorldToScreen(this.Pos, ref screenPos);

        return screenPos;
    }

    /// <summary>
    /// Checks if the team is neutral.
    /// </summary>
    /// <returns>Returns trie if the team is neutral.</returns>
    public bool IsNeutral()
    {
        return this.Team == 300;
    }

    /// <summary>
    /// Checks if the object is a minion.
    /// </summary>
    /// <returns></returns>
    public bool IsMinion()
    {
        return this.IsLaneMinion() || this.IsJungleMonster() || this.IsEpicJungle();
    }

    /// <summary>
    /// Checks if the object is a lane minion.
    /// </summary>
    /// <returns>True if its a lane minion.</returns>
    public bool IsLaneMinion()
    {
        return this.ObjectFlag == EntityTypeFlag.Minion_Lane;
    }

    /// <summary>
    /// Checks if the object is a jungle monster.
    /// </summary>
    /// <returns>True if its a jungle monster.</returns>
    public bool IsJungleMonster()
    {
        return this.ObjectFlag == EntityTypeFlag.Monster || IsEpicJungle();
    }

    /// <summary>
    /// Checks if the object is a ward.
    /// </summary>
    /// <returns>True if its a ward.</returns>
    public bool IsWard()
    {
        return this.ObjectFlag == EntityTypeFlag.Ward;
    }

    /// <summary>
    /// Checks if the object is a epic monster.
    /// </summary>
    /// <returns>True if its a epic monster.</returns>
    public bool IsEpicJungle()
    {
        return this.GetDetailedObjTypeHash() == EntityTypeFlag.Monster_Dragon ||
               this.HashedName == 8030588023250178643; //SRU_Baron
    }

    /// <summary>
    /// Checks if the object is a dragon.
    /// </summary>
    /// <returns>True if its a dragon.</returns>
    public bool IsDragon()
    {
        return this.GetDetailedObjTypeHash() == EntityTypeFlag.Monster_Dragon;
    }

    /// <summary>
    /// Checks if the object is a hero.
    /// </summary>
    /// <returns>True if its a hero.</returns>
    public bool IsHero()
    {
        return GetProperty<bool>(GetUnitComponentInfoPropertes(), Offsets.IsHero);
    }

    /// <summary>
    /// Read a property out of the objects memory space.
    /// </summary>
    /// <param name="address">The address to read from..</param>
    /// <param name="offset">The offset to read from.</param>
    /// <typeparam name="TType">The type of the property.</typeparam>
    /// <returns>The property value.</returns>
    protected TType GetProperty<TType>(long address, int offset)
        where TType : unmanaged
    {
        TType buffer = default(TType);
        NativeWrapper.ReadProcessMemory<TType>(MemoryAccessor.Handle, (IntPtr)(address + offset), ref buffer);

        return buffer;
    }

    /// <summary>
    /// Get the name of the object.
    /// </summary>
    /// <returns>The name of the object.</returns>
    private string GetObjectName()
    {
        return RiotString.GetByPtr(this.address + Offsets.ObjectName);
    }

    /// <summary>
    /// Get the hashed name.
    /// </summary>
    /// <returns>The hashed name.</returns>
    private long GetNameHash()
    {
        long ptr = GetProperty<long>(Offsets.ObjectName);
        long hash = GetProperty<long>(ptr, 0x0);

        // some objects have their hash at ptr + 0x8
        if (hash < 0x1000)
        {
            hash = GetProperty<long>(ptr, 0x8);
        }

        return hash;
    }

    private long GetUnitComponentInfoPropertes()
    {
        long unitComponentInfo = this.GetProperty<long>(Offsets.UnitComponentInfo);

        return GetProperty<long>(unitComponentInfo, Offsets.UnitComponentInfoProperties);
    }

    /// <summary>
    /// Get the bounding radius of the unit.
    /// </summary>
    /// <returns>The bounding radius of the unit.</returns>
    private float GetBoundingRadius()
    {
        float boundingRadius = GetProperty<float>(GetUnitComponentInfoPropertes(), Offsets.BoundingRadius);

        return boundingRadius > 2000f ? 65f : boundingRadius;
    }

    /// <summary>
    /// Get the ObjectType as string.
    /// </summary>
    /// <returns>The object type as string</returns>
    private string GetObjType()
    {
        long stringPtr = GetProperty<long>(GetUnitComponentInfoPropertes(), Offsets.ObjectType);
        String type = RiotString.Get(stringPtr, 50);

        return type.Split("|")[0];
    }

    /// <summary>
    /// Get the entity type flag.
    /// </summary>
    /// <returns>Return the entity type flag.</returns>
    private EntityTypeFlag GetObjectTypeFlag()
    {
        long ptr = this.GetProperty<long>(this.GetUnitComponentInfoPropertes(), Offsets.ObjectType);
        long hash = this.GetProperty<long>(ptr, 0x0);

        return (EntityTypeFlag)hash;
    }

    /// <summary>
    /// Detailed object type. Info such as different minion types etc.
    /// </summary>
    /// <returns></returns>
    private EntityTypeFlag GetDetailedObjTypeHash()
    {
        long ptr = this.GetProperty<long>(this.GetUnitComponentInfoPropertes(), Offsets.ObjectType);
        long hash = this.GetProperty<long>(ptr, Offsets.ObjectTypeDetailed);

        // some objects have their hash at ptr + 0x8
        if (hash < 0x1000)
        {
            hash = GetProperty<long>(ptr, 0x8);
        }

        return (EntityTypeFlag)hash;
    }

    #endregion
}