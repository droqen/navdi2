namespace navdi2 {
using UnityEngine;

public interface IBodCollidable
{
	Bod HitRect(Vector2 pos, Vector2 size); // do i collide with a given rect?
}
}