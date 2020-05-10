using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// The possible elemental types
/// </summary>
public enum Elements
{
    Fire,
    Water,
    Grass
}

[System.Serializable]
public class Pokemon
{
    //fields, I made everything public so we can easily see them in the inspector
    public string name;
    public int level;
    public int baseAttack;
    public int baseDefence;
    public int hp;
    public int maxHp;
    public Elements element;
    public List<Move> moves;

    /// <summary>
    /// Constructor for a Pokemon, the arguments are fairly self-explanatory
    /// </summary>
    /// <param name="name"></param>
    /// <param name="level"></param>
    /// <param name="baseAttack"></param>
    /// <param name="baseDefence"></param>
    /// <param name="hp"></param>
    /// <param name="element"></param>
    /// <param name="moves">This needs to be a List of Move objects</param>
    public Pokemon(string name, int level, int baseAttack,
        int baseDefence, int hp, Elements element, List<Move> moves)
    {
        this.level = level;
        this.baseAttack = baseAttack;
        this.baseDefence = baseDefence;
        this.name = name;
        this.hp = hp * level;
        maxHp = hp * level;
        this.element = element;
        this.moves = moves;
    }

    /// <summary>
    /// performs an attack and returns total damage, check the slides for how to calculate the damage
    /// IMPORTANT: should also apply the damage to the enemy pokemon
    /// </summary>
    /// <param name="enemy">This is the enemy pokemon that we are attacking</param>
    /// <returns>The amount of damage that was applied so we can print it for the user</returns>
    public int Attack(Pokemon enemy)
    {
        //calculate attack
        int attack = baseAttack * level;
        attack = CalculateElementalEffects(attack, enemy.element);
        int defence = enemy.CalculateDefence();
        int damage = (attack - defence)/10;

        if (damage < 0)
            return 0;
        else
        {
            enemy.ApplyDamage(damage);
            return damage;
        }
    }

    /// <summary>
    /// calculate the current amount of defence points
    /// </summary>
    /// <returns> returns the amount of defence points considering the level as well</returns>
    public int CalculateDefence()
    {
        return baseDefence * level;
    }

    /// <summary>
    /// Calculates elemental effect, check table at https://bulbapedia.bulbagarden.net/wiki/Type#Type_chart for a reference
    /// </summary>
    /// <param name="damage">The amount of pre elemental-effect damage</param>
    /// <param name="enemyType">The elemental type of the enemy</param>
    /// <returns>The damage post elemental-effect</returns>
    public int CalculateElementalEffects(int damage, Elements enemyType)
    {
        if (element == Elements.Water && enemyType == Elements.Fire ||
            element == Elements.Grass && enemyType == Elements.Water ||
            element == Elements.Fire && enemyType == Elements.Grass
            )
        {
            //it's super effective!
            return damage * 2;
        }
        if (element == Elements.Fire && enemyType == Elements.Water ||
            element == Elements.Water && enemyType == Elements.Grass ||
            element == Elements.Grass && enemyType == Elements.Fire
            )
        {
            //it's not so effective
            return damage / 2;
        }
        return damage;
    }

    /// <summary>
    /// Applies damage to the pokemon
    /// </summary>
    /// <param name="damage"></param>
    public void ApplyDamage(int damage)
    {
        hp -= damage;
    }

    /// <summary>
    /// Heals the pokemon by resetting the HP to the max
    /// </summary>
    public void Restore()
    {
        hp = maxHp;
    }
}

