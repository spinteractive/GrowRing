using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.Input;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;

public class StartApp : MonoBehaviour
{
    [Tooltip("The collection of holograms to show when the Fitbox is dismissed.")]
    public GameObject HologramCollection;

    [Tooltip("Reposition the collection of holograms relative to where the Fitbox was dismissed.")]
    public bool MoveCollectionOnDismiss = false;

    [Tooltip("The material used to render the Fitbox border.")]
    public Material FitboxMaterial;

    // The offset from the Camera to the HologramCollection when
    // the app starts up. This is used to place the Collection
    // in the correct relative position after the Fitbox is
    // dismissed.
    private Vector3 collectionStartingOffsetFromCamera;

    private float Distance = 2.0f;

    private Interpolator interpolator;
    private GestureRecognizer recognizer;
    private bool isInitialized = false;

    public GameObject logo;
    public GameObject TopDiamondRing;
    public GameObject TopDiamondRingHalo;
    public GameObject TopGoldBox;
    public GameObject TopGoldBoxHalo;
    public GameObject TopPlatinumBox;
    public GameObject TopPlatinumBoxHalo;
    public GameObject TopWhiteDiamond;
    public GameObject TopWhiteDiamondHalo;
    public GameObject TopPinkDiamond;
    public GameObject TopPinkDiamondHalo;
    public GameObject Incubator;
    public GameObject IncubatorHalo;
    public GameObject TopOrchid;
    public GameObject TopOrchidHalo;
    public GameObject TopTree;
    public GameObject TopTreeHalo;
    public GameObject TopWoman;
    public GameObject TopWomanHalo;
    public GameObject TopCouple;
    public GameObject TopCoupleHalo;
    public GameObject TopBloodDiamond;
    public GameObject TopBloodDiamondHalo;

    // KeywordRecognizer object.
    KeywordRecognizer keywordRecognizer;

    // Defines which function to call when a keyword is recognized.
    delegate void KeywordAction(PhraseRecognizedEventArgs args);
    Dictionary<string, KeywordAction> keywordCollection;

    AudioSource audioSource = null;
    AudioClip pinkDiamond1 = null;
    AudioClip incubator1 = null;
    AudioClip treeSound1 = null;
    AudioClip girlSound1 = null;
    AudioClip bigRing1 = null;
    AudioClip bloodSound1 = null;
    AudioClip tapSound = null;

    bool spinInc = false;
    bool spinBingRing = false;
    bool spinGirl = false;
    bool spinTree = false;
    bool spinPink = false;
    bool spinBlood = false;
    bool spinCouple = false;

    bool tellInc = false;
    bool tellCouple = false;
    bool tellBingRing = false;
    bool tellGirl = false;
    bool tellTree = false;
    bool tellPink = false;
    bool tellBlood = false;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Managers").GetComponent<AstronautManager>().enabled = false;

        // Add an AudioSource component and set up some defaults
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialize = false;
        audioSource.spatialBlend = 1.0f;
        audioSource.dopplerLevel = 0.0f;
        audioSource.rolloffMode = AudioRolloffMode.Custom;

        // Load the Sphere sounds from the Resources folder
        pinkDiamond1 = Resources.Load<AudioClip>("voice5a");
        incubator1 = Resources.Load<AudioClip>("voice3a");
        treeSound1 = Resources.Load<AudioClip>("voice7ab");
        girlSound1 = Resources.Load<AudioClip>("IVONAa");
        bigRing1 = Resources.Load<AudioClip>("IVONA2a");
        bloodSound1 = Resources.Load<AudioClip>("sociallyresponsible");
        tapSound = Resources.Load<AudioClip>("select");

