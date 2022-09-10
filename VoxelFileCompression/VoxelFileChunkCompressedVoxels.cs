using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace VoxelFileCompression {
  internal class VoxelFileChunkCompressedVoxels : VoxelFileChunk {
    List<Voxel> Voxels = new List<Voxel>();

    public VoxelFileChunkCompressedVoxels() { }

    public VoxelFileChunkCompressedVoxels(List<Voxel> voxels) {
      Voxels.AddRange(voxels);

      MemoryStream compressedStream = new MemoryStream();
      using (var deflater = new DeflateStream(compressedStream, CompressionLevel.Optimal)) {
        using (var bw = new BinaryWriter(deflater)) {
          bw.Write(Voxels.Count);
          bw.Write(GetStripedData());
        }
      }

      ChunkData = compressedStream.ToArray();
      Header = new VoxelFileChunkHeader("PNGS", ChunkData.Length, 0);

    }

    private byte[] GetStripedData() {
      /* The data is reformatted to be more compressible.
       * LZ-compression is more effective if a set of similar values occur in a row,
       * and putting all x-values, all y-values etc. together makes this more likely.
      */
      Voxels.Sort();
      int n = Voxels.Count;

      byte[] data = new byte[4 * n];
      for(int i = 0; i < n; i++) {
        data[0 * n + i] = Voxels[i].x;
        data[1 * n + i] = Voxels[i].y;
        data[2 * n + i] = Voxels[i].z;
        data[3 * n + i] = Voxels[i].i;
      }

      return data;

    }

    internal override VoxelFileChunk MakeCopy(bool compressed) {
      if (compressed) return base.MakeCopy(true);
      return new VoxelFileChunkVoxels(Voxels);
    }

    protected override void ProcessData() {
      Voxels.Clear();

      byte bx, by, bz, bi;

      MemoryStream inflatedData = new MemoryStream();
      using(var inflator = new DeflateStream(inflatedData, CompressionMode.Decompress))
        using (var bw = new BinaryWriter(inflator))
          bw.Write(ChunkData);

      inflatedData.Position = 0;
      int v,numVoxels;

      using (BinaryReader br = new BinaryReader(inflatedData)) numVoxels = br.ReadInt32();

      byte[] voxelData = inflatedData.ToArray();
      for (v = 0; v < numVoxels; v++) {
        bx = voxelData[0 * numVoxels + 4 + v];
        by = voxelData[1 * numVoxels + 4 + v];
        bz = voxelData[2 * numVoxels + 4 + v];
        bi = voxelData[3 * numVoxels + 4 + v];
        Voxels.Add(new Voxel(bx, by, bz, bi));
      }

    }

  }
}