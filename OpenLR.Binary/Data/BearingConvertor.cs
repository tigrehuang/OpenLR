﻿using OpenLR.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenLR.Binary.Data
{
    /// <summary>
    /// Represents a bearing convertor that encodes/decodes into the binary OpenLR format.
    /// </summary>
    public static class BearingConvertor
    {
        /// <summary>
        /// Decodes a bearing from binary data.
        /// </summary>
        /// <param name="data">The binary data.</param>
        /// <param name="byteIndex">The index of the data in the first byte.</param>
        /// <returns></returns>
        public static int Decode(byte[] data, int byteIndex)
        {
            return BearingConvertor.Decode(data, 0, byteIndex);
        }

        /// <summary>
        /// Decodes a bearing from binary data.
        /// </summary>
        /// <param name="data">The binary data.</param>
        /// <param name="startIndex">The index of the byte in data.</param>
        /// <param name="byteIndex">The index of the data in the given byte.</param>
        public static int Decode(byte[] data, int startIndex, int byteIndex)
        {
            if (byteIndex > 3) { throw new ArgumentOutOfRangeException("byteIndex", "byteIndex has to be a value in the range of [0-3]."); }

            byte classData = data[startIndex];

            // create mask.
            int mask = 31 << (3 - byteIndex);
            return (classData & mask) >> (3 - byteIndex);
        }

        /// <summary>
        /// Encodes a bearing into binary data.
        /// </summary>
        /// <param name="bearing"></param>
        /// <param name="data"></param>
        /// <param name="startIndex"></param>
        /// <param name="byteIndex"></param>
        public static void Encode(int bearing, byte[] data, int startIndex, int byteIndex)
        {
            if (byteIndex > 3) { throw new ArgumentOutOfRangeException("byteIndex", "byteIndex has to be a value in the range of [0-3]."); }

            byte dataByte = data[startIndex];

            // create mask.
            byte mask = (byte)(31 << (3 - byteIndex));

            // apply mask, reset existing data.
            dataByte = (byte)((~mask) & dataByte);

            // create byte containing bearing.
            byte bearingByte = (byte)(bearing << (3 - byteIndex));

            // add bearing.
            dataByte = (byte)(bearingByte | dataByte);

            // encode.
            data[startIndex] = dataByte;
        }
    }
}