using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RhythmGameStarter;

public class ScoreManager : MonoBehaviour
{
    private MapManager mm;
    [SerializeField] private Image progressBar;
    [SerializeField] private Text timer, placar;
    [SerializeField] private float minPerNote, maxPerNote;
    public float scoreGoal;

    [HideInInspector] public int currentTime = 0, seconds = 0;
    [HideInInspector] public float startTime = 0;
    [HideInInspector] public float score = 0; // ScoreGoal é o valor máximo da barra de progresso
    [HideInInspector] public bool destroyNotes = false, getScore = true, songFinished = false, segundoTempo = false;
    [HideInInspector] public int[] gols = new int[2];
    // private List<GameObject> notesToDestroy = new List<GameObject>();

    void Start() {
        mm = this.GetComponent<MapManager>();
    }

    void Update() {
        float time = (Time.fixedTime - startTime) / 2.27f;
        currentTime = Mathf.FloorToInt(time);
        seconds = Mathf.FloorToInt((time - currentTime) * 6);
        if(seconds >= 60){
            seconds = 0;
        }
        timer.text = $"{currentTime:00}:{seconds:0}0";
        if(songFinished && !segundoTempo){
            segundoTempo = true;
            currentTime = 0;
            songFinished = false;
            mm.SegundoTempo();
            startTime = Time.fixedTime + 1;
        }
    }

    public void SongFinished(){
        if(songFinished){
            mm.animPanel.SetActive(true);
            mm.animPanel.transform.GetChild(0).GetComponent<Text>().text = "Partida acabou";
            mm.StopCoroutine("AutoAction");
        }
        songFinished = true;
    }

    public void AlterarPlacar(){
        placar.text = $"{gols[0]} x {gols[1]}";
    }

    // Preencher barra de progresso
    public void AddScore(string acc){
        if(!getScore)
            return;
        score += Mathf.Clamp(10 * float.Parse(acc), minPerNote, maxPerNote);
        score = Mathf.Clamp(score, 0, scoreGoal);
        progressBar.fillAmount = score / scoreGoal;
    }

    // Reduzir barra de progresso
    public void ReduceScore(string scoreMissed){
        if(!getScore)
            return;
        score -= 5;
        score = Mathf.Clamp(score, 0, scoreGoal);
        progressBar.fillAmount = score / scoreGoal;
    }

    public void UseScore(float valueUsed){
        score -= valueUsed;
        score = Mathf.Clamp(score, 0, scoreGoal);
        progressBar.fillAmount = score / scoreGoal;
    }

    // Resetar os pontos da barra de progresso
    public void ResetScore(){
        score = 0;
        progressBar.fillAmount = score / scoreGoal;
    }

    // Impede as notas de aparecerem muito rápido ao trocar de cena
    public void DestroyNotes(Note note){
        if(destroyNotes){
            // notesToDestroy.Add(note.gameObject);
            // note.gameObject.tag = "Untagged";
            note.gameObject.SetActive(false);
        }
    }

    // public IEnumerator ClearNotes(){
    //     yield return new WaitForSeconds(1);
    //     for(int i = 0; i < notesToDestroy.Count; i++){
    //         Destroy(notesToDestroy[i]);
    //     }
    //     notesToDestroy = new List<GameObject>();
    // }
}
