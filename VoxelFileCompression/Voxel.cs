using System;
using System.Collections.Generic;
using System.Text;

namespace VoxelFileCompression {

  internal struct Voxel : IComparable<Voxel> {

    public byte x, y, z, i;

    public Voxel(byte X, byte Y, byte Z, byte I) {
      x = X; y = Y; z = Z; i = I;
    }
    public Voxel(int X, int Y, int Z, byte I) {
      x = (byte)X; y = (byte)Y; z = (byte)Z; i = I;
    }
    public static bool operator <(Voxel a, Voxel b) {
      if (a.x < b.x) return true; if (a.x > b.x) return false;
      if (a.y < b.y) return true; if (a.y > b.y) return false;
      if (a.z < b.z) return true; if (a.z > b.z) return false;
      if (a.i < b.i) return true; if (a.i > b.i) return false;
      return false;
    }
    public static bool operator >(Voxel b, Voxel a) {
      if (a.x < b.x) return true; if (a.x > b.x) return false;
      if (a.y < b.y) return true; if (a.y > b.y) return false;
      if (a.z < b.z) return true; if (a.z > b.z) return false;
      if (a.i < b.i) return true; if (a.i > b.i) return false;
      return false;
    }
    public static bool operator <=(Voxel a, Voxel b) {
      if (a.x < b.x) return true; if (a.x > b.x) return false;
      if (a.y < b.y) return true; if (a.y > b.y) return false;
      if (a.z < b.z) return true; if (a.z > b.z) return false;
      if (a.i < b.i) return true; if (a.i > b.i) return false;
      return true;
    }
    public static bool operator >=(Voxel b, Voxel a) {
      if (a.x < b.x) return true; if (a.x > b.x) return false;
      if (a.y < b.y) return true; if (a.y > b.y) return false;
      if (a.z < b.z) return true; if (a.z > b.z) return false;
      if (a.i < b.i) return true; if (a.i > b.i) return false;
      return true;
    }
    public static bool operator ==(Voxel a, Voxel b) {
      return a.x == b.x && a.y == b.y && a.z == b.z && a.i == b.i;
    }
    public static bool operator !=(Voxel a, Voxel b) {
      return a.x != b.x || a.y != b.y || a.z != b.z || a.i != b.i;
    }

    public int CompareTo(Voxel other) {
      if (x < other.x) return -1; if (x > other.x) return 1;
      if (y < other.y) return -1; if (y > other.y) return 1;
      if (z < other.z) return -1; if (z > other.z) return 1;
      if (i < other.i) return -1; if (i > other.i) return 1;
      return 0;
    }

    public override bool Equals(object obj) {
      if (obj.GetType() != typeof(Voxel)) return false;
      return this == (Voxel)obj;
    }

    public override int GetHashCode() {
      return (x << 24) | (y << 16) | (z << 8) | i;
    }

  }

}
