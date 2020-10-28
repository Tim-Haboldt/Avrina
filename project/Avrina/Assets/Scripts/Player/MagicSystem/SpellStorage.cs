using UnityEngine;
using System.Collections.Generic;

public class SpellStorage : MonoBehaviour
{
    /// <summary>
    ///  References to all possible spells
    /// </summary>
    [SerializeField] private Spell fireFireSpell;
    [SerializeField] private Spell fireWaterSpell;
    [SerializeField] private Spell fireEarthSpell;
    [SerializeField] private Spell fireAirSpell;
    [SerializeField] private Spell waterFireSpell;
    [SerializeField] private Spell waterWaterSpell;
    [SerializeField] private Spell waterEarthSpell;
    [SerializeField] private Spell waterAirSpell;
    [SerializeField] private Spell earthFireSpell;
    [SerializeField] private Spell earthWaterSpell;
    [SerializeField] private Spell earthEarthSpell;
    [SerializeField] private Spell earthAirSpell;
    [SerializeField] private Spell airFireSpell;
    [SerializeField] private Spell airWaterSpell;
    [SerializeField] private Spell airEarthSpell;
    [SerializeField] private Spell airAirSpell;
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
    public Spell GetCopyOfSpell(SpiritState elementOne, SpiritState elementTwo)
    {
        var nextSpell = Instantiate(this.spells[elementOne][elementTwo]);
        nextSpell.gameObject.SetActive(true);

        return nextSpell;
    }
}
