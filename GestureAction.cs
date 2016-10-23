using UnityEngine;

/// <summary>
/// GestureAction performs custom actions based on 
/// which gesture is being performed.
/// </summary>
public class GestureAction : MonoBehaviour
{
    [Tooltip("Rotation max speed controls amount of rotation.")]
    public float RotationSensitivity = 10.0f;

    private Vector3 manipulationPreviousPosition;

    private float rotationFactor;

    GameObject myParticles;
    ParticleSystem part1;

    AudioSource audioSource = null;
    AudioClip tapSound = null;


    void Start()
    {
        //particles
        myParticles = GameObject.FindGameObjectWithTag("Particles");
        part1 = myParticles.GetComponent<ParticleSystem>();
        part1.Stop();

        // Add an AudioSource component and set up some defaults
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialize = false;
        audioSource.spatialBlend = 1.0f;
        audioSource.dopplerLevel = 0.0f;
        audioSource.rolloffMode = AudioRolloffMode.Custom;

        tapSound = Resources.Load<AudioClip>("select");
    }

    void Update()
    {
        //PerformRotation();
    }

    private void PerformRotation()
    {
        if (GestureManager.Instance.IsNavigating &&
            (!ExpandModel.Instance.IsModelExpanded ||
            (ExpandModel.Instance.IsModelExpanded && HandsManager.Instance.FocusedGameObject == gameObject)))
        {
            // This will help control the amount of rotation.
            rotationFactor = GestureManager.Instance.NavigationPosition.x * RotationSensitivity;

            transform.Rotate(new Vector3(0, -1 * rotationFactor, 0));
        }
    }

    void PerformManipulationStart(Vector3 position)
    {
        manipulationPreviousPosition = position;
        //myParticles.transform.position = position;

        audioSource.clip = tapSound;
        audioSource.Play();

        part1.Play();
    }

    void PerformManipulationUpdate(Vector3 position)
    {
        if (GestureManager.Instance.IsManipulating)
        {
            Vector3 moveVector = Vector3.zero;
            moveVector = position - manipulationPreviousPosition;
            manipulationPreviousPosition = position;

            part1.Stop();

            transform.position += moveVector;
            //myParticles.transform.position += moveVector;
        }
    }

    void PerformManipulationComplete(Vector3 position)
    {
        part1.Stop();
    }
    void PerformManipulationCanceled(Vector3 position)
    {
        part1.Stop();
    }
}