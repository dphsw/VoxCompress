using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace VoxelFileCompression {
  internal class VoxelFileChunkCompressedVoxels : VoxelFileChunk {
    private List<Voxel> Voxels;

    public VoxelFileChunkCompressedVoxels(List<Voxel> voxels) {
      Voxels = new List<Voxel>();
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

  }
}