using UnityEngine;
using System.Collections.Generic;

public class SpellStorage : MonoBehaviour
{
    /// <summary>
    ///  References to all possible spells
    /// </summary>
    public Spell fireFireSpell;
    public Spell fireWaterSpell;
    public Spell fireEarthSpell;
    public Spell fireAirSpell;
    public Spell waterFireSpell;
    public Spell waterWaterSpell;
    public Spell waterEarthSpell;
    public Spell waterAirSpell;
    public Spell earthFireSpell;
    public Spell earthWaterSpell;
    public Spell earthEarthSpell;
    public Spell earthAirSpell;
    public Spell airFireSpell;
    public Spell airWaterSpell;
    public Spell airEarthSpell;
    public Spell airAirSpell;
    /// <summary>
    ///  Stores all possible spells and their combination during runtime.
    /// </summary>
    private Dictionary<SpiritState, Dictionary<SpiritState, Spell>> spells;


    /// <summary>
    ///  Create map at the beginning of the game to improve Speed
    /// </summary>
    private void Start()
    {
        this.spells = new Dictionary<SpiritState, Dictionary<SpiritState, Spell>>();

        this.spells.Add(SpiritState.Fire, new Dictionary<SpiritState, Spell>());
        this.spells.Add(SpiritState.Water, new Dictionary<SpiritState, Spell>());
        this.spells.Add(SpiritState.Earth, new Dictionary<SpiritState, Spell>());
        this.spells.Add(SpiritState.Air, new Dictionary<SpiritState, Spell>());
        
        this.spells[SpiritState.Fire].Add(SpiritState.Fire, this.fireFireSpell);
        this.spells[SpiritState.Fire].Add(SpiritState.Water, this.fireWaterSpell);
        this.spells[SpiritState.Fire].Add(SpiritState.Earth, this.fireEarthSpell);
        this.spells[SpiritState.Fire].Add(SpiritState.Air, this.fireAirSpell);

        this.spells[SpiritState.Water].Add(SpiritState.Fire, this.waterFireSpell);
        this.spells[SpiritState.Water].Add(SpiritState.Water, this.waterWaterSpell);
        this.spells[SpiritState.Water].Add(SpiritState.Earth, this.waterEarthSpell);
        this.spells[SpiritState.Water].Add(SpiritState.Air, this.waterAirSpell);

        this.spells[SpiritState.Earth].Add(SpiritState.Fire, this.earthFireSpell);
        this.spells[SpiritState.Earth].Add(SpiritState.Water, this.earthWaterSpell);
        this.spells[SpiritState.Earth].Add(SpiritState.Earth, this.earthEarthSpell);
        this.spells[SpiritState.Earth].Add(SpiritState.Air, this.earthAirSpell);

        this.spells[SpiritState.Air].Add(SpiritState.Fire, this.airFireSpell);
        this.spells[SpiritState.Air].Add(SpiritState.Water, this.airWaterSpell);
        this.spells[SpiritState.Air].Add(SpiritState.Earth, this.airEarthSpell);
        this.spells[SpiritState.Air].Add(SpiritState.Air, this.airAirSpell);
    }

    /// <summary>
    ///  Returns a copy of the cast spell
    /// </summary>
    /// <param name="elementOne"></param>
    /// <param name="elementTwo"></param>
    public GameObject GetCopyOfSpell(SpiritState elementOne, SpiritState elementTwo)
    {
        var nextSpell = Instantiate(this.spells[elementOne][elementTwo].gameObject);
        nextSpell.SetActive(true);
        return nextSpell;
    }
}
