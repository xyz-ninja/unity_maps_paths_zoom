using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gui : MonoBehaviour
{
	[HideInInspector]
	public bool isMouseOverGui = false;

	Main main;
    Text infoText;

	void Start() {
		main = GameObject.Find("Main").GetComponent<Main>();
        infoText = GameObject.Find("InfoText").GetComponent<Text>();    
    }

	void Update() {
		// выводим информацию о линии
		string info = "";
		info += "Текущий режим: ";
		
		if (main.curPaintMode == Main.PAINT_MODE.CONTINUOUS) info += "Рисование";
		else if (main.curPaintMode == Main.PAINT_MODE.POINTS) info += "По точкам";
		else info += "НЕ ВЫБРАН";

		info += "\nДлина линии: ";
		if (main.curLine == null) info += "НЕИЗВЕСТНО";
		else info += main.curLine.GetComponent<Line>().getLineLength().ToString();

        infoText.text = info;
	}

	public void ClearScreen() {
		Destroy(main.curLine);
		main.curLine = null;
	}

	public void SetCurPaintModeToContinous() {
		main.setCurPaintMode(Main.PAINT_MODE.CONTINUOUS);
	}

	public void SetCurPaintModeToPoints() {
		main.setCurPaintMode(Main.PAINT_MODE.POINTS);
	}
}
