using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FallingCubes{

    public class MainFallingCubes : MonoBehaviour
    {
        [SerializeField] Camera _camera;
        [SerializeField] GameObject _rightWall;
        [SerializeField] GameObject _leftWall;
        [SerializeField] Cube _baseCube;
        [SerializeField] Transform _spawnPoint;
        [SerializeField] KillPlane _killPlane;
        [SerializeField] Text _textScore;
        [SerializeField] Text _textBestScore;
        
        int _score;
        float _gameStart = 0;
        float _lastGameEnd = 0;
        bool _gameEnded = false;
        Color _startBackgroundColor;
    
        // Start is called before the first frame update
        void Start()
        {
            Application.targetFrameRate = Screen.currentResolution.refreshRate;

            Vector3 leftEdge = _camera.ViewportToWorldPoint(new Vector3(0, 0, 10)) + Vector3.left / 2;
            _leftWall.transform.position = leftEdge;
            Vector3 rightEdge = _camera.ViewportToWorldPoint(new Vector3(1, 0, 10)) + Vector3.right / 2;
            _rightWall.transform.position = rightEdge;

            Vector3 topEdge = _camera.ViewportToWorldPoint(new Vector3(0.5f, 1, 10));
            _spawnPoint.transform.position = topEdge + 3 * Vector3.up;
            Vector3 bottomEdge = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0, 10));
            _killPlane.transform.position = bottomEdge + 5 * Vector3.down;

            _baseCube.gameObject.SetActive(false);
            _startBackgroundColor = _camera.backgroundColor;

            _textBestScore.text = "" + BestScores.FallingCubesBest;
            ResetGame();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateScore();
            if(Input.GetMouseButtonDown(0)){
                if(_gameEnded && Time.realtimeSinceStartup - _lastGameEnd > 1f){
                    ResetGame();
                }
                else{
                    Vector2 mousePosNrm = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
                    Vector3 pos = _camera.ViewportToWorldPoint(new Vector3(mousePosNrm.x, mousePosNrm.y, 10));
                    foreach(Cube cube in FindObjectsOfType<Cube>()){
                        cube.PlayerClick(pos);
                    }
                }
            }
        }

        IEnumerator SpawnCubes(){
            while(Application.isPlaying){
                Instantiate(_baseCube, _spawnPoint.position, Quaternion.identity).gameObject.SetActive(true);
                yield return new WaitForSeconds(15);
            }
        }

        void UpdateScore(){
            if(!_gameEnded){
                _score = Mathf.FloorToInt(Time.realtimeSinceStartup - _gameStart);
            }
            _textScore.text = "" + _score;
        }

        void EndGame(){
            StopAllCoroutines();
            foreach(Cube cube in FindObjectsOfType<Cube>()){
                cube.Destroy();
            }
            _camera.backgroundColor = Color.red;
            _gameEnded = true;
            _lastGameEnd = Time.realtimeSinceStartup;

            if(_score >= BestScores.FallingCubesBest){
                BestScores.FallingCubesBest = _score;
                _textBestScore.text = "" + _score;
            }
        }

        void ResetGame(){
            _camera.backgroundColor = _startBackgroundColor;
            _score = 0;
            _textScore.text = "0";
            StartCoroutine(SpawnCubes());
            _gameEnded = false;
            _gameStart = Time.realtimeSinceStartup;
        }

        public void OnTriggerKillPlane(){
            EndGame();
        }
    }

}
