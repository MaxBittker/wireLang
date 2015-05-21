using UnityEngine;
using System.Collections;

public class WireMotion : MonoBehaviour
{
    private IEnumerator Fiber;

    void Start ()
    {
        Fiber = DynamicParse();
    }
	
	void FixedUpdate () 
    {
	    if (Fiber != null && !Fiber.MoveNext())
	    {
            Fiber = Loop ? DynamicParse() : null;
        }
	}

    public string Program;
    public float Speed = 1;
    public float SegmentLength = 1;
    public bool Loop = true;

    private IEnumerator DynamicParse()
    {
        foreach (char letter in Program)
        {
            // todo : * (or in fact any kind of non-letter and/or more-than-one-character tokenization)

            float segmentDistanceDone = 0;
            float distanceToDo = char.IsUpper(letter) ? (2 * SegmentLength) : SegmentLength;
            while (segmentDistanceDone < distanceToDo)
            {
                // would be easy to make the translation relative to some referential here
                // also, there might be some overshoot at high speeds/low frame rate... pls do corner-turns properly
                Vector3 vel = ParseDir(char.ToLowerInvariant(letter)) * Speed * Time.deltaTime;
                segmentDistanceDone += Speed * Time.deltaTime;

                transform.Translate(vel, Space.World); 

                yield return null;
            }
        }
    }

    Vector3 ParseDir(char letter)
    {
        Vector3 dir = Vector3.zero;

        switch (letter)
        {
            case 'n': dir.y = 1; break;
            case 's': dir.y = -1; break;
            case 'e': dir.x = 1; break;
            case 'w': dir.x = -1; break;
            case 't': dir.x = -1; dir.y = 1; break;
            case 'l': dir.x = -1; dir.y = -1; break;
            case 'r': dir.x = 1; dir.y = 1; break;
            case 'd': dir.x = 1; dir.y = -1; break;
	    }

        return dir.normalized;
    }
}