        logo.SetActive(true);
        TopDiamondRing.SetActive(false);
        TopPlatinumBox.SetActive(false);
        TopGoldBox.SetActive(false);
        TopWhiteDiamond.SetActive(false);
        TopPinkDiamond.SetActive(false);
        Incubator.SetActive(false);
        TopOrchid.SetActive(false);
        TopTree.SetActive(false);
        TopWoman.SetActive(false);
        TopCouple.SetActive(false);
        TopBloodDiamond.SetActive(false);

    }

    ////////////////////START//////////////////////
    private void Start()
    {

        if (interpolator == null)
        {
            interpolator = gameObject.AddComponent<Interpolator>();
        }

        // Screen-lock the Fitbox to match the OOBE Fitbox experience
        interpolator.PositionPerSecond = 0.0f;

        keywordCollection = new Dictionary<string, KeywordAction>();

        // Add keyword to start manipulation.
        keywordCollection.Add("Tell Me", TellMeCommand);

        // Initialize KeywordRecognizer with the previously added keywords.
        keywordRecognizer = new KeywordRecognizer(keywordCollection.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();

        // Set up our GestureRecognizer to listen for the SelectEvent
        recognizer = new GestureRecognizer();
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            StartIncubator();
        };
        recognizer.StartCapturingGestures();
    }

    private void TellMeCommand(PhraseRecognizedEventArgs args)
    {
        if (tellInc)
        {
            audioSource.clip = incubator1;
            spinInc = true;
        }
        else if (tellCouple)
        {
            audioSource.clip = tapSound;
            spinCouple = true;
        }
        else if (tellBingRing)
        {
            audioSource.clip = bigRing1;
            spinBingRing = true;
        }
        else if (tellGirl)
        {
            audioSource.clip = girlSound1;
            spinGirl = true;
        }
        else if (tellTree)
        {
            audioSource.clip = treeSound1;
            spinTree = true;
        }
        else if (tellPink)
        {
            audioSource.clip = pinkDiamond1;
            spinPink = true;
        }
        else if (tellBlood)
        {
            audioSource.clip = bloodSound1;
            spinBlood = true;
        }
        audioSource.Play();
    }

    ////////////////////UPDATE//////////////////////
    void Update()
    {
        if (spinInc)
        {
            Incubator.transform.Rotate(Vector3.up, Time.deltaTime * 50);
        }
        else if (spinCouple)
        {
            TopCouple.transform.Rotate(Vector3.up, Time.deltaTime * 50);
        }
        else if (spinBingRing)
        {
            TopDiamondRing.transform.Rotate(Vector3.up, Time.deltaTime * 50);
        }
        else if (spinGirl)
        {
            TopWoman.transform.Rotate(Vector3.up, Time.deltaTime * 50);
        }
        else if (spinTree)
        {
            TopTree.transform.Rotate(Vector3.up, Time.deltaTime * 50);
        }
        else if (spinPink)
        {
            TopPinkDiamond.transform.Rotate(Vector3.up, Time.deltaTime * 50);
            TopOrchid.transform.Rotate(Vector3.up, Time.deltaTime * 50);
            TopWhiteDiamond.transform.Rotate(Vector3.up, Time.deltaTime * 50);
        }
        else if (spinBlood)
        {
            TopBloodDiamond.transform.Rotate(Vector3.up, Time.deltaTime * 50);
        }
        else
        {
            return;
        }
    }

    ////////////////////INCUBATOR//////////////////////
    private void StartIncubator()
    {
        //Dismiss logo
        logo.SetActive(false);

        //Turn off mesh
        SpatialMapping.Instance.DrawVisualMeshes = false;

        //Turn on incubaotr
        Incubator.SetActive(true);
        tellInc = true;

        // dispose 
        recognizer.CancelGestures();
        recognizer.StopCapturingGestures();
        recognizer.Dispose();

        // Set up our GestureRecognizer to listen for the SelectEvent
        recognizer = new GestureRecognizer();
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            //Go to next
            StartCouple();
        };
        recognizer.StartCapturingGestures();
    }

    ////////////////////COUPLE//////////////////////
    private void StartCouple()
    {
        TopCouple.SetActive(true);
        spinInc = false;
        tellInc = false;
        tellCouple = true;

        // dispose 
        recognizer.CancelGestures();
        recognizer.StopCapturingGestures();
        recognizer.Dispose();

        // Set up our GestureRecognizer to listen for the SelectEvent
        recognizer = new GestureRecognizer();
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            StartLargeRing();
        };
        recognizer.StartCapturingGestures();
    }

    ////////////////////LARGE RING//////////////////////
    private void StartLargeRing()
    {
        TopDiamondRing.SetActive(true);
        tellCouple = false;
        spinCouple = false;
        tellBingRing = true;

        // dispose 
        recognizer.CancelGestures();
        recognizer.StopCapturingGestures();
        recognizer.Dispose();

        // Set up our GestureRecognizer to listen for the SelectEvent
        recognizer = new GestureRecognizer();
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            //Go to next
            StartGirl();
        };
        recognizer.StartCapturingGestures();
    }

    ////////////////////GIRL//////////////////////
    private void StartGirl()
    {
        TopWoman.SetActive(true);

        tellGirl = true;
        spinBingRing = false;
        tellBingRing = false;

        // dispose 
        recognizer.CancelGestures();
        recognizer.StopCapturingGestures();
        recognizer.Dispose();

        // Set up our GestureRecognizer to listen for the SelectEvent
        recognizer = new GestureRecognizer();
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            //Go to next
            StartTree();
        };
        recognizer.StartCapturingGestures();
    }

    ////////////////////TREE//////////////////////
    private void StartTree()
    {
        TopTree.SetActive(true);

        tellTree = true;
        spinGirl = false;
        tellGirl = false;

        // dispose 
        recognizer.CancelGestures();
        recognizer.StopCapturingGestures();
        recognizer.Dispose();

        // Set up our GestureRecognizer to listen for the SelectEvent
        recognizer = new GestureRecognizer();
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            //Go to next
            StartPink();
        };
        recognizer.StartCapturingGestures();
    }

    ////////////////////PINK DIAMOND//////////////////////
    private void StartPink()
    {
        TopPinkDiamond.SetActive(true);
        TopWhiteDiamond.SetActive(true);
        TopOrchid.SetActive(true);

        tellPink = true;
        spinTree = false;
        tellTree = false;

        // dispose 
        recognizer.CancelGestures();
        recognizer.StopCapturingGestures();
        recognizer.Dispose();

        // Set up our GestureRecognizer to listen for the SelectEvent
        recognizer = new GestureRecognizer();
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            //Go to next
            StartBlood();
        };
        recognizer.StartCapturingGestures();
    }

    ////////////////////BLOODY//////////////////////
    private void StartBlood()
    {
        TopBloodDiamond.SetActive(true);

        tellBlood = true;
        spinPink = false;
        tellPink = false;

        // dispose 
        recognizer.CancelGestures();
        recognizer.StopCapturingGestures();
        recognizer.Dispose();

        // Set up our GestureRecognizer to listen for the SelectEvent
        recognizer = new GestureRecognizer();
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            //Go to next
            StartBoxes();
        };
        recognizer.StartCapturingGestures();
    }

    ////////////////////RING BOXES//////////////////////
    private void StartBoxes()
    {
        //activate global speech and gesture
        GameObject.FindGameObjectWithTag("Managers").GetComponent<AstronautManager>().enabled = true;

        //Start
        TopGoldBox.SetActive(true);
        TopPlatinumBox.SetActive(true);

        StartCoroutine(TurnOffHalos());

        spinInc = false;
        spinBingRing = false;
        spinGirl = false;
        spinTree = false;
        spinPink = false;
        spinBlood = false;

        tellInc = false;
        tellBingRing = false;
        tellGirl = false;
        tellTree = false;
        tellPink = false;
        tellBlood = false;

        //Close and dispose
        recognizer.CancelGestures();
        recognizer.StopCapturingGestures();
        recognizer.Dispose();
        keywordRecognizer.Stop();
        keywordRecognizer.Dispose(); 
    }

    private IEnumerator TurnOffHalos()
    {
        yield return new WaitForSeconds(2);
        TopPinkDiamondHalo.SetActive(false);
        TopWhiteDiamondHalo.SetActive(false);
        TopBloodDiamondHalo.SetActive(false);
        TopCoupleHalo.SetActive(false);
        TopDiamondRingHalo.SetActive(false);
        IncubatorHalo.SetActive(false);
        TopGoldBoxHalo.SetActive(false);
        TopOrchidHalo.SetActive(false);
        TopTreeHalo.SetActive(false);
        TopWomanHalo.SetActive(false);
        TopCoupleHalo.SetActive(false);
        TopPlatinumBoxHalo.SetActive(false);
    }

    private void InitializeComponents()
    {
        // Return early if we've already been initialized...
        if (isInitialized || Camera.main == null)
        {
            return;
        }

        // Calculate Hologram Dimensions and Positions based on RedLine drawing
        const int drawingPixelWidth = 1440;
        const int drawingPixelHeight = 818;
        const int drawingBoxLeftEdgeScreen = 240;
        const int drawingBoxBottomEdgeScreen = 139;
        const int drawingBoxWidthScreen = 960;
        const int drawingBoxHeightScreen = 540;
        const int drawingQuadWidthScreen = 8;
        const int drawingQuadHeightScreen = 8;
        // Calculate a ratio between the actual screen dimensions and those of the RedLine drawing
        var xRatio = (float)Camera.main.pixelWidth / (float)drawingPixelWidth;
        var yRatio = (float)Camera.main.pixelHeight / (float)drawingPixelHeight;
        // Factor the real dimensions in screen space
        var realBoxLeftEdgeScreen = drawingBoxLeftEdgeScreen * xRatio;
        var realBoxBottomEdgeScreen = drawingBoxBottomEdgeScreen * yRatio;
        var realBoxWidthScreen = drawingBoxWidthScreen * xRatio;
        var realBoxHeightScreen = drawingBoxHeightScreen * yRatio;
        var realQuadWidthScreen = drawingQuadWidthScreen * xRatio;
        var realQuadHeightScreen = drawingQuadHeightScreen * yRatio;
        // Calculate the real width of the top/bottom (horizontal) quads
        var hQuadLeftEdgeScreen = new Vector3(realBoxLeftEdgeScreen, realBoxBottomEdgeScreen + (realQuadHeightScreen / 2.0f), Distance);
        var hQuadLeftEdge = Camera.main.ScreenToWorldPoint(hQuadLeftEdgeScreen);
        var hQuadRightEdgeScreen = new Vector3(realBoxLeftEdgeScreen + realBoxWidthScreen, hQuadLeftEdgeScreen.y, Distance);
        var hQuadRightEdge = Camera.main.ScreenToWorldPoint(hQuadRightEdgeScreen);
        var hQuadWid = Vector3.Distance(hQuadRightEdge, hQuadLeftEdge);
        // Calculate the real height of the top/bottom (horizontal) quads
        var hQuadBottomEdgeScreen = new Vector3(0, hQuadLeftEdge.y, Distance);
        var hQuadBottomEdge = Camera.main.ScreenToWorldPoint(hQuadBottomEdgeScreen);
        var hQuadTopEdgeScreen = new Vector3(0, hQuadBottomEdgeScreen.y + realQuadHeightScreen, Distance);
        var hQuadTopEdge = Camera.main.ScreenToWorldPoint(hQuadTopEdgeScreen);
        var hQuadHgt = Vector3.Distance(hQuadTopEdge, hQuadBottomEdge);
        // Calculate the real height of the left/right (vertical) quads
        var vQuadBottomEdgeScreen = new Vector3(realBoxLeftEdgeScreen + (realQuadWidthScreen / 2.0f), realBoxBottomEdgeScreen, Distance);
        var vQuadBottomEdge = Camera.main.ScreenToWorldPoint(vQuadBottomEdgeScreen);
        var vQuadTopEdgeScreen = new Vector3(vQuadBottomEdgeScreen.x, realBoxBottomEdgeScreen + realBoxHeightScreen, Distance);
        var vQuadTopEdge = Camera.main.ScreenToWorldPoint(vQuadTopEdgeScreen);
        var vQuadHgt = Vector3.Distance(vQuadTopEdge, vQuadBottomEdge);
        // Calculate the real width of the left/right quads...
        // ...just use the height of the horizontal quad for the width of the vertical quad
        var vQuadWid = hQuadHgt;

        // Create the Quads for our FitBox
        /*  leftQuad*/
        //CreateFitboxQuad(transform, (-hQuadWid / 2.0f) + (vQuadWid / 2.0f), 0f, vQuadWid, vQuadHgt);
        /* rightQuad*/
        //CreateFitboxQuad(transform, (hQuadWid / 2.0f) - (vQuadWid / 2.0f), 0f, vQuadWid, vQuadHgt);
        /*bottomQuad*/
        //CreateFitboxQuad(transform, 0f, (vQuadHgt / 2.0f) - (hQuadHgt / 2.0f), hQuadWid, hQuadHgt);
        /*   topQuad*/
        //CreateFitboxQuad(transform, 0f, (-vQuadHgt / 2.0f) + (hQuadHgt / 2.0f), hQuadWid, hQuadHgt);

        isInitialized = true;
    }

    private void CreateFitboxQuad(Transform parent, float xPos, float yPos, float width, float height)
    {
        var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.parent = parent;
        quad.transform.localPosition = new Vector3(xPos, yPos, 0);
        quad.transform.localScale = new Vector3(width, height, quad.transform.localScale.z);
        quad.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        quad.GetComponent<MeshRenderer>().material = FitboxMaterial;
    }

    int foo = 0;
    private void LateUpdate()
    {
        foo++;
        if (foo < 2) return;
        InitializeComponents();

        Transform cameraTransform = Camera.main.transform;

        interpolator.SetTargetPosition(cameraTransform.position + (cameraTransform.forward * Distance));
        interpolator.SetTargetRotation(Quaternion.LookRotation(-cameraTransform.forward, -cameraTransform.up));
    }

    void OnDestroy()
    {
        keywordRecognizer.Dispose();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        KeywordAction keywordAction;

        if (keywordCollection.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke(args);
        }
    }


}
