using Godot;
using System;

public partial class terrain_generation : Node
{
    [Export] uint textureRes = 32;
    [Export] Texture2D noise_text;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        noise_text.Changed += () => run_shader();
    }

    public void run_shader()
    {
        // Create a local rendering device.
        var rd = RenderingServer.CreateLocalRenderingDevice();

        // Load GLSL shader
        var shaderFile = GD.Load<RDShaderFile>("res://shaders/terrain.glsl");
        var shaderBytecode = shaderFile.GetSpirV();
        var shader = rd.ShaderCreateFromSpirV(shaderBytecode);

        // Prepare our data. We use floats in the shader, so we need 32 bit.
        var input = new float[textureRes * textureRes];
        for (int i = 0; i < textureRes * textureRes; i++)
        {
            float x = i % textureRes;
            float y = (i - x) / textureRes;
            input[i] = x + y * textureRes;
        }
        var inputBytes = new byte[sizeof(uint) + input.Length * sizeof(float)];
        Buffer.BlockCopy(new uint[] { textureRes }, 0, inputBytes, 0, sizeof(uint));
        Buffer.BlockCopy(input, 0, inputBytes, sizeof(uint), inputBytes.Length - sizeof(uint));

        // Create a storage buffer that can hold our float values.
        // Each float has 4 bytes (32 bit) so 10 x 4 = 40 bytes
        var height_buffer = rd.StorageBufferCreate((uint)inputBytes.Length, inputBytes);

        // Create a uniform to assign the buffer to the rendering device
        var height_uniform = new RDUniform
        {
            UniformType = RenderingDevice.UniformType.StorageBuffer,
            Binding = 0
        };
        height_uniform.AddId(height_buffer);

        // var img = noise_text.GetImage();
        // img.Convert(Image.Format.L8);
        // byte[] img_values = new byte[sizeof(uint) + noise_text.GetWidth() * noise_text.GetHeight()];
        // for (int i = 0; i < noise_text.GetWidth() * noise_text.GetHeight(); i++)
        // {
        //     img_values[i + 1] = img.GetData()[i];
        // }

        // var inputValues = new byte[sizeof(uint) + img_values.Length * sizeof(float)]
        // Buffer.BlockCopy(new uint[] { (uint)noise_text.GetWidth() }, 0, inputValues, 0, sizeof(uint));
        // Buffer.BlockCopy(img_values, 0, inputValues, sizeof(uint), inputValues.Length - sizeof(uint));

        // var noise_buffer = rd.StorageBufferCreate((uint)inputValues.Length, inputValues);
        // var noise_uniform = new RDUniform
        // {
        //     UniformType = RenderingDevice.UniformType.StorageBuffer,
        //     Binding = 1
        // };
        // noise_uniform.AddId(noise_buffer);

        var uniformSet = rd.UniformSetCreate(new Godot.Collections.Array<RDUniform> { height_uniform }, shader, 0);

        // Create a compute pipeline
        var pipeline = rd.ComputePipelineCreate(shader);
        var computeList = rd.ComputeListBegin();
        rd.ComputeListBindComputePipeline(computeList, pipeline);
        rd.ComputeListBindUniformSet(computeList, uniformSet, 0);
        rd.ComputeListDispatch(computeList, xGroups: textureRes / 32, yGroups: textureRes / 32, zGroups: 1);
        rd.ComputeListEnd();

        // Submit to GPU and wait for sync
        rd.Submit();
        rd.Sync();

        // Read back the data from the buffers

        var outputBytes = rd.BufferGetData(height_buffer);
        var output = new float[textureRes * textureRes];
        Buffer.BlockCopy(outputBytes, sizeof(uint), output, 0, outputBytes.Length - sizeof(uint));
        // GD.Print("Input: ", string.Join(',', input));
        // GD.Print("Output: ", string.Join(',', output));
    }
}
