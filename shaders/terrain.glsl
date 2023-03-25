#[compute]
#version 450

// Invocations in the (x, y, z) dimension
layout(local_size_x = 32, local_size_y = 32, local_size_z = 1) in;

// A binding to the buffer we create in our script
layout(set = 0, binding = 0, std430) restrict buffer DataBuffer {
    uint size;
    float data[];
}
height_buffer;

// The code we want to execute in each invocation
void main() {
    vec2 UV = vec2(gl_GlobalInvocationID.x, gl_GlobalInvocationID.y) / vec2(height_buffer.size);

    height_buffer.data[gl_GlobalInvocationID.x + height_buffer.size * gl_GlobalInvocationID.y] = gl_GlobalInvocationID.x + height_buffer.size * gl_GlobalInvocationID.y;
}