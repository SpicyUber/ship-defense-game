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
        endText.text = "VICTORY!\n\n\nThis game was made in less than two weeks for the purposes of Fonis Treasure Hunt 2025 \n\nCREDITS:\n[LEAD PROGRAMMER]\nAleksandar Ilanković\n\n[GAME DESIGNER]\nAleksandar Ilanković\n\n" +
            "[PLAYER LOGIC]\nLuka Milikić\n" +
            "\n[ENEMY LOGIC]\nTeodor Vicelarević\n\n[SHIP MOVEMENT]\nTeodor Vicelarević\n" + "\n[UI LOGIC]\nMilan Davidović\n\n[GAME MANAGER LOGIC]\nMilan Davidović\n\n[END SCENE]\nMilan Davidović\n\n[CANNON LOGIC]\nAleksandar Ilanković\n\n[SHARK FIGHT]\nAleksandar Ilanković \n\n[UI ARTIST]\nLuka Milikić\n\n[MUSIC]\nAleksandar Ilanković\n\n\nTHANKS FOR PLAYING!";





        StartCoroutine(PlayAudioAndShowText());
    }

    private void Update()
    {
      //  Clue.color = new Color(Clue.color.r, Clue.color.g, Clue.color.b, endText.color.a);
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