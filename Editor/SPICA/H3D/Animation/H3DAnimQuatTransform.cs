﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using SPICA.Math3D;
using SPICA.Serialization;
using SPICA.Serialization.Attributes;
using SPICA.Serialization.Serializer;

namespace SPICA.Formats.CtrH3D.Animation
{
    public class H3DAnimQuatTransform : ICustomSerialization
    {
        [Ignore] public readonly List<Quaternion> Rotations;

        [Ignore] public readonly List<Vector3> Scales;
        [Ignore] public readonly List<Vector3> Translations;
        private H3DAnimQuatTransformFlags Flags;

        public H3DAnimQuatTransform ()
        {
            Scales = new List<Vector3> ();
            Rotations = new List<Quaternion> ();
            Translations = new List<Vector3> ();
        }

        public bool HasScale => Scales.Count > 0;
        public bool HasRotation => Rotations.Count > 0;
        public bool HasTranslation => Translations.Count > 0;

        void ICustomSerialization.Deserialize (BinaryDeserializer Deserializer)
        {
            var Addresses = new uint[3];

            var ConstantMask = (uint) H3DAnimQuatTransformFlags.IsScaleConstant;
            var NotExistMask = (uint) H3DAnimQuatTransformFlags.IsScaleInexistent;

            Addresses[0] = Deserializer.Reader.ReadUInt32 ();
            Addresses[1] = Deserializer.Reader.ReadUInt32 ();
            Addresses[2] = Deserializer.Reader.ReadUInt32 ();

            for (var ElemIndex = 0; ElemIndex < 3; ElemIndex++) {
                var Constant = ((uint) Flags & ConstantMask) != 0;
                var Exists = ((uint) Flags & NotExistMask) == 0;

                if (Exists) {
                    Deserializer.BaseStream.Seek (Addresses[ElemIndex], SeekOrigin.Begin);

                    uint ElemFlags = 0;
                    var Address = Addresses[ElemIndex];
                    uint Count = 1;

                    if (!Constant) {
                        /*
                         * gdkchan Note:
                         * Those values have been verified and Start Frame will always be zero on observed BCHs.
                         * This may or may not change on future versions (probably not), so we can safely ignore it.
                         */
                        var StartFrame = Deserializer.Reader.ReadSingle ();
                        var EndFrame = Deserializer.Reader.ReadSingle ();

                        ElemFlags = Deserializer.Reader.ReadUInt32 ();
                        Address = Deserializer.Reader.ReadUInt32 ();
                        Count = Deserializer.Reader.ReadUInt32 ();
                    }

                    Deserializer.BaseStream.Seek (Address, SeekOrigin.Begin);

                    for (var Index = 0; Index < Count; Index++)
                        switch (ElemIndex) {
                            case 0:
                                Scales.Add (Deserializer.Reader.ReadVector3 ());
                                break;
                            case 1:
                                Rotations.Add (Deserializer.Reader.ReadQuaternion ());
                                break;
                            case 2:
                                Translations.Add (Deserializer.Reader.ReadVector3 ());
                                break;
                        }
                }

                ConstantMask >>= 1;
                NotExistMask >>= 1;
            }
        }

        bool ICustomSerialization.Serialize (BinarySerializer Serializer)
        {
            Flags = 0;

            var ConstantMask = (uint) H3DAnimQuatTransformFlags.IsScaleConstant;
            var NotExistMask = (uint) H3DAnimQuatTransformFlags.IsScaleInexistent;

            var DescPosition = Serializer.BaseStream.Position;
            var DataPosition = DescPosition + 0x10;

            for (var ElemIndex = 0; ElemIndex < 3; ElemIndex++) {
                IList Elem = null;

                switch (ElemIndex) {
                    case 0:
                        Elem = Scales;
                        break;
                    case 1:
                        Elem = Rotations;
                        break;
                    case 2:
                        Elem = Translations;
                        break;
                }

                if (Elem.Count > 0) {
                    Serializer.BaseStream.Seek (DescPosition + 4 + ElemIndex * 4, SeekOrigin.Begin);

                    Serializer.WritePointer ((uint) DataPosition);

                    Serializer.BaseStream.Seek (DataPosition, SeekOrigin.Begin);

                    if (Elem.Count > 1) {
                        Serializer.Writer.Write (0f); //Start Frame
                        Serializer.Writer.Write ((float) Elem.Count); //End Frame
                        Serializer.Writer.Write (0u); //Flags?
                        Serializer.Writer.Write (0u); //KeyFrames Ptr (Place Holder)
                        Serializer.Writer.Write (0u); //KeyFrames Count (Place Holder)

                        Serializer.Sections[(uint) H3DSectionId.Contents].Values.Add (new RefValue {
                            Value = Elem,
                            Position = Serializer.BaseStream.Position - 8,
                            HasLength = true
                        });
                    } else {
                        Flags |= (H3DAnimQuatTransformFlags) ConstantMask;

                        Serializer.WriteValue (Elem[0]);
                    }

                    DataPosition = Serializer.BaseStream.Position;
                } else {
                    Flags |= (H3DAnimQuatTransformFlags) NotExistMask;
                }

                ConstantMask >>= 1;
                NotExistMask >>= 1;
            }

            Serializer.BaseStream.Seek (DescPosition, SeekOrigin.Begin);

            Serializer.Writer.Write ((uint) Flags);

            Serializer.BaseStream.Seek (DataPosition, SeekOrigin.Begin);

            return true;
        }

        private T GetFrameValue<T> (List<T> Vector, int Frame)
        {
            if (Vector.Count > 0) {
                if (Frame < 0)
                    return Vector.First ();
                if (Frame >= Vector.Count)
                    return Vector.Last ();
                return Vector[Frame];
            }

            return default;
        }

        public Vector3 GetScaleValue (int Frame)
        {
            return GetFrameValue (Scales, Frame);
        }

        public Quaternion GetRotationValue (int Frame)
        {
            return GetFrameValue (Rotations, Frame);
        }

        public Vector3 GetTranslationValue (int Frame)
        {
            return GetFrameValue (Translations, Frame);
        }
    }
}