using UnityEngine;
using System.Collections;

public class GUIControlScript : MonoBehaviour
{
	public float x1MeanMin = -5f;
	public float x1MeanMax = 5f;
	public float x2MeanMin = -5f;
	public float x2MeanMax = 5f;
	public float x1VarMin  = 0.1f;
	public float x1VarMax  = 5f;
	public float x2VarMin  = 0.1f;
	public float x2VarMax  = 5f;
	public float covLimitThreshold = 0.999f;

	private NormalDistributionScript dist;
	private GraphScript graph;
	private bool valueChanged;

	void Start()
	{
		dist = GameObject.Find("NormalDistribution").GetComponentInChildren<NormalDistributionScript>();
		graph = GameObject.Find("Graph").GetComponentInChildren<GraphScript>();
	}

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(20, 20, 200, 400));

		Slider("X1 Mean", ref dist.x1Mean, x1MeanMin, x1MeanMax);
		Slider("X2 Mean", ref dist.x2Mean, x2MeanMin, x2MeanMax);
		Slider("X1 Variance", ref dist.x1Var, x1VarMin, x1VarMax);
		Slider("X2 Variance", ref dist.x2Var, x2VarMin, x2VarMax);

		float covLimit = CalculateCovLimit();
		Slider("Covariance of X1 and X2", ref dist.x1X2Cov, -covLimit, covLimit);

		GUILayout.EndArea();

		NotifyIfChanged();
	}

	private void Slider(string label, ref float valueRef, float min, float max)
	{
		GUILayout.Label(label);
		UpdateValue(ref valueRef, GUILayout.HorizontalSlider(valueRef, min, max));
	}

	private float CalculateCovLimit()
	{
		/* 共分散は分散共分散行列の行列式が正になる範囲を動く */
		float limit = covLimitThreshold * Mathf.Sqrt(dist.x1Var * dist.x2Var);

		/* 現在の共分散が許容範囲外であれば範囲内に移動する */
		UpdateValue(ref dist.x1X2Cov, Mathf.Max(-limit, Mathf.Min(dist.x1X2Cov, limit)));

		return limit;
	}

	private void UpdateValue(ref float target, float newValue)
	{
		if (target != newValue) {
			target = newValue;
			valueChanged = true;
		}
	}

	private void NotifyIfChanged()
	{
		if (valueChanged) {
			graph.GraphChanged();
			valueChanged = false;
		}
	}
}
