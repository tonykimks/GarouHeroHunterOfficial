using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatMove : Move
{
    private Resources.StatType[] stats;
    private int factor;

    public StatMove(string name, Resources.Type type, bool target, int accuracy, bool requireAim, int order, int cost, int penalty, Resources.StatType[] stats, int factor) : base(name, type, target, accuracy, requireAim, order, cost, penalty)
    {
        this.stats = stats;
        this.factor = factor;
    }

    public Resources.StatType[] getStats()
    {
        return this.stats;
    }

    public int getFactor()
    {
        return this.factor;
    }

}
