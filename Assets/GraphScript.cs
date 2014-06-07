using UnityEngine;
using System.Collections;

public class GraphScript : MonoBehaviour
{
	public float xmin = -5f;
	public float xmax = 5f;
	public float zmin = -5f;
	public float zmax = 5f;
	public int xmesh = 20;
	public int zmesh = 20;
	public int resolution = 500;
	public float yscale = 10f;
	public Color pointColor = new Color(1f, 1f, 0.2f);
	public float pointSize = 0.05f;

	private NormalDistributionScript dist;
	private ParticleSystem.Particle[] points;
	private bool changed;

	void Start()
	{
		dist = GameObject.Find("NormalDistribution").GetComponentInChildren<NormalDistributionScript>();
		points = new ParticleSystem.Particle[CalculateNumberOfPoints()];
		BuildMesh();
	}
	
	void Update()
	{
		if (changed) {
			CalculateProbabilities();
		}
	}

	public void GraphChanged()
	{
		changed = true;
	}

	private int CalculateNumberOfPoints()
	{
		return (xmesh + 1) * (resolution + 1) + (zmesh + 1) * (resolution + 1);
	}

	private void BuildMesh()
	{
		int i = 0;
		for (int xi = 0; xi <= xmesh; ++xi) {
			for (int zi = 0; zi <= resolution; ++zi) {
				float x = xmin + (xmax - xmin) * xi / xmesh;
				float z = zmin + (zmax - zmin) * zi / resolution;
				SetPoint(ref points[i++], x, z);
			}
		}
		for (int zi = 0; zi <= zmesh; ++zi) {
			for (int xi = 0; xi <= resolution; ++xi) {
				float x = xmin + (xmax - xmin) * xi / resolution;
				float z = zmin + (zmax - zmin) * zi / zmesh;
				SetPoint(ref points[i++], x, z);
			}
		}

		CalculateProbabilities();
	}

	private void SetPoint(ref ParticleSystem.Particle p, float x, float z)
	{
		p.position = new Vector3(x, 0f, z);
		p.color = pointColor;
		p.size = pointSize;
	}

	private void CalculateProbabilities()
	{
		for (int i = 0; i < points.Length; ++i) {
			float x = points[i].position.x;
			float z = points[i].position.z;
			float y = dist.Probability(x, z);
			points[i].position = new Vector3(x, y * yscale, z);
		}

		UpdatePoints();
	}

	private void UpdatePoints()
	{
		particleSystem.SetParticles(points, points.Length);
		changed = false;
	}
}
