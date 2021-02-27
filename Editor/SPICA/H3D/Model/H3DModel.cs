﻿using System;
using System.Collections.Generic;
using SPICA.Formats.Common;
using SPICA.Formats.CtrH3D.Model.Material;
using SPICA.Formats.CtrH3D.Model.Mesh;
using SPICA.Math3D;
using SPICA.Serialization;
using SPICA.Serialization.Attributes;

namespace SPICA.Formats.CtrH3D.Model
{
    public class H3DModel : INamed
    {
        public readonly H3DDict<H3DMaterial> Materials;

        public readonly List<H3DMesh> Meshes;

        [Range] public readonly List<H3DMesh> MeshesLayer0;
        [Range] public readonly List<H3DMesh> MeshesLayer1;
        [Range] public readonly List<H3DMesh> MeshesLayer2;
        [Range] public readonly List<H3DMesh> MeshesLayer3;

        public readonly List<bool> MeshNodesVisibility;

        public readonly H3DDict<H3DBone> Skeleton;

        [IfVersion (CmpOp.Gequal, 7)] public readonly List<H3DSubMeshCulling> SubMeshCullings;

        public H3DBoneScaling BoneScaling;
        public H3DModelFlags Flags;

        public int MeshNodesCount;

        public H3DPatriciaTree MeshNodesTree;

        public H3DMetaData MetaData;

        public ushort SilhouetteMaterialsCount;
        
        public Matrix3x4 WorldTransform;

        public H3DModel ()
        {
            WorldTransform = new Matrix3x4 ();

            Materials = new H3DDict<H3DMaterial> ();

            Meshes = new List<H3DMesh> ();

            MeshesLayer0 = new List<H3DMesh> ();
            MeshesLayer1 = new List<H3DMesh> ();
            MeshesLayer2 = new List<H3DMesh> ();
            MeshesLayer3 = new List<H3DMesh> ();

            SubMeshCullings = new List<H3DSubMeshCulling> ();

            Skeleton = new H3DDict<H3DBone> ();

            MeshNodesVisibility = new List<bool> ();

        }

        public string Name { get; set; }

        public void AddMesh (H3DMesh Mesh)
        {
            Mesh.Parent = this;

            Meshes.Add (Mesh);

            switch (Mesh.Layer) {
                case 0:
                    MeshesLayer0.Add (Mesh);
                    break;
                case 1:
                    MeshesLayer1.Add (Mesh);
                    break;
                case 2:
                    MeshesLayer2.Add (Mesh);
                    break;
                case 3:
                    MeshesLayer3.Add (Mesh);
                    break;

                default: throw new ArgumentOutOfRangeException ("Invalid Layer! Expected 0, 1, 2 or 3!");
            }
        }

        public void AddMeshes (IEnumerable<H3DMesh> Meshes)
        {
            foreach (var Mesh in Meshes) AddMesh (Mesh);
        }

        public void AddMeshes (params H3DMesh[] Meshes)
        {
            foreach (var Mesh in Meshes) AddMesh (Mesh);
        }

        public void RemoveMesh (H3DMesh Mesh)
        {
            if (Meshes.Remove (Mesh)) {
                MeshesLayer0.Remove (Mesh);
                MeshesLayer1.Remove (Mesh);
                MeshesLayer2.Remove (Mesh);
                MeshesLayer3.Remove (Mesh);
            }
        }

        public void ClearMeshes ()
        {
            Meshes.Clear ();

            MeshesLayer0.Clear ();
            MeshesLayer1.Clear ();
            MeshesLayer2.Clear ();
            MeshesLayer3.Clear ();
        }
    }
}