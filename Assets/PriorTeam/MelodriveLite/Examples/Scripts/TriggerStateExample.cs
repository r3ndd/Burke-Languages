using UnityEngine;
using Melodrive;
using Melodrive.Styles;
using Melodrive.Emotions;

public class TriggerStateExample : MelodriveObject
{
    StateTriggerObject listener;

    void Start()
    {
        md.Init(Style.House, Emotion.Happy);
        md.CreateMusicalSeed();
        md.Play();

        listener = FindObjectOfType<StateTriggerObject>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject != listener.gameObject)
                {
                    StateTriggerObject stateTrigger = listener.GetComponent<StateTriggerObject>();
                    stateTrigger.targetPosition = hit.collider.transform.position;
                    stateTrigger.state = StateTriggerObject.State.Move;
                }
            }
        }
    }
}
