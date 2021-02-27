﻿using SPICA.Serialization;
using SPICA.Serialization.Attributes;

namespace SPICA.Formats.CtrH3D.Animation
{
    public class H3DAnimVector3D : ICustomSerialization
    {
        [Ignore] private readonly H3DFloatKeyFrameGroup[] Vector;

        public H3DAnimVector3D ()
        {
            Vector = new[] {
                new H3DFloatKeyFrameGroup (),
                new H3DFloatKeyFrameGroup (),
                new H3DFloatKeyFrameGroup ()
            };
        }

        public H3DFloatKeyFrameGroup X => Vector[0];
        public H3DFloatKeyFrameGroup Y => Vector[1];
        public H3DFloatKeyFrameGroup Z => Vector[2];

        void ICustomSerialization.Deserialize (BinaryDeserializer Deserializer)
        {
            H3DAnimVector.SetVector (Deserializer, Vector);
        }

        bool ICustomSerialization.Serialize (BinarySerializer Serializer)
        {
            H3DAnimVector.WriteVector (Serializer, Vector);

            return true;
        }
    }
}