using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Singleton pattern
    public static AudioManager instance;

    //Audio sources
    public AudioSource musicSource;
    public AudioSource sfxSource;

    //Audio clips
    public AudioClip[] musicClips;
    public AudioClip[] sfxClips;

    private void Awake()
    {
        //Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //Play the first music clip
        PlayMusic(0);
    }

    public void PlayMusic(int trackNumber)
    {
        //Check if the track number is valid
        if (trackNumber >= 0 && trackNumber < musicClips.Length)
        {
            //Assign the music clip to the music source
            musicSource.clip = musicClips[trackNumber];

            //Play the music
            musicSource.Play();
        }
    }

    public void PlaySFX(int clipNumber, Transform transform)
    {
        //Check if the clip number is valid
        if (clipNumber >= 0 && clipNumber < sfxClips.Length)
        {
            //Assign the sfx clip to the sfx source
            sfxSource.clip = sfxClips[clipNumber];

            // randomize the pitch of the sfx
            sfxSource.pitch = Random.Range(0.9f, 1.1f);

            // randomize the volume of the sfx
            sfxSource.volume = Random.Range(0.9f, 1.1f);

            //Play the sfx at the position of the transform
            sfxSource.transform.position = transform.position;
            sfxSource.Play();
        }
    }


}
