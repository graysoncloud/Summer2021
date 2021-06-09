using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using TMPro;
using UnityEngine.Playables;

// 

// Specifies to Timeline what kind of GameObject this track can manipulate
[TrackBindingType(typeof(TextMeshProUGUI))]
// Specifies what clips it uses
[TrackClipType(typeof(DialogueClip))]
public class DialogueTrack : TrackAsset
{

    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<DialogueTrackMixer>.Create(graph, inputCount);
    }

}
