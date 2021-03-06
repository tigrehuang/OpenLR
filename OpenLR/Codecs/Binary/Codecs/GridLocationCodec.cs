﻿// The MIT License (MIT)

// Copyright (c) 2016 Ben Abelshausen

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using OpenLR.Codecs.Binary.Data;
using OpenLR.Model.Locations;

namespace OpenLR.Codecs.Binary.Decoders
{
    /// <summary>
    /// A decoder that decodes binary data into a grid location.
    /// </summary>
    public static class GridLocationCodec
    {
        /// <summary>
        /// Decodes the given data into a location reference.
        /// </summary>
        public static GridLocation Decode(byte[] data)
        {
            // decode box.
            var lowerLeft = CoordinateConverter.Decode(data, 1);
            var upperRight = CoordinateConverter.DecodeRelative(lowerLeft, data, 7);
            
            // decode column/row info.
            var columns = data[11] * 256 + data[12];
            var rows = data[13] * 256 + data[14];

            // create grid location.
            var grid = new GridLocation();
            grid.LowerLeft = lowerLeft;
            grid.UpperRight = upperRight;
            grid.Columns = columns;
            grid.Rows = rows;
            return grid;
        }

        /// <summary>
        /// Returns true if the given data can be decoded by this decoder.
        /// </summary>
        public static bool CanDecode(byte[] data)
        {
            // decode the header first.
            var header = HeaderConvertor.Decode(data, 0);

            // check header info.
            if (!header.ArF1 ||
                header.IsPoint ||
                header.ArF0 ||
                header.HasAttributes)
            { // header is incorrect.
                return false;
            }

            return data != null && (data.Length == 15 || data.Length == 17);
        }
    }
}