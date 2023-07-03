using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Camera_script : MonoBehaviour
{

    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offset;
    private Vector3 pos;

    private float cameraSpeed = 0.1f;
    private string currentScene;
    // Start is called before the first frame update
    void Start()
    {
        pos = target.position + offset;
        currentScene = SceneManager.GetActiveScene().name;
        if( currentScene != "Maze")
        {
            if(pos.y < 5f)
            {
                pos.y = 5f;
            }
        } else {
            pos.y = -7;
        }
        transform.position = pos;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(target != null)
        {
            Vector3 finalPosition = target.position + offset;
            // Dependent on which Scene is currently active, the borders for camera movement are enforced.
            if(currentScene != "Maze")
            {
                if(finalPosition.y < 5.5f)
                {
                    finalPosition.y = 5.5f;
                }
            } else {
                
                if(finalPosition.x < -8)
                {
                    finalPosition.x = -8;
                } else if( finalPosition.x > 10)
                {
                    finalPosition.x = 10;
                }
                if(finalPosition.y < -7)
                {
                    finalPosition.y = -7;
                } else if( finalPosition.y > 5)
                {
                    finalPosition.y = 5;
                }
            }
            Vector3 lerpPosition = Vector3.Lerp (transform.position, finalPosition, cameraSpeed);
            transform.position = lerpPosition;
        }
    }
}
