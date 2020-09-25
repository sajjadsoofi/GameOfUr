using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	Touch touch;
	public Dices dice;
	public Tiles[] tiles;
	public int currentTile = 0;
    private void Start()
    {
    }
    void Update()
	{
		
		if (Input.touchCount > 0)
		{
			touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Began)
			{
				Vector2 worldPoint = Camera.main.ScreenToWorldPoint(touch.position);
				RaycastHit2D hitInfo;
				hitInfo = Physics2D.Raycast(worldPoint, Vector2.zero);

				if (hitInfo.collider != null)
				{
					if (hitInfo.collider.tag == "tile")
					{
						currentTile += dice.diceNumber;

						if (currentTile == hitInfo.collider.GetComponent<Tiles>().tileNumber)
                        {
							Vector2 center = hitInfo.collider.GetComponent<Renderer>().bounds.center;
							this.transform.position = center;
							
						}
                        
					}
				}
				else
				{
					Debug.Log("null collider");
				}
			}
		}
	}
}
