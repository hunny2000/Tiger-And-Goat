using UnityEngine;
using System.Collections;


namespace MalbersAnimations
{
    
    public class StepTrigger : MonoBehaviour
    {
        StepsManager _StepsManager;
        [HideInInspector]
        public AudioSource StepAudio;
        [Range(0,1)]
        public float volume = 1;
        bool hastrack; // Check if already  has a track... don't put another
        bool waitrack; // Check if is time to put a track; 
        public bool HasTrack
        {
            get { return hastrack; }
            set { hastrack = value; }
        }

        void Awake()
        {
            _StepsManager = GetComponentInParent<StepsManager>();

            StepAudio = GetComponent<AudioSource>();

            if (StepAudio == null)
            {
                StepAudio = gameObject.AddComponent<AudioSource>();
            }
            

            StepAudio.volume = volume;
        }


        void OnTriggerEnter(Collider other)
        {

            if (!waitrack && _StepsManager)// && (other.gameObject.layer == GetComponentInParent<MHorse>().GroundLayer))
            {
                //Wait Half a Second before making another Step
                 StartCoroutine(WaitForStep(0.5f));

                _StepsManager.EnterStep(this);
                hastrack = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            hastrack = false; // if the feet is on the air then can put a track
        }

        IEnumerator WaitForStep(float seconds)
        {
            waitrack =  true;
            yield return new WaitForSeconds(seconds);
            waitrack = false;
        }


        void OnDrawGizmos()
        {
            SphereCollider _C = GetComponent<SphereCollider>();
            Gizmos.color = Color.cyan;
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.DrawWireSphere(Vector3.zero+ _C.center, _C .radius);
            Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.5f);
            Gizmos.DrawSphere(Vector3.zero + _C.center, _C.radius);
        }


        void OnDrawGizmosSelected()
        {
            SphereCollider _C = GetComponent<SphereCollider>();
            Gizmos.color = Color.yellow;
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.DrawWireSphere(Vector3.zero + _C.center, _C.radius);
            Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.5f);
            Gizmos.DrawSphere(Vector3.zero + _C.center, _C.radius);
        }

    }
}