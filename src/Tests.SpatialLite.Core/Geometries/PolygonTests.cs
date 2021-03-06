﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using SpatialLite.Core;
using SpatialLite.Core.API;
using SpatialLite.Core.Geometries;

namespace Tests.SpatialLite.Core.Geometries {
	public class PolygonTests {
		#region Test Data
		
		Coordinate[] _coordinatesXY = new Coordinate[] {
					new Coordinate(1, 2), new Coordinate(1.1, 2.1), new Coordinate(1.2, 2.2), new Coordinate(1.3, 2.3)
		};
		
		Coordinate[] _coordinatesXYZ = new Coordinate[] {
					new Coordinate(1, 2, 3), new Coordinate(1.1, 2.1, 3.1), new Coordinate(1.2, 2.2, 3.2), new Coordinate(1.3, 2.3, 3.3)
		};

		Coordinate[] _coordinatesXYZM = new Coordinate[] {
					new Coordinate(1, 2, 3, 10), new Coordinate(1.1, 2.1, 3.1, 20), new Coordinate(1.2, 2.2, 3.2, 30), new Coordinate(1.3, 2.3, 3.3, 40)
		};

		CoordinateList _exteriorRing3D;
		CoordinateList[] _interiorRings3D;

		#endregion

		public PolygonTests() {
			_exteriorRing3D = new CoordinateList(_coordinatesXYZ);

			_interiorRings3D = new CoordinateList[2];
			_interiorRings3D[0] = new CoordinateList(new Coordinate[] { _coordinatesXYZ[0], _coordinatesXYZ[1], _coordinatesXYZ[0] });
			_interiorRings3D[1] = new CoordinateList(new Coordinate[] { _coordinatesXYZ[1], _coordinatesXYZ[2], _coordinatesXYZ[1] });
		}

		private void CheckInteriorRings(Polygon target, CoordinateList[] expected) {
			Assert.Equal(expected.Length, target.InteriorRings.Count);
			for (int i = 0; i < expected.Length; i++) {
				Assert.Same(expected[i], target.InteriorRings[i]);
			}
		}

		#region Constructor tests

		#region Default Constructor tests

		[Fact]
		public void Constructor__CreatesEmptyPolygonAndInitializesProperties() {
			Polygon target = new Polygon();

			Assert.Equal(SRIDList.WSG84, target.Srid);
			
			Assert.NotNull(target.ExteriorRing);
			Assert.Empty(target.ExteriorRing);

			Assert.NotNull(target.InteriorRings);
			Assert.Empty(target.InteriorRings);
		}

		#endregion

		#region Constructor(SRID) tests

		[Fact]
		public void Constructor_SRID_CreatesEmptyPolygonWithCustomSRID() {
			int srid = 1;
			Polygon target = new Polygon(srid);

			Assert.Equal(srid, target.Srid);

			Assert.NotNull(target.ExteriorRing);
			Assert.Empty(target.ExteriorRing);
			
			Assert.NotNull(target.InteriorRings);
			Assert.Empty(target.InteriorRings);
		}

		#endregion

		#region Constructor(ExteriorRing)

		[Fact]
		public void Constructor_ExteriorRing_CreatesPolygonWithExteriorBoundary() {
			Polygon target = new Polygon(_exteriorRing3D);

			Assert.Equal(SRIDList.WSG84, target.Srid);
			Assert.Same(_exteriorRing3D, target.ExteriorRing);

			Assert.NotNull(target.InteriorRings);
			Assert.Empty(target.InteriorRings);
		}

		#endregion

		#region Constructor(ExteriorRing, IEnumerable<InteriorRings>)

		[Fact]
		public void Constructor_SRIDExteriorRingInteriorRings_CreatesPolygonWithExteriorBoundary() {
			int srid = 1;
			Polygon target = new Polygon(srid, _exteriorRing3D, _interiorRings3D);

			Assert.Equal(srid, target.Srid);

			Assert.NotNull(target.ExteriorRing);
			Assert.Same(_exteriorRing3D, target.ExteriorRing);

			CheckInteriorRings(target, _interiorRings3D);
		}

		#endregion

		#endregion

		#region Is3D tests

		[Fact]
		public void Is3D_ReturnsTrueFor3DExteriorRing() {
			Polygon target = new Polygon(_exteriorRing3D);

			Assert.True(target.Is3D);
		}

		[Fact]
		public void Is3D_ReturnsFalseForEmptyPolygon() {
			Polygon target = new Polygon();

			Assert.False(target.Is3D);
		}

		[Fact]
		public void Is3D_ReturnsFalseFor2DExteriorRing() {
			Polygon target = new Polygon(new CoordinateList(_coordinatesXY));

			Assert.False(target.Is3D);
		}

		#endregion

		#region IsMeasured tests

		[Fact]
		public void IsMeasured_ReturnsTrueForMeasuredExteriorRing() {
			Polygon target = new Polygon(new CoordinateList(_coordinatesXYZM));

			Assert.True(target.IsMeasured);
		}

		[Fact]
		public void IsMeasured_ReturnsFalseForEmptyPolygon() {
			Polygon target = new Polygon();

			Assert.False(target.IsMeasured);
		}

		[Fact]
		public void IsMeasured_ReturnsFalseForNonMeasuredExteriorRing() {
			Polygon target = new Polygon(new CoordinateList(_coordinatesXYZ));

			Assert.False(target.IsMeasured);
		}

		#endregion

		#region GetEnvelope() tests

		[Fact]
		public void GetEnvelope_ReturnsEmptyEnvelopeForEmptyPolygon() {
			Polygon target = new Polygon();
			double x = double.NaN;
			bool test = x == double.NaN;
			Envelope envelope = target.GetEnvelope();

			Assert.Equal(Envelope.Empty, envelope);
		}


		[Fact]
		public void GetEnvelopeReturnsEnvelopeOfLineString() {
			Envelope expectedEnvelope = new Envelope(_coordinatesXYZ);

			Polygon target = new Polygon(_exteriorRing3D);
			Envelope envelope = target.GetEnvelope();

			Assert.Equal(expectedEnvelope, envelope);
		}

		#endregion

		#region GetBoundary() tests

		[Fact]
		public void GetBoundary_ReturnsMultiLineStringWithExteriorAndInteriorRingsAndCorrectSRID() {
			int srid = 1;
			Polygon target = new Polygon(srid, _exteriorRing3D, _interiorRings3D);
			IMultiLineString boundary = target.GetBoundary() as IMultiLineString;

			Assert.NotNull(boundary);
			Assert.Equal(srid, boundary.Srid);
		}

		#endregion
	}
}
