using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VoxelFileCompression {

  internal class VoxelFileChunk {

    protected VoxelFileChunkHeader Header { get; set; }
    protected byte[] ChunkData { get; set; }
    List<VoxelFileChunk> Children { get; set; } = new List<VoxelFileChunk>();

    public int ChunkSize { get { return 12 + Header.BytesChunk + Header.BytesChildren; } }

    public static VoxelFileChunk Read(BinaryReader br) {
      VoxelFileChunkHeader header = VoxelFileChunkHeader.Read(br);
      VoxelFileChunk chunk;

      if (header.Identifier == "XYZI") chunk = new VoxelFileChunkVoxels();
      else if (header.Identifier == "PNGS") chunk = new VoxelFileChunkCompressedVoxels();
      else chunk = new VoxelFileChunk();

      chunk.Header = header;
      chunk.ChunkData = br.ReadBytes(chunk.Header.BytesChunk);
      chunk.ProcessData();
      chunk.ReadChildChunks(br, chunk.Header.BytesChildren);
      return chunk;
    }

    protected virtual void ProcessData() { }

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
      int count = 1; // Start by counting this chunk - should return 1 if have no children
      foreach (VoxelFileChunk c in Children) count += c.GetNumChunksIncChildren();
      return count;
    }

    internal void Write(BinaryWriter bw) {
      Header.Write(bw);
      bw.Write(ChunkData);
      foreach (VoxelFileChunk c in Children) c.Write(bw);
    }

    internal virtual VoxelFileChunk MakeCopy(bool compressed) {
      VoxelFileChunk chunk = new VoxelFileChunk();

      chunk.ChunkData = (byte[])ChunkData.Clone();
      foreach (VoxelFileChunk c in Children) chunk.Children.Add(c.MakeCopy(compressed));

      int childrenSize = 0;
      foreach (VoxelFileChunk c in chunk.Children) childrenSize += c.ChunkSize;

      chunk.Header = new VoxelFileChunkHeader(Header.Identifier, ChunkData.Length, childrenSize);

      return chunk;
    }

  }

}
