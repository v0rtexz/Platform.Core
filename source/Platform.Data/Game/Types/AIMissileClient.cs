using System.Numerics;
using Ensage.Data.Game.Components;
using Ensage.Data.Utils;

namespace Ensage.Data.Game.Types;

/// <summary>
/// Type for missiles. Inherited by <see cref="AIBaseClient"/>.
/// </summary>
public class AIMissileClient : AIBaseClient
{
    #region Properties

    public Vector3 StartPosition => GetProperty<Vector3>(0x38c);
    public Vector3 EndPosition => GetProperty<Vector3>(0x398);
    
    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AIMissileClient"/> class.
    /// </summary>
    /// <param name="address">The address of the unit.</param>
    public AIMissileClient(long address)
        : base(address)
    {
    }

    #endregion

    #region Methods

    public string GetMissileName()
    {
        return RiotString.GetByPtr(this.address + 0x60);
    }
    
    #endregion
}