using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace PixelWizards.Shared.Utiltiies
{

    public class DynamicTimelineBinding : MonoBehaviour
    {
        public List<GameObject> trackList = new List<GameObject>();
        public PlayableDirector timeline;
        public TimelineAsset timelineAsset;
        public bool autoBindTracks = true;

        // Use this for initialization
        private void Start()
        {
           
        }

        public void BindTimelineTracks()
        {
            Debug.Log("Binding Timeline Tracks!");
            timelineAsset = (TimelineAsset)timeline.playableAsset;
            // iterate through tracks and map the objects appropriately
            for (var i = 0; i < trackList.Count; i++)
            {
                if (trackList[i] != null)
                {
                   
                }
            }
        }
    }
}
