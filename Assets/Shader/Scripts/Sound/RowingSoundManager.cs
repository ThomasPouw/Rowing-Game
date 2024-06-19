using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class RowingSoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> rowingSoundClips;
    [SerializeField] private AudioClip currentClip;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    [SerializeField] private AudioSource audioSourceLeftPedal;
    [SerializeField] private AudioSource audioSourceRightPedal;
    [SerializeField] private bool isInUse;
    [SerializeField] private bool hasMusic;

    // Start is called before the first frame update
    void Start()
    {
        isInUse = false;
        hasMusic = rowingSoundClips.Count != 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlaySoundEffect(bool isLeftPedal)
    {
        if(!isInUse && hasMusic)
        {
            isInUse = true;
            StartCoroutine(IPlaySoundEffect(isLeftPedal));
        }
    }
    public IEnumerator IPlaySoundEffect(bool isLeftPedal)
    {
        int number = Random.Range(1, rowingSoundClips.Count);
        currentClip = rowingSoundClips[number];
        (isLeftPedal ? audioSourceLeftPedal : audioSourceRightPedal).PlayOneShot(currentClip);
        yield return new WaitForSeconds(currentClip.length);
        rowingSoundClips[number] = rowingSoundClips[0];
        rowingSoundClips[0] = currentClip;
        isInUse = false;
    }
}
