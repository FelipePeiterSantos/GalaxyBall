using UnityEngine;
using System.Collections;

public class SFXManager : MonoBehaviour {

    public AudioClip[] clips;

    static AudioClip[] _clips;

    void Awake() {
        _clips = clips;
    }

    public static void PlaySFX(int aux, bool isRightSide, float volume = 1f){
        AudioSource temp = new GameObject().AddComponent<AudioSource>();
        temp.spatialBlend = 0;
        temp.panStereo = isRightSide ? 1: -1;
        temp.volume = volume;
        temp.clip = _clips[aux];
        temp.Play();
        Destroy(temp.gameObject, _clips[aux].length);
    }
}
