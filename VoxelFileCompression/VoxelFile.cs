using System;
using System.IO;

namespace VoxelFileCompression {

  public class VoxelFile {

    public bool CompressedWhenLoaded { get; private set; }
    public int VersionIndicator { get; private set; }

    VoxelFileChunk mainChunk { get; set; }

    public int FileSize {
      get {
        return 8 + mainChunk.ChunkSize;
      }
    }

    public static VoxelFile Load(Stream stream) {

      if (stream == null) return null;

      VoxelFile voxelFile = new VoxelFile();

      using (BinaryReader br = new BinaryReader(stream)) {

        voxelFile.ReadFirstFourChars(br);
        voxelFile.ReadVersionIndicator(br);
        voxelFile.mainChunk = VoxelFileChunk.Read(br);

      }

      return voxelFile;

    }

    void ReadFirstFourChars(BinaryReader br) {
      string fourChars = new string(br.ReadChars(4));
      if (fourChars == "VOX ") CompressedWhenLoaded = false;
      else if (fourChars == "VOZ ") CompressedWhenLoaded = false;
      else throw new Exception("File is not a MagicaVoxel file");
    }

    void ReadVersionIndicator(BinaryReader br) {
      VersionIndicator = br.ReadInt32();
    }

    public int GetNumChunks() {
      return mainChunk.GetNumChunksIncChildren();
    }

  }

}
