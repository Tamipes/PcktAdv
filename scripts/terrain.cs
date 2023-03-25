using Godot;
using System;

public partial class terrain : StaticBody3D
{
	private const int Prem = 512;
	private const int V = Prem * Prem;

	// daMap = null;
	[Export]
	float heightMapStrech = 0.9f;
	[Export]
	CollisionShape3D shape = null;
	[Export]
	Texture2D myTexture = null;
	PlaneMesh MeshOfPlane = null;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		myTexture = GD.Load<Texture2D>("res://sprites/noise.tres");
		// MeshOfPlane = GD.Load<PlaneMesh>("res://shaders/terrain_mesh.tres");
		// if (MeshOfPlane == null || shape == null)
		if (myTexture == null)
		{
			GD.Print("Error loading collision shape into Terrain");
		}
		else
		{
			GD.Print("Seems to be loaded correctly in Terrain mesh geneartor;");
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
			// Material bsMaterial = MeshOfPlane.Material;
			// ShaderMaterial shMaterial = (ShaderMaterial) bsMaterial;
			// Shader NoiseShader = shMaterial.Shader;

			GD.Print("height: " + myTexture.GetHeight());
			GD.Print("Modes:" + myTexture.GetType());
			NoiseTexture2D maNoise = (NoiseTexture2D)myTexture;
			maNoise.Changed += () =>
			{
				Image img = maNoise.GetImage();
				img.Convert(Image.Format.L8);
				byte[] myData = img.GetData();
				float[] FloatData = new float[V];
				heightmap.MapWidth = img.GetWidth();
				heightmap.MapDepth = img.GetHeight();
				GD.Print("lenght:" + img.GetData().Length);
				for (int x = 0; x < V; x++)
				{
					FloatData[x] = ((float)myData[x])*heightMapStrech;
				}
				heightmap.MapData = FloatData;
				shape.Shape = heightmap;
			};
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
