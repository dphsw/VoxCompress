using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace VoxelFileCompression {

  class VoxelFileChunkVoxels : VoxelFileChunk {

    List<Voxel> Voxels = new List<Voxel>();

    public VoxelFileChunkVoxels() { }

    public VoxelFileChunkVoxels(List<Voxel> voxels) {
      Voxels.AddRange(voxels);
      int n = Voxels.Count;
      Voxel v;

      MemoryStream chunkStream = new MemoryStream();
      using (var bw = new BinaryWriter(chunkStream)) {
        bw.Write(n);

        for (int i = 0; i < n; i++) {
          v = Voxels[i];
          bw.Write(v.x);
          bw.Write(v.y);
          bw.Write(v.z);
          bw.Write(v.i);
        }
      }

      ChunkData = chunkStream.ToArray();
      Header = new VoxelFileChunkHeader("XYZI", ChunkData.Length, 0);
    }

    protected override void ProcessData() {
      Voxels.Clear();
      byte bx, by, bz, bi;

      using (BinaryReader br = new BinaryReader(new MemoryStream(ChunkData))) {
        int n = br.ReadInt32();

        for(int i = 0; i < n; i++) {
          bx = br.ReadByte();
          by = br.ReadByte();
          bz = br.ReadByte();
          bi = br.ReadByte();
          Voxels.Add(new Voxel(bx, by, bz, bi));
        }
      }
    }

    internal override VoxelFileChunk MakeCopy(bool compressed) {
      if (compressed) return new VoxelFileChunkCompressedVoxels(Voxels);
      else return base.MakeCopy(false);
    }

  }

}
