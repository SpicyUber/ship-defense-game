using System.Collections;
using TMPro;
using UnityEngine;

public class TextAfterSound : MonoBehaviour
{
    public AudioSource audioSource;
    public TextMeshProUGUI endText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        endText.gameObject.SetActive(false);
        StartCoroutine(PlayAudioAndShowText());
    }

    // Update is called once per frame
    IEnumerator PlayAudioAndShowText()
    {
        // Play the audio
        audioSource.Play();
        // Wait for the audio to finish playing
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        // Show the text after the audio has finished playing
        endText.gameObject.SetActive(true);
        endText.text = "Bitka za okean je završena\nAli rat na kopnu tek počinje.\nPripremi se da vidiš svet očima pravog gusara.\nZapiši kod: *********\nU sledećoj igri... nišan ide direktno u horizont!";
    }
}