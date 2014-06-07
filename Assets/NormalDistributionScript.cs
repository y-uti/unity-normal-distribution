using UnityEngine;
using System.Collections;

/**
 * 2 次元の多変量正規分布
 */
public class NormalDistributionScript : MonoBehaviour
{
	public float x1Mean  = 0f;
	public float x2Mean  = 0f;
	public float x1Var   = 1f;
	public float x2Var   = 1f;
	public float x1X2Cov = 0f;

	public float Probability(float x1, float x2)
	{
		float dx1 = x1 - x1Mean;
		float dx2 = x2 - x2Mean;
		float det = x1Var * x2Var - x1X2Cov * x1X2Cov;
		float coef = 1f / (2f * Mathf.PI * Mathf.Sqrt(det));
		float dist = (dx1 * dx1 * x2Var - 2f * dx1 * dx2 * x1X2Cov + dx2 * dx2 * x1Var) / det;
		float probability = coef * Mathf.Exp(-dist / 2f);

		return probability;
	}
}
