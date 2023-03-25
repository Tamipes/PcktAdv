using Godot;
using System;

public partial class terrain : StaticBody3D
{
	private const int Prem = 99;
	private const int V = Prem * Prem;

	// daMap = null;
	[Export]
	CollisionShape3D shape = null;
	PlaneMesh MeshOfPlane = null;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MeshOfPlane = GD.Load<PlaneMesh>("res://shaders/terrain_mesh.tres");
		if (MeshOfPlane == null || shape == null)
		{
			GD.Print("Error loading collision shape into Terrain");
		}
		else
		{
			GD.Print("Loaded into terrain: " + MeshOfPlane.GetType().ToString());
			// GD.Print(testVar.SubdivideDepth);
			// // var asd = testVar.SurfaceGetArrays(0);
			// var asd = testVar.GetMeshArrays();
			// var heightmap = new HeightMapShape3D();
			// heightmap.MapWidth = Prem;
			// heightmap.MapDepth = Prem;
			// float[] heightData = new float[V];
			// GD.Print(asd);
			// var a = (Godot.Vector3[])asd[0];
			// for (int i = 0; i < V; i++)
			// {
			// 	// GD.Print(i+""+asd[i]);
			// 	var b = (Godot.Vector3)a.GetValue(i);
			// 	// GD.Print(b.X);
			// 	heightData[i] = b.Y*10;
			// }
			// heightmap.MapData = heightData;
			// shape.Shape = heightmap;

			//New new code

			HeightMapShape3D heightmap = new HeightMapShape3D();

			Material bsMaterial = MeshOfPlane.Material;
			ShaderMaterial shMaterial = (ShaderMaterial) bsMaterial;
			Shader NoiseShader = shMaterial.Shader;
			

			shape.Shape = heightmap;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
