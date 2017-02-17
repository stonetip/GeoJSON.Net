// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeoJSONObject.cs" company="Joerg Battermann">
//   Copyright © Joerg Battermann 2014
// </copyright>
// <summary>
//   Defines the GeoJSONObject type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GeoJSON.Net
{
    /// <summary>
    ///     Base class for all IGeometryObject implementing types
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class GeoJSONObject : IGeoJSONObject, IEqualityComparer<GeoJSONObject>, IEquatable<GeoJSONObject>
    {
        internal static readonly DoubleTenDecimalPlaceComparer DoubleComparer = new DoubleTenDecimalPlaceComparer();

        /// <summary>
        ///     Gets or sets the (optional)
        ///     <see cref="http://geojson.org/geojson-spec.html#coordinate-reference-system-objects">Bounding Boxes</see>.
        /// </summary>
        /// <value>
        ///     The value of <see cref="BoundingBoxes" /> must be a 2*n array where n is the number of dimensions represented in
        ///     the
        ///     contained geometries, with the lowest values for all axes followed by the highest values.
        ///     The axes order of a bbox follows the axes order of geometries.
        ///     In addition, the coordinate reference system for the bbox is assumed to match the coordinate reference
        ///     system of the GeoJSON object of which it is a member.
        /// </value>
        [JsonProperty(PropertyName = "bbox", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public double[] BoundingBoxes { get; set; }


        /// <summary>
        ///     Gets the (mandatory) type of the
        ///     <see cref="http://geojson.org/geojson-spec.html#geojson-objects">GeoJSON Object</see>.
        /// </summary>
        /// <value>
        ///     The type of the object.
        /// </value>
        [JsonProperty(PropertyName = "type", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public GeoJSONObjectType Type { get; internal set; }

        /// <summary>
        ///     Called when [deserialized].
        /// </summary>
        /// <param name="streamingContext">The streaming context.</param>
        [OnDeserialized]
        private void OnDeserialized(StreamingContext streamingContext)
        {
        }



        #region IEqualityComparer, IEquatable

        /// <summary>
        ///     Determines whether the specified object is equal to the current object
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(this, obj as GeoJSONObject);
        }

        /// <summary>
        ///     Determines whether the specified object is equal to the current object
        /// </summary>
        public bool Equals(GeoJSONObject other)
        {
            return Equals(this, other);
        }

        /// <summary>
        ///     Determines whether the specified object instances are considered equal
        /// </summary>
        public bool Equals(GeoJSONObject left, GeoJSONObject right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(null, right))
                return false;

            if (left.Type != right.Type)
                return false;


            var leftIsNull = ReferenceEquals(null, left.BoundingBoxes);
            var rightIsNull = ReferenceEquals(null, right.BoundingBoxes);
            var bothAreMissing = leftIsNull && rightIsNull;

            if (bothAreMissing || leftIsNull != rightIsNull)
                return bothAreMissing;

            return left.BoundingBoxes.SequenceEqual(right.BoundingBoxes, DoubleComparer);
        }

        /// <summary>
        ///     Determines whether the specified object instances are considered equal
        /// </summary>
        public static bool operator ==(GeoJSONObject left, GeoJSONObject right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(null, right))
                return false;
            return left.Equals(right);
        }

        /// <summary>
        ///     Determines whether the specified object instances are not considered equal
        /// </summary>
        public static bool operator !=(GeoJSONObject left, GeoJSONObject right)
        {
            return !(left == right);
        }

        /// <summary>
        ///     Returns the hash code for this instance
        /// </summary>
        public override int GetHashCode()
        {
            return ((int) Type).GetHashCode();
        }

        /// <summary>
        ///     Returns the hash code for the specified object
        /// </summary>
        public int GetHashCode(GeoJSONObject obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }
}