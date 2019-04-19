using System;
using UnityEngine;

[Serializable]
public class MapData
{
    /* MAP */
    // Stores the information how many posible rythm units can be marked as important
    // and which of them are important
    // In short: The actual map data is stored in here
    public bool[] trackRythmUnits;
    // Bpm of the track the map is created for
    public float trackBPM;
    // How long does it take until the first beat of the track is played
    public float offset;
    // How many ticks are per beat
    // Defines how many part per beat can be marked as important.
    // If the variable is bigger the size of the beatmap will increase rapidly
    public int markableTicksPerBeat;

    /* UI */
    // It defines how much of the map is currently visible
    public int visibleBeats;
    // Defines how big the beat panel in reference to the display size is (ratio)
    public Vector2 beatSize;
    // Defines how big the tick panel in reference to the display size is (ratio)
    public Vector2 tickSize;
}
