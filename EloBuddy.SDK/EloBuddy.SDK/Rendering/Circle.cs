﻿using System;
using System.Linq;
using SharpDX;
using SharpDX.Direct3D9;
using Color = System.Drawing.Color;

namespace EloBuddy.SDK.Rendering
{
    public sealed class Circle
    {
        internal const float DefaultBorderWidth = 1;

        internal static Effect Effect { get; set; }
        internal static EffectHandle Technique { get; set; }
        internal static VertexBuffer VertexBuffer { get; set; }
        internal static VertexElement[] VertexElements { get; set; }
        internal static VertexDeclaration VertexDeclaration { get; set; }

        static Circle()
        {
            #region Vertex init

            const float x = 6500;
            VertexBuffer = new VertexBuffer(Drawing.Direct3DDevice, Utilities.SizeOf<Vector4>() * 6,
                Usage.WriteOnly, VertexFormat.None, Pool.Managed);
            VertexBuffer.Lock(0, 0, LockFlags.None).WriteRange(new[]
            {
                // Triangle 1
                new Vector4(-x, 0f, -x, 1.0f),
                new Vector4(-x, 0f, x, 1.0f),
                new Vector4(x, 0f, -x, 1.0f),

                // Triangle 2
                new Vector4(x, 0f, x, 1.0f),
                new Vector4(-x, 0f, x, 1.0f),
                new Vector4(x, 0f, -x, 1.0f)
            });
            VertexBuffer.Unlock();

            VertexElements = new[]
            {
                new VertexElement(0, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, 16, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                VertexElement.VertexDeclarationEnd
            };

            VertexDeclaration = new VertexDeclaration(Drawing.Direct3DDevice, VertexElements);

            #endregion

            #region Effect init

            #region Effect source

            /*
            const string effectSource = @"
                struct VS_OUTPUT
                {
                    float4 Position   : POSITION;
                    float4 Color      : COLOR0;
                    float4 Position3D : TEXCOORD0;
                };

                // Constants
                const float ANTIALIAS_WIDTH = 2;

                // Globals passed
                float4x4 ProjectionMatrix;
                float4 Color;
                float Radius;
                float Width;
                bool Filled;
                bool EnableZ;

                // Vertex Shader
                VS_OUTPUT VS(VS_OUTPUT input)
                {
                    VS_OUTPUT output = (VS_OUTPUT) 0;
	
                    output.Position = mul(input.Position, ProjectionMatrix);
                    output.Color = input.Color;
                    output.Position3D = input.Position;

                    return output;
                }

                // Pixel Shader
                float4 PS(VS_OUTPUT input) : COLOR
                {
                    VS_OUTPUT output = (VS_OUTPUT) 0;

                    output = input;
                    output.Color.x = Color.x;
                    output.Color.y = Color.y;
                    output.Color.z = Color.z;
                    output.Color.w = 0;

                    float4 v = output.Position3D;
                    float distanceSquared = abs(v.x * v.x + v.z * v.z);

                    // Check if the distance is within our max range
                    if (distanceSquared > pow(Radius + Width / 2 + ANTIALIAS_WIDTH, 2))
                    {
                        return output.Color; 
                    }

                    float distance = sqrt(distanceSquared);
                    float edgeDistance = abs(Radius - distance);

                    if(Filled && distance < Radius + Width / 2)
                    {
                        // Drawing the pure circle color to fill the circle
                        output.Color.w = Color.w;
                    }
                    else if(!Filled && edgeDistance < Width / 2)
                    {
                        // Drawing the pure circle color
                        output.Color.w = Color.w;
                    }
                    else if (edgeDistance < Width / 2 + ANTIALIAS_WIDTH)
                    {
                        // Drawing antialias
                        output.Color.w = Color.w * (((Width / 2 + ANTIALIAS_WIDTH) - edgeDistance) / ANTIALIAS_WIDTH);
                    }

                    return output.Color;
                }

                technique Main
                {
                    pass P0
                    {
                        ZEnable = EnableZ;
                        AlphaBlendEnable = TRUE;
                        DestBlend = INVSRCALPHA;
                        SrcBlend = SRCALPHA;
                        VertexShader = compile vs_2_0 VS();
                        PixelShader  = compile ps_2_0 PS();
                    }
                }";
            */

            #endregion

            #region Effect compiling

            /*
            try
            {
                // Compile source
                using (
                    var dataStream =
                        new EffectCompiler(effectSource, null, null, ShaderFlags.None).CompileEffect(ShaderFlags.None))
                {
                    byte[] buffer = new byte[16 * 1024];
                    using (var memoryStream = new System.IO.MemoryStream())
                    {
                        int read;
                        while ((read = dataStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            memoryStream.Write(buffer, 0, read);
                        }

                        var sb = new StringBuilder();
                        sb.Append("var compiledEffect = new byte[] { ");
                        foreach (byte b in memoryStream.ToArray())
                        {
                            sb.AppendFormat("0x{0:x2}, ", b);
                        }
                        sb.Append("};");
                        System.Windows.Forms.Clipboard.SetText(sb.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not compile Effect:\n{0}", e);
            }
            */

            #endregion

            #region Effect binary

            var compiledEffect = new byte[]
            {
                0x01, 0x09, 0xff, 0xfe, 0x58, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x10, 0x00, 0x00, 0x00, 0x41, 0x4e, 0x54,
                0x49, 0x41, 0x4c, 0x49, 0x41, 0x53, 0x5f, 0x57, 0x49, 0x44, 0x54, 0x48, 0x00, 0x03, 0x00, 0x00, 0x00,
                0x02, 0x00, 0x00, 0x00, 0x94, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04,
                0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x11, 0x00, 0x00, 0x00, 0x50, 0x72, 0x6f, 0x6a, 0x65, 0x63, 0x74, 0x69, 0x6f, 0x6e,
                0x4d, 0x61, 0x74, 0x72, 0x69, 0x78, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00,
                0x00, 0xd8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x43, 0x6f, 0x6c, 0x6f, 0x72, 0x00, 0x00, 0x00, 0x03, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00,
                0x52, 0x61, 0x64, 0x69, 0x75, 0x73, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x30,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x57, 0x69, 0x64, 0x74, 0x68, 0x00, 0x00,
                0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x5c, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07,
                0x00, 0x00, 0x00, 0x46, 0x69, 0x6c, 0x6c, 0x65, 0x64, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x88, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00,
                0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x45, 0x6e, 0x61, 0x62,
                0x6c, 0x65, 0x5a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                0x06, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x05, 0x00,
                0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                0x10, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x0f, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x50, 0x30, 0x00,
                0x00, 0x05, 0x00, 0x00, 0x00, 0x4d, 0x61, 0x69, 0x6e, 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x20,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x38, 0x00, 0x00, 0x00, 0x54, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xac, 0x00, 0x00, 0x00, 0xc8, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xe4, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x01, 0x00, 0x00, 0x2c, 0x01, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3c, 0x01, 0x00, 0x00, 0x58, 0x01, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x68, 0x01, 0x00, 0x00, 0x84, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x4c, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                0x44, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x98, 0x01, 0x00, 0x00, 0x94, 0x01, 0x00, 0x00, 0x0d, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0xb8, 0x01, 0x00, 0x00, 0xb4, 0x01, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0xd8, 0x01, 0x00, 0x00, 0xd4, 0x01, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0xf8, 0x01, 0x00, 0x00, 0xf4, 0x01, 0x00, 0x00, 0x92, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x18,
                0x02, 0x00, 0x00, 0x14, 0x02, 0x00, 0x00, 0x93, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x30, 0x02,
                0x00, 0x00, 0x2c, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x24, 0x06, 0x00, 0x00, 0x00, 0x02, 0xff, 0xff, 0xfe, 0xff, 0x3b, 0x00, 0x43, 0x54, 0x41, 0x42, 0x1c,
                0x00, 0x00, 0x00, 0xb7, 0x00, 0x00, 0x00, 0x00, 0x02, 0xff, 0xff, 0x03, 0x00, 0x00, 0x00, 0x1c, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0xb0, 0x00, 0x00, 0x00, 0x58, 0x00, 0x00, 0x00, 0x02, 0x00, 0x06,
                0x00, 0x01, 0x00, 0x00, 0x00, 0x60, 0x00, 0x00, 0x00, 0x70, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00,
                0x02, 0x00, 0x08, 0x00, 0x01, 0x00, 0x00, 0x00, 0x88, 0x00, 0x00, 0x00, 0x70, 0x00, 0x00, 0x00, 0x98,
                0x00, 0x00, 0x00, 0x02, 0x00, 0x07, 0x00, 0x01, 0x00, 0x00, 0x00, 0xa0, 0x00, 0x00, 0x00, 0x70, 0x00,
                0x00, 0x00, 0x43, 0x6f, 0x6c, 0x6f, 0x72, 0x00, 0xab, 0xab, 0x01, 0x00, 0x03, 0x00, 0x01, 0x00, 0x04,
                0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x46, 0x69, 0x6c, 0x6c, 0x65, 0x64, 0x00, 0xab, 0x00,
                0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x52, 0x61,
                0x64, 0x69, 0x75, 0x73, 0x00, 0xab, 0x00, 0x00, 0x03, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x70, 0x73, 0x5f, 0x32, 0x5f, 0x30, 0x00, 0x4d, 0x69, 0x63, 0x72, 0x6f,
                0x73, 0x6f, 0x66, 0x74, 0x20, 0x28, 0x52, 0x29, 0x20, 0x48, 0x4c, 0x53, 0x4c, 0x20, 0x53, 0x68, 0x61,
                0x64, 0x65, 0x72, 0x20, 0x43, 0x6f, 0x6d, 0x70, 0x69, 0x6c, 0x65, 0x72, 0x20, 0x39, 0x2e, 0x32, 0x39,
                0x2e, 0x39, 0x35, 0x32, 0x2e, 0x33, 0x31, 0x31, 0x31, 0x00, 0xfe, 0xff, 0xe3, 0x00, 0x50, 0x52, 0x45,
                0x53, 0x01, 0x02, 0x58, 0x46, 0xfe, 0xff, 0x4b, 0x00, 0x43, 0x54, 0x41, 0x42, 0x1c, 0x00, 0x00, 0x00,
                0xf7, 0x00, 0x00, 0x00, 0x01, 0x02, 0x58, 0x46, 0x04, 0x00, 0x00, 0x00, 0x1c, 0x00, 0x00, 0x00, 0x00,
                0x01, 0x00, 0x20, 0xf4, 0x00, 0x00, 0x00, 0x6c, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x7c, 0x00, 0x00, 0x00, 0x8c, 0x00, 0x00, 0x00, 0x9c, 0x00, 0x00, 0x00, 0x02, 0x00, 0x01,
                0x00, 0x01, 0x00, 0x00, 0x00, 0xa4, 0x00, 0x00, 0x00, 0xb4, 0x00, 0x00, 0x00, 0xc4, 0x00, 0x00, 0x00,
                0x02, 0x00, 0x02, 0x00, 0x01, 0x00, 0x00, 0x00, 0xcc, 0x00, 0x00, 0x00, 0xb4, 0x00, 0x00, 0x00, 0xdc,
                0x00, 0x00, 0x00, 0x02, 0x00, 0x03, 0x00, 0x01, 0x00, 0x00, 0x00, 0xe4, 0x00, 0x00, 0x00, 0xb4, 0x00,
                0x00, 0x00, 0x41, 0x4e, 0x54, 0x49, 0x41, 0x4c, 0x49, 0x41, 0x53, 0x5f, 0x57, 0x49, 0x44, 0x54, 0x48,
                0x00, 0x00, 0x00, 0x03, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x43,
                0x6f, 0x6c, 0x6f, 0x72, 0x00, 0xab, 0xab, 0x01, 0x00, 0x03, 0x00, 0x01, 0x00, 0x04, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x52, 0x61, 0x64, 0x69, 0x75, 0x73, 0x00, 0xab, 0x00, 0x00, 0x03, 0x00,
                0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x57, 0x69, 0x64, 0x74, 0x68,
                0x00, 0xab, 0xab, 0x00, 0x00, 0x03, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x74, 0x78, 0x00, 0x4d, 0x69, 0x63, 0x72, 0x6f, 0x73, 0x6f, 0x66, 0x74, 0x20, 0x28, 0x52,
                0x29, 0x20, 0x48, 0x4c, 0x53, 0x4c, 0x20, 0x53, 0x68, 0x61, 0x64, 0x65, 0x72, 0x20, 0x43, 0x6f, 0x6d,
                0x70, 0x69, 0x6c, 0x65, 0x72, 0x20, 0x39, 0x2e, 0x32, 0x39, 0x2e, 0x39, 0x35, 0x32, 0x2e, 0x33, 0x31,
                0x31, 0x31, 0x00, 0xfe, 0xff, 0x0c, 0x00, 0x50, 0x52, 0x53, 0x49, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0xfe, 0xff, 0x2a, 0x00, 0x43, 0x4c, 0x49, 0x54, 0x14, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xe0, 0x3f, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xfe, 0xff, 0x5b, 0x00, 0x46, 0x58, 0x4c, 0x43, 0x09, 0x00, 0x00,
                0x00, 0x01, 0x00, 0x50, 0xa0, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
                0x0c, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x40, 0xa0, 0x02, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x02, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00,
                0x04, 0x00, 0x00, 0x00, 0x01, 0x00, 0x40, 0xa0, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07,
                0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00,
                0x10, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x01, 0x00, 0x50, 0xa0, 0x02,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x40, 0xa0, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x0c, 0x00, 0x00, 0x00, 0x01, 0x00,
                0x00, 0x10, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x01, 0x00, 0x30, 0x10,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x10, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x04, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0xf0, 0xf0, 0xf0, 0xf0, 0x0f, 0x0f, 0x0f, 0x0f,
                0xff, 0xff, 0x00, 0x00, 0x51, 0x00, 0x00, 0x05, 0x09, 0x00, 0x0f, 0xa0, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x80, 0x00, 0x00, 0x80, 0xbf, 0x00, 0x00, 0x00, 0x00, 0x1f, 0x00, 0x00, 0x02, 0x00, 0x00,
                0x00, 0x80, 0x00, 0x00, 0x07, 0xb0, 0x05, 0x00, 0x00, 0x03, 0x00, 0x00, 0x08, 0x80, 0x00, 0x00, 0xaa,
                0xb0, 0x00, 0x00, 0xaa, 0xb0, 0x04, 0x00, 0x00, 0x04, 0x00, 0x00, 0x01, 0x80, 0x00, 0x00, 0x00, 0xb0,
                0x00, 0x00, 0x00, 0xb0, 0x00, 0x00, 0xff, 0x80, 0x07, 0x00, 0x00, 0x02, 0x00, 0x00, 0x02, 0x80, 0x00,
                0x00, 0x00, 0x80, 0x02, 0x00, 0x00, 0x03, 0x00, 0x00, 0x01, 0x80, 0x00, 0x00, 0x00, 0x81, 0x00, 0x00,
                0x00, 0xa0, 0x06, 0x00, 0x00, 0x02, 0x00, 0x00, 0x02, 0x80, 0x00, 0x00, 0x55, 0x80, 0x02, 0x00, 0x00,
                0x03, 0x00, 0x00, 0x04, 0x80, 0x00, 0x00, 0x55, 0x81, 0x07, 0x00, 0x00, 0xa0, 0x02, 0x00, 0x00, 0x03,
                0x00, 0x00, 0x02, 0x80, 0x00, 0x00, 0x55, 0x80, 0x01, 0x00, 0x00, 0xa1, 0x01, 0x00, 0x00, 0x02, 0x00,
                0x00, 0x08, 0x80, 0x09, 0x00, 0x55, 0xa0, 0x58, 0x00, 0x00, 0x04, 0x00, 0x00, 0x02, 0x80, 0x00, 0x00,
                0x55, 0x80, 0x00, 0x00, 0xff, 0x80, 0x08, 0x00, 0x00, 0xa1, 0x23, 0x00, 0x00, 0x02, 0x00, 0x00, 0x04,
                0x80, 0x00, 0x00, 0xaa, 0x80, 0x02, 0x00, 0x00, 0x03, 0x01, 0x00, 0x08, 0x80, 0x00, 0x00, 0xaa, 0x81,
                0x03, 0x00, 0x00, 0xa0, 0x05, 0x00, 0x00, 0x03, 0x01, 0x00, 0x01, 0x80, 0x01, 0x00, 0xff, 0x80, 0x04,
                0x00, 0x00, 0xa0, 0x05, 0x00, 0x00, 0x03, 0x01, 0x00, 0x01, 0x80, 0x01, 0x00, 0x00, 0x80, 0x06, 0x00,
                0xff, 0xa0, 0x02, 0x00, 0x00, 0x03, 0x01, 0x00, 0x02, 0x80, 0x00, 0x00, 0xaa, 0x80, 0x03, 0x00, 0x00,
                0xa1, 0x02, 0x00, 0x00, 0x03, 0x00, 0x00, 0x04, 0x80, 0x00, 0x00, 0xaa, 0x80, 0x02, 0x00, 0x00, 0xa1,
                0x58, 0x00, 0x00, 0x04, 0x00, 0x00, 0x04, 0x80, 0x00, 0x00, 0xaa, 0x80, 0x09, 0x00, 0x55, 0xa0, 0x09,
                0x00, 0xaa, 0xa0, 0x58, 0x00, 0x00, 0x04, 0x00, 0x00, 0x04, 0x80, 0x08, 0x00, 0x00, 0xa1, 0x00, 0x00,
                0xaa, 0x80, 0x00, 0x00, 0xff, 0x80, 0x58, 0x00, 0x00, 0x04, 0x00, 0x00, 0x08, 0x80, 0x01, 0x00, 0x55,
                0x80, 0x09, 0x00, 0x00, 0xa0, 0x01, 0x00, 0x00, 0x80, 0x58, 0x00, 0x00, 0x04, 0x00, 0x00, 0x04, 0x80,
                0x00, 0x00, 0xaa, 0x80, 0x00, 0x00, 0xff, 0x80, 0x06, 0x00, 0xff, 0xa0, 0x58, 0x00, 0x00, 0x04, 0x00,
                0x00, 0x02, 0x80, 0x00, 0x00, 0x55, 0x80, 0x00, 0x00, 0xaa, 0x80, 0x06, 0x00, 0xff, 0xa0, 0x58, 0x00,
                0x00, 0x04, 0x00, 0x00, 0x08, 0x80, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x55, 0x80, 0x09, 0x00, 0x00,
                0xa0, 0x01, 0x00, 0x00, 0x02, 0x00, 0x00, 0x07, 0x80, 0x05, 0x00, 0xe4, 0xa0, 0x01, 0x00, 0x00, 0x02,
                0x00, 0x08, 0x0f, 0x80, 0x00, 0x00, 0xe4, 0x80, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4c, 0x01,
                0x00, 0x00, 0x00, 0x02, 0xfe, 0xff, 0xfe, 0xff, 0x34, 0x00, 0x43, 0x54, 0x41, 0x42, 0x1c, 0x00, 0x00,
                0x00, 0x9b, 0x00, 0x00, 0x00, 0x00, 0x02, 0xfe, 0xff, 0x01, 0x00, 0x00, 0x00, 0x1c, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x20, 0x94, 0x00, 0x00, 0x00, 0x30, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x04,
                0x00, 0x00, 0x00, 0x44, 0x00, 0x00, 0x00, 0x54, 0x00, 0x00, 0x00, 0x50, 0x72, 0x6f, 0x6a, 0x65, 0x63,
                0x74, 0x69, 0x6f, 0x6e, 0x4d, 0x61, 0x74, 0x72, 0x69, 0x78, 0x00, 0xab, 0xab, 0xab, 0x03, 0x00, 0x03,
                0x00, 0x04, 0x00, 0x04, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x76, 0x73, 0x5f, 0x32, 0x5f, 0x30, 0x00, 0x4d,
                0x69, 0x63, 0x72, 0x6f, 0x73, 0x6f, 0x66, 0x74, 0x20, 0x28, 0x52, 0x29, 0x20, 0x48, 0x4c, 0x53, 0x4c,
                0x20, 0x53, 0x68, 0x61, 0x64, 0x65, 0x72, 0x20, 0x43, 0x6f, 0x6d, 0x70, 0x69, 0x6c, 0x65, 0x72, 0x20,
                0x39, 0x2e, 0x32, 0x39, 0x2e, 0x39, 0x35, 0x32, 0x2e, 0x33, 0x31, 0x31, 0x31, 0x00, 0x1f, 0x00, 0x00,
                0x02, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x0f, 0x90, 0x1f, 0x00, 0x00, 0x02, 0x0a, 0x00, 0x00, 0x80,
                0x01, 0x00, 0x0f, 0x90, 0x09, 0x00, 0x00, 0x03, 0x00, 0x00, 0x01, 0xc0, 0x00, 0x00, 0xe4, 0x90, 0x00,
                0x00, 0xe4, 0xa0, 0x09, 0x00, 0x00, 0x03, 0x00, 0x00, 0x02, 0xc0, 0x00, 0x00, 0xe4, 0x90, 0x01, 0x00,
                0xe4, 0xa0, 0x09, 0x00, 0x00, 0x03, 0x00, 0x00, 0x04, 0xc0, 0x00, 0x00, 0xe4, 0x90, 0x02, 0x00, 0xe4,
                0xa0, 0x09, 0x00, 0x00, 0x03, 0x00, 0x00, 0x08, 0xc0, 0x00, 0x00, 0xe4, 0x90, 0x03, 0x00, 0xe4, 0xa0,
                0x01, 0x00, 0x00, 0x02, 0x00, 0x00, 0x0f, 0xd0, 0x01, 0x00, 0xe4, 0x90, 0x01, 0x00, 0x00, 0x02, 0x00,
                0x00, 0x0f, 0xe0, 0x00, 0x00, 0xe4, 0x90, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0xff, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xdc, 0x00, 0x00,
                0x00, 0x00, 0x02, 0x58, 0x46, 0xfe, 0xff, 0x24, 0x00, 0x43, 0x54, 0x41, 0x42, 0x1c, 0x00, 0x00, 0x00,
                0x5b, 0x00, 0x00, 0x00, 0x00, 0x02, 0x58, 0x46, 0x01, 0x00, 0x00, 0x00, 0x1c, 0x00, 0x00, 0x00, 0x00,
                0x01, 0x00, 0x20, 0x58, 0x00, 0x00, 0x00, 0x30, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x38, 0x00, 0x00, 0x00, 0x48, 0x00, 0x00, 0x00, 0x45, 0x6e, 0x61, 0x62, 0x6c, 0x65, 0x5a,
                0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x74,
                0x78, 0x00, 0x4d, 0x69, 0x63, 0x72, 0x6f, 0x73, 0x6f, 0x66, 0x74, 0x20, 0x28, 0x52, 0x29, 0x20, 0x48,
                0x4c, 0x53, 0x4c, 0x20, 0x53, 0x68, 0x61, 0x64, 0x65, 0x72, 0x20, 0x43, 0x6f, 0x6d, 0x70, 0x69, 0x6c,
                0x65, 0x72, 0x20, 0x39, 0x2e, 0x32, 0x39, 0x2e, 0x39, 0x35, 0x32, 0x2e, 0x33, 0x31, 0x31, 0x31, 0x00,
                0xfe, 0xff, 0x02, 0x00, 0x43, 0x4c, 0x49, 0x54, 0x00, 0x00, 0x00, 0x00, 0xfe, 0xff, 0x0c, 0x00, 0x46,
                0x58, 0x4c, 0x43, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x10, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0xf0, 0xf0, 0xf0, 0xf0, 0x0f, 0x0f, 0x0f, 0x0f, 0xff, 0xff, 0x00, 0x00
            };

            #endregion

            //Effect = Effect.FromString(Drawing.Direct3DDevice, effectSource, ShaderFlags.None);
            Effect = Effect.FromMemory(Drawing.Direct3DDevice, compiledEffect, ShaderFlags.None);

            Technique = Effect.GetTechnique(0);

            #endregion

            // Listen to required events
            AppDomain.CurrentDomain.DomainUnload += delegate { OnDispose(); };
            AppDomain.CurrentDomain.ProcessExit += delegate { OnDispose(); };
            Drawing.OnPreReset += e => Effect.OnLostDevice();
            Drawing.OnPostReset += e => Effect.OnResetDevice();

            // For static drawing
            CircleHandle = new Circle();
        }

        internal static Circle CircleHandle { get; set; }

        public static void Draw(ColorBGRA color, float radius, params GameObject[] objects)
        {
            Draw(color, radius, DefaultBorderWidth, objects);
        }

        public static void Draw(ColorBGRA color, float radius, float borderWidth = DefaultBorderWidth, params GameObject[] objects)
        {
            Draw(color, radius, borderWidth, objects.Select(o => new Vector3(o.Position.X, o.Position.Y, NavMesh.GetHeightForPosition(o.Position.X, o.Position.Y))).ToArray());
        }

        public static void Draw(ColorBGRA color, float radius, params Vector3[] positions)
        {
            Draw(color, radius, DefaultBorderWidth, positions);
        }

        public static void Draw(ColorBGRA color, float radius, float borderWidth = DefaultBorderWidth, params Vector3[] positions)
        {
            CircleHandle.ColorBGRA = color;
            CircleHandle.Radius = radius;
            CircleHandle.BorderWidth = borderWidth;
            CircleHandle.Draw(positions);
        }

        internal static void OnDispose()
        {
            if (Effect != null)
            {
                Effect.Dispose();
                Effect = null;
            }
            if (Technique != null)
            {
                Technique.Dispose();
                Technique = null;
            }
            if (VertexBuffer != null)
            {
                VertexBuffer.Dispose();
                VertexBuffer = null;
            }
            if (VertexDeclaration != null)
            {
                VertexDeclaration.Dispose();
                VertexDeclaration = null;
            }
        }

        public Color Color
        {
            get { return Color.FromArgb(ColorBGRA.A, ColorBGRA.R, ColorBGRA.G, ColorBGRA.B); }
            set { ColorBGRA = new ColorBGRA(value.R, value.G, value.B, value.A); }
        }
        public ColorBGRA ColorBGRA { get; set; }

        public bool Filled { get; set; }
        public float Radius { get; set; }
        public float BorderWidth { get; set; }

        internal Vector4 VectorColor
        {
            get { return new Vector4(ColorBGRA.R / 255f, ColorBGRA.G / 255f, ColorBGRA.B / 255f, ColorBGRA.A / 255f); }
        }

        public Circle() : this(SharpDX.Color.Moccasin, 300)
        {
        }

        public Circle(ColorBGRA color, float radius, float borderWidth = DefaultBorderWidth, bool filled = false)
        {
            // Initialize properties
            ColorBGRA = color;
            Filled = filled;
            Radius = radius;
            BorderWidth = borderWidth;
        }

        public void Draw(params Vector3[] positions)
        {
            // Validation
            if (positions.Length == 0 || Effect == null || Effect.IsDisposed || BorderWidth <= 0)
            {
                return;
            }

            // End all current drawings
            Core.EndAllDrawing();

            // Store current declatation
            var declaration = Drawing.Direct3DDevice.VertexDeclaration;

            // Begin drawing
            Effect.Technique = Technique;
            Effect.Begin();

            // Draw circle(s)
            foreach (var pos in positions)
            {
                // Pass value to the pixel shader
                Effect.BeginPass(0);
                Effect.SetValue("ProjectionMatrix", Matrix.Translation(new Vector3(pos.X, pos.Z, pos.Y)) * Drawing.View * Drawing.Projection);
                Effect.SetValue("Color", VectorColor);
                Effect.SetValue("Radius", Radius);
                Effect.SetValue("Width", BorderWidth);
                Effect.SetValue("Filled", Filled);
                Effect.SetValue("EnableZ", false);
                Effect.EndPass();

                Drawing.Direct3DDevice.SetStreamSource(0, VertexBuffer, 0, Utilities.SizeOf<Vector4>());
                Drawing.Direct3DDevice.VertexDeclaration = VertexDeclaration;

                // Draw the circle
                Drawing.Direct3DDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            }

            // End drawing
            Effect.End();

            // Restore declaration
            Drawing.Direct3DDevice.VertexDeclaration = declaration;
        }
    }
}