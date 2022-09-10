using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace VoxelFileCompression {

  class VoxelFileChunkVoxels : VoxelFileChunk {

    List<Voxel> Voxels = new List<Voxel>();

    protected override void ProcessData() {
      Voxels.Clear();
      byte bx, by, bz, bi;

      using (BinaryReader br = new BinaryReader(new MemoryStream(ChunkData))) {
        int n = br.ReadInt32();

        for(int i = 0; i < n; i++) {
          bx = br.ReadByte();
          bz = br.ReadByte();
          by = br.ReadByte();
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
