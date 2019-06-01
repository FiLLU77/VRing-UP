using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace VRStandardAssets.ShootingGallery
{
    
    public class shottu3 : MonoBehaviour
    {

        //[SerializeField] private float m_TimeOutDuration = 2f;          // How long the target lasts before it disappears.
        [SerializeField] private float m_DestroyTimeOutDuration = 3f;   // When the target is hit, it shatters.  This is how long before the shattered pieces disappear.
        [SerializeField] private float m_RespawnDuration = 3f;
        [SerializeField] private int m_RespawnCount = 10;
        [SerializeField] private GameObject m_DestroyPrefab;            // The prefab for the shattered target.
        [SerializeField] private int m_score;

        private Transform m_CameraTransform;                            // Used to make sure the target is facing the camera.
        //private VRInteractiveItem m_InteractiveItem;                    // Used to handle the user clicking whilst looking at the target.
        private AudioSource m_Audio;                                    // Used to play the various audio clips.
        private Renderer m_Renderer;                                    // Used to make the target disappear before it is removed.
        private Collider m_Collider;                                    // Used to make sure the target doesn't interupt other shots happening.
        private bool m_IsEnding;                                        // Whether the target is currently being removed by another source.

        private void Awake()
        {
            // Setup the references.
            m_CameraTransform = Camera.main.transform;
            m_Audio = GetComponent<AudioSource>();
            //m_InteractiveItem = GetComponent<VRInteractiveItem>();
            m_Renderer = GetComponent<Renderer>();
            m_Collider = GetComponent<Collider>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Invoke("next", 1f);
            }
        }

        private void OnEnable()
        {
            //m_InteractiveItem.OnDown += HandleDown;
        }


        private void OnDisable()
        {
            //m_InteractiveItem.OnDown -= HandleDown;
        }


        private void OnDestroy()
        {
            // Ensure the event is completely unsubscribed when the target is destroyed.

        }


        public IEnumerator Restart()
        {

            yield return new WaitForSeconds(m_RespawnDuration);

            // When the target is spawned turn the visual and physical aspects on.
            m_Renderer.enabled = true;
            m_Collider.enabled = true;

            // Since the target has just spawned, it's not ending yet.
            m_IsEnding = false;


            // Make sure the target is facing the camera.
            //transform.LookAt(m_CameraTransform);

            // Start the time out for when the target would naturally despawn.
            //StartCoroutine (MissTarget());

            // Start the time out for when the game ends.
            // Note this will only come into effect if the game time remaining is less than the time out duration.
            //StartCoroutine (GameOver (gameTimeRemaining));

        }

        private void HandleDown()
        {
            // If it's already ending, do nothing else.
            if (m_IsEnding)
                return;

            // Otherwise this is ending the target's lifetime.
            m_IsEnding = true;

            // Turn off the visual and physical aspects.
            m_Renderer.enabled = false;
            m_Collider.enabled = false;

            // Add to the player's score.
            //SessionData.AddScore(m_Score);

            // Instantiate the shattered target prefab in place of this target.
            GameObject destroyedTarget = Instantiate(m_DestroyPrefab, transform.position, transform.rotation) as GameObject;

            // Destroy the shattered target after it's time out duration.
            Destroy(destroyedTarget, m_DestroyTimeOutDuration);

        }

        //detect collision
        void OnCollisionEnter(Collision col)
        {
            //collision with bullet
            if (col.gameObject.tag == "Bullet" && m_Collider.enabled)
            {
                ScoreManager.SMi.AddScore(m_score);
                HandleDown();
                StartCoroutine(Restart());
            }
            else if (col.gameObject.tag == "Fire" && m_Collider.enabled)
            {
                ScoreManager.SMi.AddScore(m_score);
                HandleDown();
                StartCoroutine(Restart());
            }
            //Invoke("next",3f); 
         } 

        void next(){
            GazeInteraction.start=2;
           SceneManager.LoadScene("main");
            //SceneManager.LoadScene("game_test");
        }
    }
}
