using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character 
{
    private Resources.Type type1;
    private Resources.Type type2;
    private string name;
    private Resources.Rank rank;
    private int level;
    Stat originalStats;
    Stat currentStats;
    Move[] currentMoves;
    Dictionary<int, Move> allMoves;
    Resources.Status status;

    private int hpFactor;
    private int energyFactor;
    private int atkFactor;
    private int spatkFactor;
    private int defFactor;
    private int spdefFactor;
    private int spdFactor;

    public Character(   Resources.Type type1, 
                        Resources.Type type2, 
                        string name,
                        Resources.Rank rank, 
                        int level, 
                        Stat originalStats,
                        Move[] currentMoves, 
                        Dictionary<int, Move> allMoves, 
                        Resources.Status status )
    {
        this.type1 = type1;
        this.type2 = type2;
        this.name = name;
        this.rank = rank;
        this.level = level;
        this.originalStats = originalStats;
        this.currentStats = new Stat(originalStats.getHp(), originalStats.getEnergy(), originalStats.getAtk(), originalStats.getSpatk(), originalStats.getDef(), originalStats.getSpdef(), originalStats.getSpd());
        this.currentMoves = currentMoves;
        this.allMoves = allMoves;
        this.status = status;

        this.hpFactor = 0;
        this.energyFactor = 0;
        this.atkFactor = 0;
        this.spatkFactor = 0;
        this.defFactor = 0;
        this.spdefFactor = 0;
        this.spdFactor = 0;
    }

    // GETTER FUNCTIONS

    public bool hasTwoTypes()
    {
        return this.type2 == Resources.Type.None;
    }

    public Resources.Type getType1()
    {
        return this.type1;
    }

    public Resources.Type getType2()
    {
        return this.type2;
    }

    public string getName()
    {
        return this.name;
    }

    public Resources.Rank getRank()
    {
        return this.rank;
    }

    public int getLevel()
    {
        return this.level;
    }

    public Stat getOriginalStats()
    {
        return this.originalStats;
    }

    public Stat getCurrentStats()
    {
        return this.currentStats;
    }

    public Move[] getMoves()
    {
        return this.currentMoves;
    }

    public Dictionary<int, Move> getMoveList()
    {
        return this.allMoves;
    }

    public Resources.Status getStatus()
    {
        return this.status;
    }

    private float getPercentageFromFactor(int factor)
    {
        if (factor == 0)
        {
            return 1f;
        }

        float num = 2f;
        float denom = 2f;
        if (factor > 0)
        {
            num += (float) factor * 0.8f;
        }
        if (factor < 0)
        {
            denom += (float) factor * 0.8f;
        }
        return num / denom;
    }

    // SETTER FUNCTIONS

    public bool changeStat( Resources.StatType type, int factor)
    {
        int newValue = 0;
        int maxFactor = 6;
        switch (type)
        {
            case Resources.StatType.Hp:
                if (this.hpFactor == maxFactor || this.hpFactor == maxFactor * -1)
                {
                    return true;
                }
                this.hpFactor += factor;
                this.hpFactor = Mathf.Min(this.hpFactor, maxFactor);
                this.hpFactor = Mathf.Max(this.hpFactor, maxFactor * -1);
                newValue = (int)Mathf.Round(this.originalStats.getHp() * getPercentageFromFactor(this.hpFactor));
                this.currentStats.setHp(newValue);
                break;
            case Resources.StatType.Energy:
                if (this.energyFactor == maxFactor || this.energyFactor == maxFactor * -1)
                {
                    return true;
                }
                this.energyFactor += factor;
                this.energyFactor = Mathf.Min(this.energyFactor, maxFactor);
                this.energyFactor = Mathf.Max(this.energyFactor, maxFactor * -1);
                newValue = (int)Mathf.Round(this.originalStats.getEnergy() * getPercentageFromFactor(this.energyFactor));
                this.currentStats.setEnergy(newValue);
                break;
            case Resources.StatType.Atk:
                if (this.atkFactor == maxFactor || this.atkFactor == maxFactor * -1)
                {
                    return true;
                }
                this.atkFactor += factor;
                this.atkFactor = Mathf.Min(this.atkFactor, maxFactor);
                this.atkFactor = Mathf.Max(this.atkFactor, maxFactor * -1);
                newValue = (int)Mathf.Round(this.originalStats.getAtk() * getPercentageFromFactor(this.atkFactor));
                this.currentStats.setAtk(newValue);
                break;
            case Resources.StatType.Spatk:
                if (this.spatkFactor == maxFactor || this.spatkFactor == maxFactor * -1)
                {
                    return true;
                }
                this.spatkFactor += factor;
                this.spatkFactor = Mathf.Min(this.spatkFactor, maxFactor);
                this.spatkFactor = Mathf.Max(this.spatkFactor, maxFactor * -1);
                newValue = (int)Mathf.Round(this.originalStats.getSpatk() * getPercentageFromFactor(this.spatkFactor));
                this.currentStats.setSpatk(newValue);
                break;
            case Resources.StatType.Def:
                if (this.defFactor == maxFactor || this.defFactor == maxFactor * -1)
                {
                    return true;
                }
                this.defFactor += factor;
                this.defFactor = Mathf.Min(this.defFactor, maxFactor);
                this.defFactor = Mathf.Max(this.defFactor, maxFactor * -1);
                newValue = (int)Mathf.Round(this.originalStats.getDef() * getPercentageFromFactor(this.defFactor));
                this.currentStats.setDef(newValue);
                break;
            case Resources.StatType.Spdef:
                if (this.spdefFactor == maxFactor || this.spdefFactor == maxFactor * -1)
                {
                    return true;
                }
                this.spdefFactor += factor;
                this.spdefFactor = Mathf.Min(this.spdefFactor, maxFactor);
                this.spdefFactor = Mathf.Max(this.spdefFactor, maxFactor * -1);
                newValue = (int)Mathf.Round(this.originalStats.getSpdef() * getPercentageFromFactor(this.spdefFactor));
                this.currentStats.setSpdef(newValue);
                break;
            case Resources.StatType.Spd:
                if (this.spdFactor == maxFactor || this.spdFactor == maxFactor * -1)
                {
                    return true;
                }
                this.spdFactor += factor;
                this.spdFactor = Mathf.Min(this.spdFactor, maxFactor);
                this.spdFactor = Mathf.Max(this.spdFactor, maxFactor * -1);
                newValue = (int)Mathf.Round(this.originalStats.getSpd() * getPercentageFromFactor(this.spdFactor));
                this.currentStats.setSpd(newValue);
                break;
            default:
                break;
        }

        // return false for not maxed out
        return false;
    }

    public void setCurrentStats( int hp, int energy, int atk, int spatk, int def, int spdef, int spd )
    {
        this.currentStats.setHp(hp);
        this.currentStats.setEnergy(energy);
        this.currentStats.setAtk(atk);
        this.currentStats.setSpatk(spatk);
        this.currentStats.setDef(def);
        this.currentStats.setSpdef(spdef);
        this.currentStats.setSpd(spd);
    }

    public void setOriginalStats( int hp, int energy, int atk, int spatk, int def, int spdef, int spd )
    {
        this.originalStats.setHp(hp);
        this.originalStats.setEnergy(energy);
        this.originalStats.setAtk(atk);
        this.originalStats.setSpatk(spatk);
        this.originalStats.setDef(def);
        this.originalStats.setSpdef(spdef);
        this.originalStats.setSpd(spd);
    }

    public void setRank( Resources.Rank rank)
    {
        this.rank = rank;
    }

    public void setLevel( int level )
    {
        this.level = level;
    }

    public void setMoves( Move[] moves)
    {
        this.currentMoves = moves;
    }

    public void setStatus( Resources.Status status)
    {
        this.status = status;
    }
}
