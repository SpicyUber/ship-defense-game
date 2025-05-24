using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextAfterSound : MonoBehaviour
{
    public AudioSource audioSource;
    public TextMeshProUGUI endText;
    public Image Clue; 
   
    void Start()
    {
        //tekst je na pocetku scene potpuno transparentan. Koristimo color property.
        //NE MENJAMO MATERIJAL U EDITORU, posto to utice na svaki tekst u igrici koji koristi taj font, a to ne zelimo.
        endText.color = new(endText.color.r,endText.color.g,endText.color.b,0);
        //odmah namestamo tekst
        endText.text = "Bitka za okean je završena\nali rat na kopnu tek počinje.\nVaša posada mora otići do Tašmajdana, \ngde je potrebno izgovoriti drevnu vradžbinu \nsakrivenu u sledećim rečima...";




        StartCoroutine(PlayAudioAndShowText());
    }

    private void Update()
    {
        Clue.color = new Color(Clue.color.r, Clue.color.g, Clue.color.b, endText.color.a);
    }
    IEnumerator PlayAudioAndShowText()
    {
        // Unity audiosource komponenta nije bas precizna ako samo pozoves Play, malo je bolje da ga zakazes posle nekog delay-a
        audioSource.PlayDelayed(0.25f);
        //kazemo korutini da ceka isto vremena koliko ce trebati audiosource-u da pokrene play.
        yield return new WaitForSeconds(0.25f);
        while (audioSource.isPlaying)
        {
            //uzimamo duzinu zvuka i delimo sa vremenom od pocetka zvuka.
            //Ovo nam daje vrednost izmedju 0 i 1 savresnu za namestanje transparentnosti boje.
            endText.color = new(endText.color.r, endText.color.g, endText.color.b, audioSource.time/ audioSource.clip.length);
            yield return null;
        }
        
         }
}