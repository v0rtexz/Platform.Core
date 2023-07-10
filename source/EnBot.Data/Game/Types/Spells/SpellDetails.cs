using Ensage.Data.Utils;

namespace Ensage.Data.Game.Types.Spells;

/// <summary>
/// Contains information about the caster, particles etc.
/// </summary>
internal class SpellDetails : MemoryObject
{
    internal string CasterName => RiotString.GetByPtr(this.address + Offsets.CasterName);
    internal long CasterNameHash => GetProperty<long>(Offsets.CasterName);

    /// <summary>
    /// Initializes a new instance of the <see cref="SpellDetails"/> class.
    /// </summary>
    /// <param name="address">The address of the instance.</param>
    internal SpellDetails(long address)
    {
        base.address = address;
    }
}