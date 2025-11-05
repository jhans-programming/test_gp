using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip buttonSFX;
    public AudioClip medkitSFX;
    public AudioClip startMusic;
    public AudioClip tutorialMusic;
    public AudioClip level1Music;
    public AudioClip level2Music;

    private void Awake()
    {
        // Singleton pattern — only one AudioManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        AudioClip newClip = null;

        switch (sceneName)
        {
            case "StartScene":
                newClip = startMusic;
                break;
            case "Tutorial Level":
                newClip = tutorialMusic;
                break;
            case "Level 1":
                newClip = level1Music;
                break;
            case "Level 2":
                newClip = level2Music;
                break;
        }

        if (newClip != null && musicSource.clip != newClip)
        {
            StartCoroutine(FadeMusic(newClip, 1f));
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        Debug.Log("test sound");
        if (clip != null)
            sfxSource.PlayOneShot(clip);
        //AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }

    public void playButton()
    {
        sfxSource.PlayOneShot(buttonSFX);
    }
    
     public void PlayMedKit()
    {
        sfxSource.PlayOneShot(medkitSFX);
    }


    IEnumerator FadeMusic(AudioClip newClip, float fadeTime = 1f)
    {
        if (musicSource.clip == newClip) yield break;

        // Fade out
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            musicSource.volume = 1 - t / fadeTime;
            yield return null;
        }
        musicSource.loop = true;
        musicSource.clip = newClip;
        musicSource.Play();

        // Fade in
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            musicSource.volume = t / fadeTime;
            yield return null;
        }

        musicSource.volume = 1f;
    }



}
