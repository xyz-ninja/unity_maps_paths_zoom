using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Main : MonoBehaviour
{
	public enum PAINT_MODE { CONTINUOUS, POINTS }; // режимы рисования: продолжительный или по точкам
	public PAINT_MODE curPaintMode; // текущий режим

	public GameObject linePrefab;
	[HideInInspector]
	public GameObject curLine;
	public float initLineWidth = 1f;

	// нужна для эмуляции нажатия двумя пальцами
	// от этой позиции отчитывается зум камеры
	// если мышкой потянуть вверх камера приблизится и наоборот
	Vector3 firstFingerPos = new Vector3(-1,-1,-1);
	float dragZCoef = 0f; // коэффициент смещения по Z

	public float zoomSpeed = 18f; // скорость приближения / отдаления
	// минимальный/максимальный размер камеры
	public float minCameraSize = 20f; 
	public float maxCameraSize = 70f;

	// отношение ширины линии к размеру камеры
	float initLineWidthToCameraSizeRatio;       // такое соотношение должно быть всегда
	float curLineWidthToCameraSizeRatio = -1;   // текущее соотношение

	void Start() {
		initLineWidthToCameraSizeRatio = initLineWidth / Camera.main.orthographicSize;
	}

	void Update() {
		if (curLine != null) {
			curLineWidthToCameraSizeRatio = curLine.GetComponent<Line>().getLineWidth() / Camera.main.orthographicSize;

            // initLineWidth [1] = initRatio [0.25]
            // x = curRatio [0.3]
            // => 1 * 0.25 / curRation

            /*if (curLineWidthToCameraSizeRatio != initLineWidthToCameraSizeRatio) {
                
                curLineWidthToCameraSizeRatio = initLineWidthToCameraSizeRatio;
            }*/

            float newLineWidth = curLine.GetComponent<Line>().getLineWidth() * (initLineWidth * initLineWidthToCameraSizeRatio / curLineWidthToCameraSizeRatio);
            curLine.GetComponent<Line>().setLineWidth(newLineWidth);

            //Debug.Log(curLineWidthToCameraSizeRatio);			 
        }

		// если курсор не лежит на ui
		if (!isMouseOverUI()) {
			// получаем позицию курсора в 3д пространстве
			Vector3 mouseLocalPos = Input.mousePosition;
			mouseLocalPos.z = Camera.main.nearClipPlane;
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mouseLocalPos);

			// если эмулируется нажатие двумя пальцами
			if (Input.GetButton("Fire2")) {
				// если позиция отчёта не задана, задаём её
				if (firstFingerPos == new Vector3(-1,-1,-1)) {
					firstFingerPos = mousePosition;
				} else {
					dragZCoef = firstFingerPos.z - mousePosition.z;

					// меняем масштаб камеры
					float dZoom = 0f;

					if (dragZCoef < -2f) {
						dZoom = -zoomSpeed * Time.deltaTime;
					} else if (dragZCoef > 2f) {
						dZoom = zoomSpeed * Time.deltaTime;
					}
					float newCameraSize = Camera.main.orthographicSize + dZoom;
					if (newCameraSize < minCameraSize) newCameraSize = minCameraSize;
					else if (newCameraSize > maxCameraSize) newCameraSize = maxCameraSize;

					Camera.main.orthographicSize = newCameraSize;
				}
			} else {
				firstFingerPos = new Vector3(-1,-1,-1);
				dragZCoef = 0f;

				// рисуем линию если зажата кнопка мыши 
				if (curPaintMode == PAINT_MODE.CONTINUOUS) {
					if (Input.GetButton("Fire1")) {
						DrawLineTo(mousePosition);
					}
				} else if (curPaintMode == PAINT_MODE.POINTS) {
					if (Input.GetButtonDown("Fire1")) {
						DrawLineTo(mousePosition);
					}
				}
			}
		}
	}

	// рисует линию к заданной позиции
	protected void DrawLineTo(Vector3 _toPos) {
		Vector3 toPosition = _toPos;
		if (curLine == null) {
			curLine = Instantiate(linePrefab, toPosition, Quaternion.identity);
			curLine.transform.SetParent(transform);
			curLine.GetComponent<Line>().SetInitPosition(toPosition);
		} else {
			Line line = curLine.GetComponent<Line>();

			line.BuildFromEndTo(toPosition);
		}
	}

	// проверяет находится ли курсор мыши на интерфейсе
	private bool isMouseOverUI() {
		return EventSystem.current.IsPointerOverGameObject();
	}

	public void setCurPaintMode(PAINT_MODE _n) {
		curPaintMode = _n;
	}
}
