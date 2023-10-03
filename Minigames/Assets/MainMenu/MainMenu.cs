using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Text _textFastTyping;
    [SerializeField] Transform _cube;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AnimateFallingCubes());
        StartCoroutine(AnimateFastTyping());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickFallingCubes(){
        SceneManager.LoadScene("FallingCubes", LoadSceneMode.Single);
    }

    public void OnClickFastTyping(){
        SceneManager.LoadScene("FastTyping", LoadSceneMode.Single);
    }

    IEnumerator AnimateFallingCubes(){
        while(true){
            _cube.Rotate(new Vector3(Time.deltaTime * 10, Time.deltaTime * 50, Time.deltaTime * -15));
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator AnimateFastTyping(){
        while(true){
            _textFastTyping.text = "l";
            yield return new WaitForSeconds(Random.Range(0.1f, 0.6f));
            _textFastTyping.text = "Al";
            yield return new WaitForSeconds(Random.Range(0.1f, 0.6f));
            _textFastTyping.text = "ABl";
            yield return new WaitForSeconds(Random.Range(0.1f, 0.6f));
            _textFastTyping.text = "l";
            yield return new WaitForSeconds(Random.Range(0.1f, 0.6f));
            _textFastTyping.text = "Jl";
            yield return new WaitForSeconds(Random.Range(0.1f, 0.6f));
            _textFastTyping.text = "JUl";
            yield return new WaitForSeconds(Random.Range(0.1f, 0.6f));
            _textFastTyping.text = "l";
            yield return new WaitForSeconds(Random.Range(0.1f, 0.6f));
            _textFastTyping.text = "Fl";
            yield return new WaitForSeconds(Random.Range(0.1f, 0.6f));
            _textFastTyping.text = "FEl";
            yield return new WaitForSeconds(Random.Range(0.1f, 0.6f));
        }
    }
}
