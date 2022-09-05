using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VoxelFileCompression {

  class VoxelFileChunkHeader {
    public string Identifier { get; private set; }
    public int BytesChunk { get; private set; }
    public int BytesChildren { get; private set; }

    public static VoxelFileChunkHeader Read(BinaryReader br) {
      VoxelFileChunkHeader header = new VoxelFileChunkHeader();
      header.Identifier = new string(br.ReadChars(4));
      header.BytesChunk = br.ReadInt32();
      header.BytesChildren = br.ReadInt32();
      return header;
    }

    internal void Write(BinaryWriter bw) {
      bw.Write(Identifier.ToCharArray());
      bw.Write(BytesChunk);
      bw.Write(BytesChildren);
    }
  }

}
