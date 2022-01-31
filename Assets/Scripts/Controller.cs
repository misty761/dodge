using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour
{
    public Image stick;
    public Transform player;
    private Vector3 orignPos = Vector3.zero;
    private Vector3 joyVec;
    private bool moveFlag;
    private float speed = 8f;
    float stickRadius = 0;

    // Start is called before the first frame update
    void Start()
    {
        orignPos = stick.transform.position;
        stickRadius = stick.rectTransform.sizeDelta.x * 1.5f;

        // 캔버스 크기에 대한 반지름 조절
        float can = transform.parent.GetComponent<RectTransform>().localScale.x;
        stickRadius *= can;

        moveFlag = false;
        
        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry_Drag = new EventTrigger.Entry();
        entry_Drag.eventID = EventTriggerType.Drag;
        entry_Drag.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
        eventTrigger.triggers.Add(entry_Drag);

        EventTrigger.Entry entry_EndDrag = new EventTrigger.Entry();
        entry_EndDrag.eventID = EventTriggerType.EndDrag;
        entry_EndDrag.callback.AddListener((data) => { OnEndDrag(); });
        eventTrigger.triggers.Add(entry_EndDrag);
        
    }

    void Update()
    {
        if (GameManager.isGameover)
        {
            stick.rectTransform.position = orignPos;
            moveFlag = false;
            return;
        }
            
        if (moveFlag) player.transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    public void OnDrag(PointerEventData touch)
    {
        // Touch는 모바일장치에서만 동작하니 주의, PC에서는 오동작할 수 있음
        
        if (stick == null) return;

        Vector3 dir = (new Vector3(touch.position.x, touch.position.y, orignPos.z) - orignPos).normalized;

        float touchAreaRadius = Vector3.Distance(orignPos, new Vector3(touch.position.x, touch.position.y, orignPos.z));
        if (touchAreaRadius > stickRadius)
        {
            // 반경을 넘어가는 경우는, 현재 가려는 방향으로, 반지름 만큼만 가도록 설정한다.
            stick.rectTransform.position = orignPos + (dir * stickRadius);
        }
        else
        {
            // 조이스틱이 반경내로 움직일때만, 드래그 된 위치로 설정한다.
            stick.rectTransform.position = touch.position;
        }

        moveFlag = true;
        Vector3 pos = touch.position;

        // 조이스틱을 이동시킬 방향을 구함
        joyVec = (pos - orignPos).normalized;

        // 조이스틱의 처음 위치와 현재 내가 터치하고있는 위치의 거리를 구한다.
        float dis = Vector3.Distance(pos, orignPos);
        speed = 8 * dis / 100;
        if (speed > 8)
        {
            speed = 8f;
        }

        player.eulerAngles = new Vector3(0, Mathf.Atan2(joyVec.x, joyVec.y) * Mathf.Rad2Deg, 0);
    }

    public void OnEndDrag()
    {
        // 드래그가 끝나면, 터치가 끝난 것임으로, 조이스틱을 원위치로 이동 시킨다.
        if (stick == null) return;

        stick.rectTransform.position = orignPos;
        moveFlag = false;
    }
}
