using System.Collections;
using System.Collections.Generic;
using RDG;
using UnityEngine;

namespace FallingCubes{

    public class Cube : MonoBehaviour
    {
        Rigidbody _rigidBody;

        // Start is called before the first frame update
        void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _rigidBody.AddForce(new Vector3(Random.Range(-10, 10), 0, 0), ForceMode.VelocityChange);
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }

        public bool PlayerClick(Vector3 pos){
            Vector3 dir = transform.position - pos;
            if(dir.magnitude < 2){
                dir.z = 0;
                _rigidBody.velocity = Vector3.zero;
                _rigidBody.AddForce(dir.normalized * 15, ForceMode.Impulse);
                Vibration.VibratePredefined(Vibration.PredefinedEffect.EFFECT_TICK, true);
                return true;
            }
            return false;
        }

        public void Destroy(){
            Destroy(gameObject);
        }

    }

}
