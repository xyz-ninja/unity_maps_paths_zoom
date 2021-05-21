using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
	protected LineRenderer lr;

	void Start() {
		lr = GetComponent<LineRenderer>();
    }

	public void SetInitPosition(Vector3 _initPos) {
		this.Start();

		if (lr == null) Debug.LogError("lr = null");

		// меняем стандартный позиции начала/конца линии
		lr.SetPosition(0, _initPos);
		lr.SetPosition(1, _initPos);
	}

	// строит линию с конца к заданной позиции
	public void BuildFromEndTo(Vector3 _pos) {
		Vector3 newPosition = _pos;
		Vector3 prevPosition = lr.GetPosition(lr.positionCount - 1);
		
		lr.positionCount++;

		Vector3 endPosition = new Vector3(newPosition.x, prevPosition.y, newPosition.z);
		lr.SetPosition(lr.positionCount - 1, endPosition);
	}

	// вовращает длину построенной линии
	public float getLineLength() {
		// находим расстояние между всеми точками
		float resultLength = 0;
		for (int i = 1; i < lr.positionCount; i++) {
			float distBetweenPoints = Vector3.Distance(lr.GetPosition(i), lr.GetPosition(i - 1));
            resultLength += distBetweenPoints;
		}

        return resultLength;
	}

    public float getLineWidth() {
        return lr.endWidth;
    }
    // задаёт ширину линии
    public void setLineWidth(float _newWidth) {
        lr.startWidth = _newWidth;
        lr.endWidth = _newWidth;
    }
}
