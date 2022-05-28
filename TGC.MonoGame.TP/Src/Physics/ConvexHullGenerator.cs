﻿using BepuPhysics.Collidables;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Numerics;

namespace TGC.MonoGame.TP.Physics
{
    internal static class ConvexHullGenerator
    {
        internal static ConvexHull Generate(Model model, float scale)
        {
            int vertexCount = GetVertexCount(model);
            TGCGame.PhysicsSimulation.BufferPool.Take<Vector3>(vertexCount, out var points);

            int pointIndex = 0;
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    Vector3[] vertices = GetVertexElement(meshPart, VertexElementUsage.Position);
                    foreach (Vector3 vertex in vertices)
                    {
                        points[pointIndex] = vertex * scale;
                        pointIndex++;
                    }
                }
            }
            
            return new ConvexHull(points, TGCGame.PhysicsSimulation.BufferPool, out _);
        }

        private static int GetVertexCount(Model model)
        {
            int vertexCount = 0;
            foreach (ModelMesh mesh in model.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    vertexCount += meshPart.NumVertices;
            return vertexCount;
        }

        private static Vector3[] GetVertexElement(ModelMeshPart meshPart, VertexElementUsage usage)
        {
            VertexDeclaration vd = meshPart.VertexBuffer.VertexDeclaration;
            VertexElement[] elements = vd.GetVertexElements();

            bool elementPredicate(VertexElement ve) => ve.VertexElementUsage == usage && ve.VertexElementFormat == VertexElementFormat.Vector3;
            if (!elements.Any(elementPredicate))
                return null;

            VertexElement element = elements.First(elementPredicate);

            Vector3[] vertexData = new Vector3[meshPart.NumVertices];
            meshPart.VertexBuffer.GetData((meshPart.VertexOffset * vd.VertexStride) + element.Offset,
                vertexData, 0, vertexData.Length, vd.VertexStride);

            return vertexData;
        }
    }
}