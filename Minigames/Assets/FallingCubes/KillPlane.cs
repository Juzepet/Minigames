using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FallingCubes{
        
    public class KillPlane : MonoBehaviour
    {
        public UnityEvent onTrigger;
        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<Cube>() != null){
                onTrigger?.Invoke();
            }
        }
    }

}