using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VoxelFileCompression {

  class VoxelFileChunkHeader {
    public string Identifier { get; private set; }
    public int BytesChunk { get; private set; }
    public int BytesChildren { get; private set; }

    public VoxelFileChunkHeader(string identifier, int bytesChunk, int bytesChildren) {
      Identifier = identifier;
      BytesChunk = bytesChunk;
      BytesChildren = bytesChildren;
    }

    public static VoxelFileChunkHeader Read(BinaryReader br) {
      string id = new string(br.ReadChars(4));
      int chunk = br.ReadInt32();
      int children = br.ReadInt32();
      VoxelFileChunkHeader header = new VoxelFileChunkHeader(id, chunk, children);
      return header;
    }

    internal void Write(BinaryWriter bw) {
      bw.Write(Identifier.ToCharArray());
      bw.Write(BytesChunk);
      bw.Write(BytesChildren);
    }
  }

}
