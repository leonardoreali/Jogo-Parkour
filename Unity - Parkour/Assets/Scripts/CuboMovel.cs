using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuboMovel : MonoBehaviour
{
    [SerializeField]
    [Range(0, 2)]
    private int _moveID;
    private Transform _startPos;
    [SerializeField]
    private Transform _finalPos;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        _startPos = this.gameObject.transform;
        rb = GetComponent<Rigidbody>();
       
    }

    // Update is called once per frame
    void Update()
    {
       
       
        if (_moveID != 0)
        {
            
            steppedOn();
            if(transform.position == _startPos.position)
            {
                _moveID = 0;
            }
            
        }
        
        
        
    }
    void steppedOn()
    {
        
        if (_moveID == 1) { rb.MovePosition(Vector3.MoveTowards(transform.position, _finalPos.position, 1f)); }
        if (_moveID == 2) { rb.MovePosition(Vector3.MoveTowards(transform.position, _startPos.position, 1f)); }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _moveID = 1;
            collision.transform.parent = this.transform;
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _moveID = 2;
           collision.transform.parent = null;
        }
    }
}
