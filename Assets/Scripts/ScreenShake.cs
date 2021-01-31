using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake instance;

    bool shaking = false;
    float shakeDuration;
    float shakeMagnitude;
    float dampingSpeed;
    Vector3 initialPosition;

    void Awake() {
        if (instance == null) 
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
    }

    public void DoShake(float _shakeDuration, float _shakeMagnitude, float _dampingSpeed) {
        shaking = true;
        shakeDuration = _shakeDuration;
        shakeMagnitude = _shakeMagnitude;
        dampingSpeed = _dampingSpeed;
        initialPosition = transform.position;
    }

    void Update() {
        if (shaking) {    
            if (shakeDuration > 0) {
                transform.position = initialPosition + Random.insideUnitSphere * shakeMagnitude;

                shakeDuration -= Time.deltaTime * dampingSpeed;
            } else {
                shaking = false;
            }
        }
    }
}
