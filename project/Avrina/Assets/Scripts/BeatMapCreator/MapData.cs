using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapData
{
    /* MAP */
    // stores the information how many posible beats can be marked as important
    // and which of them are important
    // in short: the actual map data is stored in here
    public List<bool> trackTicks;
    // bpm of the song the map is created for
    public float trackBPM;
    // how long does it take until the first beat of the song is played
    public float offset;
    // how many ticks are per beat
    // defines how many part per beat can be marked as important.
    // (how big is the data of the map)
    public int markableTicksPerBeat;

    /* UI */
    // it defines how much of the map is currently visible
    public int visibleBeats;
    // defines how big the beat panel in reference to the display size is (ratio)
    public Vector2 beatSize;
    // defines how big the tick panel in reference to the display size is (ratio)
    public Vector2 tickSize;
}
