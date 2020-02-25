using UnityEngine;
namespace MalbersAnimations
{
    [System.Serializable]
    public class MesssageItem
    {
        public string message;
        public TypeMessage typeM;
        public bool boolValue;
        public int intValue;
        public float floatValue;
        public string stringValue;

        public float time;
        public bool sent;
    }
    public class MessagesBehavior : StateMachineBehaviour
    {
        public MesssageItem[] onEnterMessage;   //Store messages to send it when Enter the animation State
        public MesssageItem[] onExitMessage;    //Store messages to send it when Exit  the animation State
        public MesssageItem[] onTimeMessage;    //Store messages to send on a specific time  in the animation State


        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (MesssageItem ontimeM in onTimeMessage)  //Set all the messages Ontime Sent = false when start
                ontimeM.sent = false;

            foreach (MesssageItem onEnterM in onEnterMessage)
                DeliverMessage(onEnterM, animator);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (MesssageItem onExitM in onExitMessage)
                DeliverMessage(onExitM, animator);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (MesssageItem onTimeM in onTimeMessage)
            {
               //if (!onTimeM.sent && (stateInfo.normalizedTime % 1) >= onTimeM.time) 
                if (!onTimeM.sent && Mathf.Abs((stateInfo.normalizedTime % 1) - onTimeM.time) <= 0.1f)
                {
                    onTimeM.sent = true;
                    DeliverMessage(onTimeM, animator);
                }
            }
        }

        void DeliverMessage(MesssageItem m, Animator anim)
        {
            switch (m.typeM)
            {
                case TypeMessage.Bool:
                    anim.SendMessage(m.message, m.boolValue, SendMessageOptions.DontRequireReceiver);
                    break;
                case TypeMessage.Int:
                    anim.SendMessage(m.message, m.intValue, SendMessageOptions.DontRequireReceiver);
                    break;
                case TypeMessage.Float:
                    anim.SendMessage(m.message, m.floatValue, SendMessageOptions.DontRequireReceiver);
                    break;
                case TypeMessage.String:
                    anim.SendMessage(m.message, m.stringValue, SendMessageOptions.DontRequireReceiver);
                    break;
                case TypeMessage.Void:
                    anim.SendMessage(m.message, SendMessageOptions.DontRequireReceiver);

                    break;
                default:
                    break;
            }
        }
    }
}