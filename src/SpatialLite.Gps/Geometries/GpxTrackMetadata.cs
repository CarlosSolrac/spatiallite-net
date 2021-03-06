﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialLite.Gps.Geometries {
    /// <summary>
    /// Contains additional information about Gpx tracks and routes
    /// </summary>
    public class GpxTrackMetadata : GpxMetadata {
        #region Public properties

        /// <summary>
        /// Gets or sets yype (classification) of route.
        /// </summary>
        public string Type { get; set; }

        #endregion
    }
}
