using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBanan : MonoBehaviour
{
	private float v;
	
    void Start()
    {
        v = 4f - 2f * Random.Range(0f, 1f) * Random.Range(0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, v * Time.deltaTime, 0);
		
		if(transform.position.y <= -5.0) Destroy(gameObject);
    }
}
