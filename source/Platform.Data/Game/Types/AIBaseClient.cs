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

using System.Reflection;
using Platform.Data.Game.Components;
using Platform.Data.Game.Engine;

namespace Platform.Data.Game.Types;

using Platform.Data.Game.Components;
using ProcessMemoryUtilities.Managed;
using System.Numerics;

/// <summary>
/// Represents all GameObjects.
/// </summary>
public abstract class AIBaseClient
{
    #region Properties

    protected long address;

    public float Health => GetProperty<float>(Offsets.Health);
    public float MaxHealth => GetProperty<float>(Offsets.MaxHealth);
    public float Mana => GetProperty<float>(Offsets.Mana);
    public float MaxMana => GetProperty<float>(Offsets.MaxMana);
    public float Armor => GetProperty<float>(Offsets.Armor);
    public float BonusArmor => GetProperty<float>(Offsets.BonusArmor);
    public float MagicResist => GetProperty<float>(Offsets.MagicResist);
    public float BonusMagicResist => GetProperty<float>(Offsets.BonusMagicResist);
    public float MovementSpeed => GetProperty<float>(Offsets.MovementSpeed);
    public int Team => GetProperty<int>(Offsets.Team);
    public Vector3 Pos => GetProperty<Vector3>(Offsets.Pos);
    public float AttackRange => GetProperty<float>(Offsets.AttackRange);
    public float BaseAttack => GetProperty<float>(Offsets.BaseAttack);
    public float BonusAttack => GetProperty<float>(Offsets.BonusAttack);
    public float BonusAtkSpeed => GetProperty<float>(Offsets.BonusAtkSpeed);
    public float AtkSpeedMulti => GetProperty<float>(Offsets.AtkSpeedMulti);
    public float Direction => GetProperty<float>(Offsets.Direction);
    public float SizeMulti => GetProperty<float>(Offsets.SizeMulti);
    public float AbilityPower => GetProperty<float>(Offsets.AbilityPower);
    public float Lethality => GetProperty<float>(Offsets.Lethality);
    public float ArmorPenPerc => GetProperty<float>(Offsets.ArmorPenPerc);
    public float AbilityPenFlat => GetProperty<float>(Offsets.AbilityPenFlat);
    public float AbilityPenPerc => GetProperty<float>(Offsets.AbilityPenPerc);
    public float AbilityHaste => GetProperty<float>(Offsets.AbilityHaste);
    public int Level => GetProperty<int>(Offsets.Level);

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
        Memory.Memory m = ComponentController.GetComponent<Memory.Memory>();
        TType buffer = default(TType);
        NativeWrapper.ReadProcessMemory<TType>(m.Handle, (IntPtr)(address + offset), ref buffer);

        return buffer;
    }

    /// <summary>
    /// Get the name of the object.
    /// </summary>
    /// <returns>The name of the object.</returns>
    public string GetName()
    {
        return RiotString.Get(this.address + Offsets.Name);
    }

    #endregion
}