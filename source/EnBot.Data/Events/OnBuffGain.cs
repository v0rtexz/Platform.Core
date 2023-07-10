using Ensage.Data.Events.Args;
using Ensage.Data.Game;
using Ensage.Data.Game.Types;
using Ensage.Data.Game.Types.Buff;
using JetBrains.Annotations;
using Serilog;

namespace Ensage.Data.Events;

public class OnBuffGain : ICallback
{
    [NotNull] private ILogger logger;
    [NotNull] private World world;
    [NotNull] private Engine engine;

    private Dictionary<long, List<BuffEntry>>
        currentBuffs = new Dictionary<long, List<BuffEntry>>();

    public OnBuffGain([NotNull] ILogger logger, [NotNull] World world, [NotNull] Engine engine)
    {
        this.logger = logger;
        this.world = world;
        this.engine = engine;

        StoreCurrentBuffsOnStart();
        InstantiateCallback();
    }

    public void StoreCurrentBuffsOnStart()
    {
        foreach (var hero in world.Heroes)
        {
            currentBuffs[hero.HashedName] = new List<BuffEntry>(hero.Buffs);
        }
    }

    public void TriggerIfConditionMet()
    {
        foreach (var hero in world.Heroes)
        {
            foreach (var buff in hero.Buffs)
            {
                if (!HasBuffWithName(hero.HashedName, buff.Name))
                {
                    OnBuffGainArgs args = new OnBuffGainArgs();
                    // New buff found
                    args.Buff = buff;
                    args.BuffHash = buff.HashedName;
                    args.Owner = hero;
                    args.OwnerHash = hero.HashedName;

                    EventManager.ExecuteEvent<OnBuffGainArgs>((short)EventID.OnBuffGain, args);
                }
            }

            currentBuffs[hero.HashedName] = hero.Buffs.ToList();
        }
    }

    private bool HasBuffWithName(long hashedName, string buffName)
    {
        if (currentBuffs.ContainsKey(hashedName))
        {
            foreach (var buff in currentBuffs[hashedName])
            {
                if (buff.Name.Equals(buffName))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void Test(OnBuffGainArgs args)
    {
        Console.WriteLine("hero: " + args.Owner.ObjectName + " Buff: " + args.Buff.Name);
    }

    public void InstantiateCallback()
    {
        EventManager.RegisterEvent((short)EventID.OnUpdate, TriggerIfConditionMet);
        EventManager.RegisterEvent<OnBuffGainArgs>((short)EventID.OnBuffGain, Test);
    }
}