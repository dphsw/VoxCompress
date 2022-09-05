using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VoxelFileCompression {

  internal class VoxelFileChunk {

    VoxelFileChunkHeader Header { get; set; }
    byte[] ChunkData { get; set; }
    List<VoxelFileChunk> Children { get; set; } = new List<VoxelFileChunk>();

    public int ChunkSize { get { return 12 + Header.BytesChunk + Header.BytesChildren; } }

    public static VoxelFileChunk Read(BinaryReader br) {
      VoxelFileChunk chunk = new VoxelFileChunk();
      chunk.Header = VoxelFileChunkHeader.Read(br);
      chunk.ChunkData = br.ReadBytes(chunk.Header.BytesChunk);
      chunk.ReadChildChunks(br, chunk.Header.BytesChildren);
      return chunk;
    }

    void ReadChildChunks(BinaryReader br, int bytesToRead) {
      int bytesRead = 0;
      VoxelFileChunk newChild;

      while (bytesRead < bytesToRead) {
        newChild = VoxelFileChunk.Read(br);
        Children.Add(newChild);
        bytesRead += newChild.ChunkSize;
      }

      if (bytesRead != bytesToRead) throw new Exception("Malformed MagicaVoxel File - child chunk sizes incorrectly specified");
    }

    public int GetNumChunksIncChildren() {
      int count = 1;
      foreach (VoxelFileChunk c in Children) count += c.GetNumChunksIncChildren();
      return count;
    }

    internal void Write(BinaryWriter bw) {
      Header.Write(bw);
      bw.Write(ChunkData);
      foreach (VoxelFileChunk c in Children) c.Write(bw);
    }
  }

}
