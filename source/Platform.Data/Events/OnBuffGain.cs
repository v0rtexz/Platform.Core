using Ensage.Data.Game;
using Ensage.Data.Game.Types.Buff;
using JetBrains.Annotations;
using Serilog;

namespace Ensage.Data.Events;

public class OnBuffGain : ICallback
{
    [NotNull] private ILogger logger;
    [NotNull] private World world;
    [NotNull] private Engine engine;

    private Dictionary<long, List<BuffEntry>> buffList = new Dictionary<long, List<BuffEntry>>();

    public OnBuffGain([NotNull] ILogger logger, [NotNull] World world, [NotNull] Engine engine)
    {
        this.logger = logger;
        this.world = world;
        this.engine = engine;

        InitBuffs();
        InstantiateCallback();
    }

    private void InitBuffs()
    {
        foreach (var hero in world.Heroes)
        {
            var copy = hero.Buffs.ToList();
            buffList[hero.HashedName] = copy;
        }
    }

    public void TriggerIfConditionMet()
    {
        BuffEntry newBuff;
        foreach (var hero in world.Heroes)
        {
            foreach (var buff in hero.Buffs)
            {
                if (buffList[hero.HashedName].AsParallel().Any(storedBuff => storedBuff.HashedName == buff.HashedName))
                {
                    // buffList[hero.HashedName].Remove(buff);
                    //   Console.WriteLine("BUFF REMOVED " + buff.Name);
                }
                else
                {
                    Console.WriteLine("New buff added for hero: " + hero.ChampionName + " " + buff.Name);
                    // Send notification or perform any desired action

                    // Update the stored buffs for the hero

                    buffList[hero.HashedName].Add(buff);
                }

                //TODO : Add OnBuffLose. Same as here but the opposite.
                // if (buffList[hero.HashedName].AsParallel().Any(storedBuff => storedBuff.HashedName == buff.HashedName))
            }
        }
    }

    private void Test()
    {
    }

    public void InstantiateCallback()
    {
        EventManager.RegisterCallback<EventDelegate.EvtOnUpdate>(() => TriggerIfConditionMet());
        // EventManager.RegisterCallback<EventDelegate.EvtOnProcessSpell>((args) => Test(args));
    }
}