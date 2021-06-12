using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePhysics : MonoBehaviour
{
    public Rigidbody2D rigidbody1;
    public Rigidbody2D rigidbody2;

    public int numberOfSubdivisions = 10;
    float lengthOfSubdivisions = 1;
    public float totalLength = 5f;
    public float subDivisionMass = 0.01f;
    public float frequency = 6;

    List<GameObject> springs = new List<GameObject>();
    LineRenderer lineRend;
    public float lineWidth = 0.1f;
    public Material lineMaterial;

    public enum RopeSide{LEFT,RIGHT};
    

    //Don't implement yet;
    float drag;


    // Start is called before the first frame update
    void Start()
    {
        GenerateRope();
        
    }

    void GenerateRope()
    {
        lengthOfSubdivisions = totalLength / numberOfSubdivisions;
        GameObject rend = new GameObject();
        rend.transform.parent = this.transform;
        rend.name = "Lines";
        lineRend = rend.AddComponent<LineRenderer>();
        lineRend.startWidth = this.lineWidth;
        lineRend.endWidth = this.lineWidth;
        lineRend.material = lineMaterial;
        for (int i = 0; i < numberOfSubdivisions; i++)
        {
            GameObject obj = new GameObject();
            obj.name = "Rope Element" + i;
            obj.transform.parent = this.transform;
            Rigidbody2D body = obj.AddComponent<Rigidbody2D>();
            body.mass = subDivisionMass;
            springs.Add(obj);
        }


        for (int i = 0; i < springs.Count; i++)
        {
            if (i == 0)
            {
                SpringJoint2D joint = springs[i].AddComponent<SpringJoint2D>();
                joint.autoConfigureDistance = false;
                joint.connectedBody = rigidbody1;
                joint.distance = this.lengthOfSubdivisions;
                joint.frequency = this.frequency;
                joint.dampingRatio = 1;
                //joint.enableCollision = true;
            }
            else
            {
                SpringJoint2D joint2 = springs[i].AddComponent<SpringJoint2D>();
                joint2.autoConfigureDistance = false;
                joint2.connectedBody = springs[i - 1].GetComponent<Rigidbody2D>();
                joint2.distance = this.lengthOfSubdivisions;
                joint2.frequency = this.frequency;
                joint2.dampingRatio = 1;
                //joint2.enableCollision = true;
            }
            if (i == springs.Count - 1)
            {
                SpringJoint2D joint2 = springs[i].AddComponent<SpringJoint2D>();
                joint2.autoConfigureDistance = false;
                joint2.connectedBody = rigidbody2;
                joint2.distance = this.lengthOfSubdivisions;
                joint2.frequency = this.frequency;
                joint2.dampingRatio = 1;
                //joint2.enableCollision = true;
            }
            else
            {
                SpringJoint2D joint = springs[i].AddComponent<SpringJoint2D>();
                joint.autoConfigureDistance = false;
                joint.connectedBody = springs[i + 1].GetComponent<Rigidbody2D>();
                joint.distance = this.lengthOfSubdivisions;
                joint.frequency = this.frequency;
                joint.dampingRatio = 1;
                //joint.enableCollision = true;

            }
            springs[i].AddComponent<CircleCollider2D>().radius = this.lengthOfSubdivisions;
            springs[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;


        }
        rigidbody1.GetComponent<SpringJoint2D>().connectedBody = springs[0].GetComponent<Rigidbody2D>();
        rigidbody1.GetComponent<SpringJoint2D>().distance = this.lengthOfSubdivisions;


        rigidbody2.GetComponent<SpringJoint2D>().connectedBody = springs[springs.Count - 1].GetComponent<Rigidbody2D>();
        rigidbody2.GetComponent<SpringJoint2D>().distance = this.lengthOfSubdivisions;
    }

    public void applyRopeEndTo(RopeSide side,Rigidbody2D rb)
    {
        if (side == RopeSide.LEFT)
        {
            this.rigidbody1.GetComponent<SpringJoint2D>().connectedBody = null;
            this.springs[0].GetComponent<SpringJoint2D>().connectedBody = rb;
            this.rigidbody1 = rb;
        }
        else
        {
            this.rigidbody2.GetComponent<SpringJoint2D>().connectedBody = null;
            this.springs[springs.Count - 1].GetComponent<SpringJoint2D>().connectedBody = rb;
            this.rigidbody2 = rb;
        }
    }

    //rope must already have length.
    public void addLength(int count)
    {
        lineRend.positionCount += count;
        int springsSize = springs.Count;
        for(int i = 0; i < count; i++)
        {

            GameObject obj = new GameObject();
            obj.name = "Rope Element" + (springsSize+i);
            obj.transform.parent = this.transform;
            Rigidbody2D body = obj.AddComponent<Rigidbody2D>();
            body.mass = subDivisionMass;
            CircleCollider2D collider =obj.AddComponent<CircleCollider2D>();
            collider.radius = this.lengthOfSubdivisions;
            springs.Add(obj);
        }

        SpringJoint2D[] joints = springs[springsSize - 1].GetComponents<SpringJoint2D>();
        foreach(SpringJoint2D joint in joints)
        {
            if (joint.connectedBody.Equals(rigidbody2))
            {
                joint.connectedBody = springs[springsSize].GetComponent<Rigidbody2D>();
            }
        }
        for (int i = 0; i < count; i++)
        {
            int index = i + springsSize;
            //check if you've not reached the last one
            if (springsSize + i < springs.Count - 1)
            {
                SpringJoint2D j = springs[index].AddComponent<SpringJoint2D>();
                j.connectedBody = springs[index+1].GetComponent<Rigidbody2D>();
                j.autoConfigureDistance = false;
    
                j.distance = this.lengthOfSubdivisions;
                j.frequency = this.frequency;
                j.dampingRatio = 1;

                SpringJoint2D j2 = springs[index].AddComponent<SpringJoint2D>();
                j2.connectedBody = springs[index - 1].GetComponent<Rigidbody2D>();
                j2.autoConfigureDistance = false;
                
                j2.distance = this.lengthOfSubdivisions;
                j2.frequency = this.frequency;
                j2.dampingRatio = 1;
            }
            else
            {
                SpringJoint2D j = springs[index].AddComponent<SpringJoint2D>();
                j.connectedBody = rigidbody2;
                j.autoConfigureDistance = false;
              
                j.distance = this.lengthOfSubdivisions;
                j.frequency = this.frequency;
                j.dampingRatio = 1;
                SpringJoint2D j2 = springs[index].AddComponent<SpringJoint2D>();
                j2.connectedBody = springs[index - 1].GetComponent<Rigidbody2D>();
                rigidbody2.GetComponent<SpringJoint2D>().connectedBody = j.GetComponent<Rigidbody2D>();
                j2.autoConfigureDistance = false;
           
                j2.distance = this.lengthOfSubdivisions;
                j2.frequency = this.frequency;
                j2.dampingRatio = 1;
            }
        }
        Debug.Log(count+" segments added.");
    }

    //this method works so long as you never hit zero.
    public void removeLength(int count)
    {
        lineRend.positionCount -= count;
        int springsCount = springs.Count;
        for(int i = springsCount-1; i > 0; i--)
        {
            if (i > springsCount - count - 1)
            {
                springs.RemoveAt(i);
            } else if (i == springsCount - count - 1)
            {
                SpringJoint2D[] joints = springs[i].GetComponents<SpringJoint2D>();
                foreach (SpringJoint2D joint in joints)
                {
                    if (joint.connectedBody == null)
                    {
                        joint.connectedBody = rigidbody2;
                    }
                }
                
            }
        }


    }

    private void Update()
    {
        lineRend.positionCount = springs.Count + 2;
        lineRend.SetPosition(0, rigidbody1.transform.position);
        for(int i = 0;i<this.springs.Count;i++)
        {
            lineRend.SetPosition(i+1,springs[i].transform.position);
        }
        lineRend.SetPosition(springs.Count+1, rigidbody2.transform.position);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            addLength(1);
        }
    }
}