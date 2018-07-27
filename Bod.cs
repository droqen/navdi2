namespace navdi2
{

using UnityEngine;

[SelectionBase]
[ExecuteInEditMode] // for OnDrawGizmos
public class Bod : BankableEntity, IBodCollidable {

////Properties
	public SpriteRenderer spriter;
	public bool collides = true;
	public Vector2 size;
	public Vector2 position {
		get { return _pos; }
		set { _pos = value; RefreshPosition(); }
	}
	public Vector2 roundPosition {
		get { return _roundPos; }
		set { position = value; }
	}
	public Vector2 centerPosition {
		get { return _roundPos + size * 0.5f; }
	}
	virtual public Vector3 depthPosition {
		get { return (Vector3)roundPosition; }
	}
	
	Vector2 _pos;
	Vector2 _roundPos;

////Useful Publics
	public enum MoveAxis {
		X = 0, H = 0, Horizontal = 0,
		Y = 1, V = 1, Vertical = 1,
	}
	public bool Move(float amount, MoveAxis axis, params IBodCollidable[] collidables) {

		bool maintainMomentum = true;

		if (amount == 0) return false;

		Vector2 moveVector;
		if (axis == MoveAxis.H) {
			moveVector = Vector2.right;
		} else if (axis == MoveAxis.V) {
			moveVector = Vector2.up;
		} else {
			throw new System.Exception("Unknown MoveAxis "+axis);
		}

		int intAmount = Round(position.x + amount) - Round(position.x);
		if (intAmount == 0) intAmount = amount < 0 ? -1 : 1;
		while(intAmount != 0) {
			bool hit = false;
			for (int i = 0; i < collidables.Length; i++) {
				// if (collidables[i].HitRect(roundPosition + moveVector * intAmount, size) != null) {
				if (collidables[i].HitRect(Round(position + moveVector * amount), size) != null) {
					hit = true;
					break;
				}
			}
			if (hit) {
				maintainMomentum = false;
				if (intAmount > 0) {
					intAmount--;
					amount--; if (amount < 0) amount = 0;
				}
				if (intAmount < 0) {
					intAmount++;
					amount++; if (amount > 0) amount = 0;
				}
			} else {
				position += moveVector * amount;
				break; // done moving!
			}
		}

		if (!maintainMomentum) {
			if (axis == MoveAxis.H) position = new Vector2(roundPosition.x,position.y);
			if (axis == MoveAxis.V) position = new Vector2(position.x,roundPosition.y);
		}

		return maintainMomentum;

	}

////IBodCollidable
	public Bod HitRect(Bod otherBod) {
		if (otherBod.collides && HitRect(otherBod.roundPosition, otherBod.size)) {
			return this;
		} else {
			return null;
		}
	}
	public Bod HitRect(Vector2 pos, Vector2 size) {
		if (this.collides
		&&	this.roundPosition.x + this.size.x 	> pos.x
		&&	this.roundPosition.y + this.size.y 	> pos.y
		&&	this.roundPosition.x 				< pos.x + size.x
		&&	this.roundPosition.y 				< pos.y + size.y
		)
			return this;
		return null;
	}

////Private
	void RefreshPosition() {
		_roundPos = new Vector2(
			Round(position.x),
			Round(position.y)
		);
		this.transform.position = depthPosition;
	}
	int Round(float value) {
		return Mathf.RoundToInt(value);
	}
	Vector2 Round(Vector2 value) {
		return new Vector2(Round(value.x),Round(value.y));
	}
	void OnDrawGizmos() {
		if (collides) {
			Vector3[] corners = new Vector3[4];
			for (int i = 0; i < 4; i++) corners[i] = transform.position;
			corners[0] += (Vector3)size; corners[1].y += size.y; corners[3].x += size.x;
			for (int i = 0; i < 4; i++) Gizmos.DrawLine(corners[i], corners[(i+1)%4]);
		} // else nope, it doesn't collide, so there's no box.
	}
}

}