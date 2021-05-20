using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HexGrid : MonoBehaviour
{
	public int width = 6;
	public int height = 6;

	public HexCell cellPrefab;
	public HexCell[] cells;
	public TextMeshProUGUI cellLabelPrefab;
	Canvas gridCanvas;
	HexMesh hexMesh;

	void Awake()
	{
		hexMesh = GetComponentInChildren<HexMesh>();
		gridCanvas = GetComponentInChildren<Canvas>();

		cells = new HexCell[height * width + Mathf.RoundToInt(width / 2)];

		for (int y = 0, i = 0; y < height; y++)
		{
			for (int x = 0; x < width + ((y + 1) % 2); x++)
			{
				CreateCell(x, y, i++);
			}
		}
	}

	void Start()
	{
		hexMesh.Triangulate(cells);
	}

	void CreateCell(int x, int y, int i)
	{
		Vector3 position;
		position.x = (x + y * 0.5f - y / 2) * (HexMetrics.innerRadius * 2f);
		position.y = y * (HexMetrics.outerRadius * 1.5f);
		position.z = 0f;

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;

		TextMeshProUGUI label = Instantiate<TextMeshProUGUI>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.y);
		label.text = x.ToString() + "\n" + y.ToString();
	}



}
