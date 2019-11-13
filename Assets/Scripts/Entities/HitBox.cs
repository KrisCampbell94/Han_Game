using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Vector2 offset = new Vector2(2, 0);

    private BoxCollider2D hitboxCollider;
    
    public bool Enabled {
        get
        {
            return hitboxCollider.enabled;
        }
        set
        {
            hitboxCollider.enabled = value;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        hitboxCollider = gameObject.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
        hitboxCollider.isTrigger = true;
        Enabled = false;
    }

    private void FixedUpdate()
    {
        if (Enabled)
        {
            // If left, make -2,0 Else make 2,0
            Vector2 offsetDelta = offset * (GetComponent<SpriteRenderer>().flipX ? -1 : 1);
            hitboxCollider.offset = offsetDelta;
        }
    }
}
